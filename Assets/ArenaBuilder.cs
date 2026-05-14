using UnityEngine;

public class ArenaBuilder : MonoBehaviour
{
    public float arenaSize = 30f;
    public Color floorColor = new Color(0.06f, 0.06f, 0.09f);
    public Color wallColor = new Color(0.04f, 0.03f, 0.06f);

    void Start()
    {
        BuildFloor();
        BuildWalls();
        BuildPillars();
        BuildTorches();
    }

    void BuildFloor()
    {
        int tiles = Mathf.CeilToInt(arenaSize * 2);
        for (int x = -tiles / 2; x < tiles / 2; x++)
        {
            for (int y = -tiles / 2; y < tiles / 2; y++)
            {
                GameObject tile = new GameObject("Tile");
                tile.transform.SetParent(transform);
                tile.transform.position = new Vector3(x + 0.5f, y + 0.5f, 1f);

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteHelper.Square;
                float shade = ((x + y) % 2 == 0) ? 0f : 0.02f;
                sr.color = new Color(
                    floorColor.r + shade,
                    floorColor.g + shade,
                    floorColor.b + shade);
                sr.sortingOrder = -1;
            }
        }
    }

    void BuildWalls()
    {
        float half = arenaSize;
        float thickness = 1f;

        CreateWall(new Vector2(0, half + thickness / 2f), new Vector2(half * 2 + thickness * 2, thickness));
        CreateWall(new Vector2(0, -half - thickness / 2f), new Vector2(half * 2 + thickness * 2, thickness));
        CreateWall(new Vector2(half + thickness / 2f, 0), new Vector2(thickness, half * 2));
        CreateWall(new Vector2(-half - thickness / 2f, 0), new Vector2(thickness, half * 2));
    }

    void BuildPillars()
    {
        // Inner cross — close-range cover
        Vector2[] inner = { new Vector2(8,0), new Vector2(-8,0), new Vector2(0,8), new Vector2(0,-8) };
        foreach (var pos in inner) CreatePillar(pos, 1.5f);

        // Mid-ring — diagonal corners
        Vector2[] mid = {
            new Vector2(14, 14), new Vector2(-14, 14),
            new Vector2(14,-14), new Vector2(-14,-14)
        };
        foreach (var pos in mid) CreatePillar(pos, 2f);
    }

    void CreatePillar(Vector2 pos, float size)
    {
        GameObject pillar = new GameObject("Pillar");
        pillar.transform.SetParent(transform);
        pillar.transform.position = pos;
        pillar.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = pillar.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Square;
        float v = Random.Range(0f, 0.02f);
        sr.color = new Color(0.13f + v, 0.1f + v, 0.16f + v);
        sr.sortingOrder = 0;

        // Cap
        CreatePillarDetail(pillar.transform, new Vector2(0f, 0.52f), new Vector3(1.18f, 0.17f, 1f),
            new Color(0.2f, 0.15f, 0.25f));
        // Base
        CreatePillarDetail(pillar.transform, new Vector2(0f, -0.52f), new Vector3(1.18f, 0.17f, 1f),
            new Color(0.2f, 0.15f, 0.25f));

        pillar.AddComponent<BoxCollider2D>();
    }

    void CreatePillarDetail(Transform parent, Vector2 localPos, Vector3 localScale, Color color)
    {
        GameObject detail = new GameObject("Detail");
        detail.transform.SetParent(parent);
        detail.transform.localPosition = localPos;
        detail.transform.localScale = localScale;
        SpriteRenderer sr = detail.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Square;
        sr.color = color;
        sr.sortingOrder = 1;
    }

    void BuildTorches()
    {
        // Wall midpoints and corners — purely atmospheric
        Vector2[] torchPos = {
            new Vector2(27, 0), new Vector2(-27, 0),
            new Vector2(0, 27), new Vector2(0,-27),
            new Vector2(25, 25), new Vector2(-25, 25),
            new Vector2(25,-25), new Vector2(-25,-25)
        };
        foreach (var pos in torchPos) CreateTorch(pos);
    }

    void CreateTorch(Vector2 pos)
    {
        GameObject torch = new GameObject("Torch");
        torch.transform.SetParent(transform);
        torch.transform.position = pos;
        torch.transform.localScale = Vector3.one * 0.45f;

        SpriteRenderer sr = torch.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Circle;
        sr.sortingOrder = 2;

        GoldShimmer shimmer = torch.AddComponent<GoldShimmer>();
        shimmer.colorA = new Color(0.9f, 0.35f, 0.05f);
        shimmer.colorB = new Color(1f, 0.75f, 0.2f);
        shimmer.speed = 5f;
    }

    void CreateWall(Vector2 pos, Vector2 size)
    {
        GameObject wall = new GameObject("Wall");
        wall.transform.SetParent(transform);
        wall.transform.position = pos;

        SpriteRenderer sr = wall.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteHelper.Square;
        sr.color = wallColor;
        sr.sortingOrder = 0;
        wall.transform.localScale = new Vector3(size.x, size.y, 1f);

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
    }
}
