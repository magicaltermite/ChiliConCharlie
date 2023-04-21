using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const string Walk_Animation = "ZombieWalking";
    private const string Hit_Animation = "Zombie_Hit";
    [SerializeField] private int health = 100; // The amount of health the unit has


    private bool hit = false;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    public void Damage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(
                "Damage cannot be negative"); // Since we dont want to be capable of dealing negative damage, i've made the script throw an error

        health -= damage; // This script is made to allow someone to damage this character
        animator.SetTrigger("HitTrigger");
        if (health <= 0) Kill();

        if (health <= 0) {
            Kill();
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;
        animator.Play(newState);
        currentState = newState;
    }


    private void Kill()
    {
        gameObject.SetActive(false);
    }
}