using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour {
    public CharacterStats characterStats;
    public float hitForce;

    [Tooltip("Indicates whether or not a hitbox is currently activated. If true it is allowed to intereact with other hitboxes")]
    public bool currentlyActive;
    [Tooltip("This is a list of all the characters that have been hit by the associated characters hitbox")]
    public List<HitboxManager> charactersHit;


    private List<Hitbox> allHitboxes;


    #region monobehaviour methods
    private void Start()
    {
        if (!this.characterStats)
        {
            Debug.LogWarning("There is no character stats attached to the hit box manager " + transform.name);
        }
    }
    #endregion monobehaviour methods

    public void OnHitboxEntered(Hitbox hitbox)
    {

    }

    public void OnHitboxExited(Hitbox hitbox)
    {

    }

    public void SetHitboxDebugMode(bool setDebug)
    {
        foreach (Hitbox hbox in allHitboxes)
        {
            hbox.SetHitboxToDebugMode(setDebug);
        }
    }
}
