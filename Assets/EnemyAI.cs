using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            player.position,
            speed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPos);
    }
}
