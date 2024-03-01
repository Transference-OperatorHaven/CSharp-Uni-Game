using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PickUp : MonoBehaviour
{
    PlayerStats ps;
    public enum statBuff
    {
        health,
        maxHealth,
        dodgeCooldown,
        dodgeVelocity,
        swordDamage,
        swordRadius,
        swordDuration,
        swordCooldown,
        gunDamage,
        gunSpeed,
        gunRoF,
        reloadTime
    }

    Color buffColour;
    int colorId;
    public statBuff _statBuff;
    [SerializeField] float duration;
    [SerializeField] float strength; //done as a decimal
    public string Name;
    public string Description;
    public bool permanent = false;
    public bool pickedUp;
    SpriteRenderer sr;
    BoxCollider2D bc;
    Rigidbody2D rb;
    [SerializeField]PowerupTextDisplay textDisplay;
    GameObject textUIObject;

    private void Start()
    {
        textDisplay = (PowerupTextDisplay)FindFirstObjectByType<PowerupTextDisplay>(FindObjectsInactive.Include);
        if(!textDisplay)
        {
            Debug.Log("didnt find" + gameObject.name);
        }
    
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        buffColour = sr.color;
        StartCoroutine(CheckPickUp());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        pickedUp = true;
        ps = collision.gameObject.GetComponent<PlayerStats>();
        sr.enabled = false;
        bc.enabled = false;
        textDisplay.DisplayPowerupInfo(Name, Description);
        ps.SetParticleColor(buffColour);

        switch (_statBuff)
        {
            case statBuff.health:
                StartCoroutine(Health());
                break;
            case statBuff.maxHealth:
                if (permanent)
                {
                    MaxHealthPermanent();
                }
                else
                {
                    StartCoroutine(MaxHealthTemp());
                }
                break;
            case statBuff.dodgeCooldown:
                StartCoroutine(DodgeCooldown());
                break;
            case statBuff.dodgeVelocity:
                StartCoroutine(DodgeVelocity());
                break;
            case statBuff.swordDamage:
                StartCoroutine(SwordDamage());
                break;
            case statBuff.swordRadius:
                StartCoroutine(SwordRadius());
                break;
            case statBuff.swordDuration:
                StartCoroutine(SwordDuration());
                break;
            case statBuff.swordCooldown:
                StartCoroutine(SwordCooldown());
                break;
            case statBuff.gunDamage:
                StartCoroutine(GunDamage());
                break;
            case statBuff.gunSpeed:
                StartCoroutine(GunSpeed());
                break;
            case statBuff.gunRoF:
                StartCoroutine(GunRoF());
                break;
            case statBuff.reloadTime:
                StartCoroutine(ReloadTime());
                break;
        }
    
     
    }

    IEnumerator CheckPickUp()
    {
        yield return new WaitForSeconds(8);
        if (!pickedUp && gameObject.name.Contains("(Clone)"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Health()
    {
        ps.ChangeHealth(strength, false);
        yield return new WaitForSeconds(duration);
        ps.RemoveColor(buffColour);
        Destroy(gameObject);

    }

    void MaxHealthPermanent()
    {
        ps.healthMax += strength;
        ps.ChangeHealth(strength, false);
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }
    IEnumerator MaxHealthTemp()
    {
        ps.healthMax += strength;
        ps.ChangeHealth(strength, false);
        yield return new WaitForSeconds(duration);
        ps.healthMax -= strength;
        ps.ChangeHealth(-strength, false);
        ps.RemoveColor(buffColour);
        Destroy(gameObject);

    }

    IEnumerator DodgeCooldown()
    {
        ps.dodgeCooldownLengthModifier += strength;
        ps.UpdateDodgeStats();
        yield return new WaitForSeconds(duration);
        ps.dodgeCooldownLengthModifier -= strength;
        ps.UpdateDodgeStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator DodgeVelocity()
    {
        ps.dodgeVelocityModifier += strength;
        ps.UpdateDodgeStats();
        yield return new WaitForSeconds(duration);
        ps.dodgeVelocityModifier -= strength;
        ps.UpdateDodgeStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator SwordDamage()
    {
        ps.swordDamageModifier += strength;
        ps.UpdateSwordStats();
        yield return new WaitForSeconds(duration);
        ps.swordDamageModifier -= strength;
        ps.UpdateSwordStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator SwordRadius()
    {
        ps.swordRadiusModifier += strength;
        ps.UpdateSwordStats();
        yield return new WaitForSeconds(duration);
        ps.swordRadiusModifier -= strength;
        ps.UpdateSwordStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator SwordDuration()
    {
        ps.swordDurationModifier += strength;
        ps.UpdateSwordStats();
        yield return new WaitForSeconds(duration);
        ps.swordDurationModifier -= strength;
        ps.UpdateSwordStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator SwordCooldown()
    {
        ps.swordCooldownLengthModifier += strength;
        ps.UpdateSwordStats();
        yield return new WaitForSeconds(duration);
        ps.swordCooldownLengthModifier -= strength;
        ps.UpdateSwordStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator GunDamage()
    {
        ps.gunDamageModifier += strength;
        ps.UpdateGunStats();
        yield return new WaitForSeconds(duration);
        ps.gunDamageModifier -= strength;
        ps.UpdateGunStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator GunSpeed()
    {
        ps.gunSpeedModifier += strength;
        ps.UpdateGunStats();
        yield return new WaitForSeconds(duration);
        ps.gunSpeedModifier -= strength;
        ps.UpdateGunStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator GunRoF()
    {
        ps.gunFireRateModifier += strength;
        ps.UpdateGunStats();
        yield return new WaitForSeconds(duration);
        ps.gunFireRateModifier -= strength;
        ps.UpdateGunStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }

    IEnumerator ReloadTime()
    {
        ps.reloadTimeModifier += strength;
        ps.UpdateGunStats();
        yield return new WaitForSeconds(duration);
        ps.reloadTimeModifier -= strength;
        ps.UpdateGunStats();
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }
}