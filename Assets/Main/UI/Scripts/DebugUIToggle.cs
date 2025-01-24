using UnityEngine;

public class DebugUIToggle : MonoBehaviour
{
    // Przypisz obiekt UI zawieraj¹cy Debug UI w edytorze Unity
    [SerializeField] private GameObject debugUI;
    [SerializeField] private bool isTestModeOn;

    void Update()
    {
        if (isTestModeOn)
        {
            // SprawdŸ liczbê dotkniêæ na ekranie
            if (Input.touchCount == 3)
            {
                // SprawdŸ, czy wszystkie dotkniêcia s¹ w fazie "Began"
                bool allTouchesBegan = true;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase != TouchPhase.Began)
                    {
                        allTouchesBegan = false;
                        break;
                    }
                }

                // Jeœli trzy palce dotknê³y ekranu, prze³¹cz widocznoœæ Debug UI
                if (allTouchesBegan)
                {
                    ToggleDebugUI();
                }
            }
        }
    }

    private void ToggleDebugUI()
    {
        if (debugUI != null)
        {
            // Prze³¹cz widocznoœæ obiektu UI
            debugUI.SetActive(!debugUI.activeSelf);
        }
        else
        {
            Debug.LogWarning("Debug UI nie jest przypisane w inspectorze!");
        }
    }
}
