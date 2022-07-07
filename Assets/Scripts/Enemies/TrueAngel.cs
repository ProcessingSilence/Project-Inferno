using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TrueAngel : PathfindingEnemy
{
    private Coroutine firing;

    public Transform firePos;
    public Transform[] trueAngelFirePos;

    public AudioClip fireSound, teleportSound;

    public float fireDistance;

    public float rapidFireRate;
    public float secondsBeforeFireAgain;

    public BossAttackTypes bossAttackTypes;

    public bool doingFinalAttack;

    public Coroutine activeAttackCoroutine;

    private Collider collider;

    public AttackObjs attackObjs;

    public int[] attackNumOrder;

    public int attackArrayIteration;

    private PlayerState myCurrentState;

    private float currentSpeed;

    private Rigidbody rb;

    private float groundedPos;

    public GameObject wings;

    private bool beginDeathProcess;

    public Transform destructionModel;

    public GameObject goalObj;

    public Vector3 finalAttackPos;

    public bool allowedToFire;
    public enum BossAttackTypes
    {
        LightRays
        
    }

    public enum PhaseTwoAttacks
    {
        Lightstorm,
        Laser,
        Summon,
        FollowerMinions,
        LightRays
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        alwaysChaseOnSee = false;
        collider = GetComponent<Collider>();
        groundedPos = transform.position.y - collider.bounds.size.y / 2;
        myCurrentState = GetComponent<PlayerState>();

        
        int bossAttacksLength = Enum.GetValues(typeof(BossAttackTypes)).Length;
        attackNumOrder = new int[bossAttacksLength];
        for (int i = 0; i < bossAttacksLength; i++)
        {
            attackNumOrder[i] = i;
        }

        FisherYatesShuffle(attackNumOrder);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(AttackDelay());

    }

    private void FixedUpdate()
    {
        // The boss area for this enemy is a giant 10000x10000 plane, so baking a navmesh would be expensive as fuck. The enemy just moves toward the player.
        var playerPos = new Vector3(GlobalVars.playerTransform.position.x, groundedPos, GlobalVars.playerTransform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPos, currentSpeed * Time.fixedDeltaTime);
    }

    //navMeshAgent.SetDestination(GlobalVars.mainPlayer.transform.position);
    
    // Update is called once per frame
    protected override void Update()
    {
        if (wings.activeSelf)
        {
            Vector3 lookAtPlayer = new Vector3(GlobalVars.playerTransform.position.x, transform.position.y, GlobalVars.playerTransform.position.z);
            wings.transform.LookAt(lookAtPlayer);
            wings.transform.position = transform.position;
        }

        //navMeshAgent.SetDestination(GlobalVars.playerTransform.position);
        if (aiState != AiStateEnum.See)
        {
            base.Update();
            //Vector3 lookAtPlayer = new Vector3(GlobalVars.playerTransform.position.x, collider.bounds.size.y/2, GlobalVars.playerTransform.position.z);
            //transform.LookAt(lookAtPlayer);
        }
        if (aiState == AiStateEnum.See && myCurrentState.health > 0 && allowedToFire)
        {
            if (myCurrentState.health <= 500 && doingFinalAttack == false)
            {
                doingFinalAttack = true;
                activeAttackCoroutine = null;
                TeleportInPlayerView(60, Vector3.zero);
                finalAttackPos = transform.position;
                
                // BE AFRAID.
                for (int i = 0; i < 4; i++)
                {
                    GlobalVars.PlaySoundObj(transform.position, spotSound[0], 1, false, 80, 0.6f);
                }

                wings.SetActive(true);
                currentSpeed = 0;
                StartCoroutine(AttackDelay());
            }

            if (activeAttackCoroutine == null)
            {               
                if (doingFinalAttack == false)
                {
                    currentSpeed = 0;
                    SetNextAttack();                    
                    
                    switch (bossAttackTypes)
                    {
                        case BossAttackTypes.LightRays:
                        {
                            activeAttackCoroutine = StartCoroutine(LightRays());
                            break;
                        }
                    }
                }

                if (doingFinalAttack)
                {
                    activeAttackCoroutine = StartCoroutine(FuckYouUp());
                }
            }

            if (doingFinalAttack)
            {
                transform.position = finalAttackPos;
            }
        }

        if (myCurrentState.health <= 0 && beginDeathProcess == false)
        {
            collider.enabled = false;
            beginDeathProcess = true;
            foreach (Transform child in destructionModel.transform)
            {
                var childRb = child.gameObject.AddComponent<Rigidbody>();
                childRb.AddForce(new Vector3(-128 +GlobalVars.RandNumTable(), -128 +GlobalVars.RandNumTable(),- 128 + GlobalVars.RandNumTable()) * 10f);
                childRb.AddTorque(new Vector3(-64 +GlobalVars.RandNumTable(), -64 +GlobalVars.RandNumTable(),- 64 + GlobalVars.RandNumTable()) * 10f);
            }

            foreach (Transform child in wings.transform)
            {
                var childRb = child.gameObject.AddComponent<Rigidbody>();
                childRb.AddForce(new Vector3(-128 +GlobalVars.RandNumTable(), -128 +GlobalVars.RandNumTable(),- 128 + GlobalVars.RandNumTable()) * 10f);
                childRb.AddTorque(new Vector3(-64 +GlobalVars.RandNumTable(), -64 +GlobalVars.RandNumTable(),- 64 + GlobalVars.RandNumTable()) * 10f);
            }
            wings.transform.parent = null;
            destructionModel.parent = null;
            GlobalVars.PlaySoundObj(transform.position, spotSound[0], 1, false, 80, 0.3f);
            Transform goalPos = Instantiate(goalObj, transform.position, Quaternion.identity).transform;
            goalPos.position = new Vector3(goalPos.position.x, 0, goalPos.position.z);
            Destroy(transform.parent.gameObject);
            //goalObj.transform.position = transform.position;
        }
    }
    

    void SetNextAttack()
    {
        attackArrayIteration++;
        // Reset iteration and shuffle attack order
                
        if (attackArrayIteration > attackNumOrder.Length-1)
        {
            attackArrayIteration = 0;
            FisherYatesShuffle(attackNumOrder);
        }

        bossAttackTypes = (BossAttackTypes) attackNumOrder[attackArrayIteration];
    }

    void FisherYatesShuffle(int[] a)
    {
        // Loops through array
        for (int i = a.Length-1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = UnityEngine.Random.Range(0,i);
			
            // Save the value of the current i, otherwise it'll overright when we swap the values
            int temp = a[i];
			
            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }
		
        /*
        // Print
        for (int i = 0; i < a.Length; i++)
        {
            Debug.Log (a[i]);
        }
        */
    }

    private IEnumerator FuckYouUp()
    {
        if (EnemySight())
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(attackObjs.shortLaser, firePos.position,
                    Quaternion.LookRotation(GlobalVars.mainPlayer.transform.position));
                GlobalVars.PlaySoundObj(transform.position, fireSound, 0.8f, false);
                if (trueAngelFirePos.Length > 0)
                {
                    attackObjs.shortLaser.GetComponent<TrueAngelProjectile>().firerPos = transform;
                    for (int j = 0; j < trueAngelFirePos.Length; j++)
                    {
                        var projectile = Instantiate(attackObjs.shortLaser, trueAngelFirePos[j].position,
                            Quaternion.LookRotation(GlobalVars.mainPlayer.transform.position), transform);
                    }
                }

                yield return new WaitForSecondsRealtime(rapidFireRate);
            }
        }

        
        if (trueAngelFirePos.Length <= 0)
            yield return new WaitForSecondsRealtime(secondsBeforeFireAgain  + GlobalVars.RandNumTable()/500f);
        else
        {
            yield return new WaitForSecondsRealtime(secondsBeforeFireAgain);
        }
        firing = null;
        activeAttackCoroutine = null;
    }

    IEnumerator LightRays()
    {
        if (doingFinalAttack == false)
        {
            currentSpeed = 3;
            bool onlyAttackedOnce = false;
            for (int i = 0; i < UnityEngine.Random.Range(4, 6); i++)
            {
                float halfPlayerCollider = GlobalVars.playerCollider.bounds.size.y / 2;
                //float belowOrAbovePlayer = GlobalVars.RandTrueOrFalse() ? -halfPlayerCollider+1 : halfPlayerCollider + 6;
    
                // Teleport to player to prevent the player from cheesing the attack with the method of getting far enough so the projectiles don't reach.
                if ((Vector3.Distance(transform.position, GlobalVars.playerTransform.position) > 60 || UnityEngine.Random.Range(0,12) > 10) && beginDeathProcess == false)
                {
                    var randOffsetVal = UnityEngine.Random.Range(-40, 40f);
                    GlobalVars.PlaySoundObj(transform.position, teleportSound, 1, false, 80, .8f);
                    TeleportInPlayerView(20f, GlobalVars.cameraPos.right * randOffsetVal);
                }
    
                for (int j = 0; j < 3; j++)
                {
                    if (GlobalVars.RandTrueOrFalse() || j == 0)
                    {
                        yield return new WaitForSecondsRealtime(0.3f - (j * 0.1f));
                        
                        bool randomVertical = GlobalVars.RandTrueOrFalse();
                        Vector3 dir = (transform.position - GlobalVars.playerTransform.position).normalized;
                        dir = new Vector3(dir.x, 0, dir.z);
                        var spawnPos = new Vector3(transform.position.x + GlobalVars.playerRb.velocity.x, -halfPlayerCollider+1f, transform.position.z+ GlobalVars.playerRb.velocity.z) + (dir * 75f);
                        var lightRay = Instantiate(attackObjs.lightRay, spawnPos, Quaternion.identity);
                        Vector3 playerPos = GlobalVars.mainPlayer.transform.position;
                        Vector3 lookPos = new Vector3(playerPos.x, lightRay.transform.position.y, playerPos.z);
                        //transform.LookAt(lookPos);
                        lightRay.transform.LookAt(lookPos);
    
                        if (randomVertical)
                        {
                            lightRay.transform.position += lightRay.transform.right * (UnityEngine.Random.Range(0,50) * ( GlobalVars.RandTrueOrFalse() ? 1 : -1));
                            lightRay.transform.rotation *= Quaternion.Euler(0,0,90);
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            onlyAttackedOnce = true;
                        }
    
                        j = 5;
                    }
                }
    
                if (onlyAttackedOnce)
                {
                    yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.3f, 0.6f));
                }
    
                if (onlyAttackedOnce == false)
                {
                    yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.5f, 0.8f));
                }
            }
        }



        activeAttackCoroutine = null;
    }
    void TeleportInPlayerView(float distance, Vector3 offset)
    {
        Vector3 teleportTo = new Vector3(GlobalVars.playerTransform.position.x, transform.position.y, GlobalVars.playerTransform.position.z);
        Vector3 cameraForward = GlobalVars.cameraPos.transform.forward;
        cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        transform.position = teleportTo + (cameraForward * distance) + offset;
    }

    IEnumerator AttackDelay()
    {
        allowedToFire = false;
        yield return new WaitForSecondsRealtime(4f);
        allowedToFire = true;
    }


    [System.Serializable]
    public class AttackObjs
    {
        public GameObject shortLaser;
        public GameObject lesserAngel;
        public GameObject lightRay;
    }
}
