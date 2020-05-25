using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Hitbox Interactions are handlers that are called anytime. This component would best be attached to our player's sprite.
/// We place it there due to the sprite being the component that flips in scale
/// </summary>
public class HitboxInteraction : MonoBehaviour
{
    /// <summary>
    /// This is the character that is associated with this hitbox interaction component.
    /// </summary>
    public Character AssociatedCharacter;


    #region action events
    /// <summary>
    /// Our hitbox has overlapped with an outside hurtbox. We have hit something
    /// </summary>
    public UnityAction<Hitbox, Hitbox> OnHitboxOverlapOtherHurtbox;
    /// <summary>
    /// Our hurtbox has overlapped with an outside hitbox, We have been hit
    /// </summary>
    public UnityAction<Hitbox, Hitbox> OnHurtboxOverlapOtherHitbox;
    /// <summary>
    /// Our hitbox is no longer overlapping with another hurtbox
    /// </summary>
    public UnityAction<Hitbox, Hitbox> OnHitboxEndOverlapOtherHurtbox;
    /// <summary>
    /// Our hurtbox has ended its overlap with another hitbox
    /// </summary>
    public UnityAction<Hitbox, Hitbox> OnHurtboxEndOverlapOtherHitbox;
    #endregion action events
}
