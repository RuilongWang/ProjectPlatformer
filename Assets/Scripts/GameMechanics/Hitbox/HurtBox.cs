using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hurtbox is the area that will take damage if it collides with an enemy hitbox
/// </summary>
public class HurtBox : MonoBehaviour {
    #region main variables
    public HitBoxManager associatedHiBboxManager { get; private set; }
    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        associatedHiBboxManager = GetComponentInParent<HitBoxManager>();
        if (!associatedHiBboxManager)
        {
            Debug.LogWarning("There should be a hitbox manager attached to the parent of " + this.transform.name);
            return;
        }
        associatedHiBboxManager.allConnectedHurtboxes.Add(this);
    }

    private void OnDestroy()
    {
        associatedHiBboxManager.allConnectedHurtboxes.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitBox hitbox = collision.GetComponent<HitBox>();
        HurtBox hurtbox = collision.GetComponent<HurtBox>();

        if (hitbox && hitbox.associatedHitBoxManager != this.associatedHiBboxManager)//Checking to make sure this is not our own hitbox
        {
            OnHurtboxEnteredEnemyHitbox(hitbox);
        }
        if (hurtbox && hurtbox.associatedHiBboxManager != this.associatedHiBboxManager)
        {

        }
    }
    #endregion monobehaviour methods


    protected virtual void OnHurtboxEnteredEnemyHitbox(HitBox enemyHitbox)
    {

    }

    /// <summary>
    /// Don't really see a use for this one, but its there if you need it...
    /// This will be called any time a hurtbox enters an enemy hurtbox
    /// </summary>
    protected virtual void OnHurtboxEnteredEnemyHurtbox(HurtBox enemyHurtbox)
    {

    }
}
