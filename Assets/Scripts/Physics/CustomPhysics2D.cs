using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics2D : MonoBehaviour {
    public Vector2 velocity { get; set; }


    #region monobehavoiur methods
    private void Update()
    {
        UpdatePositionFromVelocity();
    }
    #endregion monobehaviour methods


    private void UpdatePositionFromVelocity()
    {
        Vector3 velocityVector3 = new Vector3(velocity.x, velocity.y, 0);
        this.transform.position += velocityVector3 * Time.deltaTime;
    }
}
