using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintPopout : MonoBehaviour
{
    [SerializeField] private GameObject hintPopout;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private SpawnOnMap sprawnOnMap;
 
    public void SetHintPopoutOn()
    {
        hintPopout.SetActive(true);
    }
    public void SetHintPopoutOff()
    {
        hintPopout.SetActive(false);
    }

    public void AddHintTextToThePoint()
    {
        var eventPonter = sprawnOnMap.spawnedObjects[sprawnOnMap.spawnedObjects.Count - 1].GetComponent<EventPonter>();
        eventPonter.hint = hintText.text;
        hintText.text = "Enter the text";
    }
}
