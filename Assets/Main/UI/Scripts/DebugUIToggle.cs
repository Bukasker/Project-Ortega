using UnityEngine;

public class DebugUIToggle : MonoBehaviour
{
    // Przypisz obiekt UI zawieraj�cy Debug UI w edytorze Unity
    [SerializeField] private GameObject debugUI;
    [SerializeField] private bool isTestModeOn;

    void Update()
    {
        if (isTestModeOn)
        {
            // Sprawd� liczb� dotkni�� na ekranie
            if (Input.touchCount == 3)
            {
                // Sprawd�, czy wszystkie dotkni�cia s� w fazie "Began"
                bool allTouchesBegan = true;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase != TouchPhase.Began)
                    {
                        allTouchesBegan = false;
                        break;
                    }
                }

                // Je�li trzy palce dotkn�y ekranu, prze��cz widoczno�� Debug UI
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
            // Prze��cz widoczno�� obiektu UI
            debugUI.SetActive(!debugUI.activeSelf);
        }
        else
        {
            Debug.LogWarning("Debug UI nie jest przypisane w inspectorze!");
        }
    }
}
