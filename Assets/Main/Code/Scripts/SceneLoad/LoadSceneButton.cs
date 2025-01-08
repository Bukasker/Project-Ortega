using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{

	[SerializeField] private string sceneToLoad;
	public void ChangeScenne()
	{
		if (!string.IsNullOrEmpty(sceneToLoad))
		{
			SceneManager.LoadScene(sceneToLoad);
		}
		else
		{
			Debug.LogWarning("Nie ustawiono nazwy sceny do wczytania!");
		}
	}
}
