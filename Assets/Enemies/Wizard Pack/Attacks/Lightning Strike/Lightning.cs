using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    float damage = -1;

    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerStats>().ChangeHealth(damage, true);
        }
    }
}
