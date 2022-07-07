using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningItem : MonoBehaviour
{
    private float currentRotation;

    public float speed;

    public bool randomDirection;

    public PlayerState myState;

    void Start()
    {
        // Rotation offset;
        currentRotation = -128 + GlobalVars.RandNumTable() * 600f;
        if (randomDirection)
        {
            if (GlobalVars.RandNumTable() % 2 == 0)
            {
                speed = -speed;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myState)
        {
            if (myState.health <= 0)
            {
                gameObject.GetComponent<SpinningItem>().enabled = false;
            }
        }

        currentRotation += speed * Time.deltaTime;
        if (currentRotation > 360)
        {
            currentRotation = currentRotation - 360;
        }

        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }
}
