using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains 2 functions that allow the melee attack to attack multiple enemies at once, or the first enemy closest
// to the melee attack.
public class MeleeWeapon : WeaponStats
{
    private Collider collider;

    // Determines if melee attack can shish kabob lined up enemies in collider.
    public bool multipleCollision;

    // Get list of those already hit by the melee so they don't get hit again.
    [SerializeField] protected List<Collider> alreadyAttacked;

    // Prevents hit
    private bool hasPlayedHitSound;

    private bool flushList;

    [SerializeField] protected AudioClip throwSound;
    [SerializeField] protected AudioClip hitSound;

    [SerializeField] protected LayerMask enemyOrMapLayer;
    [SerializeField] protected LayerMask playerOrMapLayer;

    private Transform playerTransform;

    private Vector3 raycastPos;

    public bool meleeHit;
    public enum MeleeStates
    {
        Neutral,
        Throwing,
        Hitting,        
        HasHit
    }

    public MeleeStates meleeState;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collider = GetComponent<Collider>();
        ammoText = GlobalVars.sAmmoText;
        ammoText.text = "Infinite";
        playerStateScript = GlobalVars.mainPlayerState;
        playerTransform = GlobalVars.mainPlayer.transform;
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


        switch (meleeState)
        {
            case MeleeStates.Neutral:
            {
                if (alreadyAttacked.Count > 0)
                {
                    alreadyAttacked.Clear();
                }
                break;
            }
            case MeleeStates.Throwing:
            {
                if (alreadyAttacked.Count > 0)
                {
                    alreadyAttacked.Clear();
                }
                break;
            }

            case MeleeStates.Hitting:
            {
                flushList = true;
                break;
            }
        }
        collider.enabled = meleeState == MeleeStates.Hitting;
    }


/*
        collider.enabled = meleeHit == true && playerStateScript.currentState != PlayerState.PlayerStates.Dead;
        if (alreadyAttacked.Count > 0 && meleeHit == false)
        {
            FlushList();
        }
    }
*/
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DetermineFate(other);
        }
    }

    public void FlushList()
    {
        alreadyAttacked.Clear();
    }

    protected virtual void DetermineFate(Collider other)
    {
        Debug.Log("ENEMY");
        if (multipleCollision)
        {
            MultiCol_DetermineToHurt(other);
        }

        if (multipleCollision == false)
        {
            SingleCol_DetermineToHurt();
        }
        
    }

    // Everyone in the collider gets hit, unless if they're behind cover.
    protected virtual void MultiCol_DetermineToHurt(Collider other)
    {
        bool hasBeenAttacked = false;

        if (alreadyAttacked.Count > 0)
        {
            // Go through the hitObj list to make sure the enemy wasn't already attacked.
            foreach (var hitObj in alreadyAttacked)
            {
                if (hitObj)
                {
                    if (other.gameObject == hitObj.gameObject)
                    {
                        hasBeenAttacked = true;
                        Debug.Log("HAS ALREADY BEEN ATTACKED.");
                    }
                }
            }
        }

        if (hasBeenAttacked == false)
        {
            // Send a raycast out from the melee weapon to the enemy, if it doesn't detect a wall, apply damage.                      
            if (!Physics.Raycast(transform.position, other.transform.position, out var hit, Mathf.Infinity, 7))
            {

                alreadyAttacked.Add(other);
                other.GetComponent<PlayerState>().health -= damage;
                GlobalVars.PlaySoundObj(transform.position, hitSound);               
            }
            else
            {
                Debug.Log("Melee victim behind cover.");
            }
        }
    }
    
    //new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

    // Only attack the closest victim in the collider that's not behind cover, and ignore everything else.
    protected virtual void SingleCol_DetermineToHurt()
    {
        Collider[] objectsInRange = Physics.OverlapBox(transform.position, collider.bounds.size * 0.5f, Quaternion.identity);
        
        Debug.Log("objects in range: " + objectsInRange.Length);
        
        List<HurtVictim> victimList = new List<HurtVictim>();
        
        // Get all the objects and their properties as long as they have an "Enemy" tag.
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            if (objectsInRange[i].CompareTag("Enemy"))
            {
                // Send a raycast out from the weapon to the enemy to make sure they're not behind cover.
                if (!Physics.Raycast(transform.position, objectsInRange[i].transform.position, Mathf.Infinity, 7))
                {
                    meleeState = MeleeStates.HasHit;
                    // Victim confirmed, add to victim list to have their distance compared
                    victimList.Add(new HurtVictim());
                    
                    victimList[victimList.Count-1].distanceFromAttack =
                        Vector3.Distance(transform.position, objectsInRange[i].transform.position);

                    victimList[victimList.Count-1].victimObj = objectsInRange[i].gameObject;                    
                }
            }            
        }

        Debug.Log("Victim list count: " + victimList.Count);

        // Compare victims to see who is the closest to the attack.
        if (victimList.Count > 0)
        {
            meleeState = MeleeStates.HasHit;
            HurtVictim chosenVictim = new HurtVictim();
            chosenVictim.distanceFromAttack = float.MaxValue;
            
            // If there's only 1 victim, choose the first victim in list.
            if (victimList.Count == 1)
            {
                chosenVictim = victimList[0];
            }

            // Compare all victim transforms to see who is the closest in a tournament-like fashion.
            if (victimList.Count > 1)
            {
                for (int i = 0; i < victimList.Count; i++)
                {
                    if (victimList[i].distanceFromAttack < chosenVictim.distanceFromAttack)
                    {
                        chosenVictim = victimList[i];
                    }
                }    
            }

            chosenVictim.victimObj.GetComponent<PlayerState>().health -= damage;
            GlobalVars.PlaySoundObj(transform.position, hitSound);
        }
    }
}
