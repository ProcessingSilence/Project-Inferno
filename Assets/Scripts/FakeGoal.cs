using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeGoal : MonoBehaviour
{
    public Image flash;

    public GameObject trueAngel;

    public Transform player;

    private Coroutine spawnProcess;

    public GameObject model;

    public EquippedWeapon equippedWeaponScript;

    public GameObject superGrenadeLauncher;

    public Timer timerScript;
    // Start is called before the first frame update
    void Awake()
    {
        trueAngel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 20f && spawnProcess == null)
        {
            spawnProcess = StartCoroutine(SpawnProcess());
        }
    }

    IEnumerator SpawnProcess()
    {
        flash.enabled = true;
        trueAngel.SetActive(true);
        model.SetActive(false);
        equippedWeaponScript.WeaponChange(Instantiate(superGrenadeLauncher, transform.position, Quaternion.identity));
        yield return new WaitForSecondsRealtime(0.1f);
        flash.enabled = false;
        timerScript.dontStart = false;
    }
}
