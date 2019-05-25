using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameDataManager : MonoBehaviour {
    public GameObject[] gameScenes;
    public static GameDataManager instance;
    public RenderTexture mainRender,postRender;
    [HideInInspector]
    public RenderTexture templateRenderTexture;
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
    public FontAtlas fontAtlas;
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
        //VersionManager executes before GameDataManager, so the version is set in GameData beforehand
        VersionInit();
        GameData.Init();
        GameData.Save();
        GameData.money = 3000;
        GameData.coins = 300;
        GameData.screenTileHeight = 9;
        
        
        GameData.screenTileWidth = 10;
        mainRender = new RenderTexture(160, 144, 1);
        mainRender.filterMode = FilterMode.Point;
        postRender = new RenderTexture(160, 144, 1);
        postRender.filterMode = FilterMode.Point;
        templateRenderTexture = new RenderTexture(mainRender);
        

        Camera.main.targetTexture = mainRender;

    }
    public void VersionInit(){ //function to initialize everything that changes based on the version
        slots.Init();
        title.InitVersion();
        introHandler.InitVersion();
        beginHandler.InitVersion();
        PokemonDataJSON.InitVersion();
        FontAtlasInit();
    }
    public void BootGame(Version version){
        VersionManager.instance.version = version;
        GameData.version = version;
        VersionInit();
        introHandler.gameObject.SetActive(true);
        introHandler.Init();
    }
    public void ResetGame(){
        introHandler.ResetGame();
    }
	// Use this for initialization
	private void FontAtlasInit()
    {
        GameData.fontAtlas = fontAtlas;
        if (GameData.version == Version.Blue)
        {
            for (int i = 0; i < 6; i++)
            {
               GameData.fontAtlas.fontChars[i + 92] = GameData.fontAtlas.blueSlotsChars[i];
            }
        }
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
