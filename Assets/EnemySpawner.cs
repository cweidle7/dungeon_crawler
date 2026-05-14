using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 8f;

    private float timer;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.05f);
        }
    }

    void SpawnEnemy()
    {
        if (player == null) return;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 spawnPos = new Vector2(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y + Mathf.Sin(angle) * spawnRadius
        );

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // 60% normal, 30% fast, 10% tank
        float roll = Random.value;
        EnemyConfig config = enemy.GetComponent<EnemyConfig>();
        if (config != null)
        {
            if (roll < 0.6f)
                config.enemyType = EnemyConfig.Type.Normal;
            else if (roll < 0.9f)
                config.enemyType = EnemyConfig.Type.Fast;
            else
                config.enemyType = EnemyConfig.Type.Tank;
        }
    }
}
