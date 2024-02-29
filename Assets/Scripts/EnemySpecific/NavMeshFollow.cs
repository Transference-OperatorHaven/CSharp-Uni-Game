using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollow : MonoBehaviour
{

    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    [Header("AttackVariables")]
    bool isAttacking;
    public Transform attackPos;
    public Vector2 attackSize;
    public float attackRange;
    public float attackDamage;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        animator.SetFloat("Speed", agent.speed);
        if (!isAttacking)
        {
            transform.rotation = transform.position.x > target.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
            agent.SetDestination(target.position);  
        }
        if (agent.remainingDistance <= agent.stoppingDistance && !isAttacking)
        {
            
            isAttacking = true;
            StartCoroutine(Attack());
        }

    }

    IEnumerator Attack()
    {

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attacking", true);
        Vector2 distance = transform.position - target.position;;
        float t = 0;
        while(t < 0.583f)
        {
            t += Time.deltaTime;
            RaycastHit2D hit = Physics2D.BoxCast(attackPos.position, attackSize, 0, transform.right);
            if (hit)
            {
                Debug.Log("hit!");
                hit.transform.GetComponent<PlayerStats>().ChangeHealth(attackDamage, true);
            }
        }
        
        yield return new WaitForSeconds(1f);
        isAttacking=false;
        animator.SetBool("Attacking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPos.position, attackSize);
    }

}
