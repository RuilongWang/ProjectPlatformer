using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller that is used to manipulate the player
/// </summary>
public class PlayerController : MonoBehaviour {
    private MovementMechanics movementMechanics;


    #region const button names
    public enum ButtonAction
    {
        Jump,
        MeleeAttack,
        Fire
    }

    private string[] buttonActionNamesList;
    #endregion const button names

    #region monobehaviour methods
    private void Awake()
    {
        buttonActionNamesList = System.Enum.GetNames(typeof(ButtonAction));
    }

    private void Start()
    {
        movementMechanics = GetComponent<MovementMechanics>();
    }

    private void Update()
    {
        


        movementMechanics.SetHorizontalInput(GetHorizontalAxisRaw());
        if (GetButtonDown(ButtonAction.Jump))
        {
            movementMechanics.Jump();
        }
        if (GetButton(ButtonAction.Jump) && movementMechanics.isFastFalling)
        {
            movementMechanics.isFastFalling = false;
        }
        else if (!GetButton(ButtonAction.Jump) && !movementMechanics.isFastFalling)
        {
            movementMechanics.isFastFalling = true;
        }
    }
    #endregion monobehaviour methods
    


    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonAction"></param>
    /// <returns></returns>
    public bool GetButton(ButtonAction buttonAction)
    {
        return Input.GetButton(buttonActionNamesList[(int)buttonAction]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonAction"></param>
    /// <returns></returns>
    public bool GetButtonDown(ButtonAction buttonAction)
    {
        return Input.GetButtonDown(buttonActionNamesList[(int)buttonAction]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonAction"></param>
    /// <returns></returns>
    public bool GetButtonUp(ButtonAction buttonAction)
    {
        return Input.GetButtonUp(buttonActionNamesList[(int)buttonAction]);
    }

    /// <summary>
    /// Returns the current horizontal axis of the joystick
    /// </summary>
    /// <returns></returns>
    public float GetHorizontalAxisRaw()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    /// <summary>
    /// Returns the vertical axis of the joystick
    /// </summary>
    /// <returns></returns>
    public float GetVerticalAxisRaw()
    {
        return Input.GetAxisRaw("Vertical");
    }
    #region get button methods

    #endregion get button methods
    
}
