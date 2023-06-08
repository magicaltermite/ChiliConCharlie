using System;
using System.Collections;
using System.Collections.Generic;
using DialogueScript;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueTrigger : MonoBehaviour {
    
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject timeLine;
    public string[] endLines;

    private string lineToCheck;

  

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canvas.SetActive(true);
            GetComponent<Dialogue>().enabled = true;
        }
    }

    
    public void ActivateCanvas(bool activate) {
        Debug.Log("Activate canvas called " + activate);
        if (activate && !canvas.activeInHierarchy) {
            canvas.SetActive(true);
            GetComponent<Dialogue>().enabled = true;
            timeLine.GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        else if(!activate && canvas.activeInHierarchy) {
            canvas.SetActive(false);
            GetComponent<Dialogue>().enabled = false;
            timeLine.GetComponent<PlayableDirector>().playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

    }


    public bool GetLineToCheck(string lineFromDialogue) {
        foreach (string line in endLines) {
            if (string.Equals(line.Trim(), lineFromDialogue.Trim())) {
                return false;
            }
        }
        return true;
    }
    
    
    
    
}
