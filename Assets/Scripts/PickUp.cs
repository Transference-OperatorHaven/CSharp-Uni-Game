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
    SpriteRenderer sr;
    BoxCollider2D bc;
    Rigidbody2D rb;
    [SerializeField]PowerupTextDisplay textDisplay;

    private void Start()
    {
        if(FindObjectOfType<PowerupTextDisplay>() != null)
        {
            textDisplay = FindObjectOfType<PowerupTextDisplay>();
            if(textDisplay != null)
            {
                Debug.Log("well done!");
            }
        }
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        buffColour = sr.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collider?");
        
        Debug.Log("Collided!");
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
            case statBuff.reloadTime:
                StartCoroutine(ReloadTime());
                break;
        }
    
     
    }


    IEnumerator Health()
    {
        ps.healthCurrent += strength;
        yield return new WaitForSeconds(duration);
        ps.RemoveColor(buffColour);
        Destroy(gameObject);

    }

    void MaxHealthPermanent()
    {
        ps.healthMax += strength;
        ps.ChangeHealth(strength);
        ps.RemoveColor(buffColour);
        Destroy(gameObject);
    }
    IEnumerator MaxHealthTemp()
    {
        ps.healthMax += strength;
        yield return new WaitForSeconds(duration);
        ps.healthMax -= strength;
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
