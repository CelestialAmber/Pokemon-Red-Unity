using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public enum Map
{
    PalletTown,
    Route1
}

//Script to manage the world status.
public class MapManager : MonoBehaviour {
    public Player player;
    public GameObject mainmap;
    public MeshRenderer mainLayer, grassLayer, objectLayer;
    public SpriteAtlas tileAtlas;
    public Mesh mesh;
    public Material tileMat;
    public Vector2 atlasSize;
    public Vector2[] mainUv, grassUv, objectUv;
    public MeshFilter mainMesh, grassMesh, objectMesh;
    public static GridTile[,] maptiles = new GridTile[GameConstants.mapWidth, GameConstants.mapHeight];
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
        for (int y = 0; y < 11; y++)
        {
            for (int x = 0; x < 12; x++)
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
        objectMesh.mesh = mesh;
    }
    private void Awake()
    {
            tileMat.mainTexture = tileAtlas.GetSprite("tile7").texture;
            atlasSize = new Vector2(tileMat.mainTexture.width, tileMat.mainTexture.height);
            maptiles = new GridTile[GameConstants.mapWidth, GameConstants.mapHeight];
            GenerateMeshes();
        LoadMapData();
        GenerateUvs();
        SetUpUvShop();
    }
    void SetUpUvShop(){
        UvShop.Add("itemPokeball", GetUvsFromAtlas("itemPokeball"));
        UvShop.Add("cutTree", GetUvsFromAtlas("tile24"));
        UvShop.Add("transparent", new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
    }
    Vector2[] GetUvsFromAtlas(string atlasSpriteName){
        Rect rect = tileAtlas.GetSprite(atlasSpriteName).textureRect;
        return new Vector2[] { new Vector2(rect.xMin / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMin / atlasSize.y), new Vector2(rect.xMax / atlasSize.x, rect.yMax / atlasSize.y), new Vector2(rect.xMin / atlasSize.x, rect.yMax / atlasSize.y) };
    }
     void AnimateTiles()
    {
        for (int y = 0; y < 11; y++)
        {
            for (int x = 0; x < 12; x++)
            {
                GridTile tileToUse = maptiles[mod(Mathf.RoundToInt(centerPos.x  + (x - 5)) , GameConstants.mapWidth), mod(Mathf.RoundToInt(centerPos.y  + (y - 5)) , GameConstants.mapHeight)];
                if ( tileToUse != null)
                {
                    //Load the main layer.
                    GridTile loadedtile = tileToUse;
                    if (loadedtile.isAnimated)
                    {
                        
                        int frame = Mathf.FloorToInt(loadedtile.frames * (tileanimtimer / 2));
                        Vector2[] copyUvs = loadedtile.mainUvs[frame];
                        copyUvs.CopyTo(mainUv, 4 * (y * 12 + x));
                    }

                }


            }
        }
    }
    List<Vector2> mainUvs = new List<Vector2>(2112);
    List<Vector2> grassUvs = new List<Vector2>(2112);
    List<Vector2> objectUvs = new List<Vector2>(2112);
    public void LoadMap(){
        mainUvs.Clear();
        grassUvs.Clear();
        objectUvs.Clear();
        Vector2[] transparentUvs = UvShop["transparent"];
        Vector2[] itemPokeballUvs = UvShop["itemPokeball"];
        for (int y = 0; y < 11; y++)
        {
            for (int x = 0; x < 12; x++)
            {

                GridTile tileToUse = maptiles[mod(Mathf.RoundToInt(player.transform.position.x + (x - 5)) , GameConstants.mapWidth), mod(Mathf.RoundToInt(player.transform.position.y + (y - 5)) , GameConstants.mapHeight)];
               
                if (tileToUse != null)
                {
                    //Load the main layer.
                    GridTile loadedtile = tileToUse;
                    if (loadedtile.isAnimated)
                    {
                       int frame =  Mathf.FloorToInt(loadedtile.frames * (tileanimtimer / 2));
                        for (int i = 0; i < 4; i++){
                            mainUvs.Add(loadedtile.mainUvs[frame][i]);
                            grassUvs.Add(transparentUvs[i]);
                            objectUvs.Add(transparentUvs[i]);
                        }
                    }
                    else
                    {
                        
                        if(loadedtile.hasItemBall){


                            for (int i = 0; i < 4; i++)
                            {
                                objectUvs.Add(itemPokeballUvs[i]);
                            }
                        }else{
                            for (int i = 0; i < 4; i++)
                            {
                                objectUvs.Add(transparentUvs[i]);
                            }
                        }
                        if (loadedtile.hasGrass)
                        { //Does the tile have grass?
                          //yes

                            for (int i = 0; i < 4; i++)
                            {
                                mainUvs.Add(transparentUvs[i]);
                                grassUvs.Add(loadedtile.mainUvs[0][i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                mainUvs.Add(loadedtile.mainUvs[0][i]);
                                grassUvs.Add(transparentUvs[i]);
                            }
                        }
                    }

                }
                else {
                    for (int i = 0; i < 4; i++)
                    {
                        mainUvs.Add(transparentUvs[i]);
                        grassUvs.Add(transparentUvs[i]);
                        objectUvs.Add(transparentUvs[i]);
                    }
                }


            }
        }
        mainLayer.transform.position = player.transform.position - new Vector3(5.5f, 5.5f, -1);
        grassLayer.transform.position = player.transform.position - new Vector3(5.5f, 5.5f, -0.33f);
        objectLayer.transform.position = player.transform.position - new Vector3(5.5f, 5.25f, -0.75f);
        mainUv = mainUvs.ToArray();
        grassUv = grassUvs.ToArray();
        objectUv = objectUvs.ToArray();
        centerPos = player.transform.position;
    }
    void LoadMapData(){
        maptiles = Serializer.Load2D<GridTile>(Application.streamingAssetsPath + "/map.txt");
    }
    // Use this for initialization
    void Start () {
        mainmap.SetActive(false);
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
            objectMesh.mesh.uv = objectUv;
            //Update the animated tile timer.
            tileanimtimer += Time.deltaTime;
            if (tileanimtimer >= 2) tileanimtimer = 0;


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
    public bool hasTree;
    public bool isTreeCut;
    public bool hasItem;
    public bool hasItemBall;
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
