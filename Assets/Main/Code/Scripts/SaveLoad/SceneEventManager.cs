using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEventManager : MonoBehaviour
{
    public static SceneEventManager Instance { get; private set; }
    public PointerList PointerObjects { get; private set; }

    private void Awake()
    {
        // Upewnij si�, �e istnieje tylko jedna instancja tego obiektu
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subskrybuj zdarzenie �adowania scen
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Odsubskrybuj zdarzenie, aby unikn�� wyciek�w pami�ci
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Znajd� PointerList w nowo za�adowanej scenie
        PointerObjects = FindObjectOfType<PointerList>();

        if (PointerObjects != null)
        {
            Debug.Log($"PointerList zosta� znaleziony w scenie: {scene.name}");
        }
        else
        {
            Debug.LogWarning($"PointerList NIE znaleziono w scenie: {scene.name}");
        }
    }
}
