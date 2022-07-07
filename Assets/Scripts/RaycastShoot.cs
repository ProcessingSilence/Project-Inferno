using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RaycastShoot : MonoBehaviour
{
    public Camera myCamera;
    private Transform camTransform;

    public RaycastHit currentHit;

    public LayerMask allowedToHit;

    public Vector2 screenCenter;

    public EquippedWeapon equippedWeaponScript;

    public WeaponStats weaponScript;

    // Start is called before the first frame update
    void Awake()
    {
        camTransform = myCamera.transform;
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        if (equippedWeaponScript == false)
        {
            equippedWeaponScript = GetComponent<EquippedWeapon>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure the center is marked, even when the resolution is changed.
        if (screenCenter != new Vector2(Screen.width / 2, Screen.height / 2))
        {
            screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        /*
        // Get the direction of where the camera is pointing based on center of screen, tell equipped weapon they can fire. 
        if (Input.GetButtonDown("Fire1"))
        {

            //equippedWeaponScript.weaponScript.fire = true;
            //if (Physics.Raycast(camTransform.position, camTransform.forward, out currentHit, 3000f, allowedToHit))
        } 
        */       
    }

    // Get the direction of where the camera is pointing based on center of screen,
    public Vector3 GetFireDirection(Collider ownerCollider)
    {
        Ray ray = myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        /*
        if (Physics.Raycast(ray, out hit, allowedToHit))
            return hit.point;
        */

        RaycastHit[] allHits = Physics.RaycastAll(ray, Mathf.Infinity, allowedToHit);
        RaycastHit hit = default(RaycastHit);
        
        float leastDist = float.MaxValue;
        if (allHits.Length > 0)
        {
            for (int i = 0; i < allHits.Length; i++)
            {
                Debug.Log(allHits[i].collider.name, allHits[i].collider);
                // Compare all colliders and get the least distance, prevent it from hitting shooting player.
                if (allHits[i].distance < leastDist && allHits[i].collider != ownerCollider)
                {
                    leastDist = allHits[i].distance;
                    hit = allHits[i];
                }
            }

            Debug.Log("Raycasted object: " + hit.collider.gameObject);
            return hit.point;
        }      
        
        //var ray = myCamera.ScreenPointToRay(screenCenter);
        return ray.GetPoint(50);
    }
}
