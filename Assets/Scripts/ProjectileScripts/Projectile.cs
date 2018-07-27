using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for projectiles
/// </summary>

[RequireComponent(typeof(CustomPhysics2D))]
public class Projectile : MonoBehaviour {
    #region const varialbes
    public const string ANIMATION_HIT = "Hit";
    #endregion const variables

    #region main variables
    [Range(0f, 1f)]
    [Tooltip("Change this value if you want there to be some variability when a bullet fires so that it does not always fire perfectly in the same location.")]
    public float accuracy = 1;
    [Tooltip("This is the desired launch speed of the projectile")]
    public float lauchSpeed = 10;
    /// <summary>
    /// The direction that we will launch our projectile at. This will be set at run time
    /// </summary>
    public Vector2 launchVector { get; set; }

    private BoxCollider2D attachedCollder;
    private CharacterStats associatedCharacterThatFiredProjectile;
    public CustomPhysics2D rigid { get; private set; }
    public Animator anim { get; private set; }
    private ProjectileHitboxManager hitboxManager;

    #endregion main variables

    #region monobehaviour methods
    private void Awake()
    {
        rigid = GetComponent<CustomPhysics2D>();
        anim = GetComponent<Animator>();
        hitboxManager = GetComponent<ProjectileHitboxManager>();
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
    public void SetUpProjectile(CharacterStats characterThatLaunchedProjectile, Vector3 originPoint)
    {
        this.transform.position = originPoint;
        this.associatedCharacterThatFiredProjectile = characterThatLaunchedProjectile;
    }

    /// <summary>
    /// 
    /// </summary>
    public void LaunchProjectile(Vector2 launchDirection)
    {
        launchDirection = launchDirection.normalized;

        this.transform.right = launchDirection;
        this.rigid.velocity = launchDirection * lauchSpeed;
    }

    /// <summary>
    /// Updates the rotation of the projectile to match the velocity that it is currently travelling at
    /// </summary>
    private void UpdateRotationBasedOnVelocity()
    {
        Vector2 velocityUnityVector = rigid.velocity.normalized;
        this.transform.right = velocityUnityVector;
    }

    /// <summary>
    /// When our projectile collides with any valid object this method should be called.
    /// By default it will just be destroyed. Otherwise it may do damage or other functionality
    /// if needed
    /// </summary>
    /// <param name="collider"></param>
    public void OnProjectileCollision(Collider2D collider, Vector3 positionOfImpact)
    {
        transform.position = positionOfImpact;
        anim.SetTrigger(ANIMATION_HIT);
        rigid.enabled = false;
        hitboxManager.DeactivateHitboxManager();
    }
}
