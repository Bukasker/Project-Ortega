using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip AudioClipFirst;
    [SerializeField] private AudioClip AudioClipSecond;

    public void ChangeMusicForFirst()
    {
        audioSource.clip = AudioClipFirst;
        audioSource.Play();
    }
    public void ChangeMusicForSecond()
    {
        audioSource.clip = AudioClipSecond;
        audioSource.Play();
    }
}
