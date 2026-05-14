using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
    public enum Type { Normal, Fast, Tank }
    public Type enemyType = Type.Normal;

    void Start()
    {
        EnemyAI ai = GetComponent<EnemyAI>();
        EnemyHealth health = GetComponent<EnemyHealth>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float hpMult = DifficultyManager.instance != null ? DifficultyManager.instance.hpMultiplier : 1f;
        float spdMult = DifficultyManager.instance != null ? DifficultyManager.instance.speedMultiplier : 1f;

        switch (enemyType)
        {
            case Type.Fast:
                ai.speed = 5f * spdMult;
                health.hp = Mathf.RoundToInt(10 * hpMult);
                transform.localScale = Vector3.one * 0.6f;
                sr.sprite = SpriteHelper.Circle;
                sr.color = new Color(0.65f, 0.05f, 0.1f);
                break;

            case Type.Tank:
                ai.speed = 1f * spdMult;
                health.hp = Mathf.RoundToInt(60 * hpMult);
                health.xpDrop = 200;
                transform.localScale = Vector3.one * 2f;
                sr.color = new Color(0.18f, 0.02f, 0.25f);
                break;

            case Type.Normal:
                ai.speed = 2f * spdMult;
                health.hp = Mathf.RoundToInt(30 * hpMult);
                transform.localScale = Vector3.one;
                sr.color = new Color(0.48f, 0.43f, 0.38f);
                break;
        }

        health.Init();
        BuildVisual();
    }

    void BuildVisual()
    {
        switch (enemyType)
        {
            case Type.Normal: // Husk — shambling zombie
                AddPart("Head", new Vector2(0f, 0.52f), new Vector3(0.72f, 0.62f, 1f),
                    SpriteHelper.Circle, new Color(0.4f, 0.36f, 0.3f), 1);
                AddPart("EyeL", new Vector2(-0.17f, 0.56f), new Vector3(0.16f, 0.14f, 1f),
                    SpriteHelper.Square, new Color(0.08f, 0.05f, 0.1f), 2);
                AddPart("EyeR", new Vector2(0.17f, 0.56f), new Vector3(0.16f, 0.14f, 1f),
                    SpriteHelper.Square, new Color(0.08f, 0.05f, 0.1f), 2);
                break;

            case Type.Fast: // Wraith — ethereal wisp
                AddPart("Core", new Vector2(0f, 0f), new Vector3(0.5f, 0.5f, 1f),
                    SpriteHelper.Circle, new Color(0.92f, 0.28f, 0.32f, 0.9f), 1);
                AddPart("Wisp1", new Vector2(0f, -0.55f), new Vector3(0.32f, 0.48f, 1f),
                    SpriteHelper.Circle, new Color(0.65f, 0.05f, 0.1f, 0.5f), 0);
                AddPart("Wisp2", new Vector2(0.18f, -0.75f), new Vector3(0.2f, 0.3f, 1f),
                    SpriteHelper.Circle, new Color(0.65f, 0.05f, 0.1f, 0.25f), 0);
                break;

            case Type.Tank: // Colossus — armored hulk
                AddPart("ShoulderL", new Vector2(-0.52f, 0.2f), new Vector3(0.36f, 0.44f, 1f),
                    SpriteHelper.Square, new Color(0.14f, 0.01f, 0.2f), 1);
                AddPart("ShoulderR", new Vector2(0.52f, 0.2f), new Vector3(0.36f, 0.44f, 1f),
                    SpriteHelper.Square, new Color(0.14f, 0.01f, 0.2f), 1);
                AddPart("Head", new Vector2(0f, 0.52f), new Vector3(0.7f, 0.48f, 1f),
                    SpriteHelper.Square, new Color(0.16f, 0.02f, 0.22f), 1);
                AddPart("Eye", new Vector2(0f, 0.54f), new Vector3(0.44f, 0.1f, 1f),
                    SpriteHelper.Square, new Color(0.85f, 0.05f, 0.08f), 2);
                break;
        }
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
