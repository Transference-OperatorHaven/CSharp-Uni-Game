using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float strength; //done as a decimal
    public string Name;
    public string Description;
    PlayerStats ps;
    SpriteRenderer sr;
    [SerializeField]PowerupTextDisplay textDisplay;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided!");
        ps = collision.gameObject.GetComponent<PlayerStats>();
        ps.swordDamageModifier += strength;
        ps.UpdateSwordStats();
        textDisplay.DisplayPowerupInfo(Name, Description);
        sr.enabled = false;
        StartCoroutine(PowerupDuration());
    }


    IEnumerator PowerupDuration()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;

            yield return null;
        }
        ps.swordDamageModifier -= strength;
        ps.UpdateSwordStats();
        Destroy(gameObject);
    }
}
