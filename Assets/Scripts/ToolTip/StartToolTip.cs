using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartToolTip : ToolTip
{
    public float showDelay;
    // Start is called before the first frame update
    void Start()
    {
        text.color = new Color(textColor.r, textColor.g, textColor.b, 0);
        StartCoroutine(ShowDelay());
    }

    IEnumerator ShowDelay()
    {
         yield return new WaitForSecondsRealtime(showDelay);
         StartCoroutine(Fade(fadeAmount));
         StartCoroutine(WaitBeforeFade());
    }
}
