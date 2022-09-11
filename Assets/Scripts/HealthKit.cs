using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    private bool isGivingHealth;

    public float healthGiven;

    public bool goesAboveMax;

    public GameObject[] model;

    public AudioClip healthSound;

    [SerializeField] private ParticleSystem neutralItemEffect;

    [SerializeField] private ParticleSystem itemGetEffect;

    private GameObject neutralitemEffectObj;
    private GameObject itemGetEffectObj;


    // Start is called before the first frame update
    void Awake()
    {
        itemGetEffectObj = itemGetEffect.gameObject;
        itemGetEffectObj.SetActive(false);

        neutralitemEffectObj = neutralItemEffect.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isGivingHealth == false)
            {
                isGivingHealth = true;
                GiveHeathProcess(other);
            }
        }
    }

    void GiveHeathProcess(Collider other)
    {
        PlayerState playerState = other.GetComponent<PlayerState>();

        float healthAddLimit = playerState.maxHealth;

        // Allow health limit to go above max, and up to max*2
        if (goesAboveMax)
        {
            healthAddLimit *= 2;
        }

        // Only give health if player health is below the add health limit.
        if (playerState.health < healthAddLimit)
        {
            var tempHealth = playerState.health;
            tempHealth += healthGiven;
            if (tempHealth > healthAddLimit)
            {
                tempHealth = healthAddLimit;
            }

            playerState.health = tempHealth;

            for (int i = 0; i < model.Length; i++)
            {
                if (model[i])
                {
                    model[i].SetActive(false);
                }
            }
            GlobalVars.PlaySoundObj(transform.position, healthSound, 0.8f);
            gameObject.GetComponent<Collider>().enabled = false;
            neutralitemEffectObj.SetActive(false);
            itemGetEffectObj.SetActive(true);
            itemGetEffect.Play();
            //gameObject.SetActive(false);
        }

        // Health >= health limit, cancel out.
        else
        {
            isGivingHealth = false;
        }
    }
}
