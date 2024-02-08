using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Sword Variables")]
    public bool isAttacking = false;
    public float swordDamageBase, swordDamageCurrent, swordDamageModifier;
    public float swordRadiusBase, swordRadiusCurrent, swordRadiusModifier;
    public float swordDurationBase, swordDurationCurrent, swordDurationModifier;
    public float swordCooldown, swordCooldownLengthBase, swordCooldownLengthCurrent, swordCooldownLengthModifier;

    [Header("Shoot Variables")]
    public bool isAiming = false;
    public float gunDamageBase, gunDamageCurrent, gunDamageModifier, gunSpeedBase, gunSpeedCurrent, gunSpeedModifier;
    public float reloadTimeBase, reloadTimeCurrent, reloadTimeModifier;

    [Header("Particles")]
    [SerializeField] Vector4 allColors;
    [SerializeField]Color particleColor;
    ParticleSystem ps;
    [SerializeField]int numberOfBuffs = 0;
    [SerializeField]ParticleSystem.MainModule psMain;

    private void Start()
    {
        UpdateDodgeStats();
        UpdateGunStats();
        UpdateSwordStats();
        ps = GetComponentInParent<ParticleSystem>();
        psMain = ps.main;
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
        reloadTimeCurrent = reloadTimeBase * (1 + reloadTimeModifier);

    }

    public void UpdateSwordStats()
    {
        swordRadiusCurrent = swordRadiusBase * (1 + swordRadiusModifier);
        swordDurationCurrent = swordDurationBase * (1 + swordDurationModifier);
        swordCooldownLengthCurrent = swordCooldownLengthBase;
        swordDamageCurrent = swordDamageBase * (1 + swordDamageModifier);
    }

    void CalculateParticleColorAndApply()
    {
        particleColor.r = allColors.x / numberOfBuffs;
        particleColor.g = allColors.y / numberOfBuffs;
        particleColor.b = allColors.z/ numberOfBuffs;
        particleColor.a = 255;
        psMain.startColor = particleColor;
    }

    public void SetParticleColor(Color color)
    {
        Debug.Log(color);
        numberOfBuffs += 1;
        if(numberOfBuffs > 0)
        {
            ps.Play(true);
        }
        allColors += new Vector4 (color.r,color.g,color.b,color.a);
        CalculateParticleColorAndApply();

    }

    public void RemoveColor(Color color)
    {
        numberOfBuffs -= 1;
        if(numberOfBuffs <= 0)
        {
            ps.Stop(true ,ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        allColors -= new Vector4(color.r, color.g, color.b, color.a);
        CalculateParticleColorAndApply ();

    }

}
