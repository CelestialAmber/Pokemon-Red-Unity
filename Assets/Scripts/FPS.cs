using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FPS : MonoBehaviour
{
    public Text fpstext;
    float deltaTime = 0.0f;
    int lowFps, highFps;
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        int w = Screen.width, h = Screen.height;
 int fps = Mathf.RoundToInt(1.0f / deltaTime);
        if ( fps - lowFps >  50) lowFps = fps;
        if (fps > highFps) highFps = fps;
        if (fps < lowFps) lowFps = fps;
        fpstext.text =  fps  + " FPS";

    }
}