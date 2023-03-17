using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Variables
    public float speed;
    public int startingPoint; // Det først punkt i arrayet
    public Transform[] points; // Alle de punkter som platformen skal bevæge sig igennem

    // Iterator til at gå igennem hvert eneste punkt i arrayet
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        // Platformen transform.position sætte til det første punkt i arrayet
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
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

    // Søger for at spilleren også rykker sig med platformen når den bevæger sig.
    // Dog kun så længe at spillerens y-værdi er højere end platformen - altså at han står på platformen.
    // Uden dette tjek kunne en platform ramme siden af spilleren og stadig trække i spilleren.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.position.y<collision.transform.position.y)
            collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
