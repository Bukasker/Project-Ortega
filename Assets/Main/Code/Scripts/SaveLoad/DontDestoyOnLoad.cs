using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestoyOnLoad : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		Scene currentScene = SceneManager.GetActiveScene();
		string sceneName = currentScene.name;
		if (sceneName == "AR")
		{
			this.gameObject.SetActive(false);
		}
		else
		{
			this.gameObject.SetActive(true);
		}
	}
}
