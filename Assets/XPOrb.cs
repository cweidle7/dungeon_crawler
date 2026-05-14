using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpValue = 100;
    public float moveSpeed = 5f;
    public float pickupRadius = 1.5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < pickupRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (dist < 0.2f)
            {
                PlayerXP xp = player.GetComponent<PlayerXP>();
                if (xp != null) xp.AddXP(xpValue);
                SoundManager.instance?.PlayPickup();
                Destroy(gameObject);
            }
        }
    }

    public static void SpawnAt(Vector2 pos, int xp = 100)
    {
        GameObject orb = new GameObject("XPOrb");
        orb.transform.position = pos;

        SpriteRenderer sr = orb.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Square;
        sr.color = new Color(0.2f, 0.9f, 0.5f);
        orb.transform.localScale = Vector3.one * 0.5f;

        XPOrb orbScript = orb.AddComponent<XPOrb>();
        orbScript.xpValue = xp;
    }
}
