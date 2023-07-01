using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveGenerator : MonoBehaviour
{
    public static bool IsInSweetspot { get; private set; }

    public Image imageTarget;
    public Vector2 dimensions = new Vector2(128, 32);
    public float lineTolerance = 0.08f;

    public float checkInterval = 0.05f;
    public float checkSamples = 10;

    public float sweetSpotThreshold = 0.9f;

    public float speed = 10f;
    public float scale = 1f;

    private Texture2D m_texture;
    private Sprite m_sprite;

    private Color[] m_colorBuffer;

    public float GetValueAt(float x)
    {
        x *= scale;
        x += Time.time * speed;

        return Mathf.Sin(x);
    }


    private void Awake()
    {
        m_texture = new Texture2D((int)dimensions.x, (int)dimensions.y);
        m_texture.filterMode = FilterMode.Point;
        m_texture.alphaIsTransparency = true;
        m_sprite = Sprite.Create(m_texture, new Rect(0, 0, m_texture.width, m_texture.height), Vector2.zero);
        m_colorBuffer = new Color[m_texture.width * m_texture.height];
    }

    private void Update()
    {
        HandleTexture();
        HandleSweetspot();
    }
    private void HandleTexture()
    {
        int width = (int)dimensions.x;
        int height = (int)dimensions.y;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Color targetColor;

                float halfH = height / 2;
                float trueY = (y - halfH) / halfH * 2;
                float trueX = (float)x / width;
                float atX = GetValueAt(trueX);

                float diff = Mathf.Abs(trueY - atX);
                if (diff < lineTolerance)
                    targetColor = Color.white;
                else if (diff < lineTolerance * 2)
                    targetColor = new Color(1, 1, 1, 0.2f);
                else if (diff < lineTolerance * 4)
                    targetColor = new Color(1, 1, 1, 0.1f);
                else targetColor = Color.clear;

                if (x < width / 2) targetColor.a *= 0.25f;

                //if (targetColor != Color.white && Mathf.Abs(trueY - GetValueAt(0.5f)) < lineTolerance / 2f)
                //    targetColor = new Color(0.5f, 0, 0, 1);

                m_colorBuffer[y * m_texture.width + x] = targetColor;
            }
        m_texture.SetPixels(m_colorBuffer);
        m_texture.Apply();
        imageTarget.sprite = m_sprite;
    }
    private void HandleSweetspot()
    {
        bool sweetSpot = false;
        for(int i = 0; i <= checkSamples; i++)
        {
            for (int s = -1; s <= 1; s += 2)
            {
                if(GetValueAt(0.5f + checkInterval / checkSamples * i * s) >= sweetSpotThreshold)
                {
                    sweetSpot = true;
                    break;
                }    
            }
        }
        IsInSweetspot = sweetSpot;
    }
}
