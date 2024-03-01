using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{

    public GameObject[] DodgeCooldownPrefabs;
    public GameObject[] DodgeVelocityPrefabs;
    public GameObject[] GunDamagePrefabs;
    public GameObject[] GunRateOfFirePrefabs;
    public GameObject[] GunSpeedPrefabs;
    public GameObject[] HealthPrefabs;
    public GameObject[] PermaMaxHealthPrefabs;
    public GameObject[] ReloadTimePrefabs;
    public GameObject[] SwordCooldownPrefabs;
    public GameObject[] SwordDamagePrefabs;
    public GameObject[] SwordDurationPrefabs;
    public GameObject[] SwordRadiusPrefabs;
    public GameObject[] TempMaxHealthPrefabs;

    public enum statBuff
    {
        health,
        maxHealth,
        dodgeCooldown,
        dodgeVelocity,
        swordDamage,
        swordRadius,
        swordDuration,
        swordCooldown,
        gunDamage,
        gunSpeed,
        gunRoF,
        reloadTime
    }

    statBuff _statBuff;
    int randomBuff, randomBuffAlignment;
    public bool hasDropped;
    public void GetRandomBuff()
    {
        if (!hasDropped)
        {
            hasDropped = true;
            if (Random.Range(0f, 100f) <= 25f) //if 25 or less spawn (25%)
            {
                randomBuff = Mathf.RoundToInt(Random.Range(0f, 100f) / 11);
                randomBuffAlignment = Random.Range(0, 100);
                if (randomBuffAlignment < 11)
                {
                    randomBuffAlignment = 0;
                }//if less then 11 then its a bad strong effect
                else if (randomBuffAlignment >= 11 && randomBuffAlignment < 45)
                {
                    randomBuffAlignment = 1;
                }// if 11 or greater but less then 45 then it is a bad weak effect
                else if (randomBuffAlignment >= 45 && randomBuffAlignment < 85)
                {
                    randomBuffAlignment = 2;
                }// if 45 or greater and less then 85 then it is a good weak effect
                else { randomBuffAlignment = 3; } //if 85 or higher then good strong effect

                _statBuff = (statBuff)randomBuff;

                switch (_statBuff)
                {
                    case statBuff.health:

                        break;
                    case statBuff.maxHealth:

                        if (Random.Range(0, 3) == 3) //if 3 then its a permanent health buff (25%)
                        {
                             Instantiate(PermaMaxHealthPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                            
                        }
                        else //or else its a temp buff
                        {
                             Instantiate(TempMaxHealthPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                            
                        }
                        break;
                    case statBuff.dodgeCooldown:
                         Instantiate(DodgeCooldownPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.dodgeVelocity:
                         Instantiate(DodgeVelocityPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.swordDamage:
                         Instantiate(SwordDamagePrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.swordRadius:
                         Instantiate(SwordRadiusPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.swordDuration:
                         Instantiate(SwordDurationPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.swordCooldown:
                         Instantiate(SwordCooldownPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.gunDamage:
                         Instantiate(GunDamagePrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.gunSpeed:
                         Instantiate(GunSpeedPrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.gunRoF:
                         Instantiate(GunRateOfFirePrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                    case statBuff.reloadTime:
                         Instantiate(ReloadTimePrefabs[randomBuffAlignment], transform.position, Quaternion.identity);
                        
                        break;
                }
            }
        }
        

    }


}
