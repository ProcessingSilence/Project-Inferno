using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public float health;
    public float previousHealth;
    

    [HideInInspector] public float maxHealth;
    public enum PlayerStates { Neutral, Jumping, Shooting, Killed, Dead }

    public PlayerStates currentState;

    private bool isEnemy;

    private bool isPlayer;

    private PlayerController playerController;

    public AudioClip targetSound;
    public float soundPitch;

    [HideInInspector] public RequiredKillsToOpen myTracker;

    public bool dontDestroyOnDeath;

    public GameObject specialDamageParticle;
    public GameObject specialDeathParticle;

    public bool noDamageParticle;
    public bool noDeathParticle;
    
    private GameObject currentDamageParticle;

    private GameObject currentDeathParticle;

    void Awake()
    {
        GlobalVars.levelComplete = false;

        if (specialDamageParticle)
        {
            currentDamageParticle = specialDamageParticle;
        }
        else
        {
            currentDamageParticle = Resources.Load("DamageParticle") as GameObject;
        }

        if (specialDeathParticle)
        {
            currentDeathParticle = specialDeathParticle;
        }
        else
        {
            currentDeathParticle = Resources.Load("GoreParticles") as GameObject;
        }

        previousHealth = health;
    }


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        if (gameObject.CompareTag("Enemy") && dontDestroyOnDeath == false)
        {
            isEnemy = true;
        }

        if (gameObject.CompareTag("Player"))
        {
            isPlayer = true;
            playerController = GetComponent<PlayerController>();
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= GlobalVars.deathYPos)
        {
            health = -10000;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health % 1 != 0)
        {
            health = Mathf.Round(health * 1f) * 1;
        }

        if (previousHealth != health)
        {
            previousHealth = health;
            if (noDamageParticle == false)
            {
                Instantiate(currentDamageParticle, transform.position, Quaternion.identity);
            }
        }

        if (health <= 0 && currentState != PlayerStates.Dead)
        {          
            currentState = PlayerStates.Dead;
           
            if (isEnemy)
            {
                GlobalVars.PlaySoundObj(transform.position, targetSound, 1, false, 80f, soundPitch);
                if (myTracker)
                {
                    myTracker.enemyNum -= 1;
                }

                if (noDeathParticle == false)
                {
                    Instantiate(currentDeathParticle, transform.position, Quaternion.identity);
                }
            }
            

            if (isPlayer)
            {
                animator.SetTrigger("death");
                playerController.maxSpeed = 0;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Collider>().enabled = false;                
            }
        }
    }

    private void LateUpdate()
    {
        if (currentState == PlayerStates.Dead && isEnemy)
        {
            Destroy(gameObject);
        }
    }
}