using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for other objects that track if an object is destroyed.
public class EventTriggerBox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
