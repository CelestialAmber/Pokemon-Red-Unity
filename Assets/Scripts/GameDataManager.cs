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
    public Player player;
    public PC pc;

    public bool startInGame;
    private void Awake(){
        instance = this;
        Player.instance = player;
        PokemonMenu.instance = pokemonMenu;
        Bag.instance = bag;
        Options.instance = options;
        Title.instance = title;
        MainMenu.instance = mainMenu;
        CreditsHandler.instance = creditsHandler;
        BeginHandler.instance = beginHandler;
        Pokedex.instance = pokedex;
        PC.instance = pc;
        //VersionManager executes before GameDataManager, so the version is set in GameData beforehand
        VersionInit();
        GameData.instance.Init();
        GameData.instance.Save();
        GameData.instance.money = 3000;
        GameData.instance.coins = 300;
        postRender = new RenderTexture(160, 144, 1);
        postRender.filterMode = FilterMode.Point;
        templateRenderTexture = new RenderTexture(mainRender);
        GameData.instance.inGame = startInGame;

        Camera.main.targetTexture = mainRender;
        if(!startInGame) BootGame(GameData.instance.version);

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
        GameData.instance.version = version;
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
        GameData.instance.fontAtlas = fontAtlas;
        if (GameData.instance.version == Version.Blue)
        {
            for (int i = 0; i < 6; i++)
            {
               GameData.instance.fontAtlas.fontChars[i + 92] = GameData.instance.fontAtlas.blueSlotsChars[i];
            }
        }
        else {
             for (int i = 0; i < 6; i++)
            {
               GameData.instance.fontAtlas.fontChars[i + 92] = GameData.instance.fontAtlas.redSlotsChars[i];
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (GameData.instance.inGame) //are we loaded into the game?
        {
            ms += Time.deltaTime;
            if (ms >= 1f)
            {
                GameData.instance.seconds += 1;
                ms = 0;
            }
            if (GameData.instance.seconds == 60) {
                GameData.instance.minutes += 1;
                GameData.instance.seconds = 0;
            }
            if(GameData.instance.minutes == 60){
                GameData.instance.hours += 1;
                GameData.instance.minutes = 0;
            }
            if(GameData.instance.hours > 999){
                GameData.instance.hours = 999;
            }
        }
	}

}
