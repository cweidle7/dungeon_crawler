using UnityEngine;

public static class SpriteHelper
{
    private static Sprite _square;
    private static Sprite _circle;

    public static Sprite Square
    {
        get
        {
            if (_square == null)
            {
                Texture2D tex = new Texture2D(4, 4);
                Color[] pixels = new Color[16];
                for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
                tex.SetPixels(pixels);
                tex.Apply();
                _square = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4);
            }
            return _square;
        }
    }

    public static Sprite Circle
    {
        get
        {
            if (_circle == null)
            {
                int size = 64;
                Texture2D tex = new Texture2D(size, size);
                Vector2 center = new Vector2(size / 2f, size / 2f);
                float radius = size / 2f;
                for (int x = 0; x < size; x++)
                    for (int y = 0; y < size; y++)
                    {
                        float dist = Vector2.Distance(new Vector2(x, y), center);
                        float alpha = dist < radius ? 1f - (dist / radius) * 0.5f : 0f;
                        tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                    }
                tex.Apply();
                _circle = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            }
            return _circle;
        }
    }
}
