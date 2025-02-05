using System.Collections;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;
using Mapbox.Utils;

public class PlayerLocation : MonoBehaviour
{
    public AbstractMap map;
    public LocationArrayEditorLocationProvider locationProvider;

    void OnEnable()
    {
        LoadPlayerPosition();
    }

    void OnDisable()
    {
        SavePlayerPosition(); 
    }

    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Lokalizacja jest wy³¹czona.");
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
            Debug.LogError("Nie mo¿na uzyskaæ lokalizacji.");
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
        double latitude = Input.location.lastData.latitude;
        double longitude = Input.location.lastData.longitude;

        Vector3 playerPosition = map.GeoToWorldPosition(new Vector2d(latitude, longitude), true);
        transform.position = playerPosition;

        locationProvider.UpdateLocation(latitude.ToString(), longitude.ToString());
    }

    private IEnumerator UpdatePlayerPositionContinuously()
    {
        while (true)
        {
            UpdatePlayerPosition();
            yield return new WaitForSeconds(1);
        }
    }

    private void SavePlayerPosition()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            PlayerPrefs.SetFloat("PlayerLatitude", (float)Input.location.lastData.latitude);
            PlayerPrefs.SetFloat("PlayerLongitude", (float)Input.location.lastData.longitude);
            PlayerPrefs.Save();
        }
    }

    private void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("PlayerLatitude") && PlayerPrefs.HasKey("PlayerLongitude"))
        {
            double latitude = PlayerPrefs.GetFloat("PlayerLatitude");
            double longitude = PlayerPrefs.GetFloat("PlayerLongitude");

            Vector3 savedPosition = map.GeoToWorldPosition(new Vector2d(latitude, longitude), true);
            transform.position = savedPosition;

            locationProvider.UpdateLocation(latitude.ToString(), longitude.ToString());
        }
    }
}
