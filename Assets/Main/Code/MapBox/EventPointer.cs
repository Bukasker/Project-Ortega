using UnityEngine;
using Mapbox.Utils;
using TMPro;
using GeoCoordinatePortable;
using Mapbox.Examples;
using System.Xml.Linq;

public class EventPointer : MonoBehaviour
{
    public Vector2d eventPos;
    private LocationStatus playerLocation;

    private void Awake()
    {
        SaveSystem.mapPoints.Add(this);
    }

    private void OnDestroy()
    {
        SaveSystem.mapPoints.Remove(this);
    }

    private void Update()
    {
        GetDistance();
    }
    public double GetDistance()
    {
        playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
        if (playerLocation != null)
        {
            var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLat(), playerLocation.GetLocationLon());
            var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPos[0], eventPos[1]);
            var distance = currentPlayerLocation.GetDistanceTo(eventLocation);
            return distance;
        }
        return 0;
    }
}
