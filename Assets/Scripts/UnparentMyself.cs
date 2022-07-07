using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentMyself : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.parent = null;
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
