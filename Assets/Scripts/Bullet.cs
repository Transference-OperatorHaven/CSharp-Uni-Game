using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    [SerializeField] public GameObject owner;
    [SerializeField] Transform bulletFirePoint;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float bulletLifetime;
    [SerializeField] LayerMask enemyLayer;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision != null)
        {
            
            if (collision.gameObject.layer == 8)
            {
                Debug.Log("Hit!");
                collision.gameObject.GetComponent<EnemyHealth>().ChangeHealth(bulletDamage);
                if( owner.GetComponent<ComboSystem>() != null) { owner.GetComponent<ComboSystem>().ComboIncrease(bulletDamage); }
                
                Destroy(gameObject);
            }

        }
    }

    
}
