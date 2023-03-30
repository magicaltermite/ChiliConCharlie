using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100; // The amount of health the unit has



    // Start is called before the first frame update
    void Start()
    {
        
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

        if (health <= 0) {
            Kill();
        }
    }


    private void Kill() {
        this.gameObject.SetActive(false);
    }
}
