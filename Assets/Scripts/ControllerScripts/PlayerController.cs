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
        PAUSE_BUTTON,
    }

    #region const variables
    [SerializeField]
    private InputBinding[] DefaultInputs = new InputBinding[3];

    /// <summary>
    /// Axes bindings for now will just be hardcoded. There is no reason to make that bindable
    /// </summary>
    public const string HORIZONTAL_AXIS = "MoveLeftRight";
    public const string VERTICAL_AXIS = "MoveUpDown";
    #endregion const variables

    private Dictionary<ButtonInputID, UnityAction> ButtonPressedActionEventDictionary = new Dictionary<ButtonInputID, UnityAction>();
    private Dictionary<ButtonInputID, UnityAction> ButtonReleasedActionEventDictionary = new Dictionary<ButtonInputID, UnityAction>();
    private Dictionary<string, UnityAction<float>> AxisEventDictionary = new Dictionary<string, UnityAction<float>>();



    #region monobehaviour methods
    private void Awake()
    {
        
    }

    private void Update()
    {
        foreach (KeyValuePair<ButtonInputID, UnityAction> ButtonPressedEvents in ButtonPressedActionEventDictionary)
        {
            if (DefaultInputs[(int)ButtonPressedEvents.Key].IsButtonPressed())
            {
                ButtonPressedEvents.Value.Invoke();
            }
        }

        foreach (KeyValuePair<ButtonInputID, UnityAction> ButtonReleasedEvent in ButtonReleasedActionEventDictionary)
        {
            if (DefaultInputs[(int)ButtonReleasedEvent.Key].IsButtonReleased())
            {
                ButtonReleasedEvent.Value.Invoke();
            }
        }

        foreach (KeyValuePair<string, UnityAction<float>> AxisEvent in AxisEventDictionary)
        {
            AxisEvent.Value.Invoke(Input.GetAxisRaw(AxisEvent.Key));
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < DefaultInputs.Length; ++i)
        {
            DefaultInputs[i].InputName = (ButtonInputID)i;
            DefaultInputs[i].ButtonName = DefaultInputs[i].InputName.ToString();
        }
    }
#endif
#endregion monobehaviour methods


    /// <summary>
    /// 
    /// </summary>
    /// <param name="InputName"></param>
    /// <param name="IsPressedEvent"></param>
    /// <param name="FunctionToPerform"></param>
    public void BindInputToAction(ButtonInputID ButtonIDToBind, bool IsPressedEvent, UnityAction FunctionToPerform)
    {
        if (IsPressedEvent)
        {
            BindInputActionHelper(ButtonPressedActionEventDictionary, ButtonIDToBind, FunctionToPerform);   
        }
        else
        {
            BindInputActionHelper(ButtonReleasedActionEventDictionary, ButtonIDToBind, FunctionToPerform);
        }
    }

    /// <summary>
    /// Since this action is done exactly the same to multiple dictionaries it seemed fitting to have
    /// a bind input action helper to assist with binding an action to the passed in ButtonInputID
    /// </summary>
    /// <param name="DictionaryToBindInputEventTo"></param>
    /// <param name="ButtonInputToBind"></param>
    /// <param name="FunctionToPerform"></param>
    private void BindInputActionHelper(Dictionary<ButtonInputID, UnityAction> DictionaryToBindInputEventTo, ButtonInputID ButtonInputToBind, UnityAction FunctionToPerform)
    {
        if (!DictionaryToBindInputEventTo.ContainsKey(ButtonInputToBind))
        {
            DictionaryToBindInputEventTo.Add(ButtonInputToBind, null);
        }
        DictionaryToBindInputEventTo[ButtonInputToBind] += FunctionToPerform;
    }

    /// <summary>
    /// Unbinds an action from the ButtonInputID that is passed in
    /// </summary>
    /// <param name="InputName"></param>
    /// <param name="IsPressedEvent"></param>
    public void UnbindActionFromUntityInputEvent(ButtonInputID ButtonIDToBind, bool IsPressedEvent, UnityAction FunctionToUnbind)
    {
        if (IsPressedEvent)
        {
            UnbindActionFromUnityInputEventHelper(ButtonPressedActionEventDictionary, ButtonIDToBind, FunctionToUnbind);
        }
        else
        {
            UnbindActionFromUnityInputEventHelper(ButtonReleasedActionEventDictionary, ButtonIDToBind, FunctionToUnbind); 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DictionaryWeAreUnbindingFrom"></param>
    /// <param name="ButtonInputToBindTo"></param>
    /// <param name="FunctionToUnbind"></param>
    private void UnbindActionFromUnityInputEventHelper(Dictionary<ButtonInputID, UnityAction> DictionaryWeAreUnbindingFrom, ButtonInputID ButtonInputToBindTo, UnityAction FunctionToUnbind)
    {
        if (!DictionaryWeAreUnbindingFrom.ContainsKey(ButtonInputToBindTo))
        {
            Debug.LogError("The keycode input you were trying to unbind has not been set yet");
            return;
        }
        DictionaryWeAreUnbindingFrom[ButtonInputToBindTo] -= FunctionToUnbind;
        if (DictionaryWeAreUnbindingFrom[ButtonInputToBindTo] == null)
        {
            DictionaryWeAreUnbindingFrom.Remove(ButtonInputToBindTo);
        }
    }

    /// <summary>
    /// Binds an action to the 
    /// </summary>
    /// <param name="AxisName"></param>
    /// <param name="FunctionToPerform"></param>
    public void BindAxisToAction(string AxisName, UnityAction<float> FunctionToPerform)
    {
        if (!AxisEventDictionary.ContainsKey(AxisName))
        {
            AxisEventDictionary.Add(AxisName, null);
        }
        AxisEventDictionary[AxisName] += FunctionToPerform;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AxisName"></param>
    public void UnbindActionFromAxis(string AxisName, UnityAction<float> FunctionToRemove)
    {
        if (!AxisEventDictionary.ContainsKey(AxisName))
        {
            Debug.LogWarning("There was no key found with the Axis Name: " + AxisName);
            return;
        }

        AxisEventDictionary[AxisName] -= FunctionToRemove;
        if (AxisEventDictionary[AxisName] == null)
        {
            AxisEventDictionary.Remove(AxisName);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public class InputBinding
    {
        /// <summary>
        /// The two types of valid key types that you can assign
        /// </summary>
        public enum BindingType { Keyboard, GamePad}
        [Tooltip("This value is entirely for organization purposes in the editor and should not be used")]
        public string ButtonName;
        public ButtonInputID InputName { get; set; }
        public KeyCode[] ValidKeyCodeBindings = new KeyCode[2];

        /// <summary>
        /// Are any of the valid keys for this input pressed (true for one frame)
        /// </summary>
        /// <returns></returns>
        public bool IsButtonPressed()
        {
            foreach (KeyCode KeycodeToCheck in ValidKeyCodeBindings)
            {
                if (Input.GetKeyDown(KeycodeToCheck))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Are any of the valid keys for this input released (true for one frame)
        /// </summary>
        /// <returns></returns>
        public bool IsButtonReleased()
        {
            foreach (KeyCode KeycodeToCheck in ValidKeyCodeBindings)
            {
                if (Input.GetKeyUp(KeycodeToCheck))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
