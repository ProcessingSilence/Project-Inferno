using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class RequiredKillsToOpen : MonoBehaviour
{
    public GameObject[] requiredKills;

    public int enemyNum;

    public Vector3 lerpLocation;
    private Vector3 prevTransformLoc;

    public float moveSpeed;
    private bool beginMove;

    private float lerpTime;
    
    // Start is called before the first frame update
    void Awake()
    {
        enemyNum = requiredKills.Length;
        for (int i = 0; i < requiredKills.Length; i++)
        {
            requiredKills[i].GetComponent<PlayerState>().myTracker = this;
        }

        prevTransformLoc = transform.position;
    }

    private void Update()
    {
        if (enemyNum < 1 )
        {
            beginMove = true;
        }

        if (beginMove && lerpTime < 1)
        {
            lerpTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(prevTransformLoc, lerpLocation, lerpTime);
        }

        if (lerpTime >= 1)
        {
            gameObject.GetComponent<RequiredKillsToOpen>().enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lerpLocation, 0.5f);
        }
    }
}
