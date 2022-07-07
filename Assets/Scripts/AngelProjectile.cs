using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelProjectile : MonoBehaviour
{
    public float speed;

    public Vector3 endPoint;

    public float damage;

    private bool hitPlayer;

    protected bool destroyOnLand = true;
    protected virtual void Start()
    {
        Vector3 playerPos = GlobalVars.mainPlayer.transform.position;
        Vector3 playerVel = GlobalVars.playerRb.velocity;
        
        //var playerMoveSpeed = GlobalVars.playerController.moveSpeed;
        //Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal") * playerMoveSpeed*0.8f, 0, Input.GetAxisRaw("Vertical") * playerMoveSpeed*0.8f);

        Vector3 lookAtPos = playerPos  /*+playerVel*/;
        transform.LookAt(lookAtPos + (new Vector3
        ( 
           -128 + GlobalVars.RandNumTable(),
           -128 + GlobalVars.RandNumTable(),
           -128 + GlobalVars.RandNumTable()
        ) /100));
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
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

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Land"))
        {
            if (destroyOnLand)
            {
                Destroy(gameObject);
            }
        }
    }

    protected void HurtPlayer(Collider other)
    {
        other.GetComponent<PlayerState>().health -= damage;
        Destroy(gameObject);
    }
}

