using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject circle;
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
        circle.transform.localScale = new Vector3(ps.swordRadiusCurrent * 2, ps.swordRadiusCurrent * 2, ps.swordRadiusCurrent * 2);
        if (!ps.isAttacking && !ps.isAiming && !ps.isDodging)
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
        circle.GetComponent<SpriteRenderer>().enabled = true;
        ps.isAttacking = true;
        ps.swordCooldown = Time.time + ps.swordCooldownLengthCurrent;
        while (t < ps.swordDurationCurrent)
        {
            t += Time.deltaTime;
            controller.animator.Play("Attack Tree");
            objectsHit = Physics2D.OverlapCircleAll(attackPos.position, ps.swordRadiusCurrent, swordLayer);
            if (objectsHit.Length > 0)
            {
                foreach (Collider2D hit in objectsHit)
                {
                    
                    if (hit.gameObject.GetComponent<EnemyHealth>() != null)
                    {
                        combo.ComboIncrease(ps.swordDamageCurrent);
                        hit.gameObject.GetComponent<EnemyHealth>().ChangeHealth(ps.swordDamageCurrent);
                    }
                    else { Debug.Log("Hit thing with no health"); yield return null; }
                }
            }
            yield return null;
        }
        ps.isAttacking = false;
        circle.GetComponent<SpriteRenderer>().enabled = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, (ps == null) ? 0.5f : ps.swordRadiusCurrent);
    }
}
