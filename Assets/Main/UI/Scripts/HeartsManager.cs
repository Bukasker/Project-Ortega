using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HeartsManager : MonoBehaviour
{
    [SerializeField] private List<Image> hearts;
    [SerializeField] private Sprite destroyedHearth;
    [SerializeField] private Sprite reguralHearth;

    [SerializeField] private UnityEvent victoryPopoutEvent;

    [SerializeField] private UnityEvent playCollectSFX;
    [SerializeField] private UnityEvent playLoseSFX;

    private int heartLeft = 3;

    public void DecreseHearts()
    {
        playCollectSFX.Invoke();
        hearts[heartLeft-1].sprite = destroyedHearth;
        heartLeft -= 1;
        if(heartLeft == 0)
        {
            PointerSceneInfo.PointerSceneInfoInstance.isMonsterCatched = true;
            victoryPopoutEvent.Invoke();
        }
    }
    public void IncreaseHearts()
    {
        playLoseSFX.Invoke();
        heartLeft += 1;
        if (heartLeft > 3)
        {
            heartLeft = 3;
        }
        hearts[heartLeft - 1].sprite = reguralHearth;
    }
}
