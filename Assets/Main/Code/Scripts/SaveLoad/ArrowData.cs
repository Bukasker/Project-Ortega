using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrowData
{
	public float[] position = new float[3];
	public float[] rotation = new float[3];

	public ArrowData(AddArrowToSaveSystem saveObject)
	{
		Vector3 objectPos = saveObject.transform.position;
		Vector3 objectRotation = saveObject.transform.eulerAngles;

		position[0] = objectPos.x;
		position[1] = objectPos.y;
		position[2] = objectPos.z;

		rotation[0] = objectRotation.x;
		rotation[1] = objectRotation.y;
		rotation[2] = objectRotation.z;
	}
}
