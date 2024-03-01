using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float invulerabilityTime;
    public float invulerabilityDuration;
    public bool hurt, playedHurtAnim ,dead;
    LayerMask startingLayer;
    SpriteRenderer sr;
    Rigidbody2D rb;
    LootDrop loot;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        startingLayer = gameObject.layer;
        if(gameObject.GetComponent<SpriteRenderer>() != null )
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
        }
       else if (gameObject.GetComponentInChildren<SpriteRenderer>() != null )
        {
            sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        }
        rb = gameObject.GetComponent<Rigidbody2D>();
        loot = gameObject.GetComponent<LootDrop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            if(loot != null)
            {
                loot.GetRandomBuff();
            }
            dead = true;
        }
    }

    public void ChangeHealth(float val)
    {
        hurt = true;
        playedHurtAnim = true;
        currentHealth += val;
        if (invulerabilityTime < Time.time) { StartCoroutine(Invulerable()); }
    }

    IEnumerator Invulerable()
    {
        float t = 0;
        invulerabilityTime = invulerabilityDuration + Time.time;
        while (t < invulerabilityDuration)
        {
            t += Time.deltaTime;

            gameObject.layer = 7;

            yield return null;
        }

        
        gameObject.layer = startingLayer;
        hurt = false;
        playedHurtAnim = false;
    }
}
