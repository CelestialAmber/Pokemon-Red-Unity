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
    public GameObject[] gameScenes;
    public static GameDataManager instance;
    [HideInInspector]
    public RenderTexture mainRender,postRender,pass1,pass2;
    public RectTransform renderRect;
    public float ms;
    public Slots slots;
    public PokemonMenu pokemonMenu;
    public Bag bag;
    public Options options;
    public CreditsHandler creditsHandler;
    public Title title;
    public IntroHandler introHandler;
    public BeginHandler beginHandler;
    public MainMenu mainMenu;
    public Pokedex pokedex;
    private void Awake(){
        instance = this;

        PokemonMenu.instance = pokemonMenu;
        Bag.instance = bag;
        Options.instance = options;
        Title.instance = title;
        MainMenu.instance = mainMenu;
        CreditsHandler.instance = creditsHandler;
        BeginHandler.instance = beginHandler;
        Pokedex.instance = pokedex;
        slots.Init();
        title.InitVersion();
        introHandler.InitVersion();
        beginHandler.InitVersion();
        GameData.Init();
        GameData.Save();
        GameData.money = 3000;
        GameData.coins = 300;
        GameData.screenTileHeight = 9;
         foreach(GameObject scene in gameScenes){
            scene.SetActive(false);
        }
        switch(currentScene){
            case GameScene.NintendoSwitch:
                gameScenes[1].SetActive(true);
                break;
            case GameScene.SGB:
                gameScenes[0].SetActive(true);
                break;    
        }
        
        
        GameData.screenTileWidth = 10;
        mainRender = new RenderTexture(160, 144, 1);
        mainRender.filterMode = FilterMode.Point;
        postRender = new RenderTexture(160, 144, 1);
        postRender.filterMode = FilterMode.Point;
        pass1 = new RenderTexture(160, 144, 1);
        pass1.filterMode = FilterMode.Point;
        pass2 = new RenderTexture(160, 144, 1);
        pass2.filterMode = FilterMode.Point;
            renderRect.sizeDelta = new Vector2(1200, renderRect.sizeDelta.y);
        

        Camera.main.targetTexture = mainRender;

    }
    public void BootGame(Version version){
        VersionManager.instance.version = version;
        introHandler.gameObject.SetActive(true);
        introHandler.Init();
    }
    public void ResetGame(){
        introHandler.ResetGame();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameData.inGame) //are we loaded into the game?
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
