using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    [SerializeField] public PlayerStats ps;
    [SerializeField] Transform bulletFirePoint;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float bulletLifetime;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
        ps = GetComponentInParent<PlayerStats>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("collided! bullet!");

    }

    
}
