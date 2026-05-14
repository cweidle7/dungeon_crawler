using UnityEngine;
using System.Collections;

public class FlashEffect : MonoBehaviour
{
    public static void Flash(GameObject target, Color flashColor, float duration = 0.1f)
    {
        FlashEffect fe = target.GetComponent<FlashEffect>();
        if (fe == null) fe = target.AddComponent<FlashEffect>();
        fe.StopAllCoroutines();
        fe.StartCoroutine(fe.DoFlash(target, flashColor, duration));
    }

    IEnumerator DoFlash(GameObject target, Color flashColor, float duration)
    {
        SpriteRenderer[] renderers = target.GetComponentsInChildren<SpriteRenderer>();
        Color[] originals = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originals[i] = renderers[i].color;
            renderers[i].color = flashColor;
        }
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < renderers.Length; i++)
            if (renderers[i] != null) renderers[i].color = originals[i];
    }
}
