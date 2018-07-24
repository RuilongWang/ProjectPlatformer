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

    private void OnValidate()
    {
        if (rayCountForHitbox < 2)
        {
            rayCountForHitbox = 2;
        } 
    }

    /// <summary>
    /// Visually draw out what the hitbox will look like to make it more clear to the
    /// designer
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Vector3 topPoint = transform.position + (transform.up * widthOfProjectileHitbox / 2);
        Vector3 bottomPoint = transform.position + (-transform.up * widthOfProjectileHitbox / 2);

        Gizmos.DrawLine(topPoint, bottomPoint);
    }
    #endregion monobehaviour methods

    private void UpdateProjectileHitboxRays()
    {

    }
}
