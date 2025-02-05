using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextureManager : MonoBehaviour
{
	public static TextureManager Instance { get; private set; }
	public List<Texture2D> textures = new List<Texture2D>();
	
	public void SetAsInstance()
	{
		Instance = this;
	}

	private void OnMouseDown()
	{
		Instance = this;
	}
}
