using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] float fireRate, gunSpeed, damage, lifetime, magSize, reloadTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Giving weapon");
            Weapon targetScript = collision.GetComponentInChildren<Weapon>();
            targetScript.ChangeWeaponPrefab(weapon, fireRate, gunSpeed, damage, lifetime, magSize, reloadTime);
            Destroy(gameObject);
        }
        
    }
}
