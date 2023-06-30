using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugText : MonoBehaviour
{
    const int lines = 25;
    public TextMeshProUGUI LogText, ParamsText;
    public static DebugText Instance;
    Dictionary<string, string> debugParameters= new Dictionary<string, string>();
    string[] logBuffer;
    public int paramUpdateCalls, logUpdateCalls;
    Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();
    void Awake()
    {
        Instance = this;
        if (Instance == null) return;
        logBuffer = new string[lines];
        ClearAll();
    }
    void Update()
    {
        if (Instance == null) return;
        //if nothing is asking to update the list, just dont
        if (paramUpdateCalls > 0)
        {
            paramUpdateCalls = 0;

            ParamsText.text = "";
            // print the parameters in sequence
            foreach (var item in debugParameters)
            {
                ParamsText.text += item.Key + ": " + item.Value;
                ParamsText.text += '\n';
            }
        }
        if (logUpdateCalls > 0)
        {
            logUpdateCalls = 0;

            LogText.text = "";
            // print the logLines in sequence
            for (int i = 0; i < lines; i++)
            {
                LogText.text += logBuffer[i] + '\n';
            }
        }
    }
    public static void Log(object message)
    {
        if (Instance == null) return;
        for (int i = 0; i < lines - 1; i++)
        {
            Instance.logBuffer[i] = Instance.logBuffer[i + 1];
        }
        Instance.logBuffer[lines - 1] = message + "";
        Instance.logUpdateCalls++;
    }
    public static void AddParam(string paramName)
    {
        if (Instance == null) return;
        if (!Instance.debugParameters.ContainsKey(paramName) || Instance.debugParameters.Count < 23)
        {
            Instance.debugParameters.Add(paramName, "");
            Instance.paramUpdateCalls++;
        }
        else
        {
            Debug.LogError("Debug parameters already has key " + paramName + " or too many params.");
        }
    }

    public static void UpdateParam(string paramName, object value)
    {
        if (Instance == null) return;
        if (Instance.debugParameters.ContainsKey(paramName))
        {
            //dont bother updating the text if the parameter hasnt changed
            if (value.Equals(Instance.debugParameters[paramName]))
                return;
            Instance.debugParameters[paramName] = value + "";
            Instance.paramUpdateCalls++;
        }
        else
        {
            AddParam(paramName);
            UpdateParam(paramName, value);
        }
    }
    public static void RemoveParam(string paramName)
    {
        if (Instance == null) return;
        Instance.debugParameters.Remove(paramName);
        Instance.paramUpdateCalls++;
    }
    void ClearAll()
    {
        if (Instance == null) return;
        LogText.text = "";
        ParamsText.text = "";
    }

    public static void AddPoint(string pointName)
    {
        if (Instance == null) return;
        if (!Instance.points.ContainsKey(pointName) || Instance.debugParameters.Count < 23)
        {
            Instance.points.Add(pointName, Vector3.zero);
        }
        else
        {
            Debug.LogError("Debug points already has key " + pointName + " or too many params.");
        }
    }

    public static void UpdatePoint(string pointName, Vector3 point)
    {
        if (Instance == null) return;
        if (Instance.points.ContainsKey(pointName))
        {
            //dont bother updating the text if the parameter hasnt changed
            if (point.Equals(Instance.points[pointName]))
                return;
            Instance.points[pointName] = point;
        }
        else
        {
            AddPoint(pointName);
            UpdatePoint(pointName, point);
        }
    }

    public static void RemovePoint(string pointName)
    {
        if (Instance == null) return;
        Instance.points.Remove(pointName);
    }

    void OnDrawGizmos()
    {
        foreach (KeyValuePair<string, Vector3> entry in points)
        {
            Gizmos.DrawSphere(entry.Value,.1f);
        }
    }
}