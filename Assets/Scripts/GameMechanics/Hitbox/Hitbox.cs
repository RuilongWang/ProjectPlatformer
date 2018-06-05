using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The hitbox class will apply damage to a character if it clashes with a hurtbox. And depending on
/// certain conditions, this class may also interact with other hitbox classes found in enemies
/// </summary>
public class HitBox : MonoBehaviour {
    #region main vairables
    [Tooltip("The base damage to apply to an enemy if hit by this hitbox")]
    public float damageToApply = 1;
    [Tooltip("Some moves will apply a hitstun to an enemy, meaning that they can not take any actions until the hitstun has worn off. Hitstun will not stack on an enemy. It will simply use the last hitstun value that was applied")]
    public float hitStunTimeInSeconds = 0;
    [Tooltip("The force that will be applied to a hit enemy")]
    public float knockbackForce = 1;
    public Vector2 directionToApplyKnockback = Vector2.right;

    public HitBoxManager associatedHitBoxManager { get; private set; }
    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        associatedHitBoxManager = GetComponentInParent<HitBoxManager>();
        if (!associatedHitBoxManager)
        {
            Debug.LogWarning("There should be a hitbox manager that is attached to the parent of " + this.transform.name);
            return;
        }
        associatedHitBoxManager.allConnectedHitboxes.Add(this);
    }

    private void OnDestroy()
    {
        associatedHitBoxManager.allConnectedHitboxes.Remove(this);
    }

    private void OnDisable()
    {
        
    }
    #endregion monobehavour methods

}
