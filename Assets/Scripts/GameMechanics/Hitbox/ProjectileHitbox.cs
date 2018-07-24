using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitbox : HitBox {
    [Tooltip("This will be the number of rays that will appear between this the width of the projectile hitbox")]
    public int rayCountForHitbox = 5;
    [Tooltip("Size of the projectile hitbox. It will use the up and down vector when calculating the width")]
    public float widthOfProjectileHitbox = 1;

    #region monobehaviour methods
    private void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        
    }
    #endregion monobehaviour methods

    private void UpdateProjectileHitboxRays()
    {

    }
}
