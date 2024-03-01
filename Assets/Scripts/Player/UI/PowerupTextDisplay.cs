using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class PowerupTextDisplay : MonoBehaviour
{
    [SerializeField]GameObject powerupDescription;
    [SerializeField] float showTime;
    TextMeshProUGUI titleText;
    TextMeshProUGUI descriptionText;

    private void Start()
    {
        
        gameObject.SetActive(false);
        powerupDescription.SetActive(false);
        titleText = gameObject.GetComponent<TextMeshProUGUI>();
        descriptionText = powerupDescription.GetComponent<TextMeshProUGUI>();
    }

    public void DisplayPowerupInfo(string powerUpTitle, string powerUpDescription)
    {
        gameObject.SetActive(true);
        powerupDescription.SetActive(true);
        gameObject.GetComponent<TextMeshProUGUI>().text = powerUpTitle;
        powerupDescription.GetComponent<TextMeshProUGUI>().text = powerUpDescription;
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        float t = 0;
        while (t < showTime)
        {
            t += Time.deltaTime;
            titleText.color = new Color(1, 1, 1, Mathf.Lerp(0,1,(t/showTime)));
            descriptionText.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, (t / showTime)));
            yield return null;
        }
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        powerupDescription.SetActive(false);
        titleText.color = Color.white;
        descriptionText.color = Color.white;


    }
}
