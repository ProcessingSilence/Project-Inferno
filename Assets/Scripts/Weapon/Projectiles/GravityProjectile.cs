// Projectile affected by gravity.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityProjectile : ProjectilePhysics
{
    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
