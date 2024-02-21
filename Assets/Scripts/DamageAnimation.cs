using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{

    Vector3 startLoc, endLoc;
    public float duration = 0.4f;
    Coroutine wiggleCoroutine;

    private void Start()
    {
       startLoc = gameObject.transform.position;
    }

    public void DoAWiggle(float damage)
    {
        if (wiggleCoroutine == null)
        {
            startLoc = gameObject.transform.position;
        }
        gameObject.SetActive(true);
        endLoc = gameObject.transform.position;
        endLoc.y += 10 + (4 * damage);

        wiggleCoroutine = StartCoroutine(Wiggle());

    }

    IEnumerator Wiggle()
    {
        float t = 0;
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        text.color = Color.black;
        while (t < duration)
        {
            t += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startLoc, endLoc, (t / duration));
            text.color = Vector4.Lerp(Color.black, new Vector4(0.1597269f, 1, 0, 1), (t / duration));
            yield return null;
        }
        gameObject.transform.position = startLoc;
        gameObject.SetActive(false);
    }

}
