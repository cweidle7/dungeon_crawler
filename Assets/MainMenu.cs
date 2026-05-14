using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        BuildUI();
    }

    void BuildUI()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        // Camera with dark background
        Camera cam = Camera.main;
        if (cam != null) cam.backgroundColor = new Color(0.04f, 0.03f, 0.06f);

        // Canvas
        GameObject canvasObj = new GameObject("MenuCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // Full background
        GameObject bg = new GameObject("BG");
        bg.transform.SetParent(canvasObj.transform, false);
        Image bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0.04f, 0.03f, 0.06f);
        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Game title
        MakeText(canvasObj.transform, "DUNGEON CRAWLER",
            72, new Color(0.88f, 0.72f, 0.18f), FontStyle.Bold,
            new Vector2(0, 140), new Vector2(1000, 110));

        // Tagline
        MakeText(canvasObj.transform, "Survive the endless darkness",
            28, new Color(0.65f, 0.58f, 0.45f), FontStyle.Italic,
            new Vector2(0, 60), new Vector2(700, 50));

        // Divider line
        MakeDivider(canvasObj.transform, new Vector2(0, 10), new Vector2(400, 2));

        // Enter button
        MakeButton(canvasObj.transform, "ENTER THE DARKNESS", new Vector2(0, -80),
            new Vector2(380, 70), () => SceneManager.LoadScene(1));

        // Controls hint
        MakeText(canvasObj.transform, "WASD — Move     Auto-attack fires automatically     Level up to grow stronger",
            18, new Color(0.38f, 0.34f, 0.28f), FontStyle.Normal,
            new Vector2(0, -220), new Vector2(900, 36));
    }

    void MakeText(Transform parent, string text, int size, Color color, FontStyle style,
        Vector2 pos, Vector2 sizeDelta)
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

    void MakeDivider(Transform parent, Vector2 pos, Vector2 sizeDelta)
    {
        GameObject obj = new GameObject("Divider");
        obj.transform.SetParent(parent, false);
        Image img = obj.AddComponent<Image>();
        img.color = new Color(0.88f, 0.72f, 0.18f, 0.4f);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = sizeDelta;
    }

    void MakeButton(Transform parent, string label, Vector2 pos, Vector2 sizeDelta,
        UnityEngine.Events.UnityAction action)
    {
        // Button glow border
        GameObject glow = new GameObject("ButtonGlow");
        glow.transform.SetParent(parent, false);
        Image glowImg = glow.AddComponent<Image>();
        glowImg.color = new Color(0.6f, 0.1f, 0.1f, 0.6f);
        RectTransform glowRect = glow.GetComponent<RectTransform>();
        glowRect.anchorMin = new Vector2(0.5f, 0.5f);
        glowRect.anchorMax = new Vector2(0.5f, 0.5f);
        glowRect.anchoredPosition = pos;
        glowRect.sizeDelta = sizeDelta + new Vector2(6, 6);

        // Button
        GameObject btn = new GameObject("Button");
        btn.transform.SetParent(parent, false);
        Image img = btn.AddComponent<Image>();
        img.color = new Color(0.28f, 0.03f, 0.04f);
        Button button = btn.AddComponent<Button>();

        ColorBlock colors = button.colors;
        colors.normalColor    = new Color(0.28f, 0.03f, 0.04f);
        colors.highlightedColor = new Color(0.45f, 0.06f, 0.07f);
        colors.pressedColor   = new Color(0.18f, 0.02f, 0.02f);
        button.colors = colors;

        button.onClick.AddListener(action);
        RectTransform rect = btn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = sizeDelta;

        MakeText(btn.transform, label, 26, new Color(0.9f, 0.75f, 0.55f), FontStyle.Bold,
            Vector2.zero, sizeDelta);
    }
}
