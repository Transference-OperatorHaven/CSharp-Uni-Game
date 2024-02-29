using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NavMeshFollow : MonoBehaviour
{

    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    EnemyHealth eH;
    float speed;
    [Header("Attack Variables")]
    bool isAttacking;
    public Transform attackPos;
    public Vector2 attackSize;
    public float attackRange;
    public float attackDamage;
    public float attackWindUp;
    public float preAttackTime;
    public float attackDuration;
    public float attackCooldown;
    // Start is called before the first frame update
    void Start()
    {
        
        eH = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
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
        if (agent.remainingDistance > agent.stoppingDistance && !isAttacking && (Mathf.Abs(target.transform.position.y - transform.position.y) < 2))
        {
            agent.isStopped = true;
            agent.speed = 0;
            isAttacking = true;
            StartCoroutine(Attack());
        }
        
    }

    IEnumerator Attack()
    {

        yield return new WaitForSeconds(attackWindUp);
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(preAttackTime);
        RaycastHit2D[] hit = Physics2D.BoxCastAll(attackPos.position, attackSize, 0, transform.right, 0);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.tag == "Player")
            {
                Debug.Log(hit[i].transform.name);
                hit[i].transform.GetComponent<PlayerStats>().ChangeHealth(attackDamage, true);
            }
        }
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(attackCooldown);
        
        isAttacking = false;
        agent.speed = speed;
        agent.isStopped = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPos.position, attackSize);
    }

}
