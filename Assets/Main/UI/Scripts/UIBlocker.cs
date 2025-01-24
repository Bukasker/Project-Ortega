using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlocker : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        // Blokuj zdarzenie w momencie naci�ni�cia
        eventData.Use();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Blokuj zdarzenie w momencie klikni�cia
        eventData.Use();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Blokuj zdarzenie w momencie puszczenia przycisku myszy
        eventData.Use();
    }
}
