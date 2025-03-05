namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;
    using System;
    using GeoCoordinatePortable;
    using Random = UnityEngine.Random;

    public class SpawnOnMap : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] private AbstractMap map;

        [Header("Prefabs Settings")]
        [SerializeField] private float spawnScale = 100f;
        [SerializeField] private GameObject markerPrefab;
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject arrowPrefab;

        [Header("Prefab List Settings")]
        public List<GameObject> spawnedObjects = new List<GameObject>();
        public List<GameObject> arrowInstances = new List<GameObject>();
        public List<LineRenderer> lines = new List<LineRenderer>();

        private string[] locationStrings;
        private Vector2d[] locations;
        private EventPointer eventPointer;
        private GameObject eventGameObject;
        private LocationStatus playerLocation;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (PointerList.Instance == null) return;

            spawnedObjects.AddRange(PointerList.Instance.Pointers);
            lines.AddRange(PointerList.Instance.Lines);
            arrowInstances.AddRange(PointerList.Instance.Arrows);

            locations = new Vector2d[PointerList.Instance.Locations.Length];
            Array.Copy(PointerList.Instance.Locations, locations, locations.Length);
        }

        private void Update()
        {
            if (spawnedObjects.Count > 0 && spawnedObjects[0] != null)
            {
                eventGameObject = spawnedObjects[0];
                eventPointer = eventGameObject.GetComponentInChildren<EventPointer>();
                eventPointer?.GetDistance();
            }
            spawnedObjects.RemoveAll(obj => obj == null);

            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                spawnedObjects[i].transform.localPosition = map.GeoToWorldPosition(locations[i], true);
                spawnedObjects[i].transform.localScale = Vector3.one * spawnScale;

                if (i > 0 && i - 1 < lines.Count)
                {
                    lines[i - 1].SetPositions(new Vector3[] { spawnedObjects[i - 1].transform.position, spawnedObjects[i].transform.position });

                    var direction = (spawnedObjects[i].transform.position - spawnedObjects[i - 1].transform.position).normalized;
                    arrowInstances[i - 1].transform.position = spawnedObjects[i].transform.position;
                    arrowInstances[i - 1].transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
                }
            }
        }

        public void AddPlayerLocationPoint()
        {
            playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
            var playerPosition = new Vector2d(playerLocation.GetLocationLat(), playerLocation.GetLocationLon());

            Array.Resize(ref locations, locations.Length + 1);
            locations[^1] = playerPosition;
            PointerList.Instance.Locations = locations;

            var instance = Instantiate(markerPrefab);
            instance.transform.localPosition = map.GeoToWorldPosition(playerPosition, true);
            instance.transform.localScale = Vector3.one * spawnScale;
            spawnedObjects.Add(instance);
            PointerList.Instance.Pointers.Add(instance);

            if (spawnedObjects.Count > 1)
            {
                var line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                line.SetPositions(new Vector3[] { spawnedObjects[^2].transform.position, spawnedObjects[^1].transform.position });
                lines.Add(line);
                PointerList.Instance.Lines.Add(line);

                var arrowInstance = Instantiate(arrowPrefab);
                arrowInstances.Add(arrowInstance);
                PointerList.Instance.Arrows.Add(arrowInstance);
            }

            var eventPointer = instance.GetComponentInChildren<EventPointer>();
            if (eventPointer != null)
            {
                eventPointer.eventPos = playerPosition;
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

            for (int i = 0; i < numberOfObjects; i++)
            {
                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(30f, spawnRadiusInMeters);

                float earthCircumference = 40075000f;
                float latOffset = (distance / earthCircumference) * 360f;
                float lonOffset = (float)((distance / (earthCircumference * Mathf.Cos((float)playerPosition.Latitude * Mathf.Deg2Rad))) * 360f);

                float spawnLat = (float)(playerPosition.Latitude + latOffset * Mathf.Cos(angle * Mathf.Deg2Rad));
                float spawnLon = (float)(playerPosition.Longitude + lonOffset * Mathf.Sin(angle * Mathf.Deg2Rad));
                locations[i] = new Vector2d(spawnLat, spawnLon);

                var instance = Instantiate(markerPrefab);
                instance.transform.localPosition = map.GeoToWorldPosition(locations[i], true);
                instance.transform.localScale = Vector3.one * spawnScale;
                spawnedObjects.Add(instance);

                var eventPointer = instance.GetComponentInChildren<EventPointer>();
                if (eventPointer != null)
                {
                    eventPointer.eventPos = locations[i];
                }
            }

            for (int i = 1; i < numberOfObjects; i++)
            {
                var line = Instantiate(linePrefab).GetComponent<LineRenderer>();
                lines.Add(line);
                var arrowInstance = Instantiate(arrowPrefab);
                arrowInstances.Add(arrowInstance);
            }
        }
    }
}
