using UnityEngine;

public class VampireAura : MonoBehaviour
{
    public int healPerKill = 5;

    void OnEnable()
    {
        EnemyHealth.OnEnemyDied += OnKill;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= OnKill;
    }

    void OnKill()
    {
        PlayerHealth ph = GetComponent<PlayerHealth>();
        if (ph != null && ph.healthBar != null)
        {
            ph.currentHP = Mathf.Min(ph.currentHP + healPerKill, ph.maxHP);
            ph.healthBar.SetHP(ph.currentHP, ph.maxHP);
        }
    }
}
