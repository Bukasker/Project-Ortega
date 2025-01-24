using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSceneInfo : MonoBehaviour
{
    public static PointerSceneInfo PointerSceneInfoInstance;
    public bool isCameraPhotoSet = false;


    public void SetInstance()
    {
        PointerSceneInfoInstance = this;
    }
    private void OnMouseDown()
    {
        // Sprawd�, czy klikni�to na UI i czy element ma CanvasGroup z w��czonym blocksRaycasts
        if (IsPointerOverBlockingCanvasGroup())
        {
            Debug.Log("Klikni�to na UI z aktywnym blokowaniem interakcji!");
            return;
        }

        PointerSceneInfoInstance = this;
    }

    private bool IsPointerOverBlockingCanvasGroup()
    {
        if (EventSystem.current == null) return false;

        // Lista wynik�w dla raycasta
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // Sprawd�, czy kt�ry� z element�w ma CanvasGroup z blocksRaycasts
        foreach (var result in raycastResults)
        {
            var canvasGroup = result.gameObject.GetComponentInParent<CanvasGroup>();
            if (canvasGroup != null && canvasGroup.blocksRaycasts)
            {
                return true; // Znaleziono blokuj�cy element UI
            }
        }

        return false; // Nie znaleziono elementu UI blokuj�cego interakcj�
    }
}
