using System.Collections;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fightUI;
    [SerializeField] private GameObject[] cameraUI;

    [SerializeField] private Animator animator;


    public void SetCameraUIActive()
    {
        for (int i = 0; i < fightUI.Length; i++)
        {
            fightUI[i].SetActive(false);
        }

        for (int i = 0; i < cameraUI.Length; i++)
        {
            cameraUI[i].SetActive(true);
        }
    }

    public void SetFightUIActive()
    {
        animator.SetTrigger("IsTrasistingOut");
        StartCoroutine(DelayFightUI(0.5f));
    }

    private IEnumerator DelayFightUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < fightUI.Length; i++)
        {
            fightUI[i].SetActive(true);
        }

        for (int i = 0; i < cameraUI.Length; i++)
        {
            cameraUI[i].SetActive(false);
        }
    }
}
