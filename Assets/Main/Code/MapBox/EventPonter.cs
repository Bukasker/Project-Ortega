using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;
using TMPro;
public class EventPonter : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 50f;
	[SerializeField] private float amplitude = 2.0f;
	[SerializeField] private float ferequency = 0.5f;
	[SerializeField] private float pointerYPosition = 10f;
	[SerializeField] public string hint;
	private TMP_Text distanceText;
	LocationStatus playerLocation;
	public Vector2d eventPos;

	private void Awake()
	{
		SaveSystem.mapPoints.Add(this); 
	}
	private void OnDestroy()
	{
		SaveSystem.mapPoints.Remove(this);
	}
	void Update()
	{
	//	FloatAndRotatePointer();
	}
	private void FloatAndRotatePointer()
	{
		transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
		transform.position = new Vector3(transform.position.x, 
										(Mathf.Sin(Time.fixedTime * Mathf.PI * ferequency) * amplitude) + pointerYPosition, 
										transform.position.z);
	}

	public void DisplayDistance()
	{
		/*
		playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
		var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLat(),playerLocation.GetLocationLon());
		var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPos[0], eventPos[1]);
		var distance =  currentPlayerLocation.GetDistanceTo(eventLocation);
		TMP_Text distanceText = GameObject.Find("Canvas/DebugMenu/Distance Text")?.GetComponent<TMP_Text>();
		distanceText.text = "Distance: " + distance.ToString("F2") + " m";
		*/
	}
}
