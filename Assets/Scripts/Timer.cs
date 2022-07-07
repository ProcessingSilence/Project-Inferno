using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool timerBegins;

    public float currentTime;
    public float lastTime;

    public TextMeshProUGUI timerText;

    public GameObject goal;

    public bool dontStart;
    
    // Update is called once per frame
    void Update()
    {
        if (dontStart == false)
        {
            if (((int) Input.GetAxisRaw("Horizontal") != 0 || (int) Input.GetAxisRaw("Vertical") != 0 ||
                 (Input.GetButton("Fire1")) && GlobalVars.menuObj.activeSelf == false)  && timerBegins == false)
            {
                timerBegins = true;
                lastTime = Time.time;
            }

            if (timerBegins && GlobalVars.levelComplete == false)
            {
                currentTime = Time.time - lastTime;
            }
            timerText.text = currentTime.ToString("F2");
            if (GlobalVars.levelComplete)
            {
                timerText.enabled = false;
            }
        }
    }
}
