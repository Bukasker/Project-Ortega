using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

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
        // SprawdŸ, czy klikniêto na UI i czy element ma CanvasGroup z w³¹czonym blocksRaycasts
        if (IsPointerOverBlockingCanvasGroup())
        {
            Debug.Log("Klikniêto na UI z aktywnym blokowaniem interakcji!");
            return;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // Reszta istniej¹cego kodu...
            if (circleRectTransform != null && parentCanvas != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    Input.mousePosition,
                    parentCanvas.worldCamera,
                    out localPoint
                );

                circleRectTransform.anchoredPosition = localPoint;
            }

            if (canvasAnimator != null)
            {
                canvasAnimator.SetTrigger("IsTransiting");
            }

            StartCoroutine(LoadSceneWithDelay(0.4f));
        }
        else
        {
            Debug.LogWarning("Nie ustawiono nazwy sceny do wczytania!");
        }
    }

    private bool IsPointerOverBlockingCanvasGroup()
    {
        if (EventSystem.current == null) return false;

        // Lista wyników dla raycasta
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // SprawdŸ, czy któryœ z elementów ma CanvasGroup z blocksRaycasts
        foreach (var result in raycastResults)
        {
            var canvasGroup = result.gameObject.GetComponentInParent<CanvasGroup>();
            if (canvasGroup != null && canvasGroup.blocksRaycasts)
            {
                return true; // Znaleziono blokuj¹cy element UI
            }
        }

        return false; // Nie znaleziono elementu UI blokuj¹cego interakcjê
    }

    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
