using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameScene{
    SGB,
    NintendoSwitch
}
public class GameDataManager : MonoBehaviour {
    public GameScene currentScene;
    public bool widescreen;
    public GameObject[] gameScenes;
    public GameObject switchGameScreen, switchHomeScreen,switchGameControlsScreen,switchStartupScreen;
    public static GameDataManager Instance;
    public static RenderTexture mainRender,postRender;
    public RectTransform renderRect;
    public float ms;
    public bool inGame;
    JoyconManager joyconManager;
    private void Awake(){
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
        GameData.screenTileHeight = 9;
        if (widescreen)
        {
            GameData.screenTileWidth = 16;
            mainRender = new RenderTexture(256, 144, 1);
            mainRender.filterMode = FilterMode.Point;
            postRender = new RenderTexture(256, 144, 1);
            postRender.filterMode = FilterMode.Point;
            renderRect.sizeDelta = new Vector2(1920, renderRect.sizeDelta.y);
        }
        else
        {
            GameData.screenTileWidth = 10;
            mainRender = new RenderTexture(160, 144, 1);
            mainRender.filterMode = FilterMode.Point;
            postRender = new RenderTexture(160, 144, 1);
            postRender.filterMode = FilterMode.Point;
            renderRect.sizeDelta = new Vector2(1200, renderRect.sizeDelta.y);
        }

        Camera.main.targetTexture = mainRender;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (inGame) //are we loaded into the game?
        {
            ms += Time.deltaTime;
            if (ms >= 1f)
            {
                GameData.seconds += 1;
                ms = 0;
            }
            if (GameData.seconds == 60) {
                GameData.minutes += 1;
                GameData.seconds = 0;
            }
            if(GameData.minutes == 60){
                GameData.hours += 1;
                GameData.minutes = 0;
            }
            if(GameData.hours > 999){
                GameData.hours = 999;
            }
        } 
	}
}
