using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Variables")]
    [SerializeField]float ComboDecayLengthBase, ComboDecayLengthCurrent, ComboDecayLengthModifier;
    float ComboCount, ComboMultiplier;
    [SerializeField]float ComboGainBase, ComboGainCurrent, ComboGainModifier;
    [Header("UI elements")]
    public TMPro.TextMeshProUGUI ComboCountText;
    public TMPro.TextMeshProUGUI ComboMultiplierText;
    public Slider slider;
    public ParticleSystem particles;
    float t = 0;

    private void Start()
    {
        UpdateComboDecay();
        UpdateComboGain();
        DisableComboUI();
    }

    // Update is called once per frame
    void Update()
    {
        ComboCountText.text = ComboCount.ToString();
        if(ComboMultiplier < 1)
        {
            ComboMultiplierText.gameObject.SetActive(false);
        }
        else
        {
            ComboMultiplierText.gameObject.SetActive(true);
        }
        ComboMultiplierText.text = "x" + ComboMultiplier.ToString();
    }

    void DisableComboUI()
    {
        ComboCountText.gameObject.SetActive(false);
        ComboMultiplierText.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    void EnableComboUI()
    {
        ComboCountText.gameObject.SetActive(true);
        ComboMultiplierText.gameObject.SetActive(true);
        slider.gameObject.SetActive(true);
    }

    public void ComboIncrease()
    {
        ResetDecay();
        if(ComboCount <= 0)
        {
            EnableComboUI();
            slider.gameObject.SetActive(true);
            StartCoroutine(ComboDecay());
        }
        ComboCount = Mathf.Floor(ComboCount + (ComboGainCurrent));
        ComboMultiplier = Mathf.Floor(ComboCount / 10);

    }

    void ResetDecay()
    {
        t = 0;
    }

    void UpdateComboDecay()
    {
        ComboDecayLengthCurrent = ComboDecayLengthBase * (1 + ComboDecayLengthModifier);
    }
    void UpdateComboGain()
    {
        ComboGainCurrent = ComboGainBase * (1 + ComboGainModifier);
    }

    IEnumerator ComboDecay()
    {
        if (!particles.isPlaying) {particles.Play();}
        t = 0;
        while (t < ComboDecayLengthCurrent)
        {
            t += Time.deltaTime;
            Debug.Log(t);
            Debug.Log(t / ComboDecayLengthCurrent);
            slider.value = 1 - (t / ComboDecayLengthCurrent);

            yield return null;
        }
        ComboCount = 0;
        ComboMultiplier = 0;
        if (particles.isPlaying) { particles.Stop(); }
        DisableComboUI();

    }
}
