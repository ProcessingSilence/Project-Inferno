using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVars : MonoBehaviour
{
    public static GameObject soundPlayer;

    //public TargetCounter targetCounterScript;
    //public static TargetCounter targetCounter;

    public TextMeshProUGUI pressEToTakeWeaponSlot;
    public static TextMeshProUGUI pressEToTakeWeapon;

    public static Timer timer;
    public Timer timerScript;

    public GameObject cameraObj;
    
    public Transform levelViewCamera;

    public static bool levelComplete;
    public bool recievedLevelCompleteSignal;

    public GameObject crosshairObj;
    public static GameObject crosshair;

    public Slider mouseSpeedSliderComp;
    public static Slider mouseSpeedSlider;
    
    public GameObject playerObj;
    public Transform playerObjTransform;
    public static Rigidbody playerRb;
    public static GameObject mainPlayer;
    public static Transform playerTransform;
    public static PlayerState mainPlayerState;
    public static Collider playerCollider;
    public static PlayerController playerController;

    public TextMeshProUGUI ammoText;
    public static TextMeshProUGUI sAmmoText;

    public static Slider ammoSlider;
    public Slider ammoBar;

    public GameObject menuScreenObj;
    public static GameObject menuObj;

    public static GameObject canvasObj;

    public static MenuOpen menuScript;

    public int randomSeed;
    // What this is: https://youtu.be/pq3x1Jy8pYM?t=33
    // The game uses this in case we want to add a replay system since Random.value won't return the same values when played again.
    public static int[] tableBasedRng = new [] {205, 76, 160, 229, 151, 8, 0, 201, 120, 149, 63, 46, 241, 148, 90, 243, 180, 
        187, 226, 65, 175, 15, 207, 4, 191, 246, 158, 21, 233, 147, 70, 190, 214, 178, 19, 7, 40, 86, 109, 48, 195, 60, 
        55, 208, 36, 30, 221, 43, 237, 254, 81, 38, 80, 152, 247, 18, 50, 57, 14, 216, 212, 193, 250, 225, 186, 185, 197, 
        206, 29, 173, 159, 166, 72, 114, 85, 88, 3, 23, 12, 203, 117, 6, 47, 42, 162, 123, 66, 200, 236, 184, 106, 215, 
        16, 105, 59, 251, 135, 25, 170, 53, 256, 131, 121, 210, 249, 182, 252, 115, 150, 177, 92, 5, 217, 132, 218, 196, 176, 
        110, 181, 45, 87, 93, 172, 108, 142, 183, 77, 154, 164, 118, 127, 107, 239, 155, 223, 167, 174, 102, 44, 27, 211, 
        100, 122, 26, 91, 28, 141, 41, 244, 33, 101, 112, 52, 240, 10, 67, 143, 11, 179, 64, 168, 219, 169, 139, 2, 35, 
        232, 130, 113, 51, 69, 146, 17, 161, 95, 188, 242, 124, 22, 20, 136, 140, 94, 34, 73, 78, 171, 157, 13, 111, 97, 
        192, 119, 84, 231, 227, 96, 49, 31, 125, 189, 71, 74, 99, 220, 37, 224, 68, 194, 1, 98, 61, 144, 222, 202, 163, 
        199, 238, 58, 24, 82, 75, 230, 234, 56, 204, 255, 133, 138, 129, 62, 245, 79, 235, 39, 126, 9, 209, 198, 89, 253, 
        83, 54, 104, 32, 128, 248, 116, 134, 213, 165, 156, 153, 228, 145, 103, 137};

    public static int environmentRngI;
    public int playerRngI;

    public float dieAtYPos;
    public static float deathYPos;

    public static GlobalVars globalVarsScript;

    public Camera mainCamera;

    public static Transform cameraPos;
    void Awake()
    {
        globalVarsScript = this;
        canvasObj = GameObject.Find("Canvas");
        mainPlayer = playerObj;
        playerTransform = playerObjTransform;
        deathYPos = dieAtYPos;
        sAmmoText = ammoText;
        crosshair = crosshairObj;
        pressEToTakeWeapon = pressEToTakeWeaponSlot;
        //targetCounter = targetCounterScript;
        soundPlayer = Resources.Load<GameObject>("SoundPlayer");
        
        if (timerScript)
            timer = timerScript;
        
        mainPlayer = playerObj;
        mainPlayerState = mainPlayer.GetComponent<PlayerState>();
        playerCollider = mainPlayer.GetComponent<Collider>();
        playerRb = mainPlayer.GetComponent<Rigidbody>();
        playerController = mainPlayer.GetComponent<PlayerController>();
        mouseSpeedSlider = mouseSpeedSliderComp;   
        ammoSlider = ammoBar;
        menuObj = menuScreenObj;
        environmentRngI = randomSeed = Random.Range(0, 255);
        cameraPos = mainCamera.transform;

    }

    void Update()
    {
        if (levelComplete && recievedLevelCompleteSignal == false)
        {
            recievedLevelCompleteSignal = true;
            LevelComplete();
        }
    }

    public static void PlaySoundObj(Vector3 position, AudioClip clip, float volume = 1, bool hasLimitedRange = false,
        float maxDist = 80, float pitch = 1)
    {
        var currentSoundPlayer = Instantiate(soundPlayer, position, Quaternion.identity)
            .GetComponent<AudioSource>();
        if (hasLimitedRange)
        {
            currentSoundPlayer.rolloffMode = AudioRolloffMode.Linear;
            currentSoundPlayer.maxDistance = maxDist;
        }

        currentSoundPlayer.volume = volume;
        currentSoundPlayer.clip = clip;
        currentSoundPlayer.pitch = pitch;
    }

    // Sets a bool for now, but keeping as a function in case more stuff needs to be done.
    public void LevelComplete()
    {
        cameraObj.GetComponent<CinemachineBrain>().enabled = false;
        cameraObj.transform.position = levelViewCamera.position;
        cameraObj.transform.rotation = levelViewCamera.rotation;
    }

    public static int RandNumTable()
    {
        if (environmentRngI > 256)
        {
            environmentRngI = 0;
        }

        environmentRngI++;
        return tableBasedRng[environmentRngI-1];
    }

    public static bool RandTrueOrFalse()
    {
        return RandNumTable() % 2 == 0;
    }

    public static void PlayerRandNumTable()
    {
        
    }

}
