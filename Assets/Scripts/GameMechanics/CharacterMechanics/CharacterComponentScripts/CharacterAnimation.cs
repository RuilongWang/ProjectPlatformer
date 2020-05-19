using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterAnimation : MonoBehaviour
{
    #region const variables
    protected readonly int HORIZONTAL_SPEED = Animator.StringToHash("HorizontalSpeed");
    protected readonly int VERTICAL_SPEED = Animator.StringToHash("VerticalSpeed");
    protected readonly int HORIZONTAL_INPUT = Animator.StringToHash("HorizontalInput");
    protected readonly int VERTICAL_INPUT = Animator.StringToHash("VerticalInput");
    #endregion const variables

    private Character AssociatedCharacter;
    private Animator CharacterAnimator;
    private SpriteRenderer AssociatedSpriteRenderer;


    private void Awake()
    {
        AssociatedCharacter = GetComponent<Character>();
        CharacterAnimator = GetComponent<Animator>();
        AssociatedSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
        CharacterMovement CharacterMovement = AssociatedCharacter.CharacterMovement;
        //CharacterAnimator.SetFloat(HORIZONTAL_INPUT, CharacterMovement.HorizontalInput);

        if (CharacterMovement.IsCharacterFacingRight && AssociatedSpriteRenderer.transform.localScale.x < 0) 
            AssociatedSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
        else if (!CharacterMovement.IsCharacterFacingRight && AssociatedSpriteRenderer.transform.localScale.x > 0) 
            AssociatedSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
    }
}
