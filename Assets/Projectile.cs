using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;
    public bool spreadShot = false;
    public bool explosive = false;
    public bool isSpread = false;
    public bool knockback = false;
    public float knockbackForce = 5f;
    public bool lockOn = false;

    private Vector2 direction;
    private Transform target;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        if (lockOn)
        {
            if (target == null) FindTarget();
            if (target != null)
                direction = Vector2.MoveTowards(direction, (target.position - transform.position).normalized, 4f * Time.deltaTime);
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void FindTarget()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        float closest = Mathf.Infinity;
        foreach (EnemyHealth e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < closest) { closest = d; target = e.transform; }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Boss boss = col.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            if (explosive) Explode();
            Destroy(gameObject);
            return;
        }

        EnemyHealth enemy = col.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (knockback || explosive)
                ApplyKnockback(col.transform);

            if (explosive)
                Explode();

            if (spreadShot && !isSpread)
                SpawnSpread();

            Destroy(gameObject);
        }
    }

    void ApplyKnockback(Transform target)
    {
        Vector2 pushDir = (target.position - transform.position).normalized;
        KnockbackEffect.Apply(target, pushDir, knockbackForce);
    }

    void SpawnSpread()
    {
        float[] angles = { -30f, 0f, 30f };
        foreach (float a in angles)
        {
            float rad = (Mathf.Atan2(direction.y, direction.x) + a * Mathf.Deg2Rad);
            Vector2 newDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject b = new GameObject("SpreadBullet");
            b.transform.position = transform.position;
            b.transform.localScale = Vector3.one * 0.3f;

            SpriteRenderer sr = b.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteHelper.Square;
            sr.color = Color.cyan;

            CircleCollider2D c = b.AddComponent<CircleCollider2D>();
            c.isTrigger = true;

            Projectile p = b.AddComponent<Projectile>();
            p.damage = damage;
            p.isSpread = true;
            p.Init(newDir);
        }
    }

    void Explode()
    {
        ExplosionEffect.SpawnAt(transform.position);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (Collider2D hit in hits)
        {
            EnemyHealth e = hit.GetComponent<EnemyHealth>();
            if (e != null) e.TakeDamage(damage / 2);
        }
    }
}
