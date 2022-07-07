using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueAngelProjectile : MonoBehaviour
{
    public float speed;

    public Vector3 endPoint;

    public float damage;

    private bool hitPlayer;

    public float secondsBeforeTargeting;

    public Transform firerPos;

    void Awake()
    {
        firerPos = GameObject.Find("True Angel").transform.GetChild(0);
        if (firerPos == null)
        {
            firerPos = GlobalVars.playerTransform;
        }
    }

    private void Start()
    {        
        
        //var playerMoveSpeed = GlobalVars.playerController.moveSpeed;
        //Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal") * playerMoveSpeed*0.8f, 0, Input.GetAxisRaw("Vertical") * playerMoveSpeed*0.8f);


        /*
        transform.LookAt(lookAtPos + (new Vector3
        ( 
           -128 + GlobalVars.RandNumTable(),
           -128 + GlobalVars.RandNumTable(),
           -128 + GlobalVars.RandNumTable()
        ) /100));
        */
        Vector3 leftOrRightPos = transform.forward * 25;
        if (GlobalVars.RandTrueOrFalse())
        {
            leftOrRightPos = -leftOrRightPos;
        }

        transform.LookAt(transform.position + (Vector3.up*50) + (-leftOrRightPos) + RandomOffset());
        StartCoroutine(WaitBeforeTargeting());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (OutOfBounds())
        {
            Destroy(gameObject);
        }

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
            gameObject.GetComponent<Collider>().enabled = false;
        }
        
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Land"))
        {
            Destroy(gameObject);
        }
    }
    */

    IEnumerator WaitBeforeTargeting()
    {
        yield return new WaitForSecondsRealtime(secondsBeforeTargeting);
        Vector3 playerPos = GlobalVars.mainPlayer.transform.position;

        Vector3 playerVel = GlobalVars.playerRb.velocity;
        Vector3 lookAtPos = playerPos +  playerVel;
        speed *= 1.8f;
        transform.LookAt(lookAtPos + RandomOffset());
        yield return new WaitForSecondsRealtime(3);
        playerPos = GlobalVars.mainPlayer.transform.position;

        playerVel = GlobalVars.playerRb.velocity;
        lookAtPos = playerPos +  playerVel;
        //speed *= 1.8f;
        transform.LookAt(lookAtPos + RandomOffset());
        
    }

    void HurtPlayer(Collider other)
    {
        other.GetComponent<PlayerState>().health -= damage;
        Destroy(gameObject);
    }

    Vector3 RandomOffset()
    {
        return new Vector3
               (
                   -128 + GlobalVars.RandNumTable(),
                   -128 + GlobalVars.RandNumTable(),
                   -128 + GlobalVars.RandNumTable()
               ) / 100;
    }

    bool OutOfBounds()
    {
        if (firerPos != null)
        {
            return Vector3.Distance(transform.position, firerPos.position) > 400;
        }
        return Vector3.Distance(transform.position, GlobalVars.playerTransform.position) > 400;
    }
}

