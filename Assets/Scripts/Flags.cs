using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags
{
    static Dictionary<string, bool> flags = new Dictionary<string, bool>();
    static Dictionary<string, List<System.Action<bool>>> listeners = new Dictionary<string, List<System.Action<bool>>>();

    public static void AddListener(string flag, System.Action<bool> listener)
    {
        if (!listeners.ContainsKey(flag))
            listeners[flag] = new List<System.Action<bool>>();
        listeners[flag].Add(listener);
    }

    public static void RemoveListener(string flag, System.Action<bool> listener)
    {
        if (listeners.ContainsKey(flag))
            listeners[flag].Remove(listener);
    }

    public static void Add(string flag, bool value = false)
    {
        flags.Add(flag, false);
    }
    public static void Update(string flag, bool value)
    {
        if (!flags.ContainsKey(flag)) Add(flag, value);
        if (!flags[flag] == value)
        {
            flags[flag] = value;
            NotifyListeners(flag, value);
            DebugText.Log(flag + "->" + value);
        }
    }
    public static bool Get(string flag)
    {
        return flags[flag];
    }
    public static void Remove(string flag)
    {
        flags.Remove(flag);
        listeners.Remove(flag);
    }

    private static void NotifyListeners(string flag, bool value)
    {
        if (listeners.ContainsKey(flag))
        {
            foreach (System.Action<bool> listener in listeners[flag])
            {
                DebugText.Log("notify listeners");
                listener(value);
            }
        }
    }
}