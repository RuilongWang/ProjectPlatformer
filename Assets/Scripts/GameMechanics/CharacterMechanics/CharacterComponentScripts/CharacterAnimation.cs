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

    private CharacterMovement AssociatedCharacterMovmeent { get { return AssociatedCharacter.CharacterMovementComponent; } }
    private CustomPhysics2D CharacterPhysics { get { return AssociatedCharacter.Rigid; } }
    private Animator CharacterAnimator;
    private SpriteRenderer AssociatedSpriteRenderer { get { return AssociatedCharacter.CharacterSpriteRenderer; } }
    private Dictionary<int, int> TriggerInputBufferDictionary = new Dictionary<int, int>();
    private bool ShouldUpdateAnimatorManually;
    #endregion animation references

    private void Awake()
    {
        AssociatedCharacter = GetComponent<GamePlayCharacters>();
        CharacterAnimator = GetComponent<Animator>();
        if (CharacterAnimator.enabled)
        {
            Debug.LogWarning("You are using the Animator in debug mode. Please be sure to disable it when you are done.");
            ShouldUpdateAnimatorManually = false;
        }
        else
        {
            ShouldUpdateAnimatorManually = true;
        }
    }

    private void Update()
    {
        UpdateAnimatorBasedOnCharacterMovement();
        if (ShouldUpdateAnimatorManually) CharacterAnimator.Update(GameOverseer.DELTA_TIME);
    }

    /// <summary>
    /// This method is used to update the character's animator based on the character movement component
    /// </summary>
    public void UpdateAnimatorBasedOnCharacterMovement()
    {
        SetCharacterSpriteScaleBasedOnCharacterMovement();
        CharacterAnimator.SetFloat(HORIZONTAL_INPUT, Mathf.Abs(AssociatedCharacterMovmeent.MovementInput.x));
        CharacterAnimator.SetFloat(VERTICAL_INPUT, Mathf.Abs(AssociatedCharacterMovmeent.MovementInput.y));
        CharacterAnimator.SetFloat(VERTICAL_VELOCITY, CharacterPhysics.Velocity.y);
        CharacterAnimator.SetInteger(MOVEMENT_STATE, (int)AssociatedCharacterMovmeent.CurrentMovementState);
        CharacterAnimator.SetInteger(STANDING_GROUNDED_STATE, (int)AssociatedCharacterMovmeent.CurrentGroundedStandingState);
    }

    /// <summary>
    /// Sets the caracter's sprite renderer comopnent based on the direction indicated 
    /// by the CharacterMovmeent Component
    /// </summary>
    private void SetCharacterSpriteScaleBasedOnCharacterMovement()
    {
        if (AssociatedCharacterMovmeent.IsCharacterFacingRight && AssociatedSpriteRenderer.transform.localScale.x < 0)
            AssociatedSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
        else if (!AssociatedCharacterMovmeent.IsCharacterFacingRight && AssociatedSpriteRenderer.transform.localScale.x > 0)
            AssociatedSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void SetAnimationTrigger(int AnimatorHash, int AttackBufferInFrames = 0) 
    {
        CharacterAnimator.SetTrigger(AnimatorHash);
        if (AttackBufferInFrames > 0) StartCoroutine(BeginBufferTriggerInput(AnimatorHash, AttackBufferInFrames));
    }
    public void SetAnimationInt(int AnimationHash, int Value) { CharacterAnimator.SetInteger(AnimationHash, Value); }
    public void SetAnimationFloat(int AnimationHash, float Value) { CharacterAnimator.SetFloat(AnimationHash, Value); }

    private IEnumerator BeginBufferTriggerInput(int AnimationHash, int AttackBufferInFrames)
    {
        if (TriggerInputBufferDictionary.ContainsKey(AnimationHash))
        {
            TriggerInputBufferDictionary[AnimationHash] = AttackBufferInFrames;
            yield break;
        }
        TriggerInputBufferDictionary.Add(AnimationHash, AttackBufferInFrames);
        while (TriggerInputBufferDictionary[AnimationHash] > 0)
        {
            --TriggerInputBufferDictionary[AnimationHash];
            yield return null;
        }
        CharacterAnimator.SetBool(AnimationHash, false);
        TriggerInputBufferDictionary.Remove(AnimationHash);
    }
}
