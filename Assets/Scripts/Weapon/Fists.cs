using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : MeleeWeapon
{
    public float hitDuration;
    public float beforeHitDelay, afterHitDelay;

    private Coroutine hittingProcess;

    private Renderer renderer;

    private bool isThrowing;
    private void Start()
    {
        base.Start();
        weaponModel.SetActive(false);
        renderer = weaponModel.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        switch (meleeState)
        {
            case MeleeStates.Neutral:
            {
                if (Input.GetButton("Fire1") && playerStateScript.currentState != PlayerState.PlayerStates.Dead && Time.timeScale > 0) 
                {
                    meleeState = MeleeStates.Throwing;

                }

                break;
            }
            case MeleeStates.Throwing:
            {
                if (hittingProcess == null)
                {
                    hittingProcess = StartCoroutine(ThrowingProcess());
                }
                break;
            }
        }
    }

    IEnumerator ThrowingProcess()
    {
        GlobalVars.PlaySoundObj(transform.position, throwSound);
        weaponModel.SetActive(true);
        yield return new WaitForSecondsRealtime(beforeHitDelay);
        meleeState = MeleeStates.Hitting;
        renderer.material.color = Color.red;
        
        yield return new WaitForSecondsRealtime(hitDuration);
        meleeState = MeleeStates.Throwing;
        weaponModel.transform.localScale = Vector3.one;
        renderer.material.color = Color.white;
        
        yield return new WaitForSecondsRealtime(afterHitDelay);
        meleeState = MeleeStates.Neutral;        
        weaponModel.SetActive(false);
        alreadyAttacked.Clear();
        hittingProcess = null;        
    }
}
