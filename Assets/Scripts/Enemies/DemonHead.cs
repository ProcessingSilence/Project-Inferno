using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHead : EnemyStats
{
    [SerializeField] private Transform[] eyeFirePositions;
    [SerializeField] private AudioClip fireSound;

    public float fireRate;
    public float delayBeforeFire;

    [SerializeField] private bool isGiantDemonHead;

    private PlayerState myState;

    private Coroutine giantHeadDeathProcess;
    protected override void Awake()
    {
        base.Awake();
        if (isGiantDemonHead)
        {
            myState = GetComponent<PlayerState>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isGiantDemonHead == false)
        {
            fireRate = 0.5f;
            delayBeforeFire = 0.7f;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isGiantDemonHead)
        {
            if (myState.health <= 0 && giantHeadDeathProcess == null)
            {
                giantHeadDeathProcess = StartCoroutine(GiantHeadDeathProcess());
            }
        }

        base.Update();
        switch (aiState)
        {
            case AiStateEnum.See:
            {
                transform.LookAt(GlobalVars.mainPlayer.transform.position);
                break;
            }
            case AiStateEnum.Firing:
            {
                transform.LookAt(GlobalVars.mainPlayer.transform.position);
                if (firingProcess == null)
                {
                    firingProcess = StartCoroutine(FireAtPlayer());
                }

                break;
            }
            case AiStateEnum.Dead:
            {
                break;
            }
        }
    }

    IEnumerator FireAtPlayer()
    {
        if (EnemySight())
        {
            yield return new WaitForSecondsRealtime(delayBeforeFire);
            Debug.Log("FIRE");
            var playerPos = GlobalVars.mainPlayer.transform.position;
            playerPos = new Vector3(playerPos.x, playerPos.y +0.3f, playerPos.z);
        
            // Randomly instantiate at either two eye positions.
            Instantiate(enemyProj, eyeFirePositions[GlobalVars.RandNumTable() % 2].position,
                Quaternion.LookRotation(playerPos));
        
            GlobalVars.PlaySoundObj(transform.position, fireSound, 0.4f, true, 80f, UnityEngine.Random.Range(0.8f,1f));
            yield return new WaitForSecondsRealtime(fireRate * Random.Range(0.8f, 1.2f));
        }


        EndAttackProcess();
        if (isGiantDemonHead == false)
        {
            aiState = AiStateEnum.See;
        }
        firingProcess = null;  
    }

    IEnumerator GiantHeadDeathProcess()
    {
        var explosion = Resources.Load("Explosion") as GameObject;
        
        GlobalVars.PlaySoundObj(transform.position, myState.targetSound, 1, false, 80f, soundPitch);
        yield return new WaitForSecondsRealtime(0.15f);
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.4f);
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.2f);
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.3f);
        for (int i = 0; i < 42; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        }
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        var bigExplosion = Instantiate(explosion, RandomPositionOffset(), Quaternion.identity);
        bigExplosion.SetActive(false);
        var explosionScript = bigExplosion.GetComponent<Explosion>();
        explosionScript.eP.minParticleSize = 1;
        explosionScript.eP.pitch = 0.7f;
        bigExplosion.SetActive(true);
        
        
        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(gameObject);
    }

    Vector3 RandomPositionOffset()
    {
        return transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(0, 3));
    }

    protected override void EndAttackProcess()
    {
        currentTimeBeforeFire = Time.time + reactionTime;
        allowedToFire = true;
    }
}
