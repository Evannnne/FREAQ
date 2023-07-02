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

    public AudioSource musicSource;
    public float currentTime;

    private Texture2D m_texture;
    private Sprite m_sprite;

    private Color[] m_colorBuffer;

    public float GetValueAt(float x)
    {
        float pos = x + musicSource.time * 2;
        pos = Mathf.Max(0, pos);
        int index = (int)(pos * frequency) % sampleBuffer.Length;
        float sample = sampleBuffer[index];
        return sample;
    }


    private void Awake()
    {
        m_texture = new Texture2D((int)dimensions.x, (int)dimensions.y);
        m_texture.filterMode = FilterMode.Point;
        m_sprite = Sprite.Create(m_texture, new Rect(0, 0, m_texture.width, m_texture.height), Vector2.zero);
        m_colorBuffer = new Color[m_texture.width * m_texture.height];
    }

    private float[] sampleBuffer;
    private float frequency;
    private float max;
    private void Start()
    {
        sampleBuffer = new float[musicSource.clip.samples];
        musicSource.clip.LoadAudioData();
        musicSource.clip.GetData(sampleBuffer, 0);

        List<int> points = new List<int>();

        // Find kicks/snares
        int num0s = 0;
        for(int i = 0; i < sampleBuffer.Length; i++)
        {
            if (sampleBuffer[i] <= 0.3f) num0s++;
            else
            {
                if (num0s > 1500)
                {
                    points.Add(i);
                }
                num0s = 0;
            }
        }

        frequency = musicSource.clip.frequency;

        // Flush values
        for (int i = 0; i < sampleBuffer.Length; i++)
            sampleBuffer[i] = -1f;

        // Create waves
        foreach(int p in points)
        {
            float fac = 0.2f;
            for(int j = 0; j < frequency * fac * 2; j++)
            {
                for(int s = -1; s <= 1; s += 2)
                {
                    float pt = (float)j / (frequency * fac);
                    pt *= Mathf.PI / 2;
                    int index = p + j * s;
                    if (index >= 0 && index < sampleBuffer.Length)
                    {

                        sampleBuffer[index] = Mathf.Max(Mathf.Cos(pt), sampleBuffer[index]);
                        sampleBuffer[index] = Mathf.Clamp(sampleBuffer[index], -1, 1);
                    }
                }
            }
        }
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
                float halfW = width / 2;
                float trueY = (y - halfH) / halfH * 2;
                float trueX = (x - halfW) / halfW * 2;
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

                if (targetColor != Color.white && Mathf.Abs(trueY - GetValueAt(0)) < lineTolerance / 2f)
                    targetColor = new Color(0.5f, 0, 0, 1);

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
                if(GetValueAt(0 + checkInterval / checkSamples * i * s) >= sweetSpotThreshold)
                {
                    sweetSpot = true;
                    break;
                }    
            }
        }
        IsInSweetspot = sweetSpot;
    }
}
