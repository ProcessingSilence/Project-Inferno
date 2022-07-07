using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpscare : MonoBehaviour
{
    public AudioClip angel;

    public GameObject model;

    public GameObject soundPlayer;

    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        model.SetActive(false);
        StartCoroutine(JumpscareProcess());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator JumpscareProcess()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        model.SetActive(true);
        PlaySoundObj(transform.position, angel,1,false,80f, 0.6f);
        PlaySoundObj(transform.position, angel,1,false,80f, 0.6f);
        PlaySoundObj(transform.position, angel,1,false,80f, 0.6f);
        PlaySoundObj(transform.position, angel,1,false,80f, 0.6f);
        yield return new WaitForSecondsRealtime(0.5f);
        Application.Quit();
        Debug.Log("QUIT");
    }
    
    public void PlaySoundObj(Vector3 position, AudioClip clip, float volume = 1, bool hasLimitedRange = false,
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
}
