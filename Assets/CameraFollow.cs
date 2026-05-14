using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    public float smoothSpeed = 10f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = target;
    }
}
