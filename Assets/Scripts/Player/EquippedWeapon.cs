using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour
{

    public GameObject startingWeapon;
    public GameObject fistWeapon;
    
    public GameObject currentWeapon;
    public WeaponStats weaponScript;
    
    public Transform weaponPlacement;

    public bool infiniteAmmo;
    public float ammo;

    public Vector3 fireDirection;
    public RaycastShoot raycastShootScript;

    public Transform modelRotation;

    public bool equippedFists;
    // Start is called before the first frame update
    void Start()
    {
        raycastShootScript = GetComponent<RaycastShoot>();
        if (startingWeapon)
        {
            WeaponChange(Instantiate(startingWeapon));         
        }
    }

    void Update()
    {
        if (weaponScript)
        {
            weaponScript.enabled = Time.timeScale > 0;
        }

        if (currentWeapon)
        {
            if (weaponScript.ammo <= 0 || GlobalVars.mainPlayerState.health <= 0)
            {
                weaponScript.ammo = 0;
                currentWeapon = null;
            }
        }

        if (currentWeapon == false && equippedFists == false)
        {
            equippedFists = true;
            WeaponChange(Instantiate(fistWeapon));
        }



        if (!currentWeapon)
        {
            GlobalVars.sAmmoText.text = "NO WPN";
        }
    }


    public void WeaponChange(GameObject newWeapon)
    {
        /*
        foreach (Transform child in weaponPlacement)
        {
            Destroy(child.gameObject);
        }*/
        newWeapon.transform.parent = null;
        currentWeapon = newWeapon;
        
        weaponScript = currentWeapon.GetComponent<WeaponStats>();
        if (weaponScript.weaponName != "Fists")
        {
            equippedFists = false;
            foreach (Transform child in weaponPlacement)
            {
                Destroy(child.gameObject);
            }
        }

        weaponScript.raycastShootScript = raycastShootScript;
        weaponScript.modelRotation = modelRotation;
        weaponScript.playerRB = GetComponent<Rigidbody>();
        weaponScript.playerStateScript = GetComponent<PlayerState>();
        weaponScript.playerCollider = GetComponent<Collider>();
        weaponScript.myShooter = gameObject.GetComponent<Collider>();
        weaponScript.ammoText = GlobalVars.sAmmoText;
        
        currentWeapon.transform.position = weaponPlacement.position;
        currentWeapon.transform.rotation = weaponPlacement.rotation;
        
        newWeapon.transform.parent = weaponPlacement;
    }
}
