using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shows tooltip when requiredObjs list is empty.
public class ShowOnDestroyTool : ToolTip
{
    public List<GameObject> requiredObjs;

    public GameObject checkObj;

    public int objAmount;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        text.color = new Color(textColor.r, textColor.g, textColor.b, 0);
        for (int i = 0; i < requiredObjs.Count; i++)
        {
            var script = requiredObjs[i].AddComponent<CheckIfDestroyed>();
            script.myCheckingScript = this;
        }

        objAmount = requiredObjs.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (objAmount <= 0)
        {
            objAmount = 9;
            StartCoroutine(Fade(fadeAmount));
            StartCoroutine(WaitBeforeFade());
            StartCoroutine(WaitBeforeDestroy());
        }
    }
}

public class CheckIfDestroyed : MonoBehaviour
{
    public ShowOnDestroyTool myCheckingScript;    

    private void OnDestroy()
    {
        myCheckingScript.objAmount -= 1;
    }
}
