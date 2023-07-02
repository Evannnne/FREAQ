using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public AudioClip testClip;
    public void Start()
    {
        float[] samplebuffer = new float[testClip.samples];
        testClip.LoadAudioData();
        testClip.GetData(samplebuffer, 0);
        for (int i = 0; i < Mathf.Min(samplebuffer.Length, 10000); i++)
            Debug.Log(samplebuffer[i]);
    }
}
