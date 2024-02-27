using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagUIState : MonoBehaviour
{
    PlayerStats ps;
    TextMeshProUGUI text;
    Image circle;

    private void OnEnable()
    {
        Weapon.OnFire += UpdateMagUI;
        Weapon.OnReloadCompleted += UpdateMagUI;
        Weapon.OnReloadStarted += ReloadingCircleUI;
    }

    private void OnDisable()
    {
        Weapon.OnFire -= UpdateMagUI;
        Weapon.OnReloadCompleted -= UpdateMagUI;
        Weapon.OnReloadStarted -= ReloadingCircleUI; 
    }

    // Start is called before the first frame update
    void Awake()
    {
        ps = PlayerStats.FindObjectOfType<PlayerStats>();
        text = GetComponent<TextMeshProUGUI>();
        circle = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        UpdateMagUI();
    }

    void UpdateMagUI()
    {
        text.text = ps.gunMagCurrent.ToString() + " / " + ps.gunMagBase.ToString();
    }

    private void ReloadingCircleUI()
    {
        StartCoroutine(ReloadingCircle());
    }

    IEnumerator ReloadingCircle()
    {
        ps.specialReloadActive = false;
        float t = 0;
        float reloadTime = ps.reloadTimeCurrent;
        while (t < reloadTime)
        {
            circle.fillAmount = 1 - (t / reloadTime);
            if(circle.fillAmount > 0.6 && circle.fillAmount < 0.9)
            {
                if (Input.GetAxisRaw("reload") == 1)
                {
                    ps.canSpecialReload = false;
                }
            }
            if(circle.fillAmount >= 0.39 && circle.fillAmount <= 0.59 && ps.canSpecialReload)
            {
                if(Input.GetAxisRaw("reload") == 1)
                {
                    ps.specialReloadActive = true;
                    t = reloadTime;
                    circle.fillAmount = 0;
                }
            }


            t += Time.deltaTime;
            yield return null;
        }
    }
}
