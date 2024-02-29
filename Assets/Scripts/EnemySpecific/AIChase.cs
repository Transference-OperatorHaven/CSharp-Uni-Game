using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{

    public GameObject player;
    public Transform attackPos;
    public float speed;
    public float damage;
    public float offset;

    private float distance;
    [SerializeField] float detectionRadius;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask playerLayer;
    Rigidbody2D rb;
    public EnemyHealth eH;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        eH = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!eH.hurt)
        {
            attackPos.localPosition = (rb.velocity.normalized * offset);
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, transform.forward, 2, playerLayer);

            if (hit)
            {
                RaycastHit2D lineOfSight = Physics2D.Raycast(transform.position, hit.transform.position, detectionRadius, 3);
                if (!lineOfSight) 
                {

                    player = hit.transform.gameObject;
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

                }
                
                //rb.AddForce((player.transform.position - transform.position).normalized, ForceMode2D.Force);
            }
            RaycastHit2D attackHit = Physics2D.CircleCast(attackPos.position, attackRadius, transform.forward, 0, playerLayer);
            if (attackHit)
            {
                player = attackHit.transform.gameObject;
                player.GetComponent<PlayerStats>().ChangeHealth(damage, true);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
