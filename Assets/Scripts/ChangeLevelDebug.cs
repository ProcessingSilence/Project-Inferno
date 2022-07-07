using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelDebug : MonoBehaviour
{
    private string[] scenes = new[] {"TUTORIAL", "LEVEL ONE", "LEVEL TWO", "LEVEL THREE", "ENDING"};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(scenes[0]);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(scenes[1]);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(scenes[2]);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene(scenes[3]);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(scenes[3]);
        }
    }
}
