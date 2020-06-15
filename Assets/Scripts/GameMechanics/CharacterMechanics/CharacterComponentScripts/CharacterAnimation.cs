using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


[RequireComponent(typeof(Animator))]
public class CharacterAnimation : AnimationComponent
{
    #region const variables
    protected readonly int HORIZONTAL_INPUT = Animator.StringToHash("HorizontalInput");
    protected readonly int VERTICAL_INPUT = Animator.StringToHash("VerticalInput");
    protected readonly int VERTICAL_VELOCITY = Animator.StringToHash("VerticalVelocity");
    protected readonly int STANDING_GROUNDED_STATE = Animator.StringToHash("StandingGroundedState");
    protected readonly int MOVEMENT_STATE = Animator.StringToHash("MovementState");
    #endregion const variables

    #region animation references

    /// <summary>
    /// The associated character component
    /// </summary>
    private GamePlayCharacter AssociatedCharacter;

    /// <summary>
    /// A reference to the associated character movement. Based on the associated character component
    /// </summary>
    private CharacterMovement AssociatedCharacterMovmeent { get { return AssociatedCharacter.CharacterMovementComponent; } }

    /// <summary>
    /// A reference to the physics component. Based on the Associated Character Component
    /// </summary>
    private CustomPhysics2D CharacterPhysics { get { return AssociatedCharacter.Rigid; } }

    /// <summary>
    /// A reference to the Sprite Renderer. Based on the Assocoated Character Component
    /// </summary>
    private SpriteRenderer AssociatedSpriteRenderer { get { return AssociatedCharacter.CharacterSpriteRenderer; } }

    /// <summary>
    /// Trigger Input Dictionary. Contains a collection of all the buttons that are current registered as active in our buffer. This is so
    /// our animation trigger parameters can deregister after a certain amount of time has passed.
    /// </summary>
    private Dictionary<int, int> TriggerInputBufferDictionary = new Dictionary<int, int>();

    #endregion animation references

    protected override void Awake()
    {
        base.Awake();
        AssociatedCharacter = GetComponent<GamePlayCharacter>();
        
    }

    protected override void Update()
    {
        UpdateAnimatorBasedOnCharacterMovement();
        base.Update();
    }

    /// <summary>
    /// This method is used to update the character's animator based on the character movement component
    /// </summary>
    public void UpdateAnimatorBasedOnCharacterMovement()
    {
        SetCharacterSpriteScaleBasedOnCharacterMovement();
        AssociatedAnimator.SetFloat(HORIZONTAL_INPUT, Mathf.Abs(AssociatedCharacterMovmeent.MovementInput.x));
        AssociatedAnimator.SetFloat(VERTICAL_INPUT, Mathf.Abs(AssociatedCharacterMovmeent.MovementInput.y));
        AssociatedAnimator.SetFloat(VERTICAL_VELOCITY, CharacterPhysics.Velocity.y);
        AssociatedAnimator.SetInteger(MOVEMENT_STATE, (int)AssociatedCharacterMovmeent.CurrentMovementState);
        AssociatedAnimator.SetInteger(STANDING_GROUNDED_STATE, (int)AssociatedCharacterMovmeent.CurrentGroundedStandingState);
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

    /// <summary>
    /// Call this method to toggle on an animation trigger. T
    /// </summary>
    /// <param name="AnimatorHash"></param>
    /// <param name="AttackBufferInFrames"></param>
    public void SetAnimationTrigger(int AnimatorHash, int AttackBufferInFrames = 0) 
    {
        AssociatedAnimator.SetTrigger(AnimatorHash);
        if (AttackBufferInFrames > 0) StartCoroutine(BeginBufferTriggerInput(AnimatorHash, AttackBufferInFrames));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AnimationHash"></param>
    /// <param name="Value"></param>
    public void SetAnimationInt(int AnimationHash, int Value) { AssociatedAnimator.SetInteger(AnimationHash, Value); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AnimationHash"></param>
    /// <param name="Value"></param>
    public void SetAnimationFloat(int AnimationHash, float Value) { AssociatedAnimator.SetFloat(AnimationHash, Value); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AnimationHash"></param>
    /// <param name="Value"></param>
    public void SetAnimationBool(int AnimationHash, bool Value) { AssociatedAnimator.SetBool(AnimationHash, Value); }

    /// <summary>
    /// This method acts to buffer trigger events in our Animator. If a button is pressed again while our coroutine is still
    /// active, it will simply reset the buffer timer and exit out of the new cotourine.
    /// </summary>
    /// <param name="AnimationHash"></param>
    /// <param name="AttackBufferInFrames"></param>
    /// <returns></returns>
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
        AssociatedAnimator.SetBool(AnimationHash, false);
        TriggerInputBufferDictionary.Remove(AnimationHash);
    }
}
