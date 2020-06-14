using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public const int CHARACTER_ATTACK_LIGHT = 1;
    public const int CHARACTER_ATTACK_MEDIUM = 2;


    #region const variables
    private readonly int ATTACK_TRIGGER = Animator.StringToHash("Attack");
    private readonly int ATTACK_TYPE = Animator.StringToHash("AttackType");
    private const int ATTACK_BUFFER_FRAMES = 15;
    #endregion const variables

    private GamePlayCharacter AssociatedCharacter;
    private CharacterAnimation AssociatedCharacterAnimation { get { return AssociatedCharacter.CharacterAnimationComponent; } }

    

    #region monobehaviour methods
    private void Awake()
    {
        AssociatedCharacter = GetComponent<GamePlayCharacter>();
    }

    private void Update()
    {
        
    }
    #endregion monobehaivour methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="AttackType"></param>
    public void BeginAttack(int AttackType)
    {
        AssociatedCharacterAnimation.SetAnimationTrigger(ATTACK_TRIGGER, ATTACK_BUFFER_FRAMES);
    }


}
