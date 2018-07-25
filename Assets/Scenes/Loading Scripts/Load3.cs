using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load3 : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(WaitAndLoad(68f, "pok3"));
    }

    private IEnumerator WaitAndLoad(float value, string scene)
    {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene("pok3");
        //Application.LoadLevel(scene);
    }
}
