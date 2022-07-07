using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    [SerializeField] protected bool explodeAfterSeconds;
    
    public float secondsUntilExplosion;

    [SerializeField] protected bool explodeOnPlayerHit;

    [SerializeField] protected bool explodeOnCollision;

    [SerializeField] protected bool destroyOnHit;

    public GameObject explosionObj;

    private Explosion explosionScript;

    [HideInInspector] public ExplosionProperties eP;

    private bool alreadyExploded;

    public Collider myShooter;
    public Collider directHitVictim;
    
    // Start is called before the first frame update
    void Start()
    {
        if (explodeAfterSeconds)
        {
            StartCoroutine(WaitUntilExplode());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            if (explodeOnPlayerHit && other.collider != myShooter)
            {
                Explode(other);
            }
        }
    }

    private void Explode(Collision other = null)
    {
        if (alreadyExploded == false)
        {
            alreadyExploded = true;
            var explodingObj = Instantiate(explosionObj, transform.position, Quaternion.identity);
            var explosionScript = explodingObj.GetComponent<Explosion>();
            explosionScript.eP = eP;
            explosionScript.eP.shotFromWho = myShooter.transform;
            if (other != null)
            {
                explosionScript.directHitVictim = other.collider;
                other.gameObject.GetComponent<PlayerState>().health -= eP.damage;
            }
            explodingObj.SetActive(true);
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitUntilExplode()
    {
        yield return new WaitForSecondsRealtime(secondsUntilExplosion);
        Explode();
    }
}
