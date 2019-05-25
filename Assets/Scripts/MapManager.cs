using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public enum Map
{
    PalletTown,
    OakLab,
    Route1,
    ViridianCity,
    PokeCenter,
    PokeMart,
    PokemonGym,
    Route22,
    Route23,
    VictoryRoad1,
    VictoryRoad2,
    VictoryRoad3,
    IndigoPlateau,
    Lorelei,
    Bruno,
    Agatha,
    HallofFameRoom,
    Route2,
    ViridianForest,
    DiglettCave,
    PewterCity,
    Route3,
    MtMoon1,
    MtMoon2,
    MtMoon3,
    Route4,
    CeruleanCity,
    Route24,
    Route25,
    Route5,
    UndergroundRoad,
    Route6,
    VermillionCity,
    SSAnne,
    Route11,
    Route9,
    RockTunnel1,
    RockTunnel2,
    PowerPlant,
    LavenderTown,
    Route7,
    PokemonTower1,
    PokemonTower2,
    PokemonTower3,
    PokemonTower4,
    PokemonTower5,
    PokemonTower6,
    PokemonTower7,
    CeladonCity,
    GameCorner,
    RocketHideout,
    Route16,
    Route17,
    Route18,
    FuchsiaCity,
    SafariZoneCenter,
    SafariZoneEast,
    SafariZoneNorth,
    SafariZoneWest,
    SafariZoneHouse,
    Route15,
    Route14,
    Route13,
    Route12,
    SaffronCity,
    SilphCo,
    Route19,
    SeafoamIslands1,
    SeafoamIslands2,
    SeafoamIslands3,
    SeafoamIslands4,
    SeafoamIslands5,
    Route20,
    CinnabarIsland,
    Mansion1,
    Mansion2,
    Mansion3,
    Mansion4,
    Route21,
    Unknown1,
    Unknown2,
    Unknown3,
    TradeCenter,
    Colloseum,
    BillsHouse,
    House //general entry for houses. 
}

//Script to manage the world status.
public class MapManager : MonoBehaviour{
    public Player player;
    public int currentGrassEcounterTable, currentWaterEncounterTable;
    public static GridTile[,] maptiles = new GridTile[GameData.mapWidth, GameData.mapHeight];
    //The map the player is currently in.
    public Map currentMap;

    int mod(int a, int b)
    {
        return a < 0 ? b + a % b : a % b;
    }

    private void Awake()
    {

            maptiles = new GridTile[GameData.mapWidth, GameData.mapHeight];
        LoadMapData();
    }

  
    void LoadMapData(){
        maptiles = Serializer.JSONtoObject<GridTile[,]>("map.json");
    }
    // Use this for initialization
    void Start () {
       
	}
	
  

}

[System.Serializable]
public class GridTile
{

    public TilesData tiledata;
    public WarpInfo tileWarp;
    public EncounterInfo encounterInfo;
    public bool isAnimated;
    public int posx, posy;
    public string tag;
    public bool isWarp;
    public bool hasGrass;
    public bool isWater;
    public bool isWall;
    public bool isInteractable;
    public bool hasItem;
    public bool hasPokemon;
    public int frames;
    public string mainSprite;
    public List<Vector2[]> mainUvs = new List<Vector2[]>();
    public GridTile(TilesData tiledata, WarpInfo tileWarp, EncounterInfo encounterInfo, bool isAnimated, int posx, int posy, string tag, bool isWarp, bool hasGrass, bool isWall, int frames, string mainSprite, bool isWater,bool isInteractable)
    {
        this.isInteractable = isInteractable;
        this.tiledata = tiledata;
        this.tileWarp = tileWarp;
        this.encounterInfo = encounterInfo;
        this.posx = posx;
        this.posy = posy;
        this.tag = tag;
        this.isAnimated = isAnimated;
        this.isWarp = isWarp;
        this.hasGrass = hasGrass;
        this.isWall = isWall;
        this.frames = frames;
        this.mainSprite = mainSprite;
        this.isWater = isWater;

    }

}
