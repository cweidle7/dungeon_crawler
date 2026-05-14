using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer body = GetComponent<SpriteRenderer>();
        body.sprite = SpriteHelper.Square;
        body.color = new Color(0.22f, 0.28f, 0.42f);
        body.sortingOrder = 1;

        // Dark cloak behind body
        AddPart("Cloak", new Vector2(0f, -0.05f), new Vector3(1.12f, 1.18f, 1f),
            SpriteHelper.Square, new Color(0.1f, 0.06f, 0.16f), 0);

        // Helmet
        AddPart("Helmet", new Vector2(0f, 0.52f), new Vector3(0.65f, 0.58f, 1f),
            SpriteHelper.Circle, new Color(0.28f, 0.35f, 0.5f), 2);

        // Visor slit
        AddPart("Visor", new Vector2(0f, 0.52f), new Vector3(0.42f, 0.1f, 1f),
            SpriteHelper.Square, new Color(0.05f, 0.05f, 0.08f), 3);

        // Staff shaft
        AddPart("Staff", new Vector2(0.58f, -0.05f), new Vector3(0.1f, 1.15f, 1f),
            SpriteHelper.Square, new Color(0.28f, 0.2f, 0.35f), 2);

        // Orb glow (larger, faint)
        AddPart("OrbGlow", new Vector2(0.58f, 0.56f), new Vector3(0.45f, 0.45f, 1f),
            SpriteHelper.Circle, new Color(0.4f, 0.7f, 1f, 0.3f), 2);

        // Orb core (pulsing lightning blue)
        GameObject orb = AddPart("Orb", new Vector2(0.58f, 0.56f), new Vector3(0.28f, 0.28f, 1f),
            SpriteHelper.Circle, new Color(0.75f, 0.9f, 1f), 3);
        GoldShimmer shimmer = orb.AddComponent<GoldShimmer>();
        shimmer.colorA = new Color(0.5f, 0.75f, 1f);
        shimmer.colorB = new Color(0.9f, 0.97f, 1f);
        shimmer.speed = 4f;
    }

    GameObject AddPart(string partName, Vector2 localPos, Vector3 localScale,
        Sprite sprite, Color color, int sortOrder)
    {
        GameObject part = new GameObject(partName);
        part.transform.SetParent(transform);
        part.transform.localPosition = localPos;
        part.transform.localScale = localScale;
        SpriteRenderer sr = part.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        sr.sortingOrder = sortOrder;
        return part;
    }
}
