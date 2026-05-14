using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public float hpMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float bossHPMultiplier = 1f;

    private float timer = 0f;

    void Awake() { instance = this; }

    void Update()
    {
        timer += Time.deltaTime;
        hpMultiplier = 1f + (timer / 60f) * 0.5f;
        speedMultiplier = 1f + (timer / 120f) * 0.3f;
        bossHPMultiplier = 1f + (timer / 60f) * 0.8f;
    }
}
