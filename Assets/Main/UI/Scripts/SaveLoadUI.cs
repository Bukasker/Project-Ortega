using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem; 
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;

    private List<string> currentSaveFiles = new List<string>();

    private void Start()
    {
        CreateSaveButtons();
    }


    public void CreateSaveButtons()
    {
        List<string> saveFiles = saveSystem.GetSaveList();

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
                TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText == null)
                {
                    Debug.LogError("Brak komponentu TextMeshProUGUI w prefabie przycisku!");
                }
                else
                {
                    buttonText.text = Path.GetFileNameWithoutExtension(saveFile); 
                    Debug.Log("Zmieniono tekst na: " + buttonText.text);
                }

                if (button != null && buttonText != null)
                {
                    buttonText.text = Path.GetFileNameWithoutExtension(saveFile);
                    string saveFileName = saveFile;

                    button.onClick.AddListener(() => OnSaveButtonClicked(saveFileName));
                }
                currentSaveFiles.Add(saveFile);
            }
        }
    }


    /// <param name="saveFileName">Nazwa pliku zapisu.</param>
    private void OnSaveButtonClicked(string saveFileName)
    {
        Debug.Log("Wczytywanie zapisu: " + saveFileName);
        saveSystem.LoadGame(saveFileName);
    }


    public void RemoveAllSaveButtons()
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
        currentSaveFiles.Clear();
    }
}
