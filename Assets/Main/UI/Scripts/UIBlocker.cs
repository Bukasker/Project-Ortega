using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlocker : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        // Blokuj zdarzenie w momencie naciœniêcia
        eventData.Use();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Blokuj zdarzenie w momencie klikniêcia
        eventData.Use();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Blokuj zdarzenie w momencie puszczenia przycisku myszy
        eventData.Use();
    }
}
