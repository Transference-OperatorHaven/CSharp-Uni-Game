using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    private PlayerStats ps;
    private CircleCollider2D cCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInParent<PlayerStats>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("collided! bullet!");

    }

    
}
