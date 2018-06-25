using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour {

    #region main variables
    public Vector2 velocity { get; set; }

    public float lauchSpeed = 10;
    public Vector2 launchVector = Vector2.right;

    private BoxCollider2D attachedCollder;
    private CharacterStats characterThatLaunchedProjectile;

    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        
    }

    protected virtual void Update()
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
}
