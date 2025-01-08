using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HideDontDestroyOnLoadObjects : MonoBehaviour
{
	public List<GameObject> dontDestroyOnLoadObjects = new List<GameObject>();

	void Awake()
	{
		// Znajdü wszystkie obiekty w scenie DontDestroyOnLoad
		FindDontDestroyOnLoadObjects();

		// Subskrypcja zdarzenia zmiany sceny
		SceneManager.activeSceneChanged += OnSceneChanged;
	}

	private void FindDontDestroyOnLoadObjects()
	{
		// Tymczasowy obiekt pomocniczy
		GameObject temp = new GameObject();
		DontDestroyOnLoad(temp);

		// Pobierz wszystkie obiekty w DontDestroyOnLoad
		foreach (GameObject obj in temp.scene.GetRootGameObjects())
		{
			dontDestroyOnLoadObjects.Add(obj);
		}

		Destroy(temp); // Usuwamy tymczasowy obiekt
	}

	private void OnSceneChanged(Scene oldScene, Scene newScene)
	{
		if (newScene.name == "AR")
		{
			SetObjectsVisibility(false);
		}
		else
		{
			SetObjectsVisibility(true);
		}
	}

	private void SetObjectsVisibility(bool isVisible)
	{
		foreach (var obj in dontDestroyOnLoadObjects)
		{
			Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
			foreach (var renderer in renderers)
			{
				renderer.enabled = isVisible;
			}

			Canvas[] canvases = obj.GetComponentsInChildren<Canvas>();
			foreach (var canvas in canvases)
			{
				canvas.enabled = isVisible;
			}
		}
	}

	private void OnDestroy()
	{
		// Odsubskrybowanie od zdarzenia przy niszczeniu obiektu
		SceneManager.activeSceneChanged -= OnSceneChanged;
	}
}
