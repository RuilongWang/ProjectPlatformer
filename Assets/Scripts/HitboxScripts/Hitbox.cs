using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all types of hitboxes.
/// </summary>
public abstract class Hitbox : MonoBehaviour
{
    #region enums
    public enum HitboxType
    {
        Hitbox,
        Hurtbox,
    }
    #endregion enums

    #region debug references
    public Color DebugDrawColor
    {
        get
        {
            switch (mHitboxType)
            {
                case HitboxType.Hitbox:
                    return Color.red;
                case HitboxType.Hurtbox:
                    return Color.cyan;
                default:
                    return Color.white;
            }
        }
    }
    #endregion debug references

    #region hitbox variables
    public HitboxType mHitboxType;
    #endregion hitbox variables

    #region monobehaviour methods
    protected virtual void Awake()
    {
        GameOverseer.Instance.HitboxManager.AddHitboxToManager(this);
    }

    protected virtual void OnDestroy()
    {
        GameOverseer.Instance.HitboxManager.RemoveHitboxFromManager(this);
    }
    #endregion monobehaviour methods
    public abstract void UpdateHitboxBounds();
}
