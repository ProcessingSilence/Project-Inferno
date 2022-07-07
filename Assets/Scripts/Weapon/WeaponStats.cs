using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStats : MonoBehaviour
{
    public string weaponName;
    public GameObject weaponModel;
    public GameObject destructionModel;
    
    public float damage;
    public float pushForce;
    
    // How many seconds before being able to fire again, either automatically or manually.
    public float firerate;

    public bool isFiring;
    public float nextFire;
    
    // Location to instantiate projectile or have bullet effects occur.
    public Transform fireLocation;

    public bool hasAmmoLimit;
    public bool destroyOnNoAmmo;
    
    public float ammo;
    private float maxAmmo; 
    public TextMeshProUGUI ammoText;
    
    public RaycastShoot raycastShootScript;

    public Rigidbody playerRB;

    public float lastTimeFired;

    public PlayerState playerStateScript;
    public Collider playerCollider;

    public Transform modelRotation;

    public float volume;

    public Collider myShooter;
    public Slider ammoBar;

    protected MenuOpen menuScript;

    //protected float currentTime;

    public GameObject crosshair;

    private void OnEnable()
    {
        menuScript = GlobalVars.menuScript;

        foreach (Transform child in GlobalVars.crosshair.transform)
        {
            Destroy(child.gameObject);         
        }

        if (crosshair)
        {
            Instantiate(crosshair, GlobalVars.crosshair.transform);
        }
        
        if (crosshair == null)
        {
            Instantiate(Resources.Load<GameObject>("DefaultCrosshair"), GlobalVars.crosshair.transform);
        }
    }

    protected  virtual void Start()
    {
        ammoBar = GlobalVars.ammoSlider;
        maxAmmo = ammo;
    }

    protected virtual void Update()
    {
        if (hasAmmoLimit && ammo > 0)
        {
            ammoText.text = "<size=96>"+ ammo + "</size>/" + maxAmmo;
            ammoBar.value = ammo / maxAmmo;
        }

        if (hasAmmoLimit == false)
        {
            ammoText.text = "INFINITE AMMO";
        }

        if (hasAmmoLimit && destroyOnNoAmmo && ammo <= 0)
        {
            destroyOnNoAmmo = false;
            ammoText.text = "NO WPN";
            //Destroy(gameObject);
            transform.parent = null;
            foreach (Transform child in destructionModel.transform)
            {
                var childRb = child.gameObject.AddComponent<Rigidbody>();
                childRb.AddForce(new Vector3(-128 +GlobalVars.RandNumTable(), -128 +GlobalVars.RandNumTable(),- 128 + GlobalVars.RandNumTable()) * 10f);
                childRb.AddTorque(new Vector3(-128 +GlobalVars.RandNumTable(), -128 +GlobalVars.RandNumTable(),- 128 + GlobalVars.RandNumTable()) * 10f);
            }
            

            GetComponent<WeaponStats>().enabled = false;
        }
    }

    /*
    public enum WeaponTypes
    {
        Projectile, Raycast, Melee
    }

    public WeaponTypes weaponType;
    */
}
