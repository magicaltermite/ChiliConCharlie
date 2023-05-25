using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    
    public GameObject canvas;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canvas.SetActive(true);
            GetComponent<Dialogue>().enabled = enabled;
        }
    }
    
}
