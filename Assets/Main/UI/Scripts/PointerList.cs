using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointerList : MonoBehaviour
{
    public List<GameObject> Pointers;
    public List<LineRenderer> Lines;
    public List<GameObject> Arrows;
    public Vector2d[] Locations;

    public static PointerList Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Pointers.RemoveAll(obj => obj == null);
        Lines.RemoveAll(obj => obj == null);
        Arrows.RemoveAll(obj => obj == null);
    }

    public void StartCor()
    {
        StartCoroutine(ClearAllPointersAsync());
    }

    IEnumerator ClearAllPointersAsync()
    {
        foreach (var pointer in Pointers)
        {
            if (pointer != null)
            {
                Destroy(pointer);
                yield return null; 
            }
        }
        Pointers.Clear();

        // Usuwanie obiektów z Lines
        foreach (var line in Lines)
        {
            if (line != null)
            {
                Destroy(line.gameObject);
                yield return null;
            }
        }
        Lines.Clear();

        // Usuwanie obiektów z Arrows
        foreach (var arrow in Arrows)
        {
            if (arrow != null)
            {
                Destroy(arrow);
                yield return null;
            }
        }
        Arrows.Clear();
    }


}
