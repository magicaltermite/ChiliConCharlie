using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1f;
    
    Animator animator;
    
    
    private string currentState;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeAnimationState(string newState)
    {
        // Stop denne samme animation fra at overlappe den nuværende
        if (currentState == newState)
            return;
        
        // Afspil den nye animation
        animator.Play(newState);
        
        // Sæt den nyafspillede animation til den nuværende animation
        currentState = newState;
    }
    
    
}
