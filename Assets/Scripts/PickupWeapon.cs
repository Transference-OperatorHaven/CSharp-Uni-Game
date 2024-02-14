using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] float fireRate, gunSpeed, damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<Weapon>() != null)
        {
            collision.gameObject.GetComponentInChildren<Weapon>().ChangeWeaponPrefab(weapon, fireRate, gunSpeed, damage);
            Destroy(gameObject);
        }
       

    }
}
