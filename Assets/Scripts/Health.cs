using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float invulerabilityTime;
    public float invulerabilityDuration;
    LayerMask startingLayer;
    SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        startingLayer = gameObject.layer;
        sr = gameObject.GetComponent<SpriteRenderer>();
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
    }
}
