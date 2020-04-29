using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
/// <summary>
/// A child of the character class that will perform and hold values important to our Playable character
/// </summary>
public class PlayerCharacter : Character
{
    public PlayerController AssociatedPlayerController { get; private set; }

    #region monobehavoiur methods
    protected override void Awake()
    {
        base.Awake();
        AssociatedPlayerController = GetComponent<PlayerController>();
        SetupPlayerController(AssociatedPlayerController);
    }
    #endregion monobehaviour methods

    /// <summary>
    /// This method is where you should bind our player controller functions. Assign buttons and axes to their appropriate functions
    /// </summary>
    protected virtual void SetupPlayerController(PlayerController PlayerControllerToSetup)
    {
        PlayerControllerToSetup.BindAxisToAction(PlayerController.HORIZONTAL_AXIS, CharacterMovement.ApplyHorizontalInput);
        PlayerControllerToSetup.BindInputToAction(PlayerController.ButtonInputID.JUMP_BUTTON, true, CharacterMovement.Jump);
        PlayerControllerToSetup.BindInputToAction(PlayerController.ButtonInputID.JUMP_BUTTON, false, CharacterMovement.JumpReleased);
    }
}
