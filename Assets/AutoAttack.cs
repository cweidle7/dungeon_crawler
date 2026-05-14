using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public float fireRate = 1f;
    public int projectileCount = 1;
    public int baseDamage = 10;
    public bool spreadShot = false;
    public bool explosive = false;
    public bool explosiveDoubleDamage = false;
    public bool autoKnockback = false;
    public bool lockOn = false;
    private float timer;
    private int shotCount = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        shotCount++;
        bool thisRoundExplodes = explosive && (shotCount % 3 == 0);

        if (projectileCount == 1)
        {
            SpawnBullet(Vector2.right, thisRoundExplodes, Vector2.right * 0.6f);
        }
        else if (projectileCount == 2)
        {
            SpawnBullet(Vector2.right, thisRoundExplodes, Vector2.right * 0.6f);
            SpawnBullet(Vector2.left, thisRoundExplodes, Vector2.left * 0.6f);
        }
        else
        {
            float angleStep = 360f / projectileCount;
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                SpawnBullet(dir, thisRoundExplodes);
            }
        }
    }

    void SpawnBullet(Vector2 dir, bool isExplosive = false, Vector2 offset = default)
    {
        GameObject bullet = new GameObject("Bullet");
        bullet.transform.position = (Vector2)transform.position + offset;

        SpriteRenderer sr = bullet.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Square;
        sr.color = isExplosive ? new Color(0.55f, 0f, 0.05f) : new Color(0.75f, 0.9f, 1f);
        bullet.transform.localScale = isExplosive ? Vector3.one * 0.6f : Vector3.one * 0.4f;

        CircleCollider2D col = bullet.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        Projectile p = bullet.AddComponent<Projectile>();
        p.damage = isExplosive && explosiveDoubleDamage ? baseDamage * 2 : baseDamage;
        p.spreadShot = spreadShot;
        p.explosive = isExplosive;
        p.knockback = isExplosive || autoKnockback;
        p.lockOn = lockOn;
        p.Init(dir);
    }
}
