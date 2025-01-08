using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;
public class SaveSystem : MonoBehaviour
{
	[SerializeField] EventPonter ponterPrefab;
	[SerializeField] GameObject linePrefab;
	[SerializeField] GameObject arrowPrefab;
	public static List<EventPonter> mapPoints = new List<EventPonter>(); // Prefabrykaty, które mo¿na odtworzyæ
	public static List<AddLineToSaveSystem> lineGameObjects = new List<AddLineToSaveSystem>();
	public static List<AddArrowToSaveSystem> arrowGameObjects = new List<AddArrowToSaveSystem>();

	const string Point_SUB = "/Point";
	const string Point_Count_SUB = "/Point.count";


	private void Awake()
	{
		LoadGame();
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}


	public void SaveGame()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + Point_SUB + SceneManager.GetActiveScene().buildIndex;

		FileStream stream = new FileStream(path, FileMode.Create);

		// Zapisz liczbê mapPoints
		formatter.Serialize(stream, mapPoints.Count);
		// Zapisz dane mapPoints
		foreach (var point in mapPoints)
		{
			MapPointData data = new MapPointData(point);
			formatter.Serialize(stream, data);
		}

		// Zapisz liczbê lineGameObjects
		formatter.Serialize(stream, lineGameObjects.Count);
		// Zapisz dane lineGameObjects
		foreach (var line in lineGameObjects)
		{
			LineData data = new LineData(line);
			formatter.Serialize(stream, data);
		}

		// Zapisz liczbê arrowGameObjects
		formatter.Serialize(stream, arrowGameObjects.Count);
		// Zapisz dane arrowGameObjects
		foreach (var arrow in arrowGameObjects)
		{
			ArrowData data = new ArrowData(arrow);
			formatter.Serialize(stream, data);
		}

		stream.Close();

		Debug.Log("Gra zapisana w " + path);
	}

	public void LoadGame()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + Point_SUB + SceneManager.GetActiveScene().buildIndex;

		if (File.Exists(path))
		{
			FileStream stream = new FileStream(path, FileMode.Open);

			// Wczytaj liczbê mapPoints
			int mapPointCount = (int)formatter.Deserialize(stream);
			for (int i = 0; i < mapPointCount; i++)
			{
				MapPointData data = formatter.Deserialize(stream) as MapPointData;

				Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
				EventPonter ponter = Instantiate(ponterPrefab, position, Quaternion.identity);
				ponter.eventPos[0] = Convert.ToDouble(data.locationStrings[0]);
				ponter.eventPos[1] = Convert.ToDouble(data.locationStrings[1]);
			}

			// Wczytaj liczbê lineGameObjects
			int lineObjectCount = (int)formatter.Deserialize(stream);
			for (int i = 0; i < lineObjectCount; i++)
			{
				LineData data = formatter.Deserialize(stream) as LineData;

				Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
				Quaternion rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);
				var lineObject = Instantiate(linePrefab, position, rotation);

				// Ustaw pozycje linii w LineRenderer
				var lineRenderer = lineObject.GetComponent<LineRenderer>();
				if (lineRenderer != null)
				{
					Vector3 lineStart = new Vector3(data.lineStart[0], data.lineStart[1], data.lineStart[2]);
					Vector3 lineEnd = new Vector3(data.lineEnd[0], data.lineEnd[1], data.lineEnd[2]);

					lineRenderer.positionCount = 2; // Ustawienie liczby wierzcho³ków
					lineRenderer.SetPosition(0, lineStart);
					lineRenderer.SetPosition(1, lineEnd);

					// Mo¿esz tu ustawiæ inne w³aœciwoœci linii, jeœli s¹ wymagane
					lineRenderer.startWidth = 0.7f;
					lineRenderer.endWidth = 0.7f;
					lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
					lineRenderer.startColor = Color.red;
					lineRenderer.endColor = Color.red;
				}

				//lineGameObjects.Add(lineObject.GetComponent<AddLineToSaveSystem>());
			}

			// Wczytaj liczbê arrowGameObjects
			int arrowObjectCount = (int)formatter.Deserialize(stream);
			for (int i = 0; i < arrowObjectCount; i++)
			{
				ArrowData data = formatter.Deserialize(stream) as ArrowData;

				Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
				Quaternion rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);
				var arrowObject = Instantiate(arrowPrefab, position, rotation);

				//arrowGameObjects.Add(arrowObject.GetComponent<AddArrowToSaveSystem>());
			}

			stream.Close();

			Debug.Log("Gra wczytana z " + path);
		}
		else
		{
			Debug.LogError("Plik zapisu nie znaleziony w " + path);
		}
	}
	public void ClearSave()
	{
		string path = Application.persistentDataPath + Point_SUB + SceneManager.GetActiveScene().buildIndex;

		// Usuñ plik zapisu, jeœli istnieje
		if (File.Exists(path))
		{
			File.Delete(path);
			Debug.Log("Zapis zosta³ wyczyszczony z " + path);
		}
		else
		{
			Debug.LogWarning("Nie znaleziono pliku zapisu do usuniêcia w " + path);
		}

		// Wyczyœæ listy obiektów
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

		Debug.Log("Listy obiektów zosta³y wyczyszczone.");
	}

}
