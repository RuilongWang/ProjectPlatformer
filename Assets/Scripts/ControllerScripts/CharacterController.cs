using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /// <summary>
    /// The associated character that is assigned to this character controller
    /// </summary>
    protected GamePlayCharacter AssocoatedCharacter;

    #region monobehaviour methods
    protected virtual void Awake()
    {
        AssocoatedCharacter = GetComponent<GamePlayCharacter>();
    }

    #endregion monobehaviour methods


    #region button command classes
    /// <summary>
    /// Generic class that will execute two
    /// </summary>
    protected abstract class ActionCommand
    {
        public virtual void ExecuteActionPress(GamePlayCharacter AssociatedCharacter) { }
        public virtual void ExecuteActionReleased(GamePlayCharacter AssociateCharacter) { }
    }

    protected class CommandJump : ActionCommand
    {
        public override void ExecuteActionPress(GamePlayCharacter AssociatedCharacter)
        {
            AssociatedCharacter.CharacterMovementComponent.Jump();
        }

        public override void ExecuteActionReleased(GamePlayCharacter AssociateCharacter)
        {
            AssociateCharacter.CharacterMovementComponent.JumpReleased();
        }
    }

    protected class CommandAttack01 : ActionCommand
    {
        public override void ExecuteActionPress(GamePlayCharacter AssociatedCharacter)
        {
            AssociatedCharacter.CharacterAttackComponent.BeginAttack(CharacterAttack.CHARACTER_ATTACK_LIGHT);
        }
    }
    #endregion button command classes

    #region axis command classes
    /// <summary>
    /// Command class to execute functions that use an axis value.
    /// </summary>
    protected abstract class AxisCommand
    {
        public virtual void ExecuteAxisAction(GamePlayCharacter AssociatedCharacter, float RawAxisValue) { }
        
    }

    /// <summary>
    /// 
    /// </summary>
    protected class AxisHorizontalMovement : AxisCommand
    {
        public override void ExecuteAxisAction(GamePlayCharacter AssociatedCharacter, float RawAxisValue)
        {
            AssociatedCharacter.CharacterMovementComponent.ApplyHorizontalInput(RawAxisValue);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected class AxisVerticalMovement : AxisCommand
    {
        public override void ExecuteAxisAction(GamePlayCharacter AssociatedCharacter, float RawAxisValue)
        {
            AssociatedCharacter.CharacterMovementComponent.ApplyVerticalInput(RawAxisValue);
        }
    }
    #endregion axis command classes
}
