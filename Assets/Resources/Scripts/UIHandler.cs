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

    private void OnSweetSpotHit(object foo)
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(AnimateSweetspot());
        }
    }
    private IEnumerator AnimateSweetspot()
    {
        yield return CoroutineBuilder.Linear01(t =>
        {
            sweetspotImage.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
            sweetspotImage.color = Color.Lerp(new Color(1, 1, 1, 0.75f), new Color(1, 1, 1, 4), t);
        }, speed: 20f);
        yield return CoroutineBuilder.Linear01(t =>
        {
            sweetspotImage.transform.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.one, Mathf.Sqrt(t));
            sweetspotImage.color = Color.Lerp(new Color(1, 1, 1, 4), new Color(1, 1, 1, 0), Mathf.Sqrt(t));
        }, speed: 2f);
        yield return new WaitForSeconds(0.1f);
        isAnimating = false;
    }
}        