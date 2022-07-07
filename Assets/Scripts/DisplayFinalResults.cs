using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayFinalResults : MonoBehaviour
{
    private ScoringSystem scoringSystemScript;

    public TextMeshProUGUI finalTime;
    public TextMeshProUGUI parTime;
    public TextMeshProUGUI killsAmount;

    public TextMeshProUGUI finalGrade;

    public TextMeshProUGUI pressToPlayAgain;

    public TextMeshProUGUI nextLevel;
    // Start is called before the first frame update
    void Awake()
    {
        scoringSystemScript = GetComponent<ScoringSystem>();
        finalTime.text = parTime.text = killsAmount.text = finalGrade.text = pressToPlayAgain.text = nextLevel.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowResults()
    {
        StartCoroutine(DisplayResults());
    }

    IEnumerator DisplayResults()
    {
        yield return new WaitForSecondsRealtime(2f);
        finalTime.text = "TIME: ";
        yield return new WaitForSecondsRealtime(1f);
        finalTime.text += scoringSystemScript.currentTime;
        yield return new WaitForSecondsRealtime(1f);
        parTime.text = "PAR TIME: " + scoringSystemScript.parTime;
        
        yield return new WaitForSecondsRealtime(1f);
        killsAmount.text = "KILLS + TARGETS: ";
        yield return new WaitForSecondsRealtime(1f);
        killsAmount.text += (scoringSystemScript.targetsCount - scoringSystemScript.aliveEnemies) + "/" + scoringSystemScript.targetsCount;
        
        yield return new WaitForSecondsRealtime(1f);
        finalGrade.text = "GRADE ";

        yield return new WaitForSecondsRealtime(.5f);
        finalGrade.text += ".";
        yield return new WaitForSecondsRealtime(.5f);
        finalGrade.text += ".";
        yield return new WaitForSecondsRealtime(.5f);
        finalGrade.text += ". ";
        yield return new WaitForSecondsRealtime(1.5f);
        finalGrade.text += scoringSystemScript.finalGradeLetter;
        
        yield return new WaitForSecondsRealtime(2f);
        pressToPlayAgain.text = "Press 'L' to try again";
        nextLevel.text = "Press 'P' for next level";
    }
}

