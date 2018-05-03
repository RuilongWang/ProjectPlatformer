using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour {
    /// <summary>
    /// The real time that has passed every frame.
    /// </summary>
    public const float timeBetweenFrames = 1f / 60f;

    /// <summary>
    /// The time that has passed based on real time that has passed
    /// multiplied by inGameTimeScale
    /// </summary>
    public static float inGameDeltaTime
    {
        get
        {
            return timeBetweenFrames * inGameTimeScale;
        }
    }

    /// <summary>
    /// The scale value of time that has passed in game
    /// </summary>
    public static float inGameTimeScale = 1;


    #region monobehaviour methods
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    #endregion monobehaviour methods
    /// <summary>
    /// Get the amount of time that has passed since
    /// </summary>
    /// <param name="timeStarted"></param>
    /// <returns></returns>
    public float RealTimeSince(float timeStarted)
    {
        return Time.time - timeStarted;
    }
}
