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
    GameDataManager gameDataManager;
    private void Start()
    {
        gameDataManager = GameDataManager.Instance;    
    }
    // Update is called once per frame
    void Update () {
        if (gameDataManager.currentScene == GameScene.SGB)
            frame.sprite = frames[(int)version];
        else if (gameDataManager.currentScene == GameScene.NintendoSwitch)
            nintendoSwitchFrame.sprite = frames[(int)version];
       
	}

}
