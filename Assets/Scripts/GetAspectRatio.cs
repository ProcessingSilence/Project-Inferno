using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAspectRatio : MonoBehaviour
{
    public RenderTexture lowResolutionOverlay;

    public int[] aspectRatio;

    private int resolutionX, resolutionY;

    private Vector2 original_RT_Resolution = new Vector2(480, 270);
    // Start is called before the first frame update
    void Start()
    {
        var foundTexture = false;
        lowResolutionOverlay = Resources.Load<RenderTexture>("LowResRenderTexture");
        resolutionX = Screen.width;
        resolutionY = Screen.height;
        var gcdResult = GCD(resolutionX, resolutionY);
        aspectRatio = new int[2]{resolutionX/gcdResult, resolutionY/gcdResult};
        lowResolutionOverlay.width = aspectRatio[0] * 30;
        lowResolutionOverlay.height = aspectRatio[1] * 30;
    }

    // Source: https://stackoverflow.com/questions/18541832/c-sharp-find-the-greatest-common-divisor
    // Gets and returns the greatest common divisor of a number.
    private static int GCD(int a, int b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }
}
