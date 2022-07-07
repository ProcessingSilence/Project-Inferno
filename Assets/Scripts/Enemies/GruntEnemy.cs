using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemy : PathfindingEnemy
{
    public AudioClip fireSound;

    [SerializeField] private Transform firePos;

    public Animator animator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator.Play("Jump Loop", -1, Random.Range(0,1f));
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch (aiState)
        {
            case AiStateEnum.See:
            {
                break;
            }
            case AiStateEnum.Firing:
            {
                if (firingProcess == null)
                {
                    firingProcess = StartCoroutine(FireAtPlayer());
                }

                break;
            }
        }
    }
    
        
    IEnumerator FireAtPlayer()
    {
        navMeshAgent.speed = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(0.7f);
        Debug.Log("FIRE");
        var playerPos = GlobalVars.mainPlayer.transform.position;
        playerPos = new Vector3(playerPos.x, playerPos.y +0.3f, playerPos.z);
        Instantiate(enemyProj, firePos.position,
            Quaternion.LookRotation(playerPos));
        
        GlobalVars.PlaySoundObj(transform.position, fireSound, 0.5f, true);
        yield return new WaitForSecondsRealtime(0.5f);
        EndAttackProcess();
        aiState = AiStateEnum.See;
        firingProcess = null;    
    }
}
