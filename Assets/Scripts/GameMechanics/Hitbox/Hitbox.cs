using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The hitbox class will apply damage to a character if it clashes with a hurtbox. And depending on
/// certain conditions, this class may also interact with other hitbox classes found in enemies
/// </summary>
public class HitBox : MonoBehaviour {
    #region main vairables
    private bool hitboxCurrentlyActive;
    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        hitboxCurrentlyActive = true;
    }

    private void OnDisable()
    {
        
    }
    #endregion monobehavour methods

}
