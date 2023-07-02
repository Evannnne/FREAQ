using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    private static Dictionary<string, List<Action<object>>> s_lookup = new Dictionary<string, List<Action<object>>>();
    public static void TriggerEvent(string name, object parameter = null)
    {
        if (s_lookup.ContainsKey(name))
            foreach (var action in s_lookup[name])
                action.Invoke(parameter);
    }
    public static void Subscribe(string eventName, Action<object> function)
    {
        if (s_lookup.ContainsKey(eventName))
            s_lookup[eventName].Add(function);
        else s_lookup.Add(eventName, new List<Action<object>>() { function });
    }
    public static void Unsubscribe(string eventName, Action<object> function)
    {
        if (s_lookup.ContainsKey(eventName))
            s_lookup[eventName].Remove(function);
    }

    public static void ClearEventListeners() => s_lookup.Clear();
}