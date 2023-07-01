using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveGenerator : MonoBehaviour
{
    public Image imageTarget;
    public Vector2 dimensions = new Vector2(128, 32);
    public float lineTolerance = 0.08f;

    private Texture2D m_texture;
    private Sprite m_sprite;


    public float GetValueAt(float x)
    {
        float raw = (x / 10f) % 1f;
        return (raw - 0.5f) * 2f;
    }


    private void Awake()
    {
        m_texture = new Texture2D((int)dimensions.x, (int)dimensions.y);
        m_texture.filterMode = FilterMode.Point;
        m_texture.alphaIsTransparency = true;
        m_sprite = Sprite.Create(m_texture, new Rect(0, 0, m_texture.width, m_texture.height), Vector2.zero);
    }

    private void Update()
    {
        int width = (int)dimensions.x;
        int height = (int)dimensions.y;

        for(int x = 0; x < width; x++)
            for(int y = 0; y < height; y++)
            {
                float halfH = height / 2;
                float trueY = (y - halfH) / halfH;
                float atX = GetValueAt(x);

                if (Mathf.Abs(trueY - atX) < lineTolerance)
                    m_texture.SetPixel(x, y, Color.white);
                else m_texture.SetPixel(x, y, Color.clear);
            }
        m_texture.Apply();

        imageTarget.sprite = m_sprite;
    }
}
