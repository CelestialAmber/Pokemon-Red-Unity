using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


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
    BillsHouse
}


//Script to manage the world status.
public class MapManager : MonoBehaviour {
    public Player player;
    public MeshRenderer mainLayer, grassLayer,  grassBGLayer;
    public SpriteAtlas tileAtlas;
    public Mesh mesh;
    public Material templateMat;
    private Material tileMat;
    public Vector2 atlasSize;
    public Vector2[] mainUv, grassUv,  grassBGUv;
    public MeshFilter mainMesh, grassMesh, grassBGMesh;
    public int currentGrassEcounterTable, currentWaterEncounterTable;
    public static GridTile[,] maptiles = new GridTile[GameData.mapWidth, GameData.mapHeight];
    //The map the player is currently in.
    public Map currentMap;
    public Vector3 centerPos;
    public float tileanimtimer;

    int mod(int a, int b)
    {
        return a < 0 ? b + a % b : a % b;
    }

    public Dictionary<string, Vector2[]> UvShop = new Dictionary<string, Vector2[]>();
    void GenerateUvs()
    {
        Rect rect;
        foreach (GridTile tile in maptiles)
        {
            if (tile != null)
            {
                Sprite tileSprite;
                //Generate the main layer uvs.
                if (tile.isAnimated)
                {
                    for (int i = 0; i < tile.frames; i++)
                    {
                        tileSprite = tileAtlas.GetSprite(tile.mainSprite + "_" + i);
                        rect = new Rect(tileSprite.textureRect.position, tileSprite.textureRect.size);
                        tile.mainUvs.Add(new Vector2[] { new Vector2(rect.xMin / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMax / atlasSize.y), new Vector2(rect.xMin / atlasSize.x, rect.yMax / atlasSize.y) });
                    }
                }
                else
                {
                    if (tile.hasGrass)
                    {
                        tileSprite = tileAtlas.GetSprite(tile.mainSprite);
                        rect = new Rect(tileSprite.textureRect.position, tileSprite.textureRect.size);
                        tile.mainUvs.Add(new Vector2[] { new Vector2(rect.xMin / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMax / atlasSize.y), new Vector2(rect.xMin / atlasSize.x, rect.yMax / atlasSize.y) });


                    }
                    else
                    {
                        tileSprite = tileAtlas.GetSprite(tile.mainSprite);
                        rect = new Rect(tileSprite.textureRect.position, tileSprite.textureRect.size);
                        tile.mainUvs.Add(new Vector2[] { new Vector2(rect.xMin / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMax / atlasSize.y), new Vector2(rect.xMin / atlasSize.x, rect.yMax / atlasSize.y) });
                    }
                }

            }
        }
    }
    void GenerateMeshes()
    {
        mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        for (int y = 0; y < GameData.screenTileHeight + 2; y++)
        {
            for (int x = 0; x < GameData.screenTileWidth + 2; x++)
            {
                int offset = verts.Count;
                verts.AddRange(new Vector3[] { new Vector3(x, y), new Vector3(x + 1, y), new Vector3(x + 1, y + 1), new Vector3(x, y + 1) });

                tris.AddRange(new int[] { 0 + offset, 2 + offset, 1 + offset, 0 + offset, 3 + offset, 2 + offset });
            }
        }
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
        mainMesh.mesh = mesh;
        grassMesh.mesh = mesh;
           grassBGMesh.mesh = mesh;
    }
    private void Awake()
    {
        tileMat = new Material(templateMat);
        mainLayer.material = tileMat;   
        grassLayer.material = tileMat;
        grassBGLayer.material = tileMat;
            tileMat.mainTexture = tileAtlas.GetSprite("tile7").texture;
            atlasSize = new Vector2(tileMat.mainTexture.width, tileMat.mainTexture.height);
            maptiles = new GridTile[GameData.mapWidth, GameData.mapHeight];
            GenerateMeshes();
         
          
        LoadMapData();
        GenerateUvs();
        SetUpUvShop();
    }
    void SetUpUvShop(){
        UvShop.Add("transparent", new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
        UvShop.Add("white", GetUvsFromAtlas("tile1"));
    }
    Vector2[] GetUvsFromAtlas(string atlasSpriteName){
        Rect rect = tileAtlas.GetSprite(atlasSpriteName).textureRect;
        return new Vector2[] { new Vector2(rect.xMin / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMax / atlasSize.y), new Vector2(rect.xMin / atlasSize.x, rect.yMax / atlasSize.y) };
    }
     void AnimateTiles()
    {
        for (int y = 0; y < GameData.screenTileHeight + 2; y++)
        {
            for (int x = 0; x < GameData.screenTileWidth + 2; x++)
            {
                GridTile tileToUse = maptiles[mod(Mathf.RoundToInt(centerPos.x  + (x - GameData.screenTileWidth/2)) , GameData.mapWidth), mod(Mathf.RoundToInt(centerPos.y  + (y - GameData.screenTileHeight / 2 - 1)) , GameData.mapHeight)];
                if ( tileToUse != null)
                {
                    //Load the main layer.
                    GridTile loadedtile = tileToUse;
                    if (loadedtile.isAnimated)
                    {
                        
                        int frame;

                     
                        if(loadedtile.tag == "Water") frame =  Mathf.FloorToInt(loadedtile.frames * (tileanimtimer / 2.8f));
                        else   frame = Mathf.FloorToInt(loadedtile.frames * (tileanimtimer % 1.4f / 1.4f));
                        
                        Vector2[] copyUvs = loadedtile.mainUvs[frame];
                        copyUvs.CopyTo(mainUv, 4 * (y * (GameData.screenTileWidth + 2) + x));
                    }

                }


            }
        }
    }
    List<Vector2> mainUvs = new List<Vector2>(); 
    List<Vector2> grassUvs  = new List<Vector2>();
    List<Vector2> grassBGUvs = new List<Vector2>();
    public void LoadMap(){
        mainUvs.Clear();
        grassUvs.Clear();
        grassBGUvs.Clear();
        Vector2[] transparentUvs = UvShop["transparent"];
        Vector2[] whiteTileUvs = UvShop["white"];
        for (int y = 0; y < GameData.screenTileHeight + 2; y++)
        {
            for (int x = 0; x < GameData.screenTileWidth + 2; x++)
            {

                GridTile tileToUse = maptiles[mod(Mathf.RoundToInt(player.transform.position.x + (x - GameData.screenTileWidth / 2)) , GameData.mapWidth), mod(Mathf.RoundToInt(player.transform.position.y + (y - GameData.screenTileHeight / 2 - 1)) , GameData.mapHeight)];
               
                if (tileToUse != null)
                {
                    //Load the main layer.
                    GridTile loadedtile = tileToUse;
                    //is the tile an animated tile?
                    if (loadedtile.isAnimated)
                    {
                        int frame;

                     
                        if(loadedtile.tag == "Water") frame =  Mathf.FloorToInt(loadedtile.frames * (tileanimtimer / 2.8f));
                        else   frame = Mathf.FloorToInt(loadedtile.frames * (tileanimtimer % 1.4f / 1.4f));
                        for (int i = 0; i < 4; i++){
                            mainUvs.Add(loadedtile.mainUvs[frame][i]);
                            grassUvs.Add(transparentUvs[i]);
                            grassBGUvs.Add(transparentUvs[i]);
                        }
                    }
                    else
                    {
                          
                        
                        if (loadedtile.hasGrass)
                        { //Does the tile have grass?
                          //yes

                            for (int i = 0; i < 4; i++)
                            {
                                mainUvs.Add(transparentUvs[i]);
                                grassUvs.Add(loadedtile.mainUvs[0][i]);
                                grassBGUvs.Add(whiteTileUvs[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                mainUvs.Add(loadedtile.mainUvs[0][i]);
                                grassUvs.Add(transparentUvs[i]);
                                   grassBGUvs.Add(transparentUvs[i]);
                            }
                        }
                    }

                }
                else {
                    for (int i = 0; i < 4; i++)
                    {
                        mainUvs.Add(transparentUvs[i]);
                        grassUvs.Add(transparentUvs[i]);
                           grassBGUvs.Add(transparentUvs[i]);
                    }
                }


            }
        }
        //the layers are all organized from back to front
       grassBGLayer.transform.position = player.transform.position  - new Vector3(GameData.screenTileWidth / 2 + 0.5f, GameData.screenTileHeight / 2 + 1.5f, -0.03f);
        mainLayer.transform.position = player.transform.position - new Vector3(GameData.screenTileWidth / 2 + 0.5f, GameData.screenTileHeight / 2 + 1.5f , -0.02f);
         grassLayer.transform.position = player.transform.position  - new Vector3(GameData.screenTileWidth / 2 + 0.5f, GameData.screenTileHeight / 2 + 1.5f, 0f);
        mainUv = mainUvs.ToArray();
        grassUv = grassUvs.ToArray();
        grassBGUv = grassBGUvs.ToArray();
        centerPos = player.transform.position;
    }
    void LoadMapData(){
        maptiles = Serializer.Load2D<GridTile>(Application.streamingAssetsPath + "/map.txt");
    }
    // Use this for initialization
    void Start () {
       
        centerPos = transform.position;
        LoadMap();
        StartCoroutine(CoreUpdate());
	}
	
    IEnumerator CoreUpdate()
    {
        while (true)
        {

            StartCoroutine(MapUpdate());
            //Update the layer meshes' uv's.
            mainMesh.mesh.uv = mainUv;
            grassMesh.mesh.uv = grassUv;
            grassBGMesh.mesh.uv = grassBGUv;
            //Update the animated tile timer.
            tileanimtimer += Time.deltaTime;
            if (tileanimtimer >= 2.8f) tileanimtimer = 0;


            //Wait until the end of the frame to sync it with other updates;
            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator MapUpdate()
    {
        AnimateTiles();
        yield return 0;
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
    public bool hasPokedex;
    public int pokemonIconIndex; //0:bird,1:snorlax,2:fossil
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
