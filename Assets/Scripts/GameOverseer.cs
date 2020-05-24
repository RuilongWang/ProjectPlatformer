using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Our overseer acts as the primary In-Game Manager. This method should be included in any scene where
/// our game is playing. Menus will not need this overseer object
/// </summary>
public class GameOverseer : MonoBehaviour
{
    #region const variables
    public const int TARGET_FRAME_RATE = 60;
    #endregion const variables


    #region static variables
    public static float DELTA_TIME
    {
        get
        {
            //return Time.deltaTime;
            return 1f / TARGET_FRAME_RATE * Time.timeScale;
        }
    }

    public static int FRAME_COUNT { get; private set; }

    private static GameOverseer instance;

    public static GameOverseer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameOverseer>();
            }
            return instance;
        }
    }
    #endregion static varaibles

    #region singleton references
    public PhysicsManager PhysicsManager;
    public HitboxManager HitboxManager;
    #endregion singleton references

    #region monobehaviour methods
    private void Awake()
    {
        instance = this;
        GameOverseer.FRAME_COUNT = 0;
        Application.targetFrameRate = TARGET_FRAME_RATE;
    }

    private void Update()
    {
        ++GameOverseer.FRAME_COUNT;
    }


    private void LateUpdate()
    {
        HitboxManager.UpdateHitboxManager();
        PhysicsManager.UpdatePhysicsManager();
    }
    #endregion monobehaviour methods
}
