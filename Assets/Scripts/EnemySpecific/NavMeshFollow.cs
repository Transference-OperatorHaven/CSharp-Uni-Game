using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NavMeshFollow : MonoBehaviour
{

    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    EnemyHealth eH;
    BoxCollider2D bC;
    CircleCollider2D cC;
    public float detectionRadius, circleOffset;
    ParticleSystem ps;
    ParticleSystem.ShapeModule _shape;
    ParticleSystem.MainModule _main;
    float speed;
    [Header("AttackVariables")]
    [SerializeField]bool isAttacking;
    public Transform attackPos;
    public Vector2 attackSize;
    public float attackRange;
    public float attackDamage;
    public float attackWindUp;
    public float preAttackTime;
    public float attackDuration;
    public float attackCooldown;
    public float deathAnimTime;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
        bC = GetComponent<BoxCollider2D>();
        cC = GetComponent<CircleCollider2D>();
        cC.offset = new Vector2(circleOffset,0);
        cC.radius = detectionRadius;
        eH = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        _main = ps.main;
        _main.duration = attackDuration - attackWindUp;
        _main.startLifetime = attackDuration;
        _shape = ps.shape;
        _shape.scale = attackSize;
    }

    // Update is called once per frame
    void Update()
    {
        _shape.position = attackPos.localPosition;

        if (target != null)
        {
            if (eH.hurt)
            {
                animator.Play("Hurt");
                
            }
            if (eH.dead)
            {
                agent.isStopped = true;
                bC.enabled = false;
                animator.SetBool("dead", true);
                Destroy(gameObject, deathAnimTime);
            }
            animator.SetFloat("Speed", agent.speed);
            if (!isAttacking && !eH.dead)
            {
                transform.rotation = transform.position.x > target.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
                agent.SetDestination(target.position);
            }
            if (agent.remainingDistance <= agent.stoppingDistance && !isAttacking && (Mathf.Abs(target.transform.position.y - transform.position.y) < 2) && !eH.hurt & !eH.dead)
            {
                agent.isStopped = true;
                agent.speed = 0;
                isAttacking = true;
                StartCoroutine(Attack());
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            target = collision.transform;
            cC.enabled = false;
        }
    }

    IEnumerator Attack()
    {

        yield return new WaitForSeconds(attackWindUp);
        animator.SetBool("Attacking", true);
        yield return new WaitForSeconds(preAttackTime);
        ps.Play();
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
        ps.Stop();
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
