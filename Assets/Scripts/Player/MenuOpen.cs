using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Cursor = UnityEngine.Cursor;

public class MenuOpen : MonoBehaviour
{
    public bool menuOffOn;
    public GameObject menuScreen;
    public Slider mouseSpeedSlider;
    private Vector2 previousMouseSpeeds;
    public float sliderVal;

    private void OnEnable()
    {
        transform.parent = null;
        GameObject[] otherObjs = GameObject.FindGameObjectsWithTag("MenuManager");

        for (int i = 0; i < otherObjs.Length; i++)
        {
            if (otherObjs[i] != gameObject)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
        mouseSpeedSlider = GlobalVars.mouseSpeedSlider;
        gameObject.AddComponent<GetAspectRatio>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sliderVal = GlobalVars.mouseSpeedSlider.value;
        menuOffOn = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseSpeedSlider)
        {
            mouseSpeedSlider = GlobalVars.mouseSpeedSlider;
            mouseSpeedSlider.value = sliderVal;
        }

        if (mouseSpeedSlider)
        {
            sliderVal = mouseSpeedSlider.value;
        }

        if (!menuScreen)
        {
            menuScreen = GlobalVars.menuObj;
            menuOffOn = false;
            menuScreen.SetActive(false);
            GlobalVars.menuScript = this;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuOffOn = !menuOffOn;
            menuScreen.SetActive(menuOffOn);
        }

        if (menuOffOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

        if (menuOffOn == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    private void OnMouseExit()
    {
        menuOffOn = true;
    }
}
