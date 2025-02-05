using System.Collections;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneForFirstPointer : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Button button;
    [SerializeField] private Animator animator;
    private double Distance;
    private RectTransform circleRectTransform;
    private Animator canvasAnimator;
    private GameObject pointer;

    private void Awake()
    {
        FindCanvasAnimator();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCanvasAnimator();
    }

    private void FindCanvasAnimator()
    {
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        if (canvasObject != null)
        {
            canvasAnimator = canvasObject.GetComponent<Animator>();
        }
    }

    public void LoadScenePointer()
    {
        if (PointerSceneInfo.PointerSceneInfoInstance != null)
        {
            var pointerObjects = PointerList.Instance?.Pointers;

            if (pointerObjects != null && pointerObjects.Count > 0)
            {
                bool allMonstersCatched = pointerObjects.All(p => p.GetComponent<PointerSceneInfo>()?.isMonsterCatched == true);

                if (allMonstersCatched)
                {
                    if (animator != null)
                    {
                        animator.SetTrigger("CantDoPhoto");
                    }
                    return;
                }

                foreach (var p in pointerObjects)
                {
                    var info = p.GetComponent<PointerSceneInfo>();
                    if (info != null && !info.isMonsterCatched)
                    {
                        pointer = p;
                        break;
                    }
                }

                if (pointer != null)
                {
                    var info = pointer.GetComponent<PointerSceneInfo>();
                    var texturesManager = pointer.GetComponent<TextureManager>();
                    texturesManager.SetAsInstance();
                    info.SetInstance();

                    var eventPointer = pointer.GetComponent<EventPointer>();
                    Distance = eventPointer.GetDistance();

                    if ((info.isMonsterCatched && info.isCameraPhotoSet) || Distance >= 40f)
                    {
                        if (animator != null)
                        {
                            animator.SetTrigger("CantDoPhoto");
                        }
                        return;
                    }
                }
            }

            if (canvasAnimator != null)
            {
                canvasAnimator.SetTrigger("IsTransiting");
            }
            StartCoroutine(LoadSceneWithDelay(0.5f));
        }
    }


    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
