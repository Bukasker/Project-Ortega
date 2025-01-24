using UnityEngine;

public class BlockRaycast : MonoBehaviour
{
    // Referencja do CanvasGroup
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Pobranie komponentu CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();

        // Je�li CanvasGroup nie istnieje, dodaj go
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Ustawienie blokowania raycast�w
        canvasGroup.blocksRaycasts = true;
    }

    // Metoda do w��czania i wy��czania blokowania raycast�w
    public void SetBlockRaycasts(bool block)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = block;
        }
    }
}
