using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WizardNavMesh : MonoBehaviour
{
    LineRenderer line;
    [SerializeField]GameObject target;
    Transform targetTF;
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] EnemyHealth eH;
    BoxCollider2D bC;
    float speed;
    public GameObject fireballPrefab, lightningStormPrefab;
    [Header("Attack Logic")]
    [SerializeField]bool isAttacking, hover1Triggered, inHover, hover2Triggered, finalStandTriggered;
    [SerializeField]float timeSinceLastFireBall;
    [SerializeField]float timeSinceLastAttack;
    public float 
        globalAttackCD,
        fireballCD,
        fireballWindUp,
        fireballAnimDuration,
        lightningWindUp,
        lightningAnimDuration,
        healthPercentFirstHover,
        healthPercentSecondHover,
        hoverSpeedMultiplier,
        finalStandPercent,
        finalStandSpeed,
        finalStandCD,
        finalStandFireballCD
    ;
    


    [Header("Animation specifics")]
    public float deathAnimTime;


    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        target = GameObject.FindWithTag("Player");
        targetTF = target.transform;
        bC = GetComponent<BoxCollider2D>();
        eH = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        timeSinceLastFireBall = Time.time + fireballCD;
    }

    // Update is called once per frame
    void Update()
    {

        if (eH.currentHealth / eH.maxHealth <= healthPercentFirstHover &&!hover1Triggered)
        {
            agent.stoppingDistance = 0;
            if (!isAttacking && agent.speed > 0 && !agent.isStopped)
            {
                inHover = true;
                hover1Triggered = true;
                Debug.Log("before corountine)");
                StartCoroutine(HoverAttack());
            }
        }
        if(eH.currentHealth / eH.maxHealth <= healthPercentSecondHover && !hover2Triggered)
        {
            agent.stoppingDistance = 0;
            if (hover1Triggered && !isAttacking && agent.speed > 0 && !agent.isStopped)
            {
                inHover = true;
                hover2Triggered = true;
                Debug.Log("before corountine)");
                StartCoroutine(HoverAttack());
            }
        }
        if(eH.currentHealth / eH.maxHealth <= finalStandPercent && !finalStandTriggered && hover2Triggered)
        {  
            finalStandTriggered = true;
            FINALSTAND();
        }

        if(inHover && agent.remainingDistance > 0.1 )
        {
            agent.stoppingDistance = 0;
            agent.speed = speed;
            agent.SetDestination(new Vector3(0, 1, 0));
        }

        if (target != null)
        {
            if (eH.hurt && !isAttacking)
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
            if (!isAttacking && !eH.dead && !eH.hurt && !inHover)
            {
                transform.rotation = transform.position.x > targetTF.position.x ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
                agent.SetDestination(targetTF.position);
                agent.isStopped = false;
                agent.speed = speed;
            }
            if (agent.remainingDistance <= agent.stoppingDistance && !inHover)
            {
                agent.isStopped = true;
                agent.speed = 0;
                if (timeSinceLastAttack <= Time.time && !isAttacking && !eH.hurt & !eH.dead && !inHover)
                {
                    
                    isAttacking = true;
                    StartCoroutine(Attack());
                }
            }
            
        }
        
    }

    IEnumerator Attack()
    {
        if (timeSinceLastFireBall <= Time.time)
        {
            animator.SetBool("Attacking", true);
            animator.SetBool("Fireball", true);
            yield return new WaitForSeconds(fireballWindUp);
            Debug.Log("Fireball!");
            Fireball();
            timeSinceLastFireBall = fireballCD + Time.time;
            timeSinceLastAttack = globalAttackCD + Time.time;
            yield return new WaitForSeconds(fireballAnimDuration - fireballWindUp);
            isAttacking = false;
            animator.SetBool("Attacking", false);
            animator.SetBool("Fireball", false);
        }
        else
        {
            animator.SetBool("Attacking", true);
            yield return new WaitForSeconds(lightningWindUp);
            Debug.Log("Lighting!");
            Lightning();
            timeSinceLastAttack = globalAttackCD + Time.time;
            yield return new WaitForSeconds(lightningAnimDuration - lightningWindUp);
            isAttacking = false;
            animator.SetBool("Attacking", false);
            
        }
        yield return null;
        isAttacking = false;
        agent.speed = speed;
    }

    void Fireball()
    {
        
        GameObject fireballCasted = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Vector3 direction = fireballCasted.transform.position - targetTF.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireballCasted.transform.rotation = Quaternion.Euler(0,0,angle);
        fireballCasted.GetComponent<Rigidbody2D>().AddForce((direction.normalized * -1) * fireballCasted.GetComponent<FireBall>().speed, ForceMode2D.Impulse);
    }
    void Lightning()
    {
        Debug.Log("Lightning!");
        GameObject lightningStormCasted = Instantiate(lightningStormPrefab, transform.position, Quaternion.identity);
        Vector3 direction = lightningStormCasted.transform.position - targetTF.transform.position;
        lightningStormCasted.GetComponent<LightningStorm>().direction = direction;
    }

    IEnumerator HoverAttack()
    {
        Debug.Log("After Coroutine");
        gameObject.layer = 0;
        animator.SetBool("Hovering", true);
        eH.ChangeHealth(0);
        agent.stoppingDistance = 0;
        
        yield return new WaitUntil(() => agent.remainingDistance < 0.1);
        agent.speed = 0;
        for (int i = 0; i < 100; i++)
        {
            Lightning();
            yield return new WaitForSeconds(0.2f);
        }

        gameObject.layer = 8;
        inHover = false;
        agent.stoppingDistance = 8;
        animator.SetBool("Hovering", false);
    }

    void FINALSTAND()
    {
        PowerupTextDisplay powerupThreat = (PowerupTextDisplay)FindFirstObjectByType<PowerupTextDisplay>(FindObjectsInactive.Include);
        powerupThreat.DisplayPowerupInfo("ENRAGED", "150 health left");
        agent.stoppingDistance = 4;
        speed = speed * finalStandSpeed;
        globalAttackCD *= finalStandCD;
        fireballCD *= finalStandFireballCD;

        
    }

}
