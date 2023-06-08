using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour {
    
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource attackSound;

    
    void WalkSound() {
        walkSound.pitch = Random.Range(0.8f, 1.2f);
        walkSound.volume = Random.Range(0.8f, 1.2f);
        walkSound.Play();
    }

    void JumpSound() {
        jumpSound.pitch = Random.Range(0.8f, 1.2f);
        jumpSound.Play();
    }


    void AttackSound() {
        attackSound.pitch = Random.Range(0.8f, 1.2f);
        attackSound.Play();
    }

   
}
