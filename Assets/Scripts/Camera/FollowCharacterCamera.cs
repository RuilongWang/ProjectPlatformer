using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for our camera gameobject that follows our player character
/// </summary>
public class FollowCharacterCamera : MonoBehaviour
{
    public float CameraFollowSpeed = 20;

    /// <summary>
    /// The targeted character to follow
    /// </summary>
    private GamePlayCharacter TargetCharacter;

    private Vector3 TargetPosition;
    private Vector3 TargetOffset;

    #region monobehaviour methods
    private void Awake()
    {
        TargetCharacter = GetComponentInParent<GamePlayCharacter>();
        if (TargetCharacter == null)
        {
            Debug.LogError("No target character was found. Please be sure to place the camera component as a child of the desired character to follow.");
            return;
        }
        
        TargetOffset = this.transform.position - TargetCharacter.transform.position;
        this.transform.SetParent(null);//Release from our character
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }
    #endregion monobehaviour methods


    private void UpdateCameraPosition()
    {
        TargetPosition = TargetCharacter.transform.position + TargetOffset;

        this.transform.position = Vector3.Lerp(this.transform.position, TargetPosition, GameOverseer.DELTA_TIME * CameraFollowSpeed);
    }
}
