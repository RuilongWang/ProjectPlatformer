using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    #region camera variables
    public float cameraLerpSpeed;
    #endregion camera variables


    public Transform currentTargetToFollow { get; private set; }

    #region monobehaviour methods
    private void Start()
    {
        if (currentTargetToFollow == null)
        {
            currentTargetToFollow = transform.parent;
        }
    }

    private void LateUpdate()
    {
        SetCameraPositionBasedOnTargetToFollow();
    }

    #endregion monobehaviour methods

    #region camera set methods
    public void SetNewTargetToFollow(Transform newTargetToFollow)
    {
        this.currentTargetToFollow = newTargetToFollow;
    }

    private void SetCameraPositionBasedOnTargetToFollow()
    {

    }
    #endregion camera set methods
}
