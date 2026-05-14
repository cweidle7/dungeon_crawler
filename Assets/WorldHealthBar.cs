using UnityEngine;

public class WorldHealthBar : MonoBehaviour
{
    private GameObject barRoot;
    private Transform fill;
    private int maxHP;
    private int currentHP;

    public static WorldHealthBar Create(GameObject target, int hp, Color color)
    {
        WorldHealthBar whb = target.AddComponent<WorldHealthBar>();
        whb.maxHP = hp;
        whb.currentHP = hp;
        whb.BuildBar(color);
        return whb;
    }

    void BuildBar(Color color)
    {
        barRoot = new GameObject("HealthBar");
        barRoot.transform.SetParent(transform);
        barRoot.transform.localPosition = new Vector3(0, 0.8f, 0);

        // Background
        GameObject bg = new GameObject("BG");
        bg.transform.SetParent(barRoot.transform);
        bg.transform.localPosition = Vector3.zero;
        bg.transform.localScale = new Vector3(1f, 0.15f, 1f);
        SpriteRenderer bgSR = bg.AddComponent<SpriteRenderer>();
        bgSR.sprite = SpriteHelper.Square;
        bgSR.color = new Color(0.2f, 0.2f, 0.2f);
        bgSR.sortingOrder = 2;

        // Fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(barRoot.transform);
        fillObj.transform.localPosition = Vector3.zero;
        fillObj.transform.localScale = new Vector3(1f, 0.15f, 1f);
        SpriteRenderer fillSR = fillObj.AddComponent<SpriteRenderer>();
        fillSR.sprite = SpriteHelper.Square;
        fillSR.color = color;
        fillSR.sortingOrder = 3;
        fill = fillObj.transform;
    }

    public void SetHP(int current, int max)
    {
        currentHP = current;
        maxHP = max;
        if (fill == null) return;
        float ratio = Mathf.Clamp01((float)current / max);
        fill.localScale = new Vector3(ratio, 0.15f, 1f);
        fill.localPosition = new Vector3((ratio - 1f) / 2f, 0, 0);
    }

    void OnDestroy()
    {
        if (barRoot != null) Destroy(barRoot);
    }
}
