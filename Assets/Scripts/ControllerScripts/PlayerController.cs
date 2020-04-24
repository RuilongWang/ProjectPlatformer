using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    #region const variables
    public const string JUMP_ACTION = "JumpAction";
    public const string ATTACK_ACTION = "AttackAction";

    public const string HORIZONTAL_AXIS = "MoveLeftRight";
    public const string VERTICAL_AXIS = "MoveUpDown";
    #endregion const variables

    private Dictionary<string, UnityAction> ActionEventDictioanry = new Dictionary<string, UnityAction>();
    private Dictionary<string, UnityAction<float>> AxisEventDictionary = new Dictionary<string, UnityAction<float>>();


    private void Update()
    {

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="InputName"></param>
    /// <param name="IsPressedEvent"></param>
    /// <param name="FunctionToPerform"></param>
    public void BindInputToAction(string InputName, bool IsPressedEvent, Action FunctionToPerform)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="InputName"></param>
    /// <param name="IsPressedEvent"></param>
    public void UnbindInputAction(string InputName, bool IsPressedEvent)
    {

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
