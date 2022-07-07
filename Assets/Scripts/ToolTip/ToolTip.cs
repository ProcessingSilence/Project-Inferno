using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    protected TextMeshProUGUI text;

    protected Color textColor;

    public float showingTime;

    public float fadeAmount;

    public bool dontDestroy;
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if (fadeAmount == 0)
        {
            fadeAmount = 0.1f;
        }

        text = GetComponent<TextMeshProUGUI>();
        textColor = text.color;
    }

    protected IEnumerator Fade(float fadeAmount)
    {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("FADE");
            yield return new WaitForSecondsRealtime(0.1f);
            text.color += new Color(0, 0, 0, fadeAmount);
        }
    }
    
    public IEnumerator WaitBeforeFade()
    {
        if (dontDestroy == false)
        {
            yield return new WaitForSecondsRealtime(showingTime);
            StartCoroutine(Fade(-fadeAmount));
        }
    }

    public IEnumerator WaitBeforeDestroy()
    {
        if (dontDestroy == false)
        {
            yield return new WaitForSecondsRealtime(showingTime + 1f);
            Destroy(gameObject);
        }
    }
}
