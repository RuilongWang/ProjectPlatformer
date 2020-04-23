﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Our overseer acts as the primary In-Game Manager. This method should be included in any scene where
/// our game is playing. Menus will not need this overseer object
/// </summary>
public class Overseer : MonoBehaviour
{
    #region static variables
    public static float DELTA_TIME
    {
        get
        {
            return 1f / 60f * Time.timeScale;
        }
    }

    public static int FRAME_COUNT { get; private set; }

    private static Overseer instance;

    public static Overseer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Overseer>();
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
        Overseer.FRAME_COUNT = 0;
    }

    private void Update()
    {
        ++Overseer.FRAME_COUNT;
    }

    #endregion monobehaviour methods
}
