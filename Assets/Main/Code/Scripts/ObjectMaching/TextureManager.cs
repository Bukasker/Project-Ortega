using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextureManager : MonoBehaviour
{
	public static TextureManager Instance { get; private set; }
	public GameObject pointerGameObject;
	public GameObject pointerReplaceGameObject;

	public List<Texture2D> textures = new List<Texture2D>();

	private void Awake()
	{
		if (textures.Count > 0)
		{
			pointerGameObject.Destroy();
			Instantiate(pointerReplaceGameObject,this.transform);
		}
	}

	private void OnMouseDown()
	{
		Instance = this;
	}
}
