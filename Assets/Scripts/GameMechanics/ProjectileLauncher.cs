using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will handle launching objects of the Projectile class
/// </summary>
public class ProjectileLauncher : MonoBehaviour {
    [Tooltip("This is the transform that our projectile object will be launched at. We will take the forward direction and launch our object based on that")]
    public Transform launchPoint;
    public Projectile associatedProjectileToLaunch;

    /// <summary>
    /// The associated character that launched our projectile object
    /// </summary>
    private CharacterStats associatedCharacterStats;

    #region monobehaviour methods
    private void Start()
    {

        associatedCharacterStats = GetComponentInParent<CharacterStats>();
        SpawnPool.Instance.AddObjectsToSpawnPoolIfNotAddedAlready(associatedProjectileToLaunch);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            LaunchProjectile();
        }
    }
    #endregion monobehaviour methods
    /// <summary>
    /// Call this method when you want to launch the associated projectile
    /// </summary>
    public void LaunchProjectile()
    {
        Projectile projectileToLaunch = (Projectile)SpawnPool.Instance.Spawn(associatedProjectileToLaunch);
        projectileToLaunch.SetUpProjectile(associatedCharacterStats, launchPoint.position);
        projectileToLaunch.LaunchProjectile(-Mathf.Sign(transform.parent.localScale.x) * this.transform.right);
    }


}
