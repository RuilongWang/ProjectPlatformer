using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Hitbox Interactions are handlers that are called anytime 
/// </summary>
public class HitboxInteraction : MonoBehaviour
{
    #region action events
    public UnityAction<Hitbox, Hitbox> OnHitEnemy;
    public UnityAction<Hitbox, Hitbox> OnHitByEnemy;
    public UnityAction<Hitbox, Hitbox> OnClashHitboxWithEnemy;
    #endregion action events
}
