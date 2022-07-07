using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class RequiredTutorialToOpen : MonoBehaviour
{
    public Vector3 lerpLocation;
    private Vector3 prevTransformLoc;

    public float moveSpeed;
    private bool beginMove;

    private float lerpTime;
    
    // Start is called before the first frame update
    void Awake()
    {


        prevTransformLoc = transform.position;
    }

    private void Update()
    {

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
