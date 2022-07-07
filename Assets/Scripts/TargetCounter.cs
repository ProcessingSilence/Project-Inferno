/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetCounter : MonoBehaviour
{
    //public static int targetCount;

    //private TextMeshProUGUI targetText;

    //public GameObject winText;
    //public GameObject makeUsCryText;

    public GameObject lostText;

    public PlayerState playerState;
    // Start is called before the first frame update
    void Awake()
    {
        //GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        //targetCount = targets.Length;
        //targetText = GetComponent<TextMeshProUGUI>();
        //winText.SetActive(false);
        lostText.SetActive(false);
        //makeUsCryText.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (playerState.currentState != PlayerState.PlayerStates.Dead)
        {
            if (targetCount > 0)
                targetText.text = "Targets Left: " + targetCount;

            if (targetCount <= 0)
            {
                targetText.text = "";
                if (winText.activeSelf == false)
                {
                    winText.SetActive(true);
                    StartCoroutine(WaitBeforeTextShows());
                }
            }
        }

        #1#

        if (playerState.gameObject.activeSelf)
        {
            if (playerState.currentState == PlayerState.PlayerStates.Dead && lostText.activeSelf == false)
            {
                lostText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /*
    IEnumerator WaitBeforeTextShows()
    {
        yield return new WaitForSecondsRealtime(5);
        makeUsCryText.SetActive(true);
    }
    #1#
}

*/
