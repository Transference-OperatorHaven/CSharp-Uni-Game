using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollow : MonoBehaviour
{
    [Header("Target (should be player)")]
    public Transform target;
    NavMeshAgent agent;
    bool isAttacking;
    [Header("Attack Variables")]
    public Vector2 attackSize;
    public float attackRange;
    public float attackDamage;
    int attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            agent.SetDestination(target.position);  
        }
        attackPoint = transform.position.x > target.position.x ? -1 : 1;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }

    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 distance = transform.position - target.position;
        float angleOfAttack = Mathf.Atan2(distance.x, distance.y) * Mathf.Rad2Deg;
        
        float t = 0;
        while(t < 0.33f)
        {
            t += Time.deltaTime;
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, attackSize, angleOfAttack, transform.forward * attackPoint, attackRange, 6);
            if (hit)
            {
                hit.transform.GetComponent<PlayerStats>().ChangeHealth(attackDamage, true);
            }
        }
        
        yield return new WaitForSeconds(1f);
        isAttacking=false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + transform.forward * attackPoint, attackSize);
    }

}
