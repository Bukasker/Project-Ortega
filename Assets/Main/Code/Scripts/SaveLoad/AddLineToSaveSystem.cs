using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddLineToSaveSystem : MonoBehaviour
{
	private void Awake()
	{
		SaveSystem.lineGameObjects.Add(this);
	}
	private void OnDestroy()
	{
		SaveSystem.lineGameObjects.Remove(this);
	}
}
