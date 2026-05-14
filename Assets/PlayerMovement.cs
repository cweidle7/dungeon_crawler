using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.sqrMagnitude > 1f) movement.Normalize();
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
