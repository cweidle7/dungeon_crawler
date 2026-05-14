using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp = 500;
    public float speed = 1.5f;
    public int xpDrop = 1000;
    public int damage = 25;
    public float damageCooldown = 1.5f;

    private int maxHP;
    private Transform player;
    private Rigidbody2D rb;
    private WorldHealthBar healthBar;
    private float lastDamageTime;

    void Start()
    {
        if (DifficultyManager.instance != null)
            hp = Mathf.RoundToInt(hp * DifficultyManager.instance.bossHPMultiplier);
        maxHP = hp;
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        healthBar = WorldHealthBar.Create(gameObject, maxHP, new Color(0.8f, 0f, 0.8f));
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        healthBar.SetHP(hp, maxHP);
        if (hp <= 0) Die();
    }

    void Die()
    {
        DeathParticles.SpawnAt(transform.position, new Color(0.6f, 0f, 0.6f), 20);
        if (ScreenShake.instance != null) ScreenShake.instance.Shake(0.5f, 0.5f);
        XPOrb.SpawnAt(transform.position, xpDrop);
        BossSpawner.instance.OnBossDied();
        Destroy(gameObject);
    }
}
