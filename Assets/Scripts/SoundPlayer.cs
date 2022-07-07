using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource aS;

    private bool hasPlayed;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aS.clip != null && hasPlayed == false)
        {
            hasPlayed = true;
            aS.Play();
        }
        else if (aS.isPlaying == false && hasPlayed == true)
        {
            Destroy(gameObject);
        }
    }
}
