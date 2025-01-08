﻿namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using System.Collections.Generic;
    using TMPro;
    using Random = UnityEngine.Random;
    using GeoCoordinatePortable;
    using System;
    using Mapbox.Unity.Utilities;
    using Mapbox.Platform.Cache;

    public class SpawnOnMap : MonoBehaviour
    {
        [SerializeField]
        AbstractMap map;

        [SerializeField]
        [Geocode]
        string[] locationStrings;

        Vector2d[] locations;

        [SerializeField]
        float spawnScale = 100f;

        [SerializeField]
        GameObject markerPrefab;
        [SerializeField]
        GameObject Line;
        [SerializeField]
        GameObject arrowPrefab;

        [SerializeField] List<GameObject> spawnedObjects;
        [SerializeField] List<LineRenderer> lines;
        [SerializeField] List<GameObject> arrowInstances;

        private EventPonter EventPonter;
        private GameObject EventGameobject;
        LocationStatus playerLocation;

        void Start()
        {
            spawnedObjects = new List<GameObject>();
            lines = new List<LineRenderer>();
            arrowInstances = new List<GameObject>();
        }

        private void Update()
        {
            if (spawnedObjects.Count > 0)
            {
                EventGameobject = spawnedObjects[0];
                EventPonter = EventGameobject.GetComponentInChildren<EventPonter>();
                if (EventPonter != null)
                {
                    EventPonter.DisplayDistance();
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                AddPlayerLocationPoint();
            }

            int count = spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var spawnedObject = spawnedObjects[i];
                var location = locations[i];
                spawnedObject.transform.localPosition = map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);

                if (i > 0 && i - 1 < lines.Count)
                {
                    lines[i - 1].SetPosition(0, spawnedObjects[i - 1].transform.position);
                    lines[i - 1].SetPosition(1, spawnedObjects[i].transform.position);

                    var direction = (spawnedObjects[i].transform.position - spawnedObjects[i - 1].transform.position).normalized;
                    arrowInstances[i - 1].transform.position = spawnedObjects[i].transform.position;
                    Vector3 localOffset = arrowInstances[i - 1].transform.TransformDirection(0, 0, -2f);
                    arrowInstances[i - 1].transform.position += localOffset;

                    float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    arrowInstances[i - 1].transform.rotation = Quaternion.Euler(0, angleY, 0);
                }
            }
        }

        public void AddPlayerLocationPoint()
        {

            playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
            var playerLat = playerLocation.GetLocationLat();
            var playerLon = playerLocation.GetLocationLon();
            var playerPosition = new Vector2d(playerLat, playerLon);

            if (locations == null)
            {
                locations = new Vector2d[0];
            }
            Array.Resize(ref locations, locations.Length + 1);
            locations[locations.Length - 1] = playerPosition;

            var instance = Instantiate(markerPrefab);
            instance.transform.localPosition = map.GeoToWorldPosition(playerPosition, true);
            instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
            spawnedObjects.Add(instance);

            if (spawnedObjects.Count > 1)
            {
                var lineObj = Instantiate(Line);
                var line = lineObj.GetComponent<LineRenderer>();
                line.startWidth = 0.7f;
                line.endWidth = 0.7f;
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.startColor = Color.red;
                line.endColor = Color.red;
                lines.Add(line);

                line.SetPosition(0, spawnedObjects[spawnedObjects.Count - 2].transform.position);
                line.SetPosition(1, spawnedObjects[spawnedObjects.Count - 1].transform.position);

                var arrowInstance = Instantiate(arrowPrefab);
                arrowInstances.Add(arrowInstance);

                var direction = (spawnedObjects[spawnedObjects.Count - 1].transform.position - spawnedObjects[spawnedObjects.Count - 2].transform.position).normalized;
                arrowInstance.transform.position = spawnedObjects[spawnedObjects.Count - 1].transform.position;
                Vector3 localOffset = arrowInstance.transform.TransformDirection(0, 0, -2f);
                arrowInstance.transform.position += localOffset;

                float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                arrowInstance.transform.rotation = Quaternion.Euler(0, angleY, 0);
            }

            var eventPonter = instance.GetComponentInChildren<EventPonter>();
            if (eventPonter != null)
            {
                eventPonter.eventPos = playerPosition;
            }
        }

        public void SpawnAroundPlayer()
        {
            spawnedObjects.Clear();
            lines.Clear();
            arrowInstances.Clear();

            int numberOfObjects = 3;
            float spawnRadiusInMeters = 120f;
            locations = new Vector2d[numberOfObjects];

            playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
            var playerPosition = new GeoCoordinate(playerLocation.GetLocationLat(), playerLocation.GetLocationLon());

            locationStrings = new string[numberOfObjects];

            for (int i = 0; i < numberOfObjects; i++)
            {
                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(30.0f, spawnRadiusInMeters);

                double earthCircumference = 40075000.0;
                double latOffset = (distance / earthCircumference) * 360.0;
                double lonOffset = (distance / (earthCircumference * Mathf.Cos((float)(playerPosition.Latitude * Mathf.Deg2Rad)))) * 360.0;

                double offsetLat = latOffset * Mathf.Cos(angle * Mathf.Deg2Rad);
                double offsetLon = lonOffset * Mathf.Sin(angle * Mathf.Deg2Rad);

                double spawnLat = playerPosition.Latitude + offsetLat;
                double spawnLon = playerPosition.Longitude + offsetLon;

                locationStrings[i] = $"{spawnLat},{spawnLon}";

                locations[i] = new Vector2d(spawnLat, spawnLon);

                var instance = Instantiate(markerPrefab);
                instance.transform.localPosition = map.GeoToWorldPosition(locations[i], true);
                instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
                spawnedObjects.Add(instance);

                var eventPonter = instance.GetComponentInChildren<EventPonter>();
                if (eventPonter != null)
                {
                    eventPonter.eventPos = locations[i];
                }
            }
            for (int i = 1; i < locationStrings.Length; i++)
            {
                if (numberOfObjects > 0)
                {
                    var lineObj = Instantiate(Line);
                    var line = lineObj.GetComponent<LineRenderer>();
                    line.startWidth = 0.7f;
                    line.endWidth = 0.7f;
                    line.material = new Material(Shader.Find("Sprites/Default"));
                    line.startColor = Color.red;
                    line.endColor = Color.red;
                    lines.Add(line);

                    var arrowInstance = Instantiate(arrowPrefab);
                    arrowInstances.Add(arrowInstance);
                }
            }
        }
    }
}