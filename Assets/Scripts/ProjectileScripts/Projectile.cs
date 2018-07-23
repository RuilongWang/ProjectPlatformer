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
    [Range(0f, 1f)]
    public float accuracy = 1;
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


    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {

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

    /// <summary>
    /// When our projectile collides with any valid object this method should be called.
    /// By default it will just be destroyed. Otherwise it may do damage or other functionality
    /// if needed
    /// </summary>
    /// <param name="collider"></param>
    private void OnProjectileCollision(Collider2D collider)
    {

    }
}
