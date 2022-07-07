using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject lostText;

    public PlayerState playerState;

    public string nextSceneName;

    public string loseSceneName;

    void Awake()
    {
        lostText.SetActive(false);

    }
    void Update()
    {
        if (playerState.gameObject.activeSelf)
        {
            if (playerState.currentState == PlayerState.PlayerStates.Dead && lostText.activeSelf == false)
            {
                lostText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (loseSceneName == "")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            
            if (loseSceneName != "")
            {
                SceneManager.LoadScene(loseSceneName);
            }


        }
        if (Input.GetKeyDown(KeyCode.P) && GlobalVars.levelComplete)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

}
