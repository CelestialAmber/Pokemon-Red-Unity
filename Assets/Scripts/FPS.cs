using UnityEngine;
using System.Collections;
using TMPro;
public class FPS : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    float deltaTime = 0.0f;
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
 int fps = Mathf.RoundToInt(1.0f / deltaTime);
        fpsText.text =  fps  + " FPS";

    }
}