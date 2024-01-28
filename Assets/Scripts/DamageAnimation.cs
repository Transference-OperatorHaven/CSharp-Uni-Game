using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{

    Vector3 startLoc, endLoc;
    public float duration = 0.4f;

    public void DoAWiggle(float damage)
    {
        gameObject.SetActive(true);
        startLoc = gameObject.transform.position;
        endLoc = gameObject.transform.position;
        endLoc.y -= 10 + (10 * damage);

        StartCoroutine(Wiggle());

    }

    IEnumerator Wiggle()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startLoc, endLoc, (t/duration));
            yield return null;
        }
        gameObject.transform.position = startLoc;
        gameObject.SetActive(false);
    }

}
