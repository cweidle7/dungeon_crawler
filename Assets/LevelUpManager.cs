using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager instance;

    private GameObject panel;
    private bool isOpen = false;
    private bool autoKnockbackTaken = false;
    private bool explosiveTaken = false;
    private bool explosiveDoubleTaken = false;
    private bool lockOnTaken = false;

    void Awake()
    {
        instance = this;
    }

    public void ShowLevelUp()
    {
        if (isOpen) return;
        isOpen = true;
        Time.timeScale = 0f;
        BuildUI();
    }

    void BuildUI()
    {
        // EventSystem (required for button clicks)
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        // Canvas
        GameObject canvasObj = new GameObject("LevelUpCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Dark overlay
        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(canvasObj.transform, false);
        Image bg = overlay.AddComponent<Image>();
        bg.color = new Color(0.02f, 0f, 0.05f, 0.88f);
        RectTransform bgRect = overlay.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Title
        GameObject title = new GameObject("Title");
        title.transform.SetParent(canvasObj.transform, false);
        Text titleText = title.AddComponent<Text>();
        titleText.text = "POWER GRANTED — Choose Your Gift:";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 36;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.9f, 0.75f, 0.2f);
        RectTransform titleRect = title.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.1f, 0.7f);
        titleRect.anchorMax = new Vector2(0.9f, 0.9f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        // Common upgrades pool
        List<string[]> common = new List<string[]>
        {
            new string[] { "Fleet of Shadow",  "+20% movement speed" },
            new string[] { "Edge of Ruin",     "+10 bullet damage" },
            new string[] { "Iron Flesh",       "+50 max health" },
            new string[] { "Fevered Volley",   "-20% fire rate cooldown" },
            new string[] { "Barrage",          "+2 bullets per shot" },
            new string[] { "Dark Ward",        "Block the next hit you take" },
            new string[] { "Blood Drinker",    "Heal 5 HP on every kill" },
            new string[] { "Shattered Bolt",   "Bullets split into 3 on impact" }
        };

        // Pick 2 random common cards
        List<int> usedIndices = new List<int>();
        while (usedIndices.Count < 2)
        {
            int r = Random.Range(0, common.Count);
            if (!usedIndices.Contains(r)) usedIndices.Add(r);
        }

        // tier: 0=common, 1=rare, 2=ultra rare
        List<string[]> picks = new List<string[]>();
        List<int> tiers = new List<int>();
        foreach (int i in usedIndices) { picks.Add(common[i]); tiers.Add(0); }

        // 3rd slot — special tier draw:
        //   Soul Seeker guaranteed at level 5+ → ultra rare
        //   Hellfire guaranteed until taken → rare
        //   Infernal Surge guaranteed until taken → rare
        //   then roll: 15% Wrath Strike (rare), 12% Soul Seeker (ultra rare), else common
        PlayerXP playerXP = GameObject.FindWithTag("Player")?.GetComponent<PlayerXP>();
        int currentLevel = playerXP != null ? playerXP.level : 1;

        float roll = Random.value;
        if (!lockOnTaken && currentLevel >= 5)
        {
            picks.Add(new string[] { "🎯 SOUL SEEKER 🎯", "Your bolts hunt down fleeing souls" });
            tiers.Add(2);
        }
        else if (!explosiveTaken)
        {
            picks.Add(new string[] { "Hellfire", "Every 3rd shot ignites on impact" });
            tiers.Add(1);
        }
        else if (!explosiveDoubleTaken)
        {
            picks.Add(new string[] { "💥 INFERNAL SURGE 💥", "Hellfire now deals double damage" });
            tiers.Add(1);
        }
        else if (!autoKnockbackTaken && roll < 0.15f)
        {
            picks.Add(new string[] { "⚡ WRATH STRIKE ⚡", "Each strike sends enemies flying" });
            tiers.Add(1);
        }
        else if (!lockOnTaken && roll < 0.27f)
        {
            picks.Add(new string[] { "🎯 SOUL SEEKER 🎯", "Your bolts hunt down fleeing souls" });
            tiers.Add(2);
        }
        else
        {
            int r;
            do { r = Random.Range(0, common.Count); } while (usedIndices.Contains(r));
            picks.Add(common[r]);
            tiers.Add(0);
        }

        float[] xPositions = { 0.1f, 0.38f, 0.66f };

        panel = canvasObj;

        for (int i = 0; i < 3; i++)
            CreateCard(canvasObj.transform, picks[i][0], picks[i][1], xPositions[i], tiers[i]);
    }

    // tier: 0 = common, 1 = rare, 2 = ultra rare
    void CreateCard(Transform parent, string upgradeName, string desc, float xAnchor, int tier = 0)
    {
        // Per-tier visual values
        Color bgColor      = tier == 2 ? new Color(0.07f, 0.04f, 0.14f, 1f)   // deep void purple
                           : tier == 1 ? new Color(0.15f, 0.08f, 0.02f, 1f)   // dark amber
                                       : new Color(0.08f, 0.06f, 0.1f,  1f);  // stone
        Color borderColor  = tier == 2 ? new Color(0.7f, 0.8f, 1f, 1f)        // cold silver-blue
                           : tier == 1 ? new Color(1f, 0.84f, 0f, 1f)         // gold
                                       : Color.clear;
        Color titleColor   = tier == 2 ? new Color(0.82f, 0.9f, 1f)           // silver-white
                           : tier == 1 ? new Color(1f, 0.84f, 0f)             // gold
                                       : Color.white;
        string badge       = tier == 2 ? "✦ ULTRA RARE ✦"
                           : tier == 1 ? "— RARE —"
                                       : "";
        Color badgeColor   = tier == 2 ? new Color(0.7f, 0.8f, 1f)
                           : new Color(1f, 0.84f, 0f);

        // Glowing border behind card (rare + ultra rare)
        if (tier > 0)
        {
            GameObject border = new GameObject("Border");
            border.transform.SetParent(parent, false);
            Image borderImg = border.AddComponent<Image>();
            borderImg.color = borderColor;
            RectTransform borderRect = border.GetComponent<RectTransform>();
            borderRect.anchorMin = new Vector2(xAnchor, 0.2f);
            borderRect.anchorMax = new Vector2(xAnchor + 0.26f, 0.68f);
            borderRect.offsetMin = new Vector2(5, -5);
            borderRect.offsetMax = new Vector2(-5, 5);
            GoldShimmer shimmer = border.AddComponent<GoldShimmer>();
            if (tier == 2)
            {
                shimmer.colorA = new Color(0.4f, 0.55f, 1f, 1f);
                shimmer.colorB = new Color(0.85f, 0.92f, 1f, 1f);
            }
        }

        GameObject card = new GameObject("Card_" + upgradeName);
        card.transform.SetParent(parent, false);

        Image cardBg = card.AddComponent<Image>();
        cardBg.color = bgColor;

        RectTransform rect = card.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(xAnchor, 0.2f);
        rect.anchorMax = new Vector2(xAnchor + 0.26f, 0.68f);
        rect.offsetMin = new Vector2(10, 0);
        rect.offsetMax = new Vector2(-10, 0);

        Button btn = card.AddComponent<Button>();
        string captured = upgradeName;
        btn.onClick.AddListener(() => SelectUpgrade(captured));

        // Tier badge (rare/ultra rare only)
        if (tier > 0)
        {
            GameObject badgeObj = new GameObject("Badge");
            badgeObj.transform.SetParent(card.transform, false);
            Text badgeText = badgeObj.AddComponent<Text>();
            badgeText.text = badge;
            badgeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            badgeText.fontSize = 14;
            badgeText.fontStyle = FontStyle.Bold;
            badgeText.alignment = TextAnchor.MiddleCenter;
            badgeText.color = badgeColor;
            RectTransform badgeRect = badgeObj.GetComponent<RectTransform>();
            badgeRect.anchorMin = new Vector2(0, 0.88f);
            badgeRect.anchorMax = Vector2.one;
            badgeRect.offsetMin = new Vector2(4, 0);
            badgeRect.offsetMax = new Vector2(-4, 0);
        }

        // Title label
        GameObject label = new GameObject("Label");
        label.transform.SetParent(card.transform, false);
        Text labelText = label.AddComponent<Text>();
        labelText.text = upgradeName;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.fontSize = tier > 0 ? 19 : 24;
        labelText.fontStyle = FontStyle.Bold;
        labelText.alignment = TextAnchor.UpperCenter;
        labelText.color = titleColor;
        RectTransform labelRect = label.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, tier > 0 ? 0.52f : 0.6f);
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = new Vector2(8, 0);
        labelRect.offsetMax = new Vector2(-8, tier > 0 ? -4 : 0);

        // Description label
        GameObject descObj = new GameObject("Desc");
        descObj.transform.SetParent(card.transform, false);
        Text descText = descObj.AddComponent<Text>();
        descText.text = desc;
        descText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        descText.fontSize = 17;
        descText.alignment = TextAnchor.UpperCenter;
        descText.color = new Color(0.75f, 0.75f, 0.75f);
        RectTransform descRect = descObj.GetComponent<RectTransform>();
        descRect.anchorMin = Vector2.zero;
        descRect.anchorMax = new Vector2(1f, 0.5f);
        descRect.offsetMin = new Vector2(8, 8);
        descRect.offsetMax = new Vector2(-8, 0);
    }

    void SelectUpgrade(string upgradeName)
    {
        GameObject player = GameObject.FindWithTag("Player");

        switch (upgradeName)
        {
            case "Fleet of Shadow":
                PlayerMovement pm = player.GetComponent<PlayerMovement>();
                if (pm) pm.speed *= 1.2f;
                break;
            case "Edge of Ruin":
                AutoAttack aa = player.GetComponent<AutoAttack>();
                if (aa) aa.baseDamage += 10;
                break;
            case "Iron Flesh":
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph) { ph.maxHP += 50; ph.currentHP += 50; }
                break;
            case "Fevered Volley":
                AutoAttack aa2 = player.GetComponent<AutoAttack>();
                if (aa2) aa2.fireRate *= 0.8f;
                break;
            case "Barrage":
                AutoAttack aa3 = player.GetComponent<AutoAttack>();
                if (aa3) aa3.projectileCount += 2;
                break;
            case "Dark Ward":
                PlayerShield shield = player.GetComponent<PlayerShield>();
                if (shield == null) shield = player.AddComponent<PlayerShield>();
                shield.AddCharge();
                break;
            case "Blood Drinker":
                VampireAura vamp = player.GetComponent<VampireAura>();
                if (vamp == null) player.AddComponent<VampireAura>();
                break;
            case "Shattered Bolt":
                AutoAttack aa4 = player.GetComponent<AutoAttack>();
                if (aa4) aa4.spreadShot = true;
                break;
            case "Hellfire":
                AutoAttack aa5 = player.GetComponent<AutoAttack>();
                if (aa5) aa5.explosive = true;
                explosiveTaken = true;
                break;
            case "💥 INFERNAL SURGE 💥":
                AutoAttack aa7 = player.GetComponent<AutoAttack>();
                if (aa7) aa7.explosiveDoubleDamage = true;
                explosiveDoubleTaken = true;
                break;
            case "🎯 SOUL SEEKER 🎯":
                AutoAttack aa8 = player.GetComponent<AutoAttack>();
                if (aa8) aa8.lockOn = true;
                lockOnTaken = true;
                break;
            case "⚡ WRATH STRIKE ⚡":
                AutoAttack aa6 = player.GetComponent<AutoAttack>();
                if (aa6) aa6.autoKnockback = true;
                autoKnockbackTaken = true;
                break;
        }

        Destroy(panel);
        Time.timeScale = 1f;
        isOpen = false;
    }
}
