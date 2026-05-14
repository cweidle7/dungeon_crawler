using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;

    void Awake()
    {
        instance = this;
    }

    public void Show(int level)
    {
        Time.timeScale = 0f;

        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        GameObject canvasObj = new GameObject("GameOverCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 20;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Dark overlay
        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(canvasObj.transform, false);
        Image bg = overlay.AddComponent<Image>();
        bg.color = new Color(0.02f, 0f, 0.04f, 0.92f);
        RectTransform bgRect = overlay.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // DARKNESS CLAIMS YOU text
        MakeText(canvasObj.transform, "DARKNESS CLAIMS YOU", 60, new Color(0.85f, 0.1f, 0.15f),
            new Vector2(0, 60), new Vector2(700, 100), FontStyle.Bold);

        // Level reached
        MakeText(canvasObj.transform, "Souls Harvested — Level: " + level, 34, new Color(0.8f, 0.72f, 0.55f),
            new Vector2(0, -20), new Vector2(500, 60), FontStyle.Normal);

        // Rise Again button
        MakeButton(canvasObj.transform, "RISE AGAIN", new Vector2(0, -120), () =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    void MakeText(Transform parent, string text, int size, Color color,
        Vector2 pos, Vector2 sizeDelta, FontStyle style)
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

    void MakeButton(Transform parent, string label, Vector2 pos, UnityEngine.Events.UnityAction action)
    {
        GameObject btn = new GameObject("Button");
        btn.transform.SetParent(parent, false);
        Image img = btn.AddComponent<Image>();
        img.color = new Color(0.3f, 0.02f, 0.04f);
        Button button = btn.AddComponent<Button>();
        button.onClick.AddListener(action);
        RectTransform rect = btn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(200, 60);

        MakeText(btn.transform, label, 28, Color.white, Vector2.zero, new Vector2(200, 60), FontStyle.Bold);
    }
}
