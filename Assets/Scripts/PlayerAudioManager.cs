using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour {
    
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;

    
    void WalkSound() {
        walkSound.pitch = Random.Range(0.8f, 1.2f);
        walkSound.volume = Random.Range(0.8f, 1.2f);
        walkSound.Play();
    }

    void JumpSound() {
        jumpSound.pitch = Random.Range(0.8f, 1.2f);
        jumpSound.Play();
    }

    public void LandSound() {
        landSound.Play();
    }
}
