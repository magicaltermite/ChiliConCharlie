using System;
using System.Collections.Generic;
using UnityEngine;

// Så snart dette script bliver sat på et gameobject vil der automatisk også blive sat en boxcollider på
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyScript : MonoBehaviour
{
    // Reference til waypoints:
    public List<Transform> points;

    // Int værdien for det næste punkt i listen:
    public int nextID;
    
    // Værdien som tilføjes til ID for at ændre den:
    private int idChangeValue = 1;
    
    // Speed of movement or flying

    public float speed = 2f;

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        // Sæt BoxCollider til trigger
        GetComponent<BoxCollider2D>().isTrigger = true;

        // Create root GameObject
        GameObject root = new GameObject(name + "_root");

        // Reset position of Root to enemy object
        root.transform.position = transform.position;

        // Set enemy object as child of root
        transform.SetParent(root.transform);

        // Create Waypoints object
        GameObject waypoints = new GameObject("Waypoints");

        // Reset waypoint position to root
        // Make Waypoint object child of root
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        

        // Create two points (GameObjects) and reset their position to waypoints 
        // Make the points children of waypoint
        GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(waypoints.transform);p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(waypoints.transform);p2.transform.position = root.transform.position;

        // Init points list then add the points to it:
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);


    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        // Get the next Point transform
        Transform goalPoint = points[nextID];
        
        // Flip the enemy transform to look into the points direction


        // Move the enemy towards the goal point
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);

        // Check the distance between enemy and goal point to trigger next point
        if (Vector2.Distance(transform.position, goalPoint.position)<0.3f)
        {
            // Check if we are at the end of the line - (Make the change -1)
            if (nextID == points.Count - 1)
                idChangeValue = -1;

            // Check if wa are at the start of the line - (Make the change +1)
            if (nextID == 0)
                idChangeValue = 1;

            // Apply the change on the nextID
            nextID += idChangeValue;
            
            if (goalPoint.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
