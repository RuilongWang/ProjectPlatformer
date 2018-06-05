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

        if (hitbox && hitbox.associatedHitBoxManager != this.associatedHiBboxManager)//Checking to make sure this is not our own hitbox
        {

        }
    }
    #endregion monobehaviour methods

}
