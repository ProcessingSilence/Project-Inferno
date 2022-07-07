using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireDestroyToOpen : MonoBehaviour
{
    public GameObject requiredObject;

    public Vector3 lerpLocation;
    private Vector3 prevTransformLoc;

    public float moveSpeed;
    private bool beginMove;

    private float lerpTime;

    private int angelProcessThing;
    // Start is called before the first frame update
    void Awake()
    {
        prevTransformLoc = transform.position;
        if (requiredObject.CompareTag("Enemy"))
        {
            angelProcessThing = 1;
        }
    }

    private void Update()
    {
        if (requiredObject == null || (!requiredObject.CompareTag("Enemy") && angelProcessThing == 1))
        {
            angelProcessThing = 2;
            beginMove = true;
        }

        if (beginMove && lerpTime < 1)
        {
            lerpTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(prevTransformLoc, lerpLocation, lerpTime);
        }

        if (lerpTime >= 1)
        {
            RequireDestroyToOpen script = this;
            script.enabled = false;
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
