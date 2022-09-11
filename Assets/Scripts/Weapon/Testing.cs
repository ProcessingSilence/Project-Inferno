using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Transform otherTransform;

    void Update () {
        /*
        var relativePoint = transform.InverseTransformPoint(otherTransform.position);
        if (relativePoint.z < 0.0)
        {
            // Debug.Log("Object is in front");
        }
        else if (relativePoint.z > 0.0)
        {
            // Debug.Log("Object is in back");
        }
        else
        {
            // Debug.Log("Object is directly above");
        }
        */

        // Vector3 directionToTarget = otherTransform.position - transform.position;
        // float angel = Vector3.Angle(transform.forward, directionToTarget);
        // if (Mathf.Abs(angel) > 60)
        //     Debug.Log("target is behind me");
        // else
        // {
        //     Debug.Log("I see target");
        // }
    }

}
