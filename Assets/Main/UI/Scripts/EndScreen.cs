using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject victoryPopout;
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
        var pointerObjects = PointerList.Instance?.Pointers;

        if (pointerObjects != null && pointerObjects.Count > 0)
        {
            bool allMonstersCatched = pointerObjects.All(p => p.GetComponent<PointerSceneInfo>()?.isMonsterCatched == true);

            if (allMonstersCatched)
            {
                if (animator != null)
                {
                    victoryPopout.SetActive(true);
                    animator.SetTrigger("IsEnemyDead");
                }
                return;
            }
        }
    }
    public void ResetVictory()
    {
        victoryPopout.SetActive(false);
    }
}
