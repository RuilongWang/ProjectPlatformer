using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// We will have this as a child object of our character
/// </summary>
public class HitboxManager : MonoBehaviour {
    public CharacterStats characterStats;
    public float hitForce;

    [Tooltip("Indicates whether or not a hitbox is currently activated. If true it is allowed to intereact with other hitboxes")]
    public bool currentlyActive;//Could be useful for invincibility

    /// <summary>
    /// This is a list of all the characters that have been hit by the associated characters hitbox
    /// </summary>
    public List<HitboxManager> listOfCharactersHit { get; set; }

    /// <summary>
    /// A list of all attached hitboxes that are attached to this hitbox manager
    /// </summary>
    public List<Hitbox> allHitboxes { get; set; }


    #region monobehaviour methods
    private void Awake()
    {
        listOfCharactersHit = new List<HitboxManager>();
        allHitboxes = new List<Hitbox>();
    }

    private void Start()
    {
        if (!this.characterStats)
        {
            Debug.LogWarning("There is no character stats attached to the hit box manager " + transform.name);
        }
    }

    private void OnEnable()
    {
        
    }
    #endregion monobehaviour methods

    /// <summary>
    /// If a hitbox has entered, this method should be called in the hitbox's manager
    /// object
    /// </summary>
    /// <param name="hitbox"></param>
    public void OnHitboxEntered(Hitbox hitbox)
    {
        if (!currentlyActive) return;
    }

    /// <summary>
    /// If a hitbox was exited, the hitbox should call this method in the hitbox
    /// manager object
    /// </summary>
    /// <param name="hitbox"></param>
    public void OnHitboxExited(Hitbox hitbox)
    {
        if (!currentlyActive) return;
    }

    /// <summary>
    /// Shows off all the hitboxes visually to make debugging a bit easier
    /// </summary>
    /// <param name="setDebug"></param>
    public void SetHitboxDebugMode(bool setDebug)
    {
        foreach (Hitbox hbox in allHitboxes)
        {
            hbox.SetHitboxToDebugMode(setDebug);
        }
    }
}
