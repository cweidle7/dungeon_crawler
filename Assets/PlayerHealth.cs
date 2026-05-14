using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public float damageCooldown = 1f;

    private float lastDamageTime;
    private Vector2 startPosition;
    public WorldHealthBar healthBar;

    void Start()
    {
        currentHP = maxHP;
        startPosition = transform.position;
        healthBar = WorldHealthBar.Create(gameObject, maxHP, Color.green);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && Time.time > lastDamageTime + damageCooldown)
        {
            TakeDamage(10);
            lastDamageTime = Time.time;
        }
    }

    public void TakeDamage(int amount)
    {
        PlayerShield shield = GetComponent<PlayerShield>();
        if (shield != null && shield.AbsorbHit()) return;

        currentHP -= amount;
        healthBar.SetHP(currentHP, maxHP);
        FlashEffect.Flash(gameObject, Color.white, 0.1f);
        SoundManager.instance?.PlayHit();
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        int level = GetComponent<PlayerXP>() != null ? GetComponent<PlayerXP>().level : 1;
        GameOver.instance.Show(level);
    }
}
