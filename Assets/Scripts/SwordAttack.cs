using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    private PlayerStats ps;
    private ComboSystem combo;
    public LayerMask swordLayer;
    public Transform attackPos;
    public Collider2D[] objectsHit;
    RaycastHit2D[] hits;
    [SerializeField] TopDownCharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        combo = GetComponent<ComboSystem>();
        ps = GetComponent<PlayerStats>();
        controller = GetComponent<TopDownCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ps.isAttacking && !ps.isAiming)
        {
            if (Input.GetAxisRaw("Attack") == 1)
            {
                if (ps.swordCooldown < Time.time)
                {
                    StartCoroutine(Attack());
                }

            }
        }     
    }

    private IEnumerator Attack()
    {
        float t = 0;
        Debug.Log("swording!");

        ps.isAttacking = true;
        ps.swordCooldown = Time.time + ps.swordCooldownLengthCurrent;
        while (t < ps.swordDurationCurrent)
        {
            t += Time.deltaTime;
            controller.animator.Play("Attack Tree");
            objectsHit = Physics2D.OverlapCircleAll(attackPos.position, ps.swordRadiusCurrent, swordLayer);
            if (objectsHit.Length > 0)
            {
                Debug.Log(objectsHit.Length + "objects hit!");
                foreach (Collider2D hit in objectsHit)
                {
                    combo.ComboIncrease(ps.swordDamageCurrent);
                    hit.gameObject.GetComponent<EnemyHealth>().ChangeHealth(ps.swordDamageCurrent);
                    if (hit.gameObject.GetComponent<EnemyHealth>() == null) { Debug.Log("Hit thing with no health"); yield return null; }
                }
            }
            yield return null;
        }
        ps.isAttacking = false;
        

    }
}