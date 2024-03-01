using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject testEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
    }
}
