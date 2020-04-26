using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Script that handles all the player controlled input for our game
/// </summary>
public class PlayerController : MonoBehaviour
{
    public enum ButtonInputID
    {
        JUMP_BUTTON,
        ATTACK_BUTTON,
        PAUSE_GAME,
    }

    #region const variables
    public const KeyCode JUMP_ACTION = KeyCode.Space;
    public const KeyCode ATTACK_ACTION = KeyCode.J;

    public const string HORIZONTAL_AXIS = "MoveLeftRight";
    public const string VERTICAL_AXIS = "MoveUpDown";
    #endregion const variables

    private Dictionary<KeyCode, UnityAction> ButtonPressedActionEventDictionary = new Dictionary<KeyCode, UnityAction>();
    private Dictionary<KeyCode, UnityAction> ButtonReleasedActionEventDictionary = new Dictionary<KeyCode, UnityAction>();
    private Dictionary<string, UnityAction<float>> AxisEventDictionary = new Dictionary<string, UnityAction<float>>();



    #region monobehaviour methods
    private void Awake()
    {
        
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, UnityAction> ButtonPressedEvents in ButtonPressedActionEventDictionary)
        {
            if (Input.GetKeyDown(ButtonPressedEvents.Key))
            {
                ButtonPressedEvents.Value.Invoke();
            }
        }

        foreach (KeyValuePair<KeyCode, UnityAction> ButtonReleasedEvent in ButtonReleasedActionEventDictionary)
        {
            if (Input.GetKeyUp(ButtonReleasedEvent.Key))
            {
                ButtonReleasedEvent.Value.Invoke();
            }
        }

        foreach (KeyValuePair<string, UnityAction<float>> AxisEvent in AxisEventDictionary)
        {
            AxisEvent.Value.Invoke(Input.GetAxisRaw(AxisEvent.Key));
        }
    }
    #endregion monobehaviour methods


    /// <summary>
    /// 
    /// </summary>
    /// <param name="InputName"></param>
    /// <param name="IsPressedEvent"></param>
    /// <param name="FunctionToPerform"></param>
    public void BindInputToAction(KeyCode KeyCodeToBind, bool IsPressedEvent, UnityAction FunctionToPerform)
    {
        if (IsPressedEvent)
        {
            BindInputActionHelper(ButtonPressedActionEventDictionary, KeyCodeToBind, FunctionToPerform);   
        }
        else
        {
            BindInputActionHelper(ButtonReleasedActionEventDictionary, KeyCodeToBind, FunctionToPerform);
        }
    }

    private void BindInputActionHelper(Dictionary<KeyCode, UnityAction> DictionaryToBindInputEventTo, KeyCode KeyCodeToBind, UnityAction FunctionToPerform)
    {
        if (!DictionaryToBindInputEventTo.ContainsKey(KeyCodeToBind))
        {
            DictionaryToBindInputEventTo.Add(KeyCodeToBind, null);
        }
        DictionaryToBindInputEventTo[KeyCodeToBind] += FunctionToPerform;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="InputName"></param>
    /// <param name="IsPressedEvent"></param>
    public void UnbindActionFromUntityInputEvent(KeyCode KeyCodeToUnbindFrom, bool IsPressedEvent, UnityAction FunctionToUnbind)
    {
        if (IsPressedEvent)
        {
            UnbindActionFromUnityInputEventHelper(ButtonPressedActionEventDictionary, KeyCodeToUnbindFrom, FunctionToUnbind);
        }
        else
        {
            UnbindActionFromUnityInputEventHelper(ButtonReleasedActionEventDictionary, KeyCodeToUnbindFrom, FunctionToUnbind); 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DictionaryWeAreUnbindingFrom"></param>
    /// <param name="KeyCodeToUnbindFrom"></param>
    /// <param name="FunctionToUnbind"></param>
    private void UnbindActionFromUnityInputEventHelper(Dictionary<KeyCode, UnityAction> DictionaryWeAreUnbindingFrom, KeyCode KeyCodeToUnbindFrom, UnityAction FunctionToUnbind)
    {
        if (!DictionaryWeAreUnbindingFrom.ContainsKey(KeyCodeToUnbindFrom))
        {
            Debug.LogError("The keycode input you were trying to unbind has not been set yet");
            return;
        }
        DictionaryWeAreUnbindingFrom[KeyCodeToUnbindFrom] -= FunctionToUnbind;
        if (DictionaryWeAreUnbindingFrom[KeyCodeToUnbindFrom] == null)
        {
            DictionaryWeAreUnbindingFrom.Remove(KeyCodeToUnbindFrom);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AxisName"></param>
    /// <param name="FunctionToPerform"></param>
    public void BindAxisToAction(string AxisName, Action<float> FunctionToPerform)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AxisName"></param>
    public void UnbindActionFromAxis(string AxisName)
    {

    }

    
    /// <summary>
    /// 
    /// </summary>
    private class ActionEventInfo
    {
        public string InputName;
        public bool IsThisAPressedEvent;//If not we assume its a release event
    }
}
