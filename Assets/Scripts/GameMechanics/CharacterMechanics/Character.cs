using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is the most generic form of character that will be found in our game. Every
/// NPC and playable character should derive from this class
/// </summary>
public class Character : MonoBehaviour
{
    #region main variables
    [Tooltip("The name associated with the character. This is primarily for any dialogue conversations that we may encounter, but it can also be used to to keep track of types of enemies we kill for stats")]
    public string CharacterName = "No Name";
    [Tooltip("The maximum amount of helath this character can have")]
    public float CharacterMaxHealth = 100;
    /// <summary>
    /// The current health of our character. This can only be set through method in this class
    /// </summary>
    public float CharacterCurrentHealth { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public CharacterMovement CharacterMovement { get; private set; }

    /// <summary>
    /// The associated animator component for our character class
    /// </summary>
    public Animator CharacterAnimator { get; private set; }

    #endregion main varialbes


    #region monobehaivour methods
    protected virtual void Awake()
    {
        CharacterCurrentHealth = CharacterCurrentHealth;
        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterAnimator = GetComponent<Animator>();
    }
    #endregion monobehaviour methods

    #region character health methods
    /// <summary>
    /// Use this method to appropriately apply damage to our character
    /// </summary>
    /// <param name="damageTaken"></param>
    /// <param name="characterThatGaveDamage"></param>
    public virtual void CharacterTakeDamage(float damageTaken, Character characterThatGaveDamage = null)
    {
        CharacterCurrentHealth -= damageTaken;
        if (CharacterCurrentHealth <= 0)
        {
            OnCharacterDead();
        }
    }

    public virtual void CharacterAddHealth(float healthPointsToAdd)
    {
        CharacterCurrentHealth += healthPointsToAdd;
        if (CharacterCurrentHealth > CharacterMaxHealth)
        {
            CharacterCurrentHealth = CharacterMaxHealth;
        }
    }

    /// <summary>
    /// This method will be called whenever a player's health falls at or below 0.
    /// </summary>
    public virtual void OnCharacterDead()
    {
        Destroy(this.gameObject);
    }
    #endregion character health methods

}
