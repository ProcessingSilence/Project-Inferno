using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject explosion;
    private Explosion explosionScript;
    [SerializeField]private ExplosionProperties eP;
    private bool alreadyTouchedGoal;

    public Light goalLight;
    private float originalLightIntensity;
    private void Awake()
    {
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        explosion.SetActive(false);
        explosionScript = explosion.GetComponent<Explosion>();
        explosionScript.eP = eP;
        originalLightIntensity = goalLight.intensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HasTouchedGoal(other.gameObject);
        }
    }

    void HasTouchedGoal(GameObject player)
    {
        if (alreadyTouchedGoal == false)
        {
            alreadyTouchedGoal = true;
            player.gameObject.SetActive(false);
            GlobalVars.levelComplete = true;
            explosion.SetActive(true);
            goalLight.intensity *= 100;
            StartCoroutine(LightEffect());
        }
    }

    IEnumerator LightEffect()
    {
        yield return new WaitForSecondsRealtime(0.003f);
        goalLight.intensity -= originalLightIntensity;
        if (goalLight.intensity > originalLightIntensity)
        {
            StartCoroutine(LightEffect());
        }
    }
}
