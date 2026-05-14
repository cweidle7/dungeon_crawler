using UnityEngine;
using UnityEngine.UI;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner instance;

    public Sprite bossSprite;
    public float spawnInterval = 60f;
    private float timer = 0f;
    private bool bossAlive = false;
    private Transform player;

    private GameObject warningUI;
    private float warningTimer = 0f;
    private bool showingWarning = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (bossAlive) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval - 5f && !showingWarning)
            ShowWarning();

        if (showingWarning)
        {
            warningTimer += Time.deltaTime;
            if (warningTimer >= 5f)
                HideWarning();
        }

        if (timer >= spawnInterval)
        {
            SpawnBoss();
            timer = 0f;
        }
    }

    void SpawnBoss()
    {
        if (player == null) return;
        bossAlive = true;

        Vector2 spawnPos = (Vector2)player.position + new Vector2(12f, 0);

        GameObject bossObj = new GameObject("Boss");
        bossObj.transform.position = spawnPos;
        bossObj.transform.localScale = Vector3.one * 1.8f;
        bossObj.tag = "Enemy";

        SpriteRenderer sr = bossObj.AddComponent<SpriteRenderer>();
        sr.sprite = bossSprite;
        sr.color = Color.white;
        sr.sortingOrder = 1;

        Rigidbody2D rb = bossObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        BoxCollider2D col = bossObj.AddComponent<BoxCollider2D>();

        bossObj.AddComponent<Boss>();
        if (ScreenShake.instance != null) ScreenShake.instance.Shake(0.4f, 0.4f);
        SoundManager.instance?.PlayBoss();
    }

    void ShowWarning()
    {
        showingWarning = true;
        warningTimer = 0f;

        GameObject canvasObj = new GameObject("WarningCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 15;
        canvasObj.AddComponent<CanvasScaler>();

        warningUI = canvasObj;

        GameObject txt = new GameObject("Warning");
        txt.transform.SetParent(canvasObj.transform, false);
        Text t = txt.AddComponent<Text>();
        t.text = "⚠ THE ANCIENT ONE AWAKENS ⚠";
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 48;
        t.fontStyle = FontStyle.Bold;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = new Color(1f, 0.2f, 0.2f);
        RectTransform rect = txt.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.4f);
        rect.anchorMax = new Vector2(0.9f, 0.6f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    void HideWarning()
    {
        if (warningUI != null) Destroy(warningUI);
        showingWarning = false;
    }

    public void OnBossDied()
    {
        bossAlive = false;
    }
}
