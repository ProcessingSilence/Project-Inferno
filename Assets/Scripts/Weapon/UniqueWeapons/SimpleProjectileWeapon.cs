using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileWeapon : ProjectileWeapon
{
    public AudioClip shootSound;

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
        if (playerStateScript.currentState != PlayerState.PlayerStates.Dead  && ammo > 0)
        {
            isFiring = true;
            playerStateScript.currentState = PlayerState.PlayerStates.Shooting;
            var bullet = Instantiate(projectile, fireLocation.position, Quaternion.identity);
            bullet.transform.parent = null;
            bullet.transform.position = fireLocation.position;
            //bullet.GetComponent<Rigidbody>().velocity = raycastShootScript.GetFireDirection() * projectileSpeed;
            var bulletRb = bullet.GetComponent<Rigidbody>();

            var lookPos = raycastShootScript.GetFireDirection(playerCollider) - fireLocation.position;
            bulletRb.AddForce((raycastShootScript.GetFireDirection(playerCollider) - fireLocation.position /* + new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z)*/ ).normalized * projectileSpeed);
            bullet.transform.rotation = Quaternion.LookRotation(lookPos);
            //bullet.transform.rotation = modelRotation.rotation;
            if (upwardForce != 0)
            {
                bulletRb.AddForce(bullet.transform.up * upwardForce);
            }


            //bulletRb.AddTorque(transform.right * 10000f);
            GlobalVars.PlaySoundObj(transform.position, shootSound, 1f, false, 80, 0.8f);
            if (hasAmmoLimit)
            {
                ammo -= 1;
            }
        }
    }
}
