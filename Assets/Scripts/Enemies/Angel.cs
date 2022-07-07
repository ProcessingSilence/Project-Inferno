using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Angel : PathfindingEnemy
{
    private Coroutine firing;

    public Transform firePos;
    public Transform[] trueAngelFirePos;

    public AudioClip fireSound;

    public float fireDistance;

    public float rapidFireRate;
    public float secondsBeforeFireAgain;


    private PlayerState myState;

    private bool beginDeathProcess;

    private Collider collider;
    private Rigidbody rb;

    public GameObject destructionModel;
    private GameObject deathParticles;

    protected override void Awake()
    {
        base.Awake();

        myState = GetComponent<PlayerState>();
        deathParticles = myState.specialDeathParticle;

        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (myState.currentState != PlayerState.PlayerStates.Dead)
        {
            if (aiState != AiStateEnum.See)
            {
                base.Update();
            }
            switch (aiState)
            {
                case AiStateEnum.See:
                {
                    if (GlobalVars.mainPlayerState.currentState != PlayerState.PlayerStates.Dead)
                    {
                        navMeshAgent.SetDestination(GlobalVars.mainPlayer.transform.position);


                        if (Vector3.Distance(transform.position, GlobalVars.mainPlayer.transform.position) < fireDistance)
                        {
                            if (firing == null)
                            {
                                firing = StartCoroutine(FireAtPlayer());
                            }
                        }
                    }
                }
                    break;
            }
        }

        if (myState.currentState == PlayerState.PlayerStates.Dead && beginDeathProcess == false)
        {
            beginDeathProcess = true;
            collider.enabled = false;
            rb.useGravity = false;
            foreach (Transform child in destructionModel.transform)
            {
                var childRb = child.gameObject.AddComponent<Rigidbody>();

                child.gameObject.AddComponent<BoxCollider>();
                //tempCollider.size = child.gameObject.GetComponent<Renderer>().bounds;
                

                childRb.AddForce(new Vector3(-128 +GlobalVars.RandNumTable()* 2, -128 +GlobalVars.RandNumTable()* 2f,- 128 + GlobalVars.RandNumTable()) * 2f);
                childRb.AddTorque(new Vector3(-64 +GlobalVars.RandNumTable(), -64 +GlobalVars.RandNumTable(),- 64 + GlobalVars.RandNumTable()) * 10f);
                childRb.mass = 2;
                child.tag = "Land";
            }

            foreach (Transform child in destructionModel.transform)
            {
                child.parent = null;
                //child.transform.position = transform.position;
            }

            GlobalVars.PlaySoundObj(transform.position, deathSound[0], 1, true);
            if (deathParticles)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity);
            }

            gameObject.tag = "Untagged";
        }
    }

    IEnumerator FireAtPlayer()
    {
        if (EnemySight())
        {
            for (int i = 0; i < 3; i++)
            {
                if (myState.currentState != PlayerState.PlayerStates.Dead)
                {
                    Instantiate(enemyProj, firePos.position,
                        Quaternion.LookRotation(GlobalVars.mainPlayer.transform.position));
                    GlobalVars.PlaySoundObj(transform.position, fireSound, 0.8f, false);
                    if (trueAngelFirePos.Length > 0)
                    {
                        for (int j = 0; j < trueAngelFirePos.Length; j++)
                        {
                            Instantiate(enemyProj, trueAngelFirePos[j].position,
                                Quaternion.LookRotation(GlobalVars.mainPlayer.transform.position));
                        }
                    }

                    yield return new WaitForSecondsRealtime(rapidFireRate);
                }
                else
                {
                    i = 99;
                }
            }
        }

        if (myState.currentState != PlayerState.PlayerStates.Dead)
        {
            if (trueAngelFirePos.Length <= 0)
                yield return new WaitForSecondsRealtime(secondsBeforeFireAgain  + GlobalVars.RandNumTable()/500f);
            else
            {
                yield return new WaitForSecondsRealtime(secondsBeforeFireAgain);
            }
        }


        firing = null;
    }

}
 