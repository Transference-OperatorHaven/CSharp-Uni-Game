using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float invulerabilityTime;
    public float invulerabilityDuration;
    public bool hurt;
    LayerMask startingLayer;
    SpriteRenderer sr;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        startingLayer = gameObject.layer;
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeHealth(float val)
    {
        hurt = true;
        currentHealth += val;
        if (invulerabilityTime < Time.time) { StartCoroutine(Invulerable()); }
    }

    IEnumerator Invulerable()
    {
        float t = 0;
        invulerabilityTime = invulerabilityDuration + Time.time;
        while (t < invulerabilityDuration)
        {
            sr.color = Color.black;
            t += Time.deltaTime;

            gameObject.layer = 7;

            yield return null;
        }

        
        sr.color = Color.white;
        gameObject.layer = startingLayer;
        hurt = false;
    }
}
