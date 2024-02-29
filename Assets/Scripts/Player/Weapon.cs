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
    [SerializeField] float gunStatFireRate, gunStatDamage, gunSpeedModifier, gunMag;
    SpriteRenderer sr;
    public bool reloading;
    public static event System.Action OnFire;
    public static event System.Action OnReloadStarted;
    public static event System.Action OnReloadCompleted;
    public float reloadT;


    private void Start()
    {
        
        controller = gameObject.GetComponentInParent<TopDownCharacterController>();
        ChangeWeaponPrefab(weaponDefault, 0.66f, 24f, -8, 3, 8, 1.5f);



    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = controller.savedDirection * offset;
        Vector3 mousePosition = transform.position - controller.cam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.localRotation = rotation;
        

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
        
        if(ps.gunMagCurrent > 0 && !reloading)
        {
            
            StartCoroutine(FiringCooldown());
            GameObject bulletToSpawn = Instantiate(bulletPrefab, position, Quaternion.identity);
            bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(controller.savedDirection.normalized * ps.gunSpeedCurrent, ForceMode2D.Impulse);
            if(ps.gunMagCurrent > ps.specialReloadRoundsCutOff && ps.specialReloadActive)
            {
                bulletToSpawn.GetComponent<Bullet>().bulletDamage = ps.gunDamageCurrent * 3;
            }
            else
            {
                bulletToSpawn.GetComponent<Bullet>().bulletDamage = ps.gunDamageCurrent;
            }
            
            bulletToSpawn.GetComponent<Bullet>().bulletSpeed = ps.gunSpeedCurrent;
            bulletToSpawn.GetComponent<Bullet>().owner = gameObject;
            bulletToSpawn.GetComponent<Bullet>().bulletLifetime = ps.gunLifetimeBase;
            ps.gunMagCurrent--;
            OnFire?.Invoke();
        }
        else
        {
            StartReload();
        }
        

    }

    IEnumerator FiringCooldown()
    {
        float fireT = 0;
        while (fireT < controller.ps.gunFireRateCurrent)
        {
            fireT += Time.deltaTime;
            yield return null;
        }

        controller.ps.isShooting = false;
    }

    public void StartReload()
    {
        if (!reloading)
        {
            OnReloadStarted?.Invoke();
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        reloadT = 0;
        reloading = true;
        while(reloadT < controller.ps.reloadTimeCurrent)
        {
            reloadT += Time.deltaTime;
            yield return null;
            if(controller.ps.specialReloadActive)
            {
                break;
            }
        }
        Debug.Log("reloading complete");
        controller.ps.UpdateGunStats();
        controller.ps.isShooting = false;
        controller.ps.canSpecialReload = true;
        OnReloadCompleted?.Invoke();
        reloading = false;
    }

    public void ChangeWeaponPrefab(GameObject weaponPrefab, float firerate, float gunspeed, float gundamage, float lifetime, float magSize, float reloadTime)
    {
        if(activeWeapon !=  null)
        {
            Destroy(activeWeapon);
        }
        PlayerStats ps = controller.GetComponent<PlayerStats>();
        ps.gunFireRateBase = firerate;
        ps.gunSpeedBase = gunspeed;
        ps.gunDamageBase = gundamage;
        ps.gunLifetimeBase = lifetime;
        ps.gunMagBase = magSize;
        ps.reloadTimeBase = reloadTime;
        ps.UpdateGunStats();
        activeWeapon = Instantiate(weaponPrefab, transform);
        sr = activeWeapon.GetComponent<SpriteRenderer>();
        OnReloadCompleted?.Invoke();
    }


}
