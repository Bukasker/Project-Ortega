using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineData 
{
	public float[] position = new float[3];
	public float[] rotation = new float[3];
	public float[] lineStart = new float[3];
	public float[] lineEnd = new float[3];

	public LineData(AddLineToSaveSystem saveObject)
	{
		Vector3 objectPos = saveObject.transform.position;
		Vector3 objectRotation = saveObject.transform.eulerAngles;

		var lineRendere = saveObject.gameObject.GetComponent<LineRenderer>();

		Vector3 objectLineStart = lineRendere.GetPosition(0);
		Vector3 objectLineEnd = lineRendere.GetPosition(1);

		position[0] = objectPos.x;
		position[1] = objectPos.y;
		position[2] = objectPos.z;

		rotation[0] = objectRotation.x;
		rotation[1] = objectRotation.y;
		rotation[2] = objectRotation.z;

		lineStart[0] = objectLineStart.x;
		lineStart[1] = objectLineStart.y;
		lineStart[2] = objectLineStart.z;

		lineEnd[0] = objectLineEnd.x;
		lineEnd[1] = objectLineEnd.y;
		lineEnd[2] = objectLineEnd.z;
	}
}
