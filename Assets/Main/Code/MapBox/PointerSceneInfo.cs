using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSceneInfo : MonoBehaviour
{
    public static PointerSceneInfo PointerSceneInfoInstance;
    public bool isCameraPhotoSet = false;
    public bool isMonsterCatched = false;
    public string hint;

    public void SetInstance()
    {
        PointerSceneInfoInstance = this;
    }
    private void OnMouseDown()
    {
        if (IsPointerOverBlockingCanvasGroup())
        {
            return;
        }

        PointerSceneInfoInstance = this;
    }

    private bool IsPointerOverBlockingCanvasGroup()
    {
        if (EventSystem.current == null) return false;

        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var result in raycastResults)
        {
            var canvasGroup = result.gameObject.GetComponentInParent<CanvasGroup>();
            if (canvasGroup != null && canvasGroup.blocksRaycasts)
            {
                return true;
            }
        }

        return false; 
    }
}
