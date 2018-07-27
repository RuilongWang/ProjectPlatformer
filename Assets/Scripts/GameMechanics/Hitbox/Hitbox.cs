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
    protected virtual void Start()
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
        if (associatedHitBoxManager)
            associatedHitBoxManager.allConnectedHitboxes.Remove(this);
    }

    private void OnDisable()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HitBox enemyHitbox = collision.GetComponent<HitBox>();
        HurtBox enemyHurtbox = collision.GetComponent<HurtBox>();

        if (enemyHitbox && enemyHitbox.associatedHitBoxManager != this.associatedHitBoxManager)
        {
            OnHitboxEnterEnemyHitbox(enemyHitbox);
        }
        if (enemyHurtbox && enemyHurtbox.associatedHiBboxManager != this.associatedHitBoxManager)
        {
            OnHitboxEnterEnemyHurtbox(enemyHurtbox);
        }
    }
    #endregion monobehavour methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyHitbox"></param>
    protected virtual void OnHitboxEnterEnemyHitbox(HitBox enemyHitbox)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyHurtbox"></param>
    protected virtual void OnHitboxEnterEnemyHurtbox(HurtBox enemyHurtbox)
    {

    }

    /// <summary>
    /// Potentially we may want to interact with the environment when we collide with the terrain when we hit a wall or the ground
    /// This method will always be called if we hit something in the environment layer
    /// </summary>
    /// <param name="collider"></param>
    protected virtual void OnHitboxEnteredEnvironmentCollider(Collider2D collider)
    {
        
    }

}
