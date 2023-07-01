using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CoroutineBuilder
{
    private static UnityEngine.UI.Image s_coroutineInvoker;

    public static Coroutine Linear01(Action<float> onIteration, float speed = 1, bool useFixedUpdate = false, bool ignoreTimescale = false)
    {
        return StartCoroutine(_Linear01(onIteration, speed, useFixedUpdate, ignoreTimescale));
    }
    public static Coroutine StartCoroutine(IEnumerator target)
    {
        if (s_coroutineInvoker == null)
        {
            s_coroutineInvoker = new GameObject("StaticCoroutineInvoker", typeof(UnityEngine.UI.Image))
                                .GetComponent<UnityEngine.UI.Image>();
            s_coroutineInvoker.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
        return s_coroutineInvoker.StartCoroutine(target);
    }
    private static IEnumerator _Linear01(Action<float> onIteration, float speed = 1, bool useFixedUpdate = false, bool ignoreTimescale = false)
    {
        float t = 0;
        while (t < 1)
        {
            onIteration.Invoke(t);
            t += (ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime) * speed;

            if (useFixedUpdate) yield return new WaitForFixedUpdate();
            else yield return null;
        }
        onIteration.Invoke(1);
        if (useFixedUpdate) yield return new WaitForFixedUpdate();
        else yield return null;
    }


}