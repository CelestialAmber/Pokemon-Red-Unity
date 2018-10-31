using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FPS : MonoBehaviour
{
    public Text fpstext;
    float deltaTime = 0.0f;
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
 int fps = Mathf.RoundToInt(1.0f / deltaTime);
        fpstext.text =  fps  + " FPS";

    }
}