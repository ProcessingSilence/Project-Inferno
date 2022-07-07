using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ExplosionProperties eP = new ExplosionProperties();

    public bool viewGizmo;
    
    // Ignore this target since they already took direct hit damage from the explosive.
    public Collider directHitVictim;
    
    private List<GameObject> disabledLayerObjs;
    [SerializeField]private List<HurtVictim> allVictims;

    public LayerMask allowedToHit;
    // Start is called before the first frame update
    void Start()
    {
        Kaboom();
    }


    public void Kaboom()
    {
        disabledLayerObjs = new List<GameObject>();
        allVictims = new List<HurtVictim>();
        var explosionParticles = Instantiate(eP.explosionParticles, transform.position, Quaternion.identity);
        explosionParticles.GetComponent<ParticleSystemRenderer>().minParticleSize = eP.minParticleSize;
        if (eP.pitch <= 0)
        {
            eP.pitch = 1;
        }

        GlobalVars.PlaySoundObj(transform.position, eP.explosionSound, 0.6f, false, 80f, eP.pitch);

        if (eP.damage > 0)
        {
            GetAllVictimsAndSort();

            ApplyAllDamage();
        }


        var restoreCheck = 0;
        foreach (var target in disabledLayerObjs)
        {
            target.layer = 0;
            target.GetComponent<Collider>().enabled = true;
            restoreCheck++;
        }

        if (restoreCheck == disabledLayerObjs.Count)
        {
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (viewGizmo)
        {
            Gizmos.DrawSphere(transform.position, eP.radius);
        }
    }

    private void GetAllVictimsAndSort()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, eP.radius);

        if (directHitVictim)
        {
            directHitVictim.gameObject.layer = 2;
            directHitVictim.GetComponent<Collider>().enabled = false;
            disabledLayerObjs.Add(directHitVictim.gameObject);
        }

        // Get all the objects and their properties as long as they have a PlayerState script
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            PlayerState playerState = objectsInRange[i].gameObject.GetComponent<PlayerState>();
            if (playerState != null)
            {
                allVictims.Add(new HurtVictim());
                int iterationNum = allVictims.Count - 1;
                
                // Distance
                allVictims[iterationNum].distanceFromAttack = Vector3.Distance(transform.position,
                    objectsInRange[i].gameObject.transform.position);
                
                // GameObject
                allVictims[iterationNum].victimObj = objectsInRange[i].gameObject;
                
                // Collider
                allVictims[iterationNum].victimCol = objectsInRange[i];
                
                // PlayerState script
                allVictims[iterationNum].playerState = playerState;
                
            }
        }

        // Sort all victims from closest distance to farthest distance from explosion transform.
        allVictims.Sort(delegate(HurtVictim x, HurtVictim y)
        {
            return x.distanceFromAttack.CompareTo(y.distanceFromAttack);
            
        });
    }

    private void ApplyAllDamage()
    {
        for (int i = 0; i < allVictims.Count; i++)
        {           
            GameObject target = allVictims[i].victimObj;
            if (allVictims[i].victimCol != directHitVictim)
            {
                bool exposed = false;
                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out var hit, allowedToHit))
                {
                    exposed = hit.collider == allVictims[i].victimCol;
                }

                if (exposed)
                {
                    // Don't apply layer if player, or you fall through the stage! D:
                    if (target.CompareTag("Enemy"))
                    {
                        allVictims[i].victimCol.enabled = false;
                        target.gameObject.layer = 2;
                        disabledLayerObjs.Add(target.gameObject);
                    }

                    // Linear falloff of explosion damage
                    float proximity = (transform.position - target.transform.position).magnitude;
                    float effect = 1 - ((proximity / eP.radius)/2);

                    var recievedDamage = eP.damage * effect;
                    
                    // Sometimes the damage becomes positive for some reason, explosions shouldn't heal.
                    if (recievedDamage > 0)
                    {
                        recievedDamage = -recievedDamage;
                    }
                    
                    allVictims[i].playerState.health += recievedDamage;
                }
            }
        }
    }


}

[Serializable]
public class ExplosionProperties
{
    public float force;
    public float damage;
    public float minParticleSize;

    public float radius;

    public GameObject explosionParticles;

    public AudioClip explosionSound;

    public Transform shotFromWho;
    public float pitch;
}

[Serializable]
class HurtVictim
{
    public float distanceFromAttack;
    public GameObject victimObj;
    public Collider victimCol;
    public PlayerState playerState;
}