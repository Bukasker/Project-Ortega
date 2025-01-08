using System.Collections;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;

public class PlayerLocation : MonoBehaviour
{
	public AbstractMap map;
	public LocationArrayEditorLocationProvider locationProvider; // Dodajemy referencj� do LocationProvider

	IEnumerator Start()
	{
		if (!Input.location.isEnabledByUser)
		{
			Debug.LogError("Lokalizacja jest wy��czona.");
			yield break;
		}

		Input.location.Start();
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		if (Input.location.status == LocationServiceStatus.Failed)
		{
			Debug.LogError("Nie mo�na uzyska� lokalizacji.");
			yield break;
		}
		else
		{
			UpdatePlayerPosition();
		}

		StartCoroutine(UpdatePlayerPositionContinuously());
	}

	private void UpdatePlayerPosition()
	{
		var latitude = Input.location.lastData.latitude.ToString();
		var longitude = Input.location.lastData.longitude.ToString();

		Vector3 playerPosition = map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude), true);
		transform.position = playerPosition;

		// Aktualizujemy pozycj� w LocationProvider
		locationProvider.UpdateLocation(latitude, longitude);
	}

	private IEnumerator UpdatePlayerPositionContinuously()
	{
		while (true)
		{
			UpdatePlayerPosition();
			yield return new WaitForSeconds(1);
		}
	}
}
