using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for projectiles
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CustomPhysics2D))]
public class Projectile : MonoBehaviour {

    #region main variables
    public float lauchSpeed = 10;
    public Vector2 launchVector = Vector2.right;

    private BoxCollider2D attachedCollder;
    private CharacterStats characterThatLaunchedProjectile;
    private CustomPhysics2D rigid;

    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        rigid = GetComponent<CustomPhysics2D>();   
    }

    protected virtual void Update()
    {
        UpdateRotationBasedOnVelocity();
    }
    #endregion monobehaviour methods

    /// <summary>
    /// Call this method after spawning a projectile to properly set up our projectile
    /// </summary>
    /// <param name="characterThatLaunchedProjectile"></param>
    /// <param name="launchVector"></param>
    public void SetUpProjectile(CharacterStats characterThatLaunchedProjectile, Vector2 launchVector)
    {
        this.characterThatLaunchedProjectile = characterThatLaunchedProjectile;
    }

    /// <summary>
    /// Updates where our projectile will be located
    /// </summary>
    private void UpdateCollisionPoint()
    {

    }

    /// <summary>
    /// Updates the rotation of the projectile to match the velocity that it is currently travelling at
    /// </summary>
    private void UpdateRotationBasedOnVelocity()
    {
        Vector2 velocityUnityVector = rigid.velocity.normalized;

    }
}
