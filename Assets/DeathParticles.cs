using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    public static void SpawnAt(Vector2 pos, Color color, int count = 8)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject p = new GameObject("Particle");
            p.transform.position = pos;
            p.transform.localScale = Vector3.one * 0.2f;

            SpriteRenderer sr = p.AddComponent<SpriteRenderer>();
            sr.sprite = SpriteHelper.Square;
            sr.color = color;
            sr.sortingOrder = 4;

            Particle particle = p.AddComponent<Particle>();
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            particle.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(2f, 5f);
        }
    }
}

public class Particle : MonoBehaviour
{
    public Vector2 velocity;
    private float lifetime = 0.5f;
    private float elapsed = 0f;
    private SpriteRenderer sr;

    void Start() { sr = GetComponent<SpriteRenderer>(); }

    void Update()
    {
        elapsed += Time.deltaTime;
        transform.position += (Vector3)(velocity * Time.deltaTime);
        velocity *= 0.9f;
        float alpha = 1f - (elapsed / lifetime);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (elapsed >= lifetime) Destroy(gameObject);
    }
}
