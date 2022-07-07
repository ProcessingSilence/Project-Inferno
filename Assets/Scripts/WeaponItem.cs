using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    public GameObject weaponSlot;

    private string weaponName;

    public GameObject weaponModel;

    private bool weaponIsTaken;

    [SerializeField] private AudioClip pickupSound;

    public bool nearItem;

    public GameObject currentPlayer;

    public EquippedWeapon equippedWeaponScript;
    // Start is called before the first frame update
    void Awake()
    {
        weaponName = weaponSlot.name;

        // Each weapon has a slot for their model so that they can be used for the WeaponItem pickup.
        weaponModel = Instantiate(weaponSlot.GetComponent<WeaponStats>().weaponModel, transform, false);
        weaponModel.transform.localScale *= 2;
        weaponModel.transform.localPosition = Vector3.zero;
        var spinningItem = weaponModel.AddComponent<SpinningItem>();
        spinningItem.speed = 200;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentPlayer && !equippedWeaponScript)
        {
            equippedWeaponScript = currentPlayer.GetComponent<EquippedWeapon>();
        }

        if (equippedWeaponScript)
        {
            if ((Input.GetKeyDown(KeyCode.E) || equippedWeaponScript.currentWeapon.name.Contains(weaponName)) && weaponIsTaken == false && nearItem)
            {
                weaponIsTaken = true;
                GlobalVars.pressEToTakeWeapon.text = "";
                currentPlayer.gameObject.GetComponent<EquippedWeapon>().WeaponChange(Instantiate(weaponSlot));  
                GlobalVars.PlaySoundObj(transform.position, pickupSound, 1, false);
                Destroy(gameObject);
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!currentPlayer)
            {
                currentPlayer = other.gameObject;
            }
            nearItem = true;
            //Debug.Log("Player");
            if (GlobalVars.pressEToTakeWeapon.text != "Press 'E' to switch to " + weaponName)
            {
                GlobalVars.pressEToTakeWeapon.text = "Press 'E' to switch to  " + weaponName;
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            nearItem = false;
            GlobalVars.pressEToTakeWeapon.text = "";
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (Application.isEditor && !Application.isPlaying /*&& UnityEditor.Selection.activeGameObject != gameObject*/)
        {
            Gizmos.DrawSphere(transform.position, 2);
        }
    }
}
