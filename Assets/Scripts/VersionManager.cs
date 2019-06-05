using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Version{
    Red,
    Blue
}
public class VersionManager : MonoBehaviour {
    public Version version;
    public Image frame;
    public Sprite[] frames;
    public GameObject sgbCanvas;
    public Camera renderCamera;
    public static VersionManager instance;
    public void Awake(){
        instance = this;
        GameData.instance.version = version;
    }
    private void Start()
    {
    }
    // Update is called once per frame
    void Update () {
        frame.sprite = frames[(int)version];
    }


}
