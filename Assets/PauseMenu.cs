using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject pauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Don't pause if level-up or game-over screen is open
            if (Time.timeScale == 0f && !isPaused) return;
            if (isPaused) Resume();
            else Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        BuildUI();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseCanvas != null) Destroy(pauseCanvas);
    }

    void BuildUI()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        pauseCanvas = new GameObject("PauseCanvas");
        Canvas canvas = pauseCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 25;
        CanvasScaler scaler = pauseCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        pauseCanvas.AddComponent<GraphicRaycaster>();

        // Dark overlay
        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(pauseCanvas.transform, false);
        Image bg = overlay.AddComponent<Image>();
        bg.color = new Color(0.02f, 0f, 0.05f, 0.88f);
        RectTransform bgRect = overlay.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(pauseCanvas.transform, false);
        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0.07f, 0.05f, 0.1f, 1f);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(420, 380);
        panelRect.anchoredPosition = Vector2.zero;

        // Gold border
        GameObject border = new GameObject("Border");
        border.transform.SetParent(pauseCanvas.transform, false);
        Image borderImg = border.AddComponent<Image>();
        RectTransform borderRect = border.GetComponent<RectTransform>();
        borderRect.anchorMin = new Vector2(0.5f, 0.5f);
        borderRect.anchorMax = new Vector2(0.5f, 0.5f);
        borderRect.sizeDelta = new Vector2(426, 386);
        borderRect.anchoredPosition = Vector2.zero;
        GoldShimmer shimmer = border.AddComponent<GoldShimmer>();
        shimmer.colorA = new Color(0.5f, 0.35f, 0.05f, 1f);
        shimmer.colorB = new Color(0.88f, 0.72f, 0.18f, 1f);
        shimmer.speed = 2f;

        // Title
        MakeText(panel.transform, "— PAUSED —", 48, new Color(0.88f, 0.72f, 0.18f),
            FontStyle.Bold, new Vector2(0, 120), new Vector2(380, 70));

        // Divider
        GameObject divider = new GameObject("Divider");
        divider.transform.SetParent(panel.transform, false);
        Image div = divider.AddComponent<Image>();
        div.color = new Color(0.88f, 0.72f, 0.18f, 0.3f);
        RectTransform divRect = divider.GetComponent<RectTransform>();
        divRect.anchorMin = new Vector2(0.5f, 0.5f);
        divRect.anchorMax = new Vector2(0.5f, 0.5f);
        divRect.sizeDelta = new Vector2(300, 2);
        divRect.anchoredPosition = new Vector2(0, 72);

        // Resume button
        MakeButton(panel.transform, "RESUME", new Vector2(0, 10),
            new Color(0.1f, 0.22f, 0.12f), Resume);

        // Restart button
        MakeButton(panel.transform, "RESTART", new Vector2(0, -70),
            new Color(0.28f, 0.03f, 0.04f), () =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });

        // Main Menu button
        MakeButton(panel.transform, "MAIN MENU", new Vector2(0, -150),
            new Color(0.1f, 0.08f, 0.14f), () =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            });

        // Hint
        MakeText(panel.transform, "Press ESC to resume", 16, new Color(0.4f, 0.36f, 0.3f),
            FontStyle.Italic, new Vector2(0, -205), new Vector2(380, 30));
    }

    void MakeText(Transform parent, string text, int size, Color color,
        FontStyle style, Vector2 pos, Vector2 sizeDelta)
    {
        GameObject obj = new GameObject("Text");
        obj.transform.SetParent(parent, false);
        Text t = obj.AddComponent<Text>();
        t.text = text;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = size;
        t.fontStyle = style;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = color;
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = sizeDelta;
    }

    void MakeButton(Transform parent, string label, Vector2 pos, Color bgColor,
        UnityEngine.Events.UnityAction action)
    {
        GameObject btn = new GameObject("Btn_" + label);
        btn.transform.SetParent(parent, false);
        Image img = btn.AddComponent<Image>();
        img.color = bgColor;
        Button button = btn.AddComponent<Button>();

        ColorBlock colors = button.colors;
        colors.normalColor = bgColor;
        colors.highlightedColor = new Color(bgColor.r + 0.1f, bgColor.g + 0.1f, bgColor.b + 0.1f);
        colors.pressedColor = new Color(bgColor.r - 0.05f, bgColor.g - 0.05f, bgColor.b - 0.05f);
        button.colors = colors;

        button.onClick.AddListener(action);
        RectTransform rect = btn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(300, 55);

        MakeText(btn.transform, label, 24, new Color(0.88f, 0.82f, 0.68f),
            FontStyle.Bold, Vector2.zero, new Vector2(300, 55));
    }
}
