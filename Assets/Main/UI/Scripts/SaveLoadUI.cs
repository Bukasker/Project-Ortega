using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Dodaj przestrze� nazw TextMeshPro

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem; // Odniesienie do SaveSystem
    [SerializeField] private GameObject buttonPrefab; // Prefab przycisku
    [SerializeField] private Transform buttonParent; // Rodzic dla przycisk�w

    private List<string> currentSaveFiles = new List<string>(); // Lista zapis�w, kt�re zosta�y utworzone

    private void Start()
    {
        CreateSaveButtons();
    }

    /// <summary>
    /// Tworzy przyciski dla istniej�cych zapis�w gry, je�li przyciski dla zapis�w jeszcze nie istniej�.
    /// </summary>
    public void CreateSaveButtons()
    {
        // Pobierz list� zapis�w
        List<string> saveFiles = saveSystem.GetSaveList();

        // Je�li zapis�w jest mniej ni� wcze�niej zapisanych, usu� stare przyciski
        if (saveFiles.Count != currentSaveFiles.Count)
        {
            RemoveAllSaveButtons();
        }

        // Je�li zapisy s� r�ne, tw�rz nowe przyciski
        foreach (string saveFile in saveFiles)
        {
            // Sprawd�, czy przycisk dla tego zapisu ju� istnieje
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
                    buttonText.text = Path.GetFileNameWithoutExtension(saveFile); // Ustaw nazw� zapisu na przycisku
                    Debug.Log("Zmieniono tekst na: " + buttonText.text);
                }

                if (button != null && buttonText != null)
                {
                    buttonText.text = Path.GetFileNameWithoutExtension(saveFile); // Ustaw nazw� zapisu na przycisku
                    string saveFileName = saveFile; // Zachowaj kontekst zapisu

                    button.onClick.AddListener(() => OnSaveButtonClicked(saveFileName));
                }

                // Dodaj nazw� zapisu do listy, aby zapami�ta�, �e ju� zosta� stworzony przycisk
                currentSaveFiles.Add(saveFile);
            }
        }
    }

    /// <summary>
    /// Funkcja wywo�ywana po klikni�ciu przycisku zapisu.
    /// </summary>
    /// <param name="saveFileName">Nazwa pliku zapisu.</param>
    private void OnSaveButtonClicked(string saveFileName)
    {
        Debug.Log("Wczytywanie zapisu: " + saveFileName);
        saveSystem.LoadGame(saveFileName);
    }

    /// <summary>
    /// Usuwa wszystkie stworzone przyciski zapis�w.
    /// </summary>
    public void RemoveAllSaveButtons()
    {
        // Przechodzi przez wszystkie dzieci obiektu buttonParent i usuwa je
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        // Czy�ci list� zapisanych plik�w
        currentSaveFiles.Clear();
    }
}
