using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClipOnAnimation : StateMachineBehaviour
{
    public AudioClip soundClip; 
    private AudioSource audioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (soundClip != null)
        {
            if (audioSource == null)
            {
                audioSource = animator.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = animator.gameObject.AddComponent<AudioSource>();
                }
            }

            // Odtworzenie dŸwiêku
            audioSource.PlayOneShot(soundClip);
        }
    }
}
