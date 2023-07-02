using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Image sweetspotImage;

    private bool isAnimating = false;

    private void Awake()
    {
        EventHandler.Subscribe("SweetspotHit", OnSweetSpotHit);
        EventHandler.Subscribe("SweetspotMissed", OnSweetSpotMissed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating && PlayerController.Instance.CanClick)
        {
            Color target = WaveGenerator.IsInSweetspot ? new Color(1, 1, 1, 0.50f) : Color.clear;
            sweetspotImage.color = Color.Lerp(sweetspotImage.color, target, Time.deltaTime * 10);
        }
    }

    private Coroutine m_currentCoroutine;
    private void OnSweetSpotHit(object foo)
    {
        if (m_currentCoroutine != null) StopCoroutine(m_currentCoroutine);
        m_currentCoroutine = StartCoroutine(AnimateSweetspot(true));
    }
    private void OnSweetSpotMissed(object foo)
    {
        if (m_currentCoroutine != null) StopCoroutine(m_currentCoroutine);
        m_currentCoroutine = StartCoroutine(AnimateSweetspot(false));
    }
    private IEnumerator AnimateSweetspot(bool success)
    {
        float t = 0;

        Color targetColor = success ? new Color(1, 1, 1, 4) : new Color(1, 0, 0, 1);
        while(t <= 1)
        {
            sweetspotImage.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
            sweetspotImage.color = Color.Lerp(new Color(1, 1, 1, 0.75f), targetColor, t);
            t += Time.deltaTime * 20;
            yield return null;
        }
        t = 0;
        while (t <= 1)
        {
            sweetspotImage.transform.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.one, Mathf.Sqrt(t));
            sweetspotImage.color = Color.Lerp(targetColor, new Color(1, 1, 1, 0), Mathf.Sqrt(t));
            t += Time.deltaTime * 2;
            yield return null;
        }
    }
}        