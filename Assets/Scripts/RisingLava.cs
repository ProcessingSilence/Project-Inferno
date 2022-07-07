using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{
    public float secondsB4TargetReached;

    public float endpointYPos;
    private Vector3 endpoint;

    private bool isRising;

    private float moveSpeed;
    public Vector3 startPos;

    public float currentTime;

    public bool startMoving;

    public Timer timerScript;

    //private float t;
    //private float previousT;

    private Stages lavaStages; 
    private enum Stages
    {
        NotMoving,
        Rising,
        RisingAfterDestination
    }

    //private float velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        //endpointYPos *= 2;
        timerScript = GlobalVars.timer;
        var distance = Vector3.Distance(startPos, new Vector3(0, endpointYPos, 0));
        secondsB4TargetReached *= 2;
        endpointYPos += distance;
        transform.position = startPos;
        lavaStages = Stages.NotMoving;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerScript.currentTime > 0 && lavaStages != Stages.Rising)
        {
            startMoving = true;
        }

        if (startMoving)
        {
            startMoving = false;
            ResetAndStart();
        }

        if (lavaStages == Stages.Rising && currentTime < secondsB4TargetReached)
        {
            //previousT = t;
            //t = currentTime / secondsB4TargetReached;
            //t = t * t * (3f - 2f * t);
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endpoint, currentTime/ secondsB4TargetReached);
            //transform.position = Vector3.SmoothDamp(transform.position, endpoint, ref velocity, secondsB4TargetReached);
        }
        
        /*
        if (currentTime >= secondsB4TargetReached)
        {
            lavaStages = Stages.RisingAfterDestination;
           
        }

        if (lavaStages == Stages.RisingAfterDestination && currentTime < secondsB4TargetReached * 2)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(endpoint, new Vector3(transform.position.x, 1000 + endpointYPos, transform.position.z), currentTime - 1 / secondsB4TargetReached);
        }
        */

        /*
        else if (isRising && previousT > t)
        {
            isRising = false;
            transform.position = endpoint;
        }
        */
    }

    public void ResetAndStart()
    {
        lavaStages = Stages.Rising;
        //previousT = t = 0;
        endpoint = new Vector3(transform.position.x, endpointYPos, transform.position.z);
        currentTime = 0;
        transform.position = startPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerState>().health = -10000;
        }
    }
}
