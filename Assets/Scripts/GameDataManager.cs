using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : Singleton<GameDataManager> {
    public GameObject[] gameScenes;
    public RenderTexture mainRender,postRender;
    [HideInInspector]
    public RenderTexture templateRenderTexture;
    public float ms;
    public Slots slots;
    public PokemonMenu pokemonMenu;
    public Options options;
    public Title title;
    public IntroHandler introHandler;
    public OakIntroCutsceneHandler oakIntroCutsceneHandler;
    public Pokedex pokedex;
    public FontAtlas fontAtlas;
    public PC pc;
    public bool startInGame;
    
    private void Awake(){
        /*
        For some reason, starting with Unity 2019.4 this is needed to make the game
        run in the editor at the right speed, even though it's still 60fps in a build -_-
        */
        Application.targetFrameRate = 60;
        PokemonMenu.instance = pokemonMenu;
        Options.instance = options;
        Title.instance = title;
        OakIntroCutsceneHandler.instance = oakIntroCutsceneHandler;
        Pokedex.instance = pokedex;
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
        oakIntroCutsceneHandler.InitVersion();
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

	private void FontAtlasInit(){
        GameData.instance.fontAtlas = fontAtlas;
        if(GameData.instance.version == Version.Blue){
            for(int i = 0; i < 6; i++){
               GameData.instance.fontAtlas.fontChars[i + 92] = GameData.instance.fontAtlas.blueSlotsChars[i];
            }
        }
        else{
            for(int i = 0; i < 6; i++){
               GameData.instance.fontAtlas.fontChars[i + 92] = GameData.instance.fontAtlas.redSlotsChars[i];
            }
        }
    }
	
	// Update is called once per frame
	void Update(){
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
