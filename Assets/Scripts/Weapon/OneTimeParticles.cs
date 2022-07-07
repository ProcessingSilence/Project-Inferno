using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeParticles : MonoBehaviour
{
    private ParticleSystem particleSystem;

    [SerializeField] private Light light;

    private float lightIntensity;

    public float currentTime;
    // Start is called before the first frame update
    void Awake()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        if (light)
        {
            lightIntensity = light.intensity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (light && currentTime < particleSystem.main.duration)
        {
            light.intensity = Mathf.Lerp(lightIntensity, 0, currentTime/particleSystem.main.duration);
            currentTime += Time.deltaTime;
        }

        if (particleSystem.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
