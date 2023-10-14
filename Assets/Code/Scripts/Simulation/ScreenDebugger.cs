using System.Collections.Generic;
using Assets.Code.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenDebugger : MonoBehaviour
{
    uint queueSize = 40;

    GUIStyle labelStyle;

    string input;

    Vector2 scroll;

    public void OnReturn()
    {
        CommandManager.HandleCommand(input);
        input = "";
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("The simulation starts");

        labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.white;
        labelStyle.fontSize = 16;

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
        SimulationManager.logMsgQueue.Enqueue($"[{type}]: {logString}");

        if (type == LogType.Exception)
        {
            SimulationManager.logMsgQueue.Enqueue(stackTrace);
        }

        while (SimulationManager.logMsgQueue.Count > queueSize)
        {
            SimulationManager.logMsgQueue.Dequeue();
        }
    }

    private void OnGUI()
    {

        if (SimulationManager.logMsgQueue.Count != 0)
        {
            GUI.Box(new Rect(Screen.width - 820, Screen.height - 355, 800, 300), "");

            Rect viewport = new(Screen.width - 830, 0, 790, 20 * SimulationManager.logMsgQueue.Count);

            scroll = GUI.BeginScrollView(new Rect(Screen.width - 815, Screen.height - 350, 795, 290), scroll, viewport);

            object[] logMsgArray = SimulationManager.logMsgQueue.ToArray();

            for (int i = 0; i <= logMsgArray.Length - 1; i++)
            {
                string msg = $"|> {logMsgArray[i]}";

                Rect labelRect = new(Screen.width - 825, 20 * i, viewport.width - 10, 20);

                GUI.Label(labelRect, msg, labelStyle);
            }

            GUI.EndScrollView();

        }

        GUI.Box(new Rect(Screen.width - 820, Screen.height - 50, 800, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0.25f);
        input = GUI.TextField(new Rect(Screen.width - 815, Screen.height - 45, 790, 25f), input, labelStyle);

    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            OnReturn();
        }
    }
}
