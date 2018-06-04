using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hitbox : MonoBehaviour {
    [Header("Debug Variables")]
    [Tooltip("Mark this value true if you want the hitbox sprites to be visible")]
    public bool debugSettingsActive = false;
    [Tooltip("For debugging purposes only. Visually shows the position, size and rotation of hitboxes")]
    public SpriteRenderer hitboxSprite;

    [Header("Hitbox Variables")]
    [Tooltip("This is the raw damage this hitbox will output before applying multipliers or other boons or banes")]
    public float damageToApply;
    [Tooltip("The direction that a knockback force that will be applied to a hit target. NOTE: If you do not want an enemy to have any knockback, you can set knockback force to 0")]
    public Vector2 knockBackDirection;
    [Tooltip("The force that will be applied to a hit enemy. You can set the knockback force to 0 if you do not want our enemy to be knocked back at all")]
    public float knockBackForce;
    [Tooltip("The amount of time in seconds that an enemy will be stuck in hit stun if hit by this particular move. Setting this value to 0 will not effect a hit enemy")]
    public float hitStunTime = .2f;
    /// <summary>
    /// This object should be found in the parent object of the hitbox
    /// </summary>
    protected HitboxManager associatedHitboxManager;

    #region monobehaviour methods
    private void Start()
    {
        associatedHitboxManager = this.transform.parent.GetComponent<HitboxManager>();
        if (!associatedHitboxManager)
        {
            Debug.LogWarning("There is NO HitboxManager associated with this Hitbox object");
        }
        else
        {
            associatedHitboxManager.allHitboxes.Add(this);
        }

        SetHitboxToDebugMode(this.debugSettingsActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hitbox hitbox = collision.GetComponent<Hitbox>();
        if (hitbox)
        {
            this.associatedHitboxManager.OnHitboxEntered(hitbox);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Hitbox hitbox = collision.GetComponent<Hitbox>();

    }
    #endregion monobehaviour methods

    public void SetHitboxToDebugMode(bool setDebug)
    {
        this.debugSettingsActive = setDebug;
        hitboxSprite.gameObject.SetActive(setDebug);
    }



}
