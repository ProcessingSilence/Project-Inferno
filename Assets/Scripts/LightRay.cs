using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    private Vector3 startSize, endSize;

    private float timeElapsed;

    private float lerpDuration = 0.5f;
    
    public float speed;

    public Vector3 endPoint;

    public float damage;

    private bool hitPlayer;

    protected bool destroyOnLand = true;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        transform.localScale = startSize = new Vector3(0, transform.localScale.y, transform.localScale.z);
        destroyOnLand = false;
        endSize = new Vector3(150, transform.localScale.y, transform.localScale.z);
        startPos = transform.position;
    }

    void Start()
    {
//        Vector3 playerPos = GlobalVars.mainPlayer.transform.position;
//        Vector3 lookPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
//        transform.LookAt(lookPos);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
    }    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(startPos, transform.position) > 500)
        {
            Destroy(gameObject);
        }

        if (timeElapsed < lerpDuration)
        {
            transform.localScale = Vector3.Lerp(startSize, endSize, timeElapsed / lerpDuration);
            timeElapsed += Time.fixedDeltaTime;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hitPlayer == false)
        {
            hitPlayer = true;
            HurtPlayer(other);           
        }

        if (other.CompareTag("Land"))
        {
            if (destroyOnLand)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Land"))
        {
            if (destroyOnLand)
            {
                Destroy(gameObject);
            }
        }
    }

    void HurtPlayer(Collider other)
    {
        other.GetComponent<PlayerState>().health -= damage;
        Destroy(gameObject);
    }

}
