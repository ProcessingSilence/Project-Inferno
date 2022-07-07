using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluntProjectile : ProjectileProperties
{
    public AudioClip hitTarget;
    public Collider myShooter;    
    /*
    void DestroyTarget(Collision other)
    {
        GlobalVars.PlaySoundObj(transform.position, hitTarget, 0.8f);
        //TargetCounter.targetCount -= 1;
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
    */
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("TAG: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Land"))
        {
            LandCollision();
        }
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            if (other.collider != myShooter)
            {
                other.gameObject.GetComponent<PlayerState>().health -= damage;
                Destroy(gameObject);
            }
        }             
    }
}
