using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Dodaj przestrzeñ nazw TextMeshPro

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem; // Odniesienie do SaveSystem
    [SerializeField] private GameObject buttonPrefab; // Prefab przycisku
    [SerializeField] private Transform buttonParent; // Rodzic dla przycisków

    private List<string> currentSaveFiles = new List<string>(); // Lista zapisów, które zosta³y utworzone

    private void Start()
    {
        CreateSaveButtons();
    }

    /// <summary>
    /// Tworzy przyciski dla istniej¹cych zapisów gry, jeœli przyciski dla zapisów jeszcze nie istniej¹.
    /// </summary>
    public void CreateSaveButtons()
    {
        // Pobierz listê zapisów
        List<string> saveFiles = saveSystem.GetSaveList();

        // Jeœli zapisów jest mniej ni¿ wczeœniej zapisanych, usuñ stare przyciski
        if (saveFiles.Count != currentSaveFiles.Count)
        {
            RemoveAllSaveButtons();
        }

        // Jeœli zapisy s¹ ró¿ne, twórz nowe przyciski
        foreach (string saveFile in saveFiles)
        {
            // SprawdŸ, czy przycisk dla tego zapisu ju¿ istnieje
            if (!currentSaveFiles.Contains(saveFile))
            {
                GameObject buttonInstance = Instantiate(buttonPrefab, buttonParent);
                Button button = buttonInstance.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>(); // Pobierz komponent TextMeshProUGUI

                if (buttonText == null)
                {
                    Debug.LogError("Brak komponentu TextMeshProUGUI w prefabie przycisku!");
                }
                else
                {
                    buttonText.text = Path.GetFileNameWithoutExtension(saveFile); // Ustaw nazwê zapisu na przycisku
                    Debug.Log("Zmieniono tekst na: " + buttonText.text);
                }

                if (button != null && buttonText != null)
                {
                    buttonText.text = Path.GetFileNameWithoutExtension(saveFile); // Ustaw nazwê zapisu na przycisku
                    string saveFileName = saveFile; // Zachowaj kontekst zapisu

                    button.onClick.AddListener(() => OnSaveButtonClicked(saveFileName));
                }

                // Dodaj nazwê zapisu do listy, aby zapamiêtaæ, ¿e ju¿ zosta³ stworzony przycisk
                currentSaveFiles.Add(saveFile);
            }
        }
    }

    /// <summary>
    /// Funkcja wywo³ywana po klikniêciu przycisku zapisu.
    /// </summary>
    /// <param name="saveFileName">Nazwa pliku zapisu.</param>
    private void OnSaveButtonClicked(string saveFileName)
    {
        Debug.Log("Wczytywanie zapisu: " + saveFileName);
        saveSystem.LoadGame(saveFileName);
    }

    /// <summary>
    /// Usuwa wszystkie stworzone przyciski zapisów.
    /// </summary>
    public void RemoveAllSaveButtons()
    {
        // Przechodzi przez wszystkie dzieci obiektu buttonParent i usuwa je
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // Czyœci listê zapisanych plików
        currentSaveFiles.Clear();
    }
}
