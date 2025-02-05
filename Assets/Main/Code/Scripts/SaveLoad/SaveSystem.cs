using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] EventPointer ponterPrefab;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject arrowPrefab;
    public static List<EventPointer> mapPoints = new List<EventPointer>();
    public static List<AddLineToSaveSystem> lineGameObjects = new List<AddLineToSaveSystem>();
    public static List<AddArrowToSaveSystem> arrowGameObjects = new List<AddArrowToSaveSystem>();

    const string SaveFolder = "/Saves/";

    private void Awake()
    {
        TestLoadLatestSave();
        Directory.CreateDirectory(Application.persistentDataPath + SaveFolder); // Upewnij siê, ¿e folder istnieje
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        string path = GenerateSavePath();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        // Zapisz dane obiektów
        formatter.Serialize(stream, mapPoints.Count);
        foreach (var point in mapPoints)
        {
            MapPointData data = new MapPointData(point);
            formatter.Serialize(stream, data);
        }

        formatter.Serialize(stream, lineGameObjects.Count);
        foreach (var line in lineGameObjects)
        {
            LineData data = new LineData(line);
            formatter.Serialize(stream, data);
        }

        formatter.Serialize(stream, arrowGameObjects.Count);
        foreach (var arrow in arrowGameObjects)
        {
            ArrowData data = new ArrowData(arrow);
            formatter.Serialize(stream, data);
        }

        stream.Close();
        Debug.Log("Gra zapisana w " + path);
    }
    public void TestLoadLatestSave()
    {
        string latestSavePath = GetLatestSavePath();
        if (!string.IsNullOrEmpty(latestSavePath) && File.Exists(latestSavePath))
        {
            Debug.Log("Próba wczytania ostatniego zapisu z pliku: " + latestSavePath);
            LoadGame(); // Wczytaj ostatni zapis
            Debug.Log("Ostatni zapis wczytany pomyœlnie.");
        }
        else
        {
            Debug.LogWarning("Brak zapisów do wczytania.");
        }
    }
    public void LoadGame(string saveFileName = null)
    {
        string path = string.IsNullOrEmpty(saveFileName)
            ? GetLatestSavePath()
            : Application.persistentDataPath + SaveFolder + saveFileName;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // Wczytaj dane obiektów
            int mapPointCount = (int)formatter.Deserialize(stream);
            for (int i = 0; i < mapPointCount; i++)
            {
                MapPointData data = formatter.Deserialize(stream) as MapPointData;
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                EventPointer ponter = Instantiate(ponterPrefab, position, Quaternion.identity);
                ponter.eventPos[0] = Convert.ToDouble(data.locationStrings[0]);
                ponter.eventPos[1] = Convert.ToDouble(data.locationStrings[1]);
            }

            int lineObjectCount = (int)formatter.Deserialize(stream);
            for (int i = 0; i < lineObjectCount; i++)
            {
                LineData data = formatter.Deserialize(stream) as LineData;
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                Quaternion rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);

                // Tworzymy instancjê linii
                var lineInstance = Instantiate(linePrefab, position, rotation);

                // Pobieramy komponent LineRenderer i ustawiamy jego pozycje
                var lineRenderer = lineInstance.GetComponent<LineRenderer>();
                if (lineRenderer != null)
                {
                    Vector3 lineStart = new Vector3(data.lineStart[0], data.lineStart[1], data.lineStart[2]);
                    Vector3 lineEnd = new Vector3(data.lineEnd[0], data.lineEnd[1], data.lineEnd[2]);

                    lineRenderer.SetPosition(0, lineStart);
                    lineRenderer.SetPosition(1, lineEnd);

                    lineRenderer.startColor = Color.red;
                    lineRenderer.endColor = Color.red;

                    if (lineRenderer.material == null)
                    {
                        Material lineMaterial = new Material(Shader.Find("Sprites/Default")); // Mo¿esz wybraæ inny shader
                        lineRenderer.material = lineMaterial;
                    }
                }
            }

            int arrowObjectCount = (int)formatter.Deserialize(stream);
            for (int i = 0; i < arrowObjectCount; i++)
            {
                ArrowData data = formatter.Deserialize(stream) as ArrowData;
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                Quaternion rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);
                Instantiate(arrowPrefab, position, rotation);
            }

            stream.Close();
            Debug.Log("Gra wczytana z " + path);
        }
        else
        {
            Debug.LogError("Plik zapisu nie znaleziony w " + path);
        }
    }

    public List<string> GetSaveList()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + SaveFolder, "*.sav");
        List<string> saveFiles = new List<string>();
        foreach (var file in files)
        {
            saveFiles.Add(Path.GetFileName(file));
        }
        return saveFiles;
    }

    private string GenerateSavePath()
    {
        int saveIndex = 1;
        string path;
        do
        {
            path = Application.persistentDataPath + SaveFolder + "save_" + saveIndex + ".sav";
            saveIndex++;
        } while (File.Exists(path));

        return path;
    }

    private string GetLatestSavePath()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + SaveFolder, "*.sav");
        if (files.Length > 0)
        {
            Array.Sort(files, StringComparer.Ordinal);
            return files[^1]; // Ostatni zapis
        }
        return null;
    }

    public void ClearSave(string saveFileName = null)
    {
        string path = string.IsNullOrEmpty(saveFileName)
            ? GetLatestSavePath()
            : Application.persistentDataPath + SaveFolder + saveFileName;

        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Zapis " + path + " zosta³ usuniêty.");
        }
        else
        {
            Debug.LogWarning("Nie znaleziono pliku zapisu do usuniêcia.");
        }

        ClearGameData();
    }

    public void ClearAllSaves()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + SaveFolder, "*.sav");
        foreach (var file in files)
        {
            File.Delete(file);
        }
        ClearGameData();
        Debug.Log("Wszystkie zapisy zosta³y wyczyszczone.");
    }

    private void ClearGameData()
    {
        foreach (var point in mapPoints)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }
        }
        mapPoints.Clear();

        foreach (var line in lineGameObjects)
        {
            if (line != null)
            {
                Destroy(line.gameObject);
            }
        }
        lineGameObjects.Clear();

        foreach (var arrow in arrowGameObjects)
        {
            if (arrow != null)
            {
                Destroy(arrow.gameObject);
            }
        }
        arrowGameObjects.Clear();

        Debug.Log("Wszystkie dane gry zosta³y wyczyszczone.");
    }
}
