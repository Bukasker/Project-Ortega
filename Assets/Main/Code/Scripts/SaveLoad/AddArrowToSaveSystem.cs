using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddArrowToSaveSystem : MonoBehaviour
{
	private void Awake()
	{
		SaveSystem.arrowGameObjects.Add(this);
	}
	private void OnDestroy()
	{
		SaveSystem.arrowGameObjects.Remove(this);
	}
}
