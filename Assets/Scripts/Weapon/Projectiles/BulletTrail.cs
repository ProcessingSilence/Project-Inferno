using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public bool destroySelf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroySelf)
        {
            destroySelf = false;
            StartCoroutine(WaitBeforeDestroy());
        }
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSecondsRealtime(4f);
        Destroy(gameObject);
    }
}
