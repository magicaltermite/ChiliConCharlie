using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{
    public float changeTimer;
    public string sceneName;
    // Update is called once per frame
    private void Update()
    {
        changeTimer -= Time.deltaTime;
        if(changeTimer <=0)
            SceneManager.LoadScene(sceneName);
    }
}
