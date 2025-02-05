using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] EventPointer ponterPrefab;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject arrowPrefab;

    public static List<EventPointer> mapPoints = new List<EventPointer>();
    public static List<AddLineToSaveSystem> lineGameObjects = new List<AddLineToSaveSystem>();
    public static List<AddArrowToSaveSystem> arrowGameObjects = new List<AddArrowToSaveSystem>();

    private string saveFolder;

    private void Awake()
    {
        saveFolder = Application.persistentDataPath + "/Saves/";

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);  // Tworzy katalog, jeœli nie istnieje
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        string path = GenerateSavePath();

        try
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, mapPoints.Count);
                foreach (var point in mapPoints)
                {
                    formatter.Serialize(stream, new MapPointData(point));
                }

                formatter.Serialize(stream, lineGameObjects.Count);
                foreach (var line in lineGameObjects)
                {
                    formatter.Serialize(stream, new LineData(line));
                }

                formatter.Serialize(stream, arrowGameObjects.Count);
                foreach (var arrow in arrowGameObjects)
                {
                    formatter.Serialize(stream, new ArrowData(arrow));
                }
            }

            Debug.Log("Gra zapisana w " + path);
        }
        catch (Exception e)
        {
            Debug.LogError("B³¹d podczas zapisywania gry: " + e.Message);
        }
    }

    public void LoadGame(string saveFileName = null)
    {
        string path = string.IsNullOrEmpty(saveFileName) ? GetLatestSavePath() : saveFolder + saveFileName;

        if (!File.Exists(path))
        {
            Debug.LogError("Plik zapisu nie znaleziony w " + path);
            return;
        }

        try
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                int mapPointCount = (int)formatter.Deserialize(stream);
                for (int i = 0; i < mapPointCount; i++)
                {
                    MapPointData data = formatter.Deserialize(stream) as MapPointData;
                    Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                    Instantiate(ponterPrefab, position, Quaternion.identity);
                }

                int lineObjectCount = (int)formatter.Deserialize(stream);
                for (int i = 0; i < lineObjectCount; i++)
                {
                    LineData data = formatter.Deserialize(stream) as LineData;
                    Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                    Quaternion rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);
                    Instantiate(linePrefab, position, rotation);
                }

                int arrowObjectCount = (int)formatter.Deserialize(stream);
                for (int i = 0; i < arrowObjectCount; i++)
                {
                    ArrowData data = formatter.Deserialize(stream) as ArrowData;
                    Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                    Quaternion rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);
                    Instantiate(arrowPrefab, position, rotation);
                }
            }

            Debug.Log("Gra wczytana z " + path);
        }
        catch (Exception e)
        {
            Debug.LogError("B³¹d podczas wczytywania gry: " + e.Message);
        }
    }
    public List<string> GetSaveList()
    {
        List<string> saveFiles = new List<string>();

        if (Directory.Exists(saveFolder))
        {
            string[] files = Directory.GetFiles(saveFolder, "*.sav");
            foreach (var file in files)
            {
                saveFiles.Add(Path.GetFileName(file));
            }
        }
        else
        {
            Debug.LogWarning("Folder zapisu nie istnieje: " + saveFolder);
        }

        return saveFiles;
    }

    private string GenerateSavePath()
    {
        int saveIndex = 1;
        string path;
        do
        {
            path = saveFolder + "save_" + saveIndex + ".sav";
            saveIndex++;
        } while (File.Exists(path));

        return path;
    }

    private string GetLatestSavePath()
    {
        string[] files = Directory.GetFiles(saveFolder, "*.sav");
        if (files.Length > 0)
        {
            Array.Sort(files, StringComparer.Ordinal);
            return files[^1]; // Ostatni zapis
        }
        return null;
    }

    public void ClearAllSaves()
    {
        string[] files = Directory.GetFiles(saveFolder, "*.sav");
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
                Debug.Log("Plik usuniêty: " + file);
            }
            catch (Exception e)
            {
                Debug.LogError("Nie uda³o siê usun¹æ pliku: " + file + " - B³¹d: " + e.Message);
            }
        }

        ClearGameData();
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

        Resources.UnloadUnusedAssets();
        Debug.Log("Wszystkie dane gry zosta³y wyczyszczone.");
    }
}
