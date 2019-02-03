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
    public Image frame, nintendoSwitchFrame;
    public Sprite[] frames;
    public GameObject sgbCanvas;
    public Camera renderCamera;
    public static VersionManager instance;
    public void Awake(){
        instance = this;
    }
    private void Start()
    {
    }
    // Update is called once per frame
    void Update () {
        if (GameDataManager.instance.currentScene == GameScene.SGB)
            frame.sprite = frames[(int)version];
        else if (GameDataManager.instance.currentScene == GameScene.NintendoSwitch)
            nintendoSwitchFrame.sprite = frames[(int)version];
       
	}

}
