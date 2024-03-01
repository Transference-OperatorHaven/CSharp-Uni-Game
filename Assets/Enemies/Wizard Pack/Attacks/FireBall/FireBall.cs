using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]ParticleSystem ps, ps2;
    [SerializeField]CircleCollider2D cC,cC2;
    Rigidbody2D rb;
    [SerializeField] public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
        rb = GetComponent<Rigidbody2D>();
        cC2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            ps.Stop();
            if (collision.transform.tag == "Player")
            {
                collision.transform.GetComponent<PlayerStats>().ChangeHealth(-2, true);
                cC2.enabled=true;
                ps.Stop();
                ps2.Play();
                Destroy(gameObject, 0.2f);
            }
            if (collision.transform.gameObject.layer == 3)
            {
                cC2.enabled = true;
                ps.Stop();
                ps2.Play();
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerStats>().ChangeHealth(-2, true);
        }
    }
}
