using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Example : MonoBehaviour
{
    // These are the Scene names. Make sure to set them in the Inspector window.
    public string myFirstScene, mySecondScene;

    private string nextButton = "Load Next Scene";
    private string nextScene;
    private static bool created = false;

    private Rect buttonRect;
    private int width, height;

    void Awake()
    {
        Debug.Log("Awake:" + SceneManager.GetActiveScene().name);

        // Ensure the script is not deleted while loading.
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Specify the items for each scene.
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        width = Screen.width;
        height = Screen.height;
        buttonRect = new Rect(width / 8, height / 3, 3 * width / 4, height / 3);
    }

    void OnGUI()
    {
        // Return the current Active Scene in order to get the current Scene name.
        Scene scene = SceneManager.GetActiveScene();

        // Check if the name of the current Active Scene is your first Scene.
        if (scene.name == myFirstScene)
        {
            nextButton = "Load Next Scene";
            nextScene = mySecondScene;
        }
        else
        {
            nextButton = "Load Previous Scene";
            nextScene = myFirstScene;
        }

        // Display the button used to swap scenes.
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("button"));
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.fontSize = 12 * (width / 200);

        if (GUI.Button(buttonRect, nextButton, buttonStyle))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}