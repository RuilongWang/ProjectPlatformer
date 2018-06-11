using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMechanics : MonoBehaviour {
    #region main variables


    private Animator anim;
    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    #endregion monobehaviour methods

    private void UseMeleeAttack()
    {

    }
}
