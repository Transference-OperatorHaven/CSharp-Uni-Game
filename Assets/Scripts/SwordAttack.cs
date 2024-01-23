using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    private PlayerStats ps;
    public LayerMask attackLayer;
    public Transform attackPos;
    public Collider2D[] objectsHit;
    RaycastHit2D[] hits;
    [SerializeField] TopDownCharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerStats>();
        controller = GetComponent<TopDownCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.isAttacking)
        {
            if (Input.GetAxisRaw("Attack") == 1)
            {
                if (ps.attackCooldown < Time.time)
                {
                    StartCoroutine(Attack());
                }

            }
        }     
    }

    private IEnumerator Attack()
    {
        float t = 0;
        Debug.Log("attacking!");

        ps.isAttacking = true;
        ps.attackCooldown = Time.time + ps.attackCooldownLengthCurrent;
        while (t < ps.attackDurationCurrent)
        {
            t += Time.deltaTime;
            controller.animator.Play("Attack Tree");
            objectsHit = Physics2D.OverlapCircleAll(attackPos.position, ps.attackRadiusCurrent, attackLayer);
            if (objectsHit.Length > 0)
            {
                Debug.Log(objectsHit.Length + "objects hit!");
                foreach (Collider2D hit in objectsHit)
                {
                    hit.gameObject.GetComponent<Health>().ChangeHealth(ps.attackDamageCurrent);
                    if (hit.gameObject.GetComponent<Health>() == null) { Debug.Log("Hit thing with no health"); yield return null; }
                }
            }
            yield return null;
        }
        ps.isAttacking = false;
        

    }
}
