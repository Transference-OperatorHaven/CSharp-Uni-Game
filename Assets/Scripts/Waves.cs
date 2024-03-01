using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Waves : MonoBehaviour
{
    int waveCount;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] Transform wizardSpawn;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject wizardPrefab;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int enemyCount = 0;
    bool gameStarted, spawningEnemies;
    public List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        spawningEnemies = true;
        StartCoroutine(StartNewWave());
    }

    
    private void FixedUpdate()
    {
        for (int i = 0; i < enemies.Count; i++)
        {

            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
                
            }
        }
        if(Input.GetAxis("Submit") == 1)
        {
            waveCount = 9;
        }
        if(!spawningEnemies &&  enemies.Count == 0)
        {
            spawningEnemies = true;
            StartCoroutine(StartNewWave());
        }
    }
    void NewWave()
    {
        waveCount++;

        enemies.Clear();

        if(waveCount % 10 == 0 && waveCount >= 10)
        {
            text.text = "Wave: WIZARD BOSS";
            enemyCount = 1;
            GameObject wizard = (GameObject)Instantiate(wizardPrefab, wizardSpawn.position, Quaternion.identity);
            enemies.Add(wizard);
        }
        else
        {
            text.text = "Wave: " + waveCount;
            for (int i = 0; i < waveCount * 2; i++)
            {
                enemyCount++;
                GameObject enemyToBeAdded = (GameObject)Instantiate(enemyPrefabs[Random.Range(0,3)], spawnPos[i % 3].position, Quaternion.identity);
                enemies.Add(enemyToBeAdded);
                
            }
        }

        StartCoroutine(SpawnEnemies());
    }


    IEnumerator SpawnEnemies()
    { 
        for(int i = 0;i < enemies.Count;i++)
        {
            enemies[i].SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }

        spawningEnemies = false;

    }
    IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(2f);
        if (!gameStarted)
        {
            gameStarted = true;
        }
        NewWave();
    }


}
