using Mapbox.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void ChangeScenneForLastObject()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            var pointerObjects = PointerList.Instance?.Pointers;

            if (pointerObjects != null && pointerObjects.Count > 0)
            {
                var pointer = pointerObjects[pointerObjects.Count - 1];
                var info = pointer.GetComponent<PointerSceneInfo>();
                var texturesMenager = pointer.GetComponent<TextureManager>();
                texturesMenager.SetAsInstance();
                info.SetInstance();
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }
    public void ChangeScenneForFirstObject()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
