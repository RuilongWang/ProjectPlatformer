using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMechanics : MonoBehaviour {
    /// <summary>
    /// If we have multiple attack types, for example neutral attack vs forward tilted attack, we may want to distinguish,
    /// which version should be buffered
    /// </summary>
    public enum AttackType
    {
        Neutral_Attack,
    }

    #region const variables
    private const string ATTACK_ANIMATRION_TRIGGER = "Attack";
    #endregion const variables


    #region main variables
    [Tooltip("This variable refers to the amount of time in seconds before we deactivate our attack trigger. This gives some buffer to the player, so they don't have to be perfect with their inputs when attack")]
    public float timeToDeactivateAttackTrigger = 0.2f;

    /// <summary>
    /// A dictionary reference that contains the time remaining before our buffered input is reset
    /// </summary>
    private Dictionary<AttackType, float> bufferedButtonPressTimer = new Dictionary<AttackType, float>();

    private string[] attackTypeNames;

    private Animator anim;
    #endregion main variables

    #region monobehaviour methods
    private void Awake()
    {
        attackTypeNames = System.Enum.GetNames(typeof(AttackType));
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }
    #endregion monobehaviour methods

    /// <summary>
    /// Call this to carry out an attack animation. This will return true if the input
    /// was registered
    /// </summary>
    /// <returns></returns>
    private bool MeleeAttack()
    {

        return false;
    }

    private void SetBufferedAttackType(AttackType attackType)
    {
        bufferedButtonPressTimer[attackType] = timeToDeactivateAttackTrigger;

        if (bufferedButtonPressTimer[attackType] <= 0)
        {
            StartCoroutine(AttackInputBufferCoroutine(attackType));
        }
            
    }

    /// <summary>
    /// This enumerator method is used to buffer inputs that the player uses. We can queue up 
    /// attack events. This method will reset a trigger, so that we do not activate it well before
    /// setting the input
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackInputBufferCoroutine(AttackType attackType)
    {
        while (bufferedButtonPressTimer[attackType] > 0)
        {
            bufferedButtonPressTimer[attackType] -= Time.deltaTime;
            yield return null;
        }

        if (anim.GetBool(attackTypeNames[(int)attackType]))
        {
            anim.ResetTrigger(attackTypeNames[(int)attackType]);
        }
    }
}
