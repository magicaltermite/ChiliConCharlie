using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100; // The amount of health the unit has

    const string Walk_Animation = "ZombieWalking";
    const string Hit_Animation = "Zombie_Hit";

    private string currentState;

    bool hit = false;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void Damage(int damage) {
        if(damage < 0) {
            throw new System.ArgumentOutOfRangeException("Damage cannot be negative"); // Since we dont want to be capable of dealing negative damage, i've made the script throw an error
        }

        health -= damage; // This script is made to allow someone to damage this character
        animator.SetTrigger("HitTrigger");
        if (health <= 0) {
            Kill();
        }
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;
        animator.Play(newState);
        currentState = newState;
    }

    private void Kill() {
        this.gameObject.SetActive(false);
    }
}
