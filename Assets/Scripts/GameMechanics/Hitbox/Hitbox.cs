using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hitbox : MonoBehaviour {
    public bool debugSettingsActive = false;
    [Tooltip("For debugging purposes only. Visually shows the position, size and rotation of hitboxes")]
    public SpriteRenderer hitboxSprite;
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
