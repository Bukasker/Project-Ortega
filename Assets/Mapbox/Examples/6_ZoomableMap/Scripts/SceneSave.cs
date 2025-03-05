using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ObjectData
{
	public string name;
	public Vector3 position;
	public Quaternion rotation;
}

[System.Serializable]
public class SceneState
{
	public string sceneName;
	public List<ObjectData> objects = new List<ObjectData>();
}

public class SceneSave : MonoBehaviour
{
	private const string SaveFilePath = "sceneState.json";

	public void SaveSceneState()
	{
		SceneState sceneState = new SceneState
		{
			sceneName = SceneManager.GetActiveScene().name
		};

		foreach (GameObject obj in FindObjectsOfType<GameObject>())
		{
			ObjectData data = new ObjectData
			{
				name = obj.name,
				position = obj.transform.position,
				rotation = obj.transform.rotation
			};
			sceneState.objects.Add(data);
		}

		string json = JsonUtility.ToJson(sceneState, true);
		File.WriteAllText(GetSaveFilePath(), json);

		Debug.Log("Zapisano stan sceny: " + json);
	}
	public void LoadSceneState()
	{
		string filePath = GetSaveFilePath();
		if (File.Exists(filePath))
		{
			string json = File.ReadAllText(filePath);
			SceneState sceneState = JsonUtility.FromJson<SceneState>(json);

			if (sceneState.sceneName != SceneManager.GetActiveScene().name)
			{
				Debug.LogWarning("Stan sceny nie pasuje do aktualnie za³adowanej sceny.");
				return;
			}

			foreach (ObjectData data in sceneState.objects)
			{
				GameObject obj = new GameObject(data.name);
				obj.transform.position = data.position;
				obj.transform.rotation = data.rotation;
			}

			Debug.Log("Wczytano stan sceny: " + json);
		}
		else
		{
			Debug.LogWarning("Brak zapisanego stanu sceny.");
		}
	}

	private string GetSaveFilePath()
	{
		return Path.Combine(Application.persistentDataPath, SaveFilePath);
	}
}
