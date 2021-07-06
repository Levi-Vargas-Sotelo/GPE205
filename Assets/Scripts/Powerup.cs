using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
    // Variables for the powerups to do something to the tank that picked them up
    public float speedModifier;
    public float healthModifier;
    public float maxHealthModifier;
    public float fireRateModifier;
    public float shootForceModifier;
    public float bulletDamageModifier;

    // Variables to set the duration of the effects on the tank
    public float duration;
    public bool isPermanent;

    // Function that adds those previous variables to the tank to power it up
    public void OnActivate (TankData target)
    {
        target.moveSpeed += speedModifier;
        target.health += healthModifier;
        target.maxHealth += maxHealthModifier;
        target.fireRate += fireRateModifier;
        target.shootForce += shootForceModifier;
        target.bulletDamage += bulletDamageModifier;
    }

    // Function that removes the variables from the tank due to the powerup duration ending
    public void OnDeactivate (TankData target)
    {
        target.moveSpeed -= speedModifier;
        target.health -= healthModifier;
        target.maxHealth -= maxHealthModifier;
        target.fireRate -= fireRateModifier;
        target.shootForce -= shootForceModifier;
        target.bulletDamage -= bulletDamageModifier;
    }
}
