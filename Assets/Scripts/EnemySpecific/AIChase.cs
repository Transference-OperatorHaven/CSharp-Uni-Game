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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        attackPos.localPosition = (rb.velocity.normalized * offset);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, transform.forward, 2, playerLayer);

        if(hit)
        {
            player = hit.transform.gameObject;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        RaycastHit2D attackHit = Physics2D.CircleCast(attackPos.position, attackRadius, transform.forward, 0 , playerLayer);
        if(attackHit)
        {
            player = attackHit.transform.gameObject;
            player.GetComponent<PlayerStats>().ChangeHealth(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
