using UnityEngine;
using System.Collections;

public class KnockbackEffect : MonoBehaviour
{
    public static void Apply(Transform target, Vector2 direction, float force)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        KnockbackEffect effect = target.gameObject.AddComponent<KnockbackEffect>();
        effect.StartCoroutine(effect.DoKnockback(rb, direction, force));
    }

    IEnumerator DoKnockback(Rigidbody2D rb, Vector2 direction, float force)
    {
        EnemyAI ai = rb.GetComponent<EnemyAI>();
        Boss boss = rb.GetComponent<Boss>();

        if (ai != null) ai.enabled = false;
        if (boss != null) boss.enabled = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (ai != null) ai.enabled = true;
        if (boss != null) boss.enabled = true;

        Destroy(this);
    }
}
