using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class LoadSceneOnTouch : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; 

    private RectTransform circleRectTransform;
    private Animator canvasAnimator;
    private Canvas parentCanvas;
    private Transform circleTransform;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializeSceneElements();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeSceneElements();
    }

    private void InitializeSceneElements()
    {
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        if (canvasObject != null)
        {
            parentCanvas = canvasObject.GetComponent<Canvas>();
            canvasAnimator = canvasObject.GetComponent<Animator>();

            circleTransform = canvasObject.transform.Find("CircleEffect");
            if (circleTransform != null)
            {
                circleRectTransform = circleTransform.GetComponent<RectTransform>();
            }
        }
    }

    private void OnMouseDown()
    {
        if (IsPointerOverBlockingCanvasGroup())
        {
            Debug.Log("Klikniêto na UI z aktywnym blokowaniem interakcji!");
            return;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
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

            StartCoroutine(LoadSceneWithDelay(0.5f));
        }
        else
        {
            Debug.LogWarning("Nie ustawiono nazwy sceny do wczytania!");
        }
    }

    private bool IsPointerOverBlockingCanvasGroup()
    {
        if (EventSystem.current == null) return false;

        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var result in raycastResults)
        {
            var canvasGroup = result.gameObject.GetComponentInParent<CanvasGroup>();
            if (canvasGroup != null && canvasGroup.blocksRaycasts)
            {
                return true; 
            }
        }

        return false; 
    }

    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
