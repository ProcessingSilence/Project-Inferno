using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : ProjectileWeapon
{
    public AudioClip shootSound;

    [SerializeField]public ExplosionProperties eP;

    private float maxSpeed;

    private float maxUpwardForce;

    protected override void Start()
    {
        base.Start();
        maxSpeed = projectileSpeed * 2;
        maxUpwardForce = upwardForce * 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Time.time > nextFire)
        {
            if (Input.GetButton("Fire1"))
            {
                nextFire = Time.time + firerate;
                Fire();
            }

            if (!Input.GetButton("Fire1") && playerStateScript.currentState == PlayerState.PlayerStates.Shooting)
            {
                playerStateScript.currentState = PlayerState.PlayerStates.Neutral;
            }
        }



    }

    public void Fire()
    {
        if (playerStateScript.currentState != PlayerState.PlayerStates.Dead && ammo > 0)
        {
            isFiring = true;
            playerStateScript.currentState = PlayerState.PlayerStates.Shooting;
            
            var projectileObj = Instantiate(projectile, fireLocation.position, Quaternion.identity);
            projectileObj.transform.parent = null;
            projectileObj.transform.position = fireLocation.position;

            var explosiveProj = projectileObj.GetComponent<ExplosiveProjectile>();
            explosiveProj.eP = eP;
            explosiveProj.myShooter = myShooter;
            //projectileObj.GetComponent<Rigidbody>().velocity = raycastShootScript.GetFireDirection() * projectileSpeed;
            var projectileObjRb = projectileObj.GetComponent<Rigidbody>();
            projectileObjRb.AddForce((raycastShootScript.GetFireDirection(playerCollider) - fireLocation.position /* + new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z)*/ ).normalized * projectileSpeed);
            projectileObjRb.AddForce(Vector3.up * upwardForce);
            //projectileObj.transform.rotation = Quaternion.Euler(raycastShootScript.GetFireDirection(playerCollider) - fireLocation.position /* + new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z)*/ ).normalized;
            projectileObj.transform.rotation = transform.parent.parent.rotation;
            projectileObjRb.AddTorque(projectileObj.transform.position * 10000f);
            GlobalVars.PlaySoundObj(transform.position, shootSound, 0.6f);
            if (hasAmmoLimit)
            {
                ammo -= 1;
            }
        }
    }
}
