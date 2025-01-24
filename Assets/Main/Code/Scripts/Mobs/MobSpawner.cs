using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject createdPrefabs;
    [SerializeField] List<Monster> monsters;

    /*
    void Start()
    {
        Monster selectedMonster = GetRandomMonster();
        if (selectedMonster != null)
        {
            Debug.Log("Wylosowano potwora: " + selectedMonster.MonsterName);
        }
    }
    */
    private Monster GetRandomMonster()
    {
        // Obliczenie sumy szans
        float totalChance = 0f;
        foreach (var monster in monsters)
        {
            totalChance += monster.spawnChance;
        }

        // Wylosowanie wartoœci w zakresie od 0 do totalChance
        float randomValue = Random.Range(0, totalChance);

        // Iterowanie przez listê i wybór potwora na podstawie szans
        float cumulativeChance = 0f;
        foreach (var monster in monsters)
        {
            cumulativeChance += monster.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return monster;
            }
        }

        return null; // W razie braku (nie powinno siê zdarzyæ, jeœli lista ma potwory)
    }
} 
