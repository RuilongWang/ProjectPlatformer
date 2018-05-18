using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    #region camera variables
    public float cameraLerpSpeed;
    public Vector3 offsetFromParent;

    #endregion camera variables


    public Transform currentTargetToFollow { get; private set; }

    #region monobehaviour methods
    private void Start()
    {
        if (currentTargetToFollow == null)
        {
            currentTargetToFollow = transform.parent;
        }

        offsetFromParent = transform.position - currentTargetToFollow.position;
    }

    private void LateUpdate()
    {
        SetCameraPositionBasedOnTargetToFollow();
    }

    #endregion monobehaviour methods

    #region camera set methods
    /// <summary>
    /// Sets a new target to follow.
    /// </summary>
    /// <param name="newTargetToFollow"></param>
    public void SetNewTargetToFollow(Transform newTargetToFollow, Vector2 offsetFromParent)
    {
        this.offsetFromParent = offsetFromParent;
        this.currentTargetToFollow = newTargetToFollow;
    }

    /// <summary>
    /// Transitions the camera position toward the target
    /// </summary>
    private void SetCameraPositionBasedOnTargetToFollow()
    {
        transform.position = currentTargetToFollow.position + offsetFromParent;
    }
    #endregion camera set methods
}
