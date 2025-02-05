using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestiarySlot : MonoBehaviour
{
    public Monster Monster;
    public int numberOfStars = 0;

    public Image BeastImage;
    public TMP_Text BeastName;

    public List<Image> stars;
    public Sprite activeStartSprite;

    public void UpdateSlot()
    {
        BeastImage.sprite = Monster.Icon;
        BeastName.text = Monster.name;

        for (int i = 0; i <= numberOfStars - 1; i++)
        {
            stars[i].sprite = activeStartSprite;
        }
    }
    void OnDisable()
    {
        PlayerPrefs.SetInt("numberOfStars", numberOfStars);
        PlayerPrefs.Save();
    }
    public void LoadPlayerPrefs()
    {
        numberOfStars = PlayerPrefs.GetInt("numberOfStars", 0);
    }
}
