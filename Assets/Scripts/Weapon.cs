using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    TopDownCharacterController controller;
    public Transform playerTransform;
    [SerializeField] public GameObject activeWeapon;
    [SerializeField] GameObject weaponDefault;
    [SerializeField]float offset;
    [SerializeField] float gunStatFireRate, gunStatDamage, gunSpeedModifier;
    SpriteRenderer sr;

    private void Start()
    {
        
        controller = gameObject.GetComponentInParent<TopDownCharacterController>();
        playerTransform = gameObject.GetComponentInParent<Transform>();
        ChangeWeaponPrefab(weaponDefault, 0.66f, 24f, 8);



    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = controller.savedDirection * offset;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        if(controller.ps.isAiming)
        {
            sr.enabled = true;
        }
        else
        {
            sr.enabled=false;
        }



    }

    public void Fire(GameObject bulletPrefab, Vector2 position, PlayerStats ps)
    {
        StartCoroutine(FiringCooldown());
        Debug.Log("Firing!");
        GameObject bulletToSpawn = Instantiate(bulletPrefab, position, Quaternion.identity);
        bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(controller.savedDirection.normalized * ps.gunSpeedCurrent, ForceMode2D.Impulse);
        bulletToSpawn.GetComponent<Bullet>().bulletDamage = ps.gunDamageCurrent;
        bulletToSpawn.GetComponent<Bullet>().bulletSpeed = ps.gunSpeedCurrent;
        bulletToSpawn.GetComponent<Bullet>().owner = gameObject;
        bulletToSpawn.GetComponent<Bullet>().bulletLifetime = ps.gunLifetimeCurrent;

    }

    IEnumerator FiringCooldown()
    {
        float t = 0;
        while (t < controller.ps.gunFireRateCurrent)
        {
            t += Time.deltaTime;
            yield return null;
        }

        controller.ps.isShooting = false;
    }

    public void ChangeWeaponPrefab(GameObject weaponPrefab, float firerate, float gunspeed, float gundamage)
    {
        if(activeWeapon !=  null)
        {
            Destroy(activeWeapon);
        }
        controller.GetComponent<PlayerStats>().gunFireRateBase = firerate;
        controller.GetComponent<PlayerStats>().gunSpeedBase = gunspeed;
        controller.GetComponent<PlayerStats>().gunDamageBase = gundamage;
        controller.GetComponent<PlayerStats>().UpdateGunStats();
        activeWeapon = Instantiate(weaponPrefab, transform);
        sr = activeWeapon.GetComponent<SpriteRenderer>();
    }


}
