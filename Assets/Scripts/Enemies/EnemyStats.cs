using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStats : MonoBehaviour
{
    public Transform target;
    
    // Gives the shooting rate a delay before being allowed to fire.
    public float reactionTime;

    protected float newReactionDelay;
    protected float reactionDelay;

    protected bool allowedToFire = true;
    private bool playerTempOutOfSight;
    
    // How far up close the enemy will go (set to 0 to have enemy try to touch player).
    public float minRangeFromPlayer;
    
    public float fireDamage;
    public float firePush;
    
    protected Coroutine firingProcess;
    
    public float meleeDamage;
    public float meleePush;

    public float timeBeforeFire;
    [SerializeField]protected float currentTimeBeforeFire;

    public AiStateEnum aiState;
    
    // Makes the enemy have no sight limit.
    public bool threeSixtyDegreeSight;

    private PlayerState entityState;

    [SerializeField] protected Transform mainPlayer;

    // alt target for enemy infighting
    [HideInInspector]private Transform altTarget;

    //public float distance;
    public bool unlimitedSight;

    public AudioClip[] spotSound;
    public AudioClip[] deathSound;
    public float soundPitch;
    
    public GameObject enemyProj;

    public bool noLimitAlertSound;

    public Transform specialLookPosition;

    
    public enum AiStateEnum
    {
        Neutral,
        Patrol,
        See,
        Reacting,
        Hurt,
        Firing,
        DoneFiring,
        Melee,
        Dead,
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        entityState = GetComponent<PlayerState>();
        if (deathSound.Length > 0)
        {
            var chosenDeathSound = deathSound[Random.Range(0, deathSound.Length)];
            entityState.targetSound = chosenDeathSound;
        }

        if (soundPitch <= 0.001f)
        {
            soundPitch = 1;
        }

        entityState.soundPitch = soundPitch;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (aiState == AiStateEnum.Neutral || aiState == AiStateEnum.Patrol)
        {
            IsEnemySpotted();
        }

        if (aiState == AiStateEnum.See)
        {
            if (!EnemySight() && playerTempOutOfSight == false)
            {
                playerTempOutOfSight = true;
                //Debug.Log("I know you're hiding...");
                
                reactionDelay = currentTimeBeforeFire - (Time.time);
                //Debug.Log("reactionDelay: " + reactionDelay);
                currentTimeBeforeFire = Time.time + reactionDelay;
            }

            if (EnemySight() && playerTempOutOfSight)
            {
                //Debug.Log("Can't hide from me!");
                playerTempOutOfSight = false;
            }
            if (Time.time > currentTimeBeforeFire && allowedToFire && EnemySight())
            {
                allowedToFire = false;
                //Debug.Log("Time.time: " + Time.time + " > " + currentTimeBeforeFire);
                //Debug.Log("Ready to fire");
                BeginAttackProcess();
            }
        }
    }

    protected bool EnemySight()
    {
        //Debug.Log("Enemy Sight working");
        //distance = Vector3.Distance(transform.position, mainPlayer.position);
        if (Vector3.Distance(transform.position, mainPlayer.position) <= 600 || unlimitedSight == true)
        {
            Vector3 directionToTarget = mainPlayer.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);



            // Player is behind my front view.
            if (Mathf.Abs(angle) > 60  && threeSixtyDegreeSight == false)
            {
                //Debug.Log("You're behind me");
            }
            
            // Player is in front of front view
            else if (Mathf.Abs(angle) <= 60 || (threeSixtyDegreeSight && (Vector3.Distance(mainPlayer.transform.position, transform.position) < 500)))
            {
                Vector3 lookPos = Vector3.zero;
                if (specialLookPosition)
                {
                    lookPos = specialLookPosition.position;
                }
                if (!specialLookPosition)
                {
                    lookPos = transform.position;
                }

                // Send out raycast to make sure player isn't behind cover. 
                if (Physics.Raycast(lookPos, mainPlayer.transform.position - lookPos, out var hit))
                {
                    var exposed = (hit.collider == GlobalVars.playerCollider);
                    if (exposed)
                    {
                        return true;
                    }
                    else
                    {
                        //Debug.Log("Can't see you behind cover");
                    }
                }
            }
        }
        return false;
    }

    protected void IsEnemySpotted()
    {
        // This is a bad method, but since void Start, OnEnable, or Awake refuses to get the component, this is the only
        // way to do this.
        if (!mainPlayer)
        {
            mainPlayer = GlobalVars.playerTransform;
        }

        if (aiState == AiStateEnum.Neutral || aiState == AiStateEnum.Patrol)
        {
            if (EnemySight())
            {
                newReactionDelay = reactionTime * (GlobalVars.RandNumTable() / 255f);
                //Debug.Log("New reaction time: " + newReactionDelay);
                currentTimeBeforeFire = Time.time + newReactionDelay;
                aiState = AiStateEnum.See;
                //Debug.Log("I SEE YOU.");
                if (spotSound.Length > 0)
                {                   
                    if (spotSound.Length == 1)
                    {
                        GlobalVars.PlaySoundObj(transform.position, spotSound[0], 1, !noLimitAlertSound, 80f, soundPitch);
                    }

                    if (spotSound.Length > 1)
                    {
                        GlobalVars.PlaySoundObj(transform.position, spotSound[Random.Range(0, spotSound.Length)], 1, !noLimitAlertSound, 80f, soundPitch);
                    }

                }

            }
        }
    }

    protected void BeginAttackProcess()
    {      
        aiState = AiStateEnum.Firing;      
    }

    protected virtual void EndAttackProcess()
    {
        newReactionDelay = reactionTime * (GlobalVars.RandNumTable() / 255f);
        //Debug.Log("New reaction time: " + newReactionDelay);

        //Debug.Log("Time.time: " + Time.time + " + newReactionDelay: " + newReactionDelay + " = " + (Time.time + newReactionDelay));

        //Debug.Log("currentReactionTime = " +currentReactionTime);
        reactionDelay = newReactionDelay;
        currentTimeBeforeFire = Time.time + newReactionDelay;
        allowedToFire = true;
        
    }

}
