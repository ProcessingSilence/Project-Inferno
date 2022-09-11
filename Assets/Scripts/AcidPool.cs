using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    public float damage;

    public float damageRate;

    private Coroutine activateDamageRate;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && activateDamageRate == null)
        {
            activateDamageRate = StartCoroutine(HurtPlayer(other.gameObject));
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && activateDamageRate == null)
        {
            activateDamageRate = StartCoroutine(HurtPlayer(other.gameObject));
        }
    }

    IEnumerator HurtPlayer(GameObject other)
    {
        other.gameObject.GetComponent<PlayerState>().health -= damage;
        yield return new WaitForSecondsRealtime(damageRate);
        activateDamageRate = null;
    }
}
