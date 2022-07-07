using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeed : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;

    public Slider slider;

    public CinemachineFreeLook myCameraStats;

    public Vector2 currentMouseSetting;
    
    private Vector2 mouseIteration = new Vector2(1f, 150f);
    // Start is called before the first frame update
    void Awake()
    {
        currentMouseSetting = new Vector2(myCameraStats.m_XAxis.m_MaxSpeed,myCameraStats.m_YAxis.m_MaxSpeed);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!myCameraStats)
        {
            myCameraStats= GameObject.Find("TPS Camera").GetComponent<CinemachineFreeLook>();
        }

        var sliderVal = slider.value;
        sliderVal = Mathf.Round(sliderVal * 100f) / 100f;
        myCameraStats.m_YAxis.m_MaxSpeed = sliderVal;
        myCameraStats.m_XAxis.m_MaxSpeed = sliderVal * 150;
        textDisplay.text = "Mouse speed: " + sliderVal;
    }
}
