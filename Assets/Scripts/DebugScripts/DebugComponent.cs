using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DebugComponent : MonoBehaviour
{
    public enum DebugType : uint
    {
        CHARACTER,  //Anything involving character stats
        PHYSICS,    //Anything involving physics
        HITBOXES,   //Anything involving hitbox interactions
    }

    #region monobehaviour methods
    protected virtual void Awake()
    {

    }

    protected virtual void OnDestroy()
    {

    }
    #endregion monobehaviour methods


    /// <summary>
    /// Method will be called anytime Debug options are turned on
    /// </summary>
    public abstract void OnBeginDebug();

    /// <summary>
    /// Method will be called anytime Debug options are turned off
    /// </summary>
    public abstract void OnEndDebug();

    /// <summary>
    /// Returns the debug option type so that we can categorize each debug object
    /// </summary>
    /// <returns></returns>
    public abstract DebugType GetDebugType();
}
