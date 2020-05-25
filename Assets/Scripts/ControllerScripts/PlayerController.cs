using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Script that handles all the player controlled input for our game
/// 
/// The purpose of this class is to allow an easier time when giving the player an option to change their controller layout
/// </summary>
public class PlayerController : CharacterController
{
    #region const variables
    private const string KEYBOARD_HORIZONTAL_AXIS = "KeyboardHorizontal";
    private const string KEYBOARD_VERTICAL_AXIS = "KeyboardVertical";

    private const string JOYSTICK_HORIZONTAL_AXIS = "JoyHorizontal";
    private const string JOYSTICK_VERTICAL_AXIS = "JoyVertical";

    #endregion const variables

    private List<PlayerActionCommand> ButtonEventList;
    private List<PlayerAxisCommand> AxesEventList;
    private PlayerCharacter AssociatedPlayerCharacter;

    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        SetupDefaultControllerInputs();
        AssociatedPlayerCharacter = (PlayerCharacter)AssocoatedCharacter;
    }

    private void Update()
    {
        foreach (PlayerActionCommand PlayerAction in ButtonEventList)
        {
            if (PlayerAction.WasButtonPressed()) PlayerAction.ExecutePressedCommand(AssociatedPlayerCharacter);
        }

        foreach (PlayerAxisCommand PlayerAxis in AxesEventList)
        {
            PlayerAxis.ExecuteAxisAction(AssociatedPlayerCharacter, PlayerAxis.GetClampedAxisRaw());
        }
    }
    #endregion monobehaivour methods
    /// <summary>
    /// Setup our default controlls
    /// </summary>
    private void SetupDefaultControllerInputs()
    {
        ButtonEventList = new List<PlayerActionCommand>();
        AxesEventList = new List<PlayerAxisCommand>();

        //Button Events
        ButtonEventList.Add(new PlayerActionCommand(new CommandJump(), KeyCode.Space, KeyCode.JoystickButton0));


        //Axis Value Events
        AxesEventList.Add(new PlayerAxisCommand(new AxisHorizontalMovement(), 
            KEYBOARD_HORIZONTAL_AXIS, JOYSTICK_HORIZONTAL_AXIS));
        AxesEventList.Add(new PlayerAxisCommand(new AxisVerticalMovement(),
            KEYBOARD_VERTICAL_AXIS, JOYSTICK_VERTICAL_AXIS));

    }

    /// <summary>
    /// 
    /// </summary>
    private class PlayerActionCommand
    {
        public KeyCode DefaultKeyCode;
        public KeyCode AltKeyCode;
        protected ActionCommand ActionCommandToExecute;

        
        public PlayerActionCommand(ActionCommand ActionCommandToExecute, KeyCode DefaultKeyCode, KeyCode AltKeyCode)
        {
            this.ActionCommandToExecute = ActionCommandToExecute;
            this.DefaultKeyCode = DefaultKeyCode;
            this.AltKeyCode = AltKeyCode;
        }

        /// <summary>
        /// Returns whether or not the player has pressed the assigned button value
        /// </summary>
        /// <returns></returns>
        public bool WasButtonPressed()
        { return Input.GetKeyDown(DefaultKeyCode) || Input.GetKeyDown(AltKeyCode); }
        
        /// <summary>
        /// Returns whether or not the player has released the assigned button value
        /// </summary>
        /// <returns></returns>
        public bool WasButtonReleased()
        { return Input.GetKeyUp(DefaultKeyCode) || Input.GetKeyUp(AltKeyCode); }

        /// <summary>
        /// Runs the command to execute when a player has pressed the assigned button value
        /// </summary>
        /// <param name="AssociatedCharacter"></param>
        public void ExecuteReleaseCommand(GamePlayCharacters AssociatedCharacter)
        { ActionCommandToExecute.ExecuteActionReleased(AssociatedCharacter);}

        /// <summary>
        /// Runs the command to execute when a player has released the assigned button value
        /// </summary>
        /// <param name="AssociatedCharacter"></param>
        public void ExecutePressedCommand(GamePlayCharacters AssociatedCharacter)
        { ActionCommandToExecute.ExecuteActionPress(AssociatedCharacter); }
    }

    /// <summary>
    /// 
    /// </summary>
    private class PlayerAxisCommand
    {
        public string DefaultAxisName;
        public string AltAxisName;
        public AxisCommand AxisCommandToExecute;

        public PlayerAxisCommand(AxisCommand AxisCommandToExecute, string DefaultAxisName, string AltAxisName)
        {
            this.AxisCommandToExecute = AxisCommandToExecute;
            this.DefaultAxisName = DefaultAxisName;
            this.AltAxisName = AltAxisName;
        }

        public void ExecuteAxisAction(GamePlayCharacters AssocoatedCharacter, float AxisValue)
        { AxisCommandToExecute.ExecuteAxisAction(AssocoatedCharacter, AxisValue); }

        public float GetClampedAxisRaw()
        { return Mathf.Clamp(Input.GetAxisRaw(DefaultAxisName) + Input.GetAxisRaw(AltAxisName), -1, 1); }
    }

}
