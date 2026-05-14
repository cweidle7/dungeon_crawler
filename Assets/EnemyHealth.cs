using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event System.Action OnEnemyDied;

    public int hp = 30;
    public int xpDrop = 100;
    private int maxHP;
    private WorldHealthBar healthBar;
    private bool isDead = false;

    void Start()
    {
        if (healthBar == null)
            Init();
    }

    public void Init()
    {
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject.transform.Find("HealthBar")?.gameObject);
            Destroy(healthBar);
            healthBar = null;
        }
        maxHP = hp;
        healthBar = WorldHealthBar.Create(gameObject, maxHP, Color.red);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (healthBar == null) Init();
        hp -= amount;
        healthBar.SetHP(hp, maxHP);
        FlashEffect.Flash(gameObject, Color.white, 0.08f);
        if (hp <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        OnEnemyDied?.Invoke();
        DeathParticles.SpawnAt(transform.position, GetComponent<SpriteRenderer>()?.color ?? Color.red);
        XPOrb.SpawnAt(transform.position, xpDrop);
        Destroy(gameObject);
    }
}
