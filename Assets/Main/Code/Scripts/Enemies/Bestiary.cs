using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Bestiary : MonoBehaviour
{
    public static Bestiary instance;
    public Dictionary<Monster, int> defeatedMonsters = new Dictionary<Monster, int>();
    [SerializeField] private List<BestiarySlot> bestiarySlots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignBestiarySlots();
        if(PointerSceneInfo.PointerSceneInfoInstance != null)
        {
            if (PointerSceneInfo.PointerSceneInfoInstance.isMonsterCatched)
            {
                CheckForDefeatedMonsters();
            }
        }
    }

    private void AssignBestiarySlots()
    {
        bestiarySlots = new List<BestiarySlot>(FindObjectsOfType<BestiarySlot>());
    }


    public void AddNewDefeatedMonster(Monster monsterToCheck, int catches)
    {
        LoadBestiarySlotPrefs();
        List<Monster> keysToRemove = new List<Monster>();

        foreach (var entry in defeatedMonsters)
        {
            var monster = entry.Key;
            var count = entry.Value;

            if (monster.MonsterName == monsterToCheck.MonsterName && count < catches)
            {
                keysToRemove.Add(monster); 
            }
        }

        foreach (var key in keysToRemove)
        {
            defeatedMonsters.Remove(key);
        }
        keysToRemove.Clear();
        defeatedMonsters[monsterToCheck] = catches;
    }

    private void LoadBestiarySlotPrefs()
    {
        for (int i = 0; i < bestiarySlots.Count; i++)
        {
            bestiarySlots[i].LoadPlayerPrefs();
        }
    }

    public void CheckForDefeatedMonsters()
    {
        foreach (var entry in defeatedMonsters)
        {
            var monster = entry.Key;
            var count = entry.Value;

            foreach (var slot in bestiarySlots)
            {
                if (slot.Monster.MonsterName == monster.MonsterName)
                {

                    int stars = 0;
                    switch (count)
                    {
                        case 0:
                            stars = 3;
                            break;
                        case 1:
                            stars = 2;
                            break;
                        case >= 2:
                            stars = 1;
                            break;
                    }

                    if (stars > slot.numberOfStars)
                    {
                        slot.numberOfStars = stars;
                        slot.UpdateSlot();
                    }
                    break; 
                }
            }
        }
    }
}
