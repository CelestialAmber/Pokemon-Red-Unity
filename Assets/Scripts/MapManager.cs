using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    Route10,
    RockTunnel1,
    RockTunnel2,
    PowerPlant,
    LavenderTown,
    PokemonTower1,
    PokemonTower2,
    PokemonTower3,
    PokemonTower4,
    PokemonTower5,
    PokemonTower6,
    PokemonTower7,
    Route8,
    Route7,
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
    House, //general entry for houses. 
    VictoryRoadGate,
    IndigoPlateauLobby
}

//Script to manage the world status.
public class MapManager : MonoBehaviour{
    public Player player;
    public int currentGrassEcounterTable, currentWaterEncounterTable;


    
    //The map the player is currently in.
    public Map currentMap;



    public static MapManager instance;

    private void Awake()
    {
        instance = this;
    }
    public List<MapCollider> mapColliders = new List<MapCollider>();

    public TilemapRenderer currentMapGrassTilemap; //grass tilemap of the current map for the wild battle grass layer effect
  
   
    // Use this for initialization
    void Start () {
       
	}
	
      

  

}






[System.Serializable]
  public class MapTile{
          public MapTile(Vector3Int pos){
               this.pos = pos;
              pos -= new Vector3Int(1,1,0);
              foreach(MapCollider mapCollider in MapManager.instance.mapColliders){
              Tilemap overworldTilemap = mapCollider.overworldTilemap;
              Tilemap grassTilemap = mapCollider.grassTilemap;
              Tilemap waterTilemap = mapCollider.waterTilemap;
              Tilemap ledgeTilemap = mapCollider.ledgeTilemap;
              
             
              bool hasOverworldTile = overworldTilemap.HasTile(pos);
              hasGrass = grassTilemap.HasTile(pos);
              isWater = waterTilemap.HasTile(pos); 
              isLedge =  ledgeTilemap.HasTile(pos);
              hasTile = hasOverworldTile || isWater || isLedge;
              if(hasOverworldTile){
                  TileBase overworldTile = overworldTilemap.GetTile(pos);
                  if(overworldTile is AnimatedTile){
                      
                    
                      isWall = ((AnimatedTile)overworldTile).m_TileColliderType != Tile.ColliderType.None;
                  }
                  else if(overworldTile is Tile){
                      isWall = ((Tile)overworldTile).colliderType != Tile.ColliderType.None;
                  }
              }
              else isWall = false;
              if(hasOverworldTile){
                  TileBase tile = overworldTilemap.GetTile(pos);
                  if(tile is AnimatedTile) tileName = ((AnimatedTile)overworldTilemap.GetTile(pos)).name;
                   else if(tile is Tile) tileName = ((Tile)overworldTilemap.GetTile(pos)).name; 
              }
              if(isLedge) tileName = ((Tile)ledgeTilemap.GetTile(pos)).name;
              if(isWater){
                  TileBase tile = waterTilemap.GetTile(pos);
                  if(tile is AnimatedTile) tileName = ((AnimatedTile)waterTilemap.GetTile(pos)).name;
                   else if(tile is Tile) tileName = ((Tile)waterTilemap.GetTile(pos)).name; 
              }
              if(hasTile) break;
              }

          }
        public bool hasTile;
        public bool hasGrass;
        public bool isWater;
        public bool isWall;
        public bool isLedge;

        public Vector3Int pos;

        public string tileName;

    
}
