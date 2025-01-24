using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fightUI;
    [SerializeField] private GameObject[] cameraUI;


    public void Update()
    {
        
    }


    public void SetCameraUIActive()
    {
        for (int i = 0; i < fightUI.Length; i++)
        {
            fightUI[i].SetActive(false);
        }

        for (int i = 0; i < cameraUI.Length; i++)
        {
            cameraUI[i].SetActive(true);
        }
    }

    public void SetFightUIActive()
    {
        for (int i = 0; i < fightUI.Length; i++)
        {
            fightUI[i].SetActive(true);
        }

        for (int i = 0; i < cameraUI.Length; i++)
        {
            cameraUI[i].SetActive(false);
        }
    }
}
