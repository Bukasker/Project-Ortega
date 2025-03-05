using UnityEngine;

public class DebugUIToggle : MonoBehaviour
{
    [SerializeField] private GameObject debugUI;
    [SerializeField] private bool isTestModeOn;

    void Update()
    {
        if (isTestModeOn)
        {
            if (Input.touchCount == 3)
            {
                bool allTouchesBegan = true;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase != TouchPhase.Began)
                    {
                        allTouchesBegan = false;
                        break;
                    }
                }
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
            debugUI.SetActive(!debugUI.activeSelf);
        }
        else
        {
            Debug.LogWarning("Debug UI nie jest przypisane w inspectorze!");
        }
    }
}
