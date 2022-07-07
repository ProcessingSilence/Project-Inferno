using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectilePhysics : MonoBehaviour
{
    protected Rigidbody rb;
    public bool isBouncy;
    public Collider myShooter;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
