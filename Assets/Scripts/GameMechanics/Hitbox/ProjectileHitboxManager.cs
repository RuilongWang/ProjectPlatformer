using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitboxManager : HitBoxManager {
    [Tooltip("The associated projectile object")]
    public Projectile associatedProjectile;


    #region override methods
    public override void ActivateHitboxManager()
    {
        base.ActivateHitboxManager();
        foreach (HitBox hbox in allConnectedHitboxes)
        {
            hbox.gameObject.SetActive(true);
        }
    }
    #endregion override methods

}
