using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    public float speed;
    public float range;
    public float currentTime;

    void Awake()
    {
        currentTime = Time.time;
    }

    void Update()
    {
        var newPosition = transform.position;
        newPosition .y += Mathf.Sin((Time.time - currentTime) * speed) * range * Time.deltaTime;
        transform.position = newPosition ;
    }
}
