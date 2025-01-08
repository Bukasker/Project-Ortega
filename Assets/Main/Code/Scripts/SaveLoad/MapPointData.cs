using Mapbox.Unity.Utilities;
using UnityEngine;

[System.Serializable]
public class MapPointData
{
	public float[] position = new float[3];  // Pozycja obiektu
	[Geocode]
	public string[] locationStrings = new string[2];

	public MapPointData(EventPonter eventPonter)
	{
		Vector3 poinPos = eventPonter.transform.position;
		locationStrings[0] = eventPonter.eventPos[0].ToString();
		locationStrings[1] = eventPonter.eventPos[1].ToString();

		position[0] = poinPos.x;
		position[1] = poinPos.y;
		position[2] = poinPos.z;
	}
}

