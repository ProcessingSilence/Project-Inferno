using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDemonHeadProjectile : AngelProjectile
{
    public bool hurtOtherEnemies;
    // Update is called once per frame
    void Update()
    {
        
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (hurtOtherEnemies)
        {
            // Giant demon head doesn't give a fuck, he kills everything.
            if (other.CompareTag("Enemy") && other.gameObject.name != "GiantDemonHead" && !other.gameObject.name.Contains("Target"))
            {
                other.GetComponent<PlayerState>().health -= damage;
                Destroy(gameObject);
            }
        }
    }
}
