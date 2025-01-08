using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneOnTouch : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Nazwa sceny do za³adowania

    private RectTransform circleRectTransform; // RectTransform ko³a
    private Animator canvasAnimator; // Animator przypisany do Canvas
    private Canvas parentCanvas; // Canvas, na którym znajduje siê ko³o

    private void Start()
    {
        // ZnajdŸ obiekt Canvas po tagu
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        if (canvasObject != null)
        {
            parentCanvas = canvasObject.GetComponent<Canvas>();
            canvasAnimator = canvasObject.GetComponent<Animator>();

            if (parentCanvas == null)
            {
                Debug.LogWarning("Canvas nie zosta³ znaleziony na obiekcie Canvas!");
            }

            if (canvasAnimator == null)
            {
                Debug.LogWarning("Animator nie zosta³ znaleziony na obiekcie Canvas!");
            }

            // ZnajdŸ obiekt o nazwie "CircleEffect" w dzieciach Canvas
            Transform circleTransform = canvasObject.transform.Find("CircleEffect");
            if (circleTransform != null)
            {
                circleRectTransform = circleTransform.GetComponent<RectTransform>();
                if (circleRectTransform == null)
                {
                    Debug.LogWarning("Nie znaleziono RectTransform na obiekcie CircleEffect!");
                }
            }
            else
            {
                Debug.LogWarning("Nie znaleziono obiektu CircleEffect w dzieciach Canvas!");
            }
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu Canvas z tagiem 'Canvas' w scenie!");
        }
    }

    private void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // Ustaw pozycjê ko³a na miejsce klikniêcia myszk¹
            if (circleRectTransform != null && parentCanvas != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform, // RectTransform Canvas
                    Input.mousePosition, // Pozycja kursora
                    parentCanvas.worldCamera, // Kamera przypisana do Canvas
                    out localPoint // Wyjœcie: punkt w lokalnych wspó³rzêdnych Canvas
                );

                // Ustaw now¹ pozycjê dla kó³ka
                circleRectTransform.anchoredPosition = localPoint;
            }
            else
            {
                Debug.LogWarning("Nie przypisano RectTransformu dla ko³a lub brak Canvas!");
            }

            // Uruchom trigger animacji
            if (canvasAnimator != null)
            {
                canvasAnimator.SetTrigger("IsTransiting");
            }

            // Poczekaj 0.4 sekundy i za³aduj scenê
            StartCoroutine(LoadSceneWithDelay(0.4f));
        }
        else
        {
            Debug.LogWarning("Nie ustawiono nazwy sceny do wczytania!");
        }
    }

    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
