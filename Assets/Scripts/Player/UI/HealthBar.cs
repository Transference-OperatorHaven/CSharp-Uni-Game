using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public PlayerStats ps;
    [SerializeField]GameObject heartPrefab;
    List<HeartIcon> hearts = new List<HeartIcon>();


    private void OnEnable()
    {
        PlayerStats.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDamaged -= DrawHearts;
    }

    // Start is called before the first frame update
    void Start()
    {
        ps = PlayerStats.FindObjectOfType<PlayerStats>();
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        for(int i = 0; i < ps.healthMax; i++)
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(ps.healthCurrent - (i), 0, 1);
            hearts[i].SetHeartImage((HeartState)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {  
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HeartIcon heartComponent = newHeart.GetComponent<HeartIcon>();
        heartComponent.SetHeartImage(HeartState.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HeartIcon>();
    }
}
