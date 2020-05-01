﻿using UnityEngine;
using System;
using System.Text;

public class OnScreenLogger : MonoBehaviour
{
    private struct LogEntry
    {
        public string Text { get; private set; }
        public LogType Type { get; private set; }
        public float KillTime { get; private set; }

        public void SetLogEntry(string text, LogType logType, float duration = 1.0f)
        {
            Text = text;
            Type = logType;
            KillTime = Time.time + duration;
        }

        public void Reset()
        {
            KillTime = -1.0f;
        }
    }

    [Header("Settings")]
    [SerializeField, Tooltip("Max amount of log entries.")]
    private int capacity = 8;

    [SerializeField, Tooltip("In seconds.")]
    private float onScreenDuration = 1.0f;

    public bool showLogType = true;
    public bool showStackTrace = false;

    [Header("Colors")]
    #region Colors
    public Color backgroundColor = Color.grey;
    public Color textColor = Color.white;
    public Color logColor = Color.white;
    public Color warningColor = Color.yellow;
    public Color errorColor = Color.red;
    public Color exceptionColor = Color.red;
    #endregion Colors

    private GUIStyle backgroundStyle = new GUIStyle();
    private LogEntry[] _logEntries;

    private void Awake()
    {
        backgroundStyle.normal.background = Texture2D.whiteTexture;
        backgroundStyle.richText = true;
        _logEntries = new LogEntry[capacity];
    }

    private void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }
    
    private void OnGUI()
    {
        //if(GUI.Button(new Rect(x: 5.0f, y: 5.0f, width: 100.0f, height: 100.0f), text: "Click me"))
        //{
        //    Debug.Log(message: "clicked");
        //}

        Color previousColor = GUI.contentColor;

        GUI.backgroundColor = backgroundColor;
        GUILayout.BeginVertical(backgroundStyle);
        for(int i = 0; i < _logEntries.Length; i++)
        {
            if(_logEntries[i].KillTime < Time.time)
            {
                continue;
            }
            GUILayout.Label(_logEntries[i].Text);
        }
        GUILayout.EndVertical();
        GUI.contentColor = previousColor;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (_logEntries[_logEntries.Length - 1].KillTime > Time.time)
        {
            _logEntries[0].Reset();
        }

        Array.Sort(_logEntries, SortEntryComparison);
        int index = Array.FindIndex(_logEntries, AvailableEntryComparison);

        BuildString(logString, stackTrace, type, out string text);

        _logEntries[index].SetLogEntry(text, type, onScreenDuration);
    }

    private void BuildString(in string logString, in string stackTrace, in LogType type, out string finalText)
    {
        StringBuilder text = new StringBuilder();
        if (showLogType)
        {
            text.Append("<color=#");
            text.Append(ColorUtility.ToHtmlStringRGBA(GetLogTypeColor(type)));
            text.Append(">");
            text.Append(type.ToString());
            text.Append(": </color>");
            text.Append("\t");
        }

        text.Append("<color=#");
        text.Append(ColorUtility.ToHtmlStringRGBA(textColor));
        text.Append(">");
        text.Append(logString);

        if (showStackTrace)
        {
            text.Append(logString);
            text.Append(stackTrace);
        }
        else
        {
            text.Append(logString);
        }

        text.Append("</color>");
       
        finalText = text.ToString();
    }

    private Color GetLogTypeColor(in LogType type)
    {
        switch(type)
        {
            case LogType.Log:
                return logColor;
            case LogType.Warning:
                return warningColor;
            case LogType.Error:
                return errorColor;
            default:
                return exceptionColor;
        }
    }

    private static bool AvailableEntryComparison(LogEntry entry)
    {
        return entry.KillTime < Time.time;
    }

    //Reverse
    private static int SortEntryComparison(LogEntry a, LogEntry b)
    {
        if(a.KillTime < Time.time)
        {
            if(b.KillTime < Time.time)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if(b.KillTime < Time.time)
            {
                return -1;
            }
            else
            {
                return a.KillTime.CompareTo(b.KillTime);
            }
        }
    }
}
