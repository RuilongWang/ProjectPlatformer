using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {
    public float maxHealth = 100;
    public float currentHealth { get; protected set; }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
    }

    public void Heal(float hpAdded)
    {
        currentHealth += hpAdded;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
    }
}
