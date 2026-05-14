using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    void Awake() { instance = this; }

    public void Shake(float duration = 0.3f, float magnitude = 0.3f)
    {
        StopAllCoroutines();
        StartCoroutine(DoShake(duration, magnitude));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 original = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, original.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = original;
    }
}
