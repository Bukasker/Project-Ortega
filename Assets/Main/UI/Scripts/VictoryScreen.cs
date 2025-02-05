using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private List<Image> listOfStars;
    [SerializeField] private Sprite disactivateStar;
    [SerializeField] private MobSpawner mobSpawner;
    [SerializeField] private int numerOfCatches = 0;

    public void AddNumberOfCatches()
    {
        numerOfCatches++;
    }

    public void ShowVictoryScreen()
    {
        Bestiary.instance.AddNewDefeatedMonster(mobSpawner.activeMonster,numerOfCatches);
        victoryScreen.SetActive(true);
        animator.SetTrigger("IsEnemyDead");
        switch (numerOfCatches)
        {
            case 1:
                listOfStars[0].sprite = disactivateStar;
                break;
            case >= 2:
                listOfStars[0].sprite = disactivateStar;
                listOfStars[1].sprite = disactivateStar;
                break;
        }
    }
}
