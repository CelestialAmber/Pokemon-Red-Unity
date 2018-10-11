using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameScene{
    SGB,
    NintendoSwitch
}
public class GameDataManager : MonoBehaviour {
    public GameScene currentScene;
    public GameObject[] gameScenes;
    public GameObject switchGameScreen, switchHomeScreen,switchGameControlsScreen,switchStartupScreen;
    public static GameDataManager Instance;
    JoyconManager joyconManager;
    void Awake(){
        Instance = this;
        foreach(GameObject scene in gameScenes){
            scene.SetActive(false);
        }
        joyconManager = JoyconManager.Instance;
        if(joyconManager.j.Count == 2){
            Debug.Log("Both joycons were detected, setting scene to the Nintendo Switch scene");
            currentScene = GameScene.NintendoSwitch;
        }
        switch(currentScene){
            case GameScene.NintendoSwitch:
                gameScenes[1].SetActive(true);
                switchGameScreen.SetActive(true);
                break;
            case GameScene.SGB:
                gameScenes[0].SetActive(true);
                break;    
        }
        Inputs.Init();
        Get.Init();
        GameData.Init();
        GameData.money = 3000;
        GameData.coins = 300;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
