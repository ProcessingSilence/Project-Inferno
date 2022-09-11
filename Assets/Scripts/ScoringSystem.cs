using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public Timer timerScript;
    public bool calculateScore;

    public float parTime;
    private float timeLimit, maxTimeLimit;
    public float currentTime;

    public float timeScore;

    public float killScore;

    public float finalScore;

    public int targetsCount;
    public int aliveEnemies;

    [HideInInspector]public string[] grade = {"S","A", "B", "C", "D", "F"};

    public string finalGradeLetter;

    private DisplayFinalResults displayFinalResults;

    // Start is called before the first frame update
    void Awake()
    {
        targetsCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        displayFinalResults = GetComponent<DisplayFinalResults>();
        maxTimeLimit = parTime + ((parTime / 2) * 5);
        timeLimit = parTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.levelComplete == false)
        {
            // Keep subtracting points to timeScore and adding to time limit until it's done 5 times.
            if (timerScript.currentTime < maxTimeLimit)
            {
                if (timerScript.currentTime > timeLimit)
                {
                    timeLimit += parTime / 2f;
                    timeScore += 75f / 5f;
                }
            }
        }

        if (GlobalVars.levelComplete)
        {
            // Show the results, but still calculate the score as it is displaying in case an enemy dies after the player
            // gets to the goal.
            if (calculateScore == false)
            {
                calculateScore = true;
                displayFinalResults.ShowResults();
            }

            currentTime = Mathf.Round(timerScript.currentTime * 100f) / 100f;
            /*
            if (timerScript.currentTime <= parTime)
            {
                timeScore = 50;
            }
            else if (timerScript.currentTime <= parTime * 2)
            {
                timeScore = 40;
            }
            else if (timerScript.currentTime <= parTime * 3)
            {
                timeScore = 30;
            }
            else if (timerScript.currentTime <= parTime * 4)
            {
                timeScore = 20;
            }
            else if (timerScript.currentTime <= parTime * 5)
            {
                timeScore = 10;
            }
            else
            {
                timeScore = 0;
            }
            */
            // Debug.Log("ENEMIES ALIVE:  " + GameObject.FindGameObjectsWithTag("Enemy").Length);

            if (targetsCount > 0)
            {
                aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                killScore = (((float)aliveEnemies / targetsCount) * 75);
            }

            // Give full kill score if there's no enemies, also to prevent NaN error.
            if (targetsCount <= 0)
            {
                killScore = 0;
            }



            finalScore = 100 - (timeScore + killScore);

            // S
            if (finalScore >= 100)
            {
                finalGradeLetter = grade[0];
            }
            // A
            else if (finalScore >= 80)
            {
                finalGradeLetter = grade[1];
            }
            // B
            else if (finalScore >= 60)
            {
                finalGradeLetter = grade[2];
            }
            // C
            else if (finalScore >= 40)
            {
                finalGradeLetter = grade[3];
            }
            // D
            else if (finalScore >= 20)
            {
                finalGradeLetter = grade[4];
            }
            // F
            else
            {
                finalGradeLetter = grade[5];
            }

        }
    }
}
