using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimation : MonoBehaviour
{


    public PlayerState playerStateScript;

    public Transform main;
    public Transform myCamera;
    private BoxCollider boxCollider;

    private Rigidbody rb;

    public GameObject playerModel;
    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = playerModel.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        
        rb = playerModel.GetComponent<Rigidbody>();
        rb.useGravity = false;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GlobalVars.levelComplete)
        {
            gameObject.SetActive(false);
        }
        
        if (main.gameObject.activeSelf)
        {
            switch (playerStateScript.currentState)
            {
                case PlayerState.PlayerStates.Dead:
                {
                    boxCollider.enabled = true;

                    if (rb.useGravity == false)
                    {
                        rb.useGravity = true;
                        rb.AddTorque(150,100,150);
                    }

                    break;
                }
                case PlayerState.PlayerStates.Shooting:
                {
                    transform.eulerAngles = new Vector3(0, myCamera.eulerAngles.y, 0);
                    break;
                }

                case PlayerState.PlayerStates.Neutral:
                {
                    if (playerStateScript.currentState != PlayerState.PlayerStates.Killed || playerStateScript.currentState != PlayerState.PlayerStates.Dead)
                    {
                        transform.eulerAngles = new Vector3(0, myCamera.eulerAngles.y, 0);
                    }
                    break;
                }
            }
        
            if (playerStateScript.currentState != PlayerState.PlayerStates.Dead)
            {
                transform.position = main.position;
            }
        }
    }
}
