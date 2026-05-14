using UnityEngine;
using UnityEngine.UI;

public class GoldShimmer : MonoBehaviour
{
    public Color colorA = new Color(1f, 0.6f, 0f, 1f);
    public Color colorB = new Color(1f, 1f, 0.4f, 1f);
    public float speed = 3f;

    private Image img;
    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        img = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.unscaledDeltaTime * speed;
        float pulse = (Mathf.Sin(timer) + 1f) / 2f;
        Color c = Color.Lerp(colorA, colorB, pulse);
        if (img != null) img.color = c;
        if (sr != null) sr.color = c;
    }
}
