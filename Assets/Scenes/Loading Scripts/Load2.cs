using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load2 : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(WaitAndLoad(43f, "Load3"));
    }

    private IEnumerator WaitAndLoad(float value, string scene)
    {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene("Load3");
        //Application.LoadLevel(scene);
    }
}
