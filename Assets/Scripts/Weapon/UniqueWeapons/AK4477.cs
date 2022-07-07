using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK4477 : ProjectileWeapon
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
        if (playerStateScript.currentState != PlayerState.PlayerStates.Dead && ammo > 0)
        {
            //Vector3 addforceDirection = raycastShootScript.GetFireDirection(playerCollider) - fireLocation.position.normalized * projectileSpeed;
            isFiring = true;
            playerStateScript.currentState = PlayerState.PlayerStates.Shooting;
            var bullet = Instantiate(projectile, fireLocation.position, Quaternion.identity);
            bullet.transform.parent = null;
            bullet.transform.position = fireLocation.position;
            bullet.GetComponent<BluntProjectile>().myShooter = myShooter;
            //bullet.GetComponent<Rigidbody>().velocity = raycastShootScript.GetFireDirection() * projectileSpeed;
            var bulletRB = bullet.GetComponent<Rigidbody>();
            bulletRB.AddForce((raycastShootScript.GetFireDirection(playerCollider) - fireLocation.position /* + new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z)*/).normalized * projectileSpeed);
            //bulletRB.AddForce(addforceDirection);
                
            GlobalVars.PlaySoundObj(transform.position, shootSound, volume);
            if (hasAmmoLimit)
            {
                ammo -= 1;
            }
        }
    }
}
