using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public float secondsBeforeDestroy;
    public float damage;
    
    public bool negateCollisionDamageOnLand;

    public bool destroyOnLand;

    public BulletTrail bulletTrailScript;
    
    /*
    public bool isExplosive;
    public float explosiveRange;
    public float explosionForce;
    */

    void Start()
    {
        StartCoroutine(WaitBeforeDestroy());
    }
    

    
    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSecondsRealtime(secondsBeforeDestroy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("TAG: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Land"))
        {
            LandCollision();
        }
    }

    protected void LandCollision()
    {
        if (destroyOnLand)
        {
            if (bulletTrailScript)
            {
                bulletTrailScript.transform.parent = null;
                bulletTrailScript.destroySelf = true;
            }

            Destroy(gameObject);
        }

        if (negateCollisionDamageOnLand)
        {
            damage = 0;
        }
    }
}
