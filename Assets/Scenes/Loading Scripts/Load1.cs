using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load1 : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(WaitAndLoad(25f, "Load2"));
    }

    private IEnumerator WaitAndLoad(float value, string scene)
    {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene("Load2");
        //Application.LoadLevel(scene);
    }
}
