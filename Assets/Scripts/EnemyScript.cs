using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public int startingPoint; 
    public Transform[] points;
    private int i;
    
    Animator animator;
    private string currentState;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
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
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(CompareTag("MovePoint")){
            if(transform.position.y<collision.transform.position.y)
                collision.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(CompareTag("MovePoint"))
            collision.transform.SetParent(null);
    }

    
    
    private void IdleMove()
    {
        // Et IF-Statement som tjekker om punktet er blevet nået. Hvis det er nået sættes det næste punkt
        // i arrayet som det næste mål. Hvis ikke der er flere punkter startes der forfra.
        if(Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        // Bevæger platformen mod en given position.
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

}
