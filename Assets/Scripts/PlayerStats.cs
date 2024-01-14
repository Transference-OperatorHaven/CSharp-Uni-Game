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
    [Header("Ability Variables")]
    [HideInInspector]public bool isDodging = false, dodgeSpecialUnlocked;
    public float dodgeCooldown, dodgeCooldownPeriod, dodgeDistance;




}
