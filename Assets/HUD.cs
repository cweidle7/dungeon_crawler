using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour
{
    private PlayerXP playerXP;

    private Image xpBar;
    private Text levelText;
    private Text timerText;
    private float survivalTime = 0f;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerXP = player.GetComponent<PlayerXP>();

        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        BuildHUD();
    }

    void BuildHUD()
    {
        GameObject canvasObj = new GameObject("HUDCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // XP Bar background (bottom center)
        MakeBar(canvasObj.transform, new Vector2(0, 20), new Vector2(600, 20),
            new Color(0.05f, 0.08f, 0.05f), new Vector2(0.5f, 0), new Vector2(0.5f, 0), "XPBarBG");

        // XP Bar fill (bottom center) — soul green
        xpBar = MakeBar(canvasObj.transform, new Vector2(0, 20), new Vector2(0, 20),
            new Color(0.15f, 0.75f, 0.38f), new Vector2(0.5f, 0), new Vector2(0.5f, 0), "XPBar");
        xpBar.rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Level display (above XP bar, bottom center)
        levelText = MakeLabel(canvasObj.transform, "Level 1",
            new Vector2(0, 45), new Vector2(300, 36), new Vector2(0.5f, 0), new Vector2(0.5f, 0));
        levelText.fontSize = 24;
        levelText.fontStyle = FontStyle.Bold;
        levelText.color = new Color(0.88f, 0.72f, 0.18f);
        levelText.alignment = TextAnchor.MiddleCenter;

        // Timer (top center)
        timerText = MakeLabel(canvasObj.transform, "0:00",
            new Vector2(0, -20), new Vector2(160, 40), new Vector2(0.5f, 1), new Vector2(0.5f, 1));
        timerText.fontSize = 28;
        timerText.fontStyle = FontStyle.Bold;
        timerText.color = new Color(0.82f, 0.76f, 0.6f);
        timerText.alignment = TextAnchor.UpperCenter;
    }

    Image MakeBar(Transform parent, Vector2 anchoredPos, Vector2 size,
        Color color, Vector2 anchorMin, Vector2 anchorMax, string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        Image img = obj.AddComponent<Image>();
        img.color = color;
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = size;
        return img;
    }

    Text MakeLabel(Transform parent, string text, Vector2 anchoredPos, Vector2 size,
        Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject obj = new GameObject("Label");
        obj.transform.SetParent(parent, false);
        Text t = obj.AddComponent<Text>();
        t.text = text;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 20;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleLeft;
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = size;
        return t;
    }

    void Update()
    {
        survivalTime += Time.deltaTime;

        if (playerXP != null)
        {
            float xpRatio = (float)playerXP.currentXP / playerXP.xpToNextLevel;
            xpBar.rectTransform.sizeDelta = new Vector2(600 * xpRatio, 20);
            levelText.text = "Level " + playerXP.level;
        }

        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);
        timerText.text = minutes + ":" + seconds.ToString("00");
    }
}
