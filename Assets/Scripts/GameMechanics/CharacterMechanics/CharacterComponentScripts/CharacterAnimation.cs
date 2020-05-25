using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterAnimation : MonoBehaviour
{
    #region const variables
    protected readonly int HORIZONTAL_INPUT = Animator.StringToHash("HorizontalInput");
    protected readonly int VERTICAL_INPUT = Animator.StringToHash("VerticalInput");
    protected readonly int VERTICAL_VELOCITY = Animator.StringToHash("VerticalVelocity");
    protected readonly int STANDING_GROUNDED_STATE = Animator.StringToHash("StandingGroundedState");
    protected readonly int MOVEMENT_STATE = Animator.StringToHash("MovementState");
    #endregion const variables

    #region animation references
    private GamePlayCharacters AssociatedCharacter;

    private CharacterMovement AssociatedCharacterMovmeent { get { return AssociatedCharacter.CharacterMovement; } }
    private CustomPhysics2D CharacterPhysics { get { return AssociatedCharacter.Rigid; } }
    private Animator CharacterAnimator { get { return AssociatedCharacter.CharacterAnimator; } }
    private SpriteRenderer AssociatedSpriteRenderer { get { return AssociatedCharacter.CharacterSpriteRenderer; } }
    #endregion animation references

    private void Awake()
    {
        AssociatedCharacter = GetComponent<GamePlayCharacters>();

    }

    private void Update()
    {
        UpdateAnimatorBasedOnCharacterMovement();
    }

    /// <summary>
    /// This method is used to update the character's animator based on the character movement component
    /// </summary>
    public void UpdateAnimatorBasedOnCharacterMovement()
    {
        if (AssociatedCharacterMovmeent.IsCharacterFacingRight && AssociatedSpriteRenderer.transform.localScale.x < 0) 
            AssociatedSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
        else if (!AssociatedCharacterMovmeent.IsCharacterFacingRight && AssociatedSpriteRenderer.transform.localScale.x > 0) 
            AssociatedSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
        CharacterAnimator.SetFloat(HORIZONTAL_INPUT, Mathf.Abs(AssociatedCharacterMovmeent.MovementInput.x));
        CharacterAnimator.SetFloat(VERTICAL_INPUT, Mathf.Abs(AssociatedCharacterMovmeent.MovementInput.y));
        CharacterAnimator.SetFloat(VERTICAL_VELOCITY, CharacterPhysics.Velocity.y);
        CharacterAnimator.SetInteger(MOVEMENT_STATE, (int)AssociatedCharacterMovmeent.CurrentMovementState);
        CharacterAnimator.SetInteger(STANDING_GROUNDED_STATE, (int)AssociatedCharacterMovmeent.CurrentGroundedStandingState);
    }
}
