using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private int charges = 0;
    private GameObject shieldVisual;

    public void AddCharge()
    {
        charges++;
        if (shieldVisual == null) CreateVisual();
        shieldVisual.SetActive(true);
    }

    public bool AbsorbHit()
    {
        if (charges <= 0) return false;
        charges--;
        if (charges <= 0 && shieldVisual != null)
            shieldVisual.SetActive(false);
        return true;
    }

    void CreateVisual()
    {
        shieldVisual = new GameObject("ShieldVisual");
        shieldVisual.transform.SetParent(transform);
        shieldVisual.transform.localPosition = Vector3.zero;
        shieldVisual.transform.localScale = Vector3.one * 1.8f;

        SpriteRenderer sr = shieldVisual.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Square;
        sr.color = new Color(0.3f, 0.6f, 1f, 0.4f);
        sr.sortingOrder = 1;
    }
}
