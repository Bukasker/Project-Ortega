using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationHint : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;

    private void Update()
    {
        if(PointerSceneInfo.PointerSceneInfoInstance != null)
        {
            promptText.text = PointerSceneInfo.PointerSceneInfoInstance.hint;
        }
    }
}
