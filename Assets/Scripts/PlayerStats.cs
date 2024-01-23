using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // ! Health
    [Header("Health Variables")]
    public float playerHealthCurrent;
    public float playerHealthMax = 5;
    // ! Speed Stuff
    [Header("Speed Variables")]
    public float playerSpeed = 1;
    public float playerSpeedMax = 100;


    // ! Abilities stuff
    [Header("Dodge Variables")]
    public bool isDodging = false;
    public bool dodgeSpecialUnlocked;
    public float dodgeDuration, dodgeCooldown, dodgeCooldownLengthBase, dodgeCooldownLengthCurrent, dodgeCooldownLengthModifier, dodgeVelocityBase, dodgeVelocityCurrent, dodgeVelocityModifier;

    [Header("Shoot Variables")]
    public bool isAiming = false;
    public float gunDamageBase, gunDamageCurrent, gunDamageModifier, gunSpeedBase, gunSpeedCurrent, gunSpeedModifier;
    public float reloadTimeBase, reloadTimeCurrent, reloadTimeModifier;

    private void Start()
    {
        UpdateDodgeStats();
        UpdateGunStats();

    }

    public void UpdateDodgeStats()
    { 

        dodgeCooldownLengthCurrent = dodgeCooldownLengthBase * (1 - dodgeCooldownLengthModifier);
        dodgeVelocityCurrent = dodgeVelocityBase * (1 + dodgeVelocityModifier);

    }

    public void UpdateGunStats()
    {

        gunDamageCurrent = gunDamageBase * (1 + gunDamageModifier);
        gunSpeedCurrent = gunSpeedBase * (1 + gunSpeedModifier);

    }

}
