using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{

    public AudioClip mySound;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        StartCoroutine(MainUpdate());
    }
    IEnumerator MainUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
            yield return StartCoroutine(Example());
            Application.LoadLevel("Load3");
        }
    }

    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
        print(Time.time);
    }

}
