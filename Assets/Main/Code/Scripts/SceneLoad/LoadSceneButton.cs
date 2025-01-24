using Mapbox.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void ChangeScenne()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // Uzyskaj dost�p do PointerList z mened�era scen
            var pointerObjects = SceneEventManager.Instance?.PointerObjects;

            if (pointerObjects != null && pointerObjects.pointers.Count > 0)
            {
                var pointer = pointerObjects.pointers[pointerObjects.pointers.Count - 1];
                var info = pointer.GetComponent<PointerSceneInfo>();
                info.SetInstance();
            }
            else
            {
                Debug.LogWarning("PointerList nie zosta� znaleziony lub jest pusty przed zmian� sceny.");
            }

            // Za�aduj now� scen�
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Nie ustawiono nazwy sceny do wczytania!");
        }
    }
}
