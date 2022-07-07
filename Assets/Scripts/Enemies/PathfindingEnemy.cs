using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingEnemy : EnemyStats
{
    [SerializeField] protected Vector3[] patrolPoints;
    private int destIteration;
    [SerializeField] protected bool isFlyingEnemy;
    public float chaseSpeed;
    public float patrolSpeed;
    public float patrolWaitTime;

    protected NavMeshAgent navMeshAgent;

    protected bool alwaysChaseOnSee = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 0;

        if (aiState == AiStateEnum.Patrol)
        {
            SetPatrolPoints();
        }
    }


    protected override void Update()
    {
        base.Update();

        switch (aiState)
        {
            case AiStateEnum.See:
            {
                if (alwaysChaseOnSee)
                {
                    navMeshAgent.speed = chaseSpeed;
                }
                //navMeshAgent.SetDestination(GlobalVars.mainPlayer.transform.position);
                break;
            }
        }
    }


    protected void SetPatrolPoints()
    {
        if (patrolPoints.Length > 0 && aiState == AiStateEnum.Patrol)
        {
            if (isFlyingEnemy == false)
            {               
                // The patrol points send a downward raycast so that they are placed directly on the ground.
                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(patrolPoints[i], Vector3.down, out hit, Mathf.Infinity))
                    {
                        patrolPoints[i] = hit.point;
                    }
                    else
                    {
                        Debug.LogError("Warning: Patrol point " + i + " did not find ground from raycast" );
                    }
                }
            }
            aiState = AiStateEnum.Patrol;
        }
    }
}
