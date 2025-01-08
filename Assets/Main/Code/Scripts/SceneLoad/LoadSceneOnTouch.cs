using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneOnTouch : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Nazwa sceny do za�adowania

    private RectTransform circleRectTransform; // RectTransform ko�a
    private Animator canvasAnimator; // Animator przypisany do Canvas
    private Canvas parentCanvas; // Canvas, na kt�rym znajduje si� ko�o

    private void Start()
    {
        // Znajd� obiekt Canvas po tagu
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        if (canvasObject != null)
        {
            parentCanvas = canvasObject.GetComponent<Canvas>();
            canvasAnimator = canvasObject.GetComponent<Animator>();

            if (parentCanvas == null)
            {
                Debug.LogWarning("Canvas nie zosta� znaleziony na obiekcie Canvas!");
            }

            if (canvasAnimator == null)
            {
                Debug.LogWarning("Animator nie zosta� znaleziony na obiekcie Canvas!");
            }

            // Znajd� obiekt o nazwie "CircleEffect" w dzieciach Canvas
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
            // Ustaw pozycj� ko�a na miejsce klikni�cia myszk�
            if (circleRectTransform != null && parentCanvas != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform, // RectTransform Canvas
                    Input.mousePosition, // Pozycja kursora
                    parentCanvas.worldCamera, // Kamera przypisana do Canvas
                    out localPoint // Wyj�cie: punkt w lokalnych wsp�rz�dnych Canvas
                );

                // Ustaw now� pozycj� dla k�ka
                circleRectTransform.anchoredPosition = localPoint;
            }
            else
            {
                Debug.LogWarning("Nie przypisano RectTransformu dla ko�a lub brak Canvas!");
            }

            // Uruchom trigger animacji
            if (canvasAnimator != null)
            {
                canvasAnimator.SetTrigger("IsTransiting");
            }

            // Poczekaj 0.4 sekundy i za�aduj scen�
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
