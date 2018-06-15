using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMechanics : MonoBehaviour {
    #region const variables
    private const string ATTACK_ANIMATRION_TRIGGER = "Attack";
    #endregion const variables


    #region main variables
    [Tooltip("This variable refers to the amount of time in seconds before we deactivate our attack trigger. This gives some buffer to the player, so they don't have to be perfect with their inputs when attack")]
    public float timeToDeactivateAttackTrigger = .25f;

    private float deactivateAttackTriggerTimer = 0;
    private Animator anim;
    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }
    #endregion monobehaviour methods


    private bool CheckDeactivateAttackTrigger()
    {


        return false;
    }

    /// <summary>
    /// Call this to carry out an attack animation. This will return true if the input
    /// was registered
    /// </summary>
    /// <returns></returns>
    private bool UseMeleeAttack()
    {
        return false;
    }
}
