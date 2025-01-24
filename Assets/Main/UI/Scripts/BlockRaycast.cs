using UnityEngine;

public class BlockRaycast : MonoBehaviour
{
    // Referencja do CanvasGroup
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Pobranie komponentu CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();

        // Jeœli CanvasGroup nie istnieje, dodaj go
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Ustawienie blokowania raycastów
        canvasGroup.blocksRaycasts = true;
    }

    // Metoda do w³¹czania i wy³¹czania blokowania raycastów
    public void SetBlockRaycasts(bool block)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = block;
        }
    }
}
