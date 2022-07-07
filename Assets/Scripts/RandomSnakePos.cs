using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSnakePos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalVars.RandTrueOrFalse())
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        RandomSnakePos thisScript = this;
        Destroy(thisScript);
    }
}
