using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtEffect : MonoBehaviour
{
    public Image hurtImage;

    public PlayerState playerStateScript;

    private float fullHealth;


    private Vector3 originalImgScale;

    private Coroutine activateEffect;

    public float currentAlpha;

    private float currentTime;

    public float lastPercent, currentPercent;

    public AudioClip hurtSound;

    public bool noMoreEffects;
    
    // Start is called before the first frame update
    void Start()
    {
        fullHealth = playerStateScript.health;
        RestartPercentage();
        originalImgScale = hurtImage.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentPercent = (playerStateScript.health / fullHealth);
        var damage = lastPercent - currentPercent;
        if (lastPercent > currentPercent && noMoreEffects == false)
        {
            lastPercent = currentPercent;
            //Debug.Log("DAMAGE"+damage);
            currentTime = Time.time;
            if (playerStateScript.health > 0)
            {
                currentAlpha = damage * 2;
            }
            
            if (playerStateScript.health <= 0)
            {
                currentAlpha = 1;
                noMoreEffects = true;
            }
            if (currentAlpha < 0)
            {
                currentAlpha = -currentAlpha;
            }

            hurtImage.transform.localRotation = Quaternion.Euler(0,0,Random.Range(0,349));
            hurtImage.transform.localScale = Vector3.one * Random.Range(41,75); 
            GlobalVars.PlaySoundObj(Vector3.zero, hurtSound, currentAlpha, false);
        }

        if (currentAlpha > 0)
        {
            currentAlpha += (currentTime - Time.time)/40;
            hurtImage.transform.localPosition = new Vector3(Random.Range(-10,10),Random.Range(-10,10),0);
        }

        
        hurtImage.color = new Color(hurtImage.color.r, hurtImage.color.g, hurtImage.color.b, currentAlpha);
    }

    public void RestartPercentage()
    {
        lastPercent = currentPercent = playerStateScript.health / fullHealth;
    }

    /*
    IEnumerator ActivateEffect( float currentDamage)
    {
        yield return new WaitForSecondsRealtime(1f);
    }
    */
}
