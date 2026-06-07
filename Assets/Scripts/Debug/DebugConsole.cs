using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.InputSystem;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI consoleText;
    [SerializeField] private int maxLogs = 5;

    private Queue<string> logQueue = new Queue<string>();
    private float lastTapTime = 0f;
    private const float doubleTapThreshold = 0.3f;
    private bool isConsoleVisible = true;

    private void Start()
    {
        if (consoleText == null)
        {
            consoleText = GetComponent<TextMeshProUGUI>();
        }
        UpdateConsoleText();
    }

    private void Update()
    {
        bool tapped = false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            tapped = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            tapped = true;
        }

        if (tapped)
        {
            if (Time.unscaledTime - lastTapTime < doubleTapThreshold)
            {
                ToggleConsole();
                lastTapTime = 0f; // Reset to avoid triple taps registering as two double taps
            }
            else
            {
                lastTapTime = Time.unscaledTime;
            }
        }
    }

    private void ToggleConsole()
    {
        isConsoleVisible = !isConsoleVisible;
        if (consoleText != null)
        {
            consoleText.enabled = isConsoleVisible;
        }
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string colorHex = "#FFFFFF"; // Default white

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                colorHex = "#FF0000"; // Red
                break;
            case LogType.Warning:
                colorHex = "#FFFF00"; // Yellow
                break;
            case LogType.Log:
                colorHex = "#FFFFFF"; // White
                break;
        }

        string formattedLog = $"<color={colorHex}>{logString}</color>";
        logQueue.Enqueue(formattedLog);

        while (logQueue.Count > maxLogs)
        {
            logQueue.Dequeue();
        }

        UpdateConsoleText();
    }

    private void UpdateConsoleText()
    {
        if (consoleText == null) return;

        if (logQueue.Count == 0)
        {
            consoleText.text = "No logs yet...";
            return;
        }

        StringBuilder sb = new StringBuilder();
        foreach (string log in logQueue)
        {
            sb.AppendLine(log);
        }
        
        consoleText.text = sb.ToString();
    }
}
