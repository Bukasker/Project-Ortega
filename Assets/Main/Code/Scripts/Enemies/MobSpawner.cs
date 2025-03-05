using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> monstersGameObjects; 
    [SerializeField] private List<Monster> monsters;
    public Monster activeMonster;
    private int lastSpawnedIndex = -1;

    [SerializeField] private CatchBar catchBar;

    private Monster GetRandomMonster()
    {
        float totalChance = 0f;
        foreach (var monster in monsters)
        {
            totalChance += monster.spawnChance;
        }

        float randomValue = Random.Range(0, totalChance);

        float cumulativeChance = 0f;
        for (int i = 0; i < monsters.Count; i++)
        {
            cumulativeChance += monsters[i].spawnChance;
            if (randomValue <= cumulativeChance)
            {
                lastSpawnedIndex = i;
                return monsters[i];
            }
        }

        return null;
    }

    public void ActivateMonster()
    {
        activeMonster = GetRandomMonster();

        catchBar.speed = activeMonster.MobDifficulty;

        if (lastSpawnedIndex != -1 && lastSpawnedIndex < monstersGameObjects.Count)
        {
            GameObject monsterGameObject = monstersGameObjects[lastSpawnedIndex];
            if (monsterGameObject != null)
            {
                monsterGameObject.SetActive(true);
            }
        }
    }
}
