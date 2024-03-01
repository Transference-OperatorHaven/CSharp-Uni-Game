using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStorm : MonoBehaviour
{
    [SerializeField]GameObject lightningPrefab;
    [SerializeField] int numberOfStrikes;
    [SerializeField] float timeBetweenStrikes;
    public Vector2 direction;

    private void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        for(int i = 0; i < numberOfStrikes; i++)
        {
            GameObject lightningStrike = Instantiate(lightningPrefab, ((direction.normalized*-1) * (i*2)) + new Vector2(transform.position.x,transform.position.y), Quaternion.identity, gameObject.transform);
            yield return new WaitForSeconds(timeBetweenStrikes);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
