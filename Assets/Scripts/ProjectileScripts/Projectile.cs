using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour {

    #region main variables
    public float lauchSpeed = 10;
    public Vector2 launchVector = Vector2.right;

    private BoxCollider2D attachedCollder;

    #endregion main variables

    #region monobehaviour methods
    private void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }
    #endregion monobehaviour methods

    private void UpdateCollisionPoint()
    {

    }
}
