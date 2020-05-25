using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug : MonoBehaviour
{
    #region static variables
    private static UIDebug instance;
    public static UIDebug Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIDebug>();
            }
            return instance;
        }
    }
    #endregion static variables

    private List<LogMessage> ListDebugLogMessagesToDisplay = new List<LogMessage>();

    [Header("UI Components")]
    public Text TextDebugLog;

    private void Awake()
    {
        instance = this;
        UpdateLogMessageTextComponent();
    }

    private void Start()
    {
        StartCoroutine(CoroutineUpdateDebugLogText());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameDebugger.ActivateDeactivateDebugSetting(GameDebugger.DebugSetting.DRAW_COLLISION_BOXES);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameDebugger.ActivateDeactivateDebugSetting(GameDebugger.DebugSetting.DRAW_HITBOXES);
        }

        for (int i = ListDebugLogMessagesToDisplay.Count - 1; i >= 0; --i)
        {
            if (ListDebugLogMessagesToDisplay[i].TimeRemainingBeforeClear < 0) RemoveLogMessage(ListDebugLogMessagesToDisplay[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Message"></param>
    /// <param name="TimeToDisplayInSeconds"></param>
    public void AddLogMessage(string Message, float TimeToDisplayInSeconds = .01f)
    {
        LogMessage Log = new LogMessage();
        Log.Message = Message;
        Log.TimeRemainingBeforeClear = TimeToDisplayInSeconds;
        ListDebugLogMessagesToDisplay.Add(Log);
    }

    private void RemoveLogMessage(LogMessage LogToRemove)
    {
        ListDebugLogMessagesToDisplay.Remove(LogToRemove);
        UpdateLogMessageTextComponent();
    }

    private void UpdateLogMessageTextComponent()
    {
        string FullMessageToDisplay = "";
        foreach (LogMessage Log in ListDebugLogMessagesToDisplay)
        {
            FullMessageToDisplay += Log.Message + '\n';
        }
        TextDebugLog.text = FullMessageToDisplay;
    }

    private IEnumerator CoroutineUpdateDebugLogText()
    {
        while (true)
        {
            foreach(LogMessage Log in ListDebugLogMessagesToDisplay)
            {
                Log.TimeRemainingBeforeClear -= GameOverseer.DELTA_TIME;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private class LogMessage
    {
        public float TimeRemainingBeforeClear;
        public string Message;
    }
}
