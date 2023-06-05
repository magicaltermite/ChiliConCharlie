using System;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const string Walk_Animation = "ZombieWalking";
    private const string Hit_Animation = "Zombie_Hit";
    private const string _deathAnimation = "DeathAnimation";
    [SerializeField] private int health = 100; // The amount of health the unit has

    private Animator animator;

    private string currentState;

    public bool isHit = false;

    public bool isDead = false;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
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
        isHit = true;
        Invoke(nameof(HitReset),0.6f);
        if (health <= 0) Kill();
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
        isDead = true;
        ChangeAnimationState(_deathAnimation);
        Invoke(nameof(RemoveEnemy), 1.5f);
    }

    private void RemoveEnemy()
    {
        gameObject.SetActive(false);
    }
    
    private void HitReset()
    {
        if (isHit)
        {
            isHit = false;
        }
    }
}