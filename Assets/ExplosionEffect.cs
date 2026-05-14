using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public static void SpawnAt(Vector2 pos)
    {
        GameObject obj = new GameObject("Explosion");
        obj.transform.position = pos;
        obj.AddComponent<ExplosionEffect>();
    }

    private SpriteRenderer sr;
    private float duration = 0.4f;
    private float elapsed = 0f;

    void Start()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Circle;
        sr.color = new Color(1f, 0.5f, 0f, 0.8f);
        sr.sortingOrder = 5;
        transform.localScale = Vector3.one * 0.3f;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;

        transform.localScale = Vector3.one * Mathf.Lerp(0.3f, 4f, t);
        sr.color = new Color(1f, Mathf.Lerp(0.5f, 0.1f, t), 0f, Mathf.Lerp(0.8f, 0f, t));

        if (elapsed >= duration)
            Destroy(gameObject);
    }

}
