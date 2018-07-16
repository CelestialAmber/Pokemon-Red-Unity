using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Net;
using UnityEngine;
using System.Threading;
#if (UNITY_EDITOR)
using UnityEditor;
#endif
using System.Runtime.Serialization.Formatters.Binary;
//Save/Loader for Tile Pool

public class Serializer
{
    //Should test WWW loading code be enabled, or default to file loading?
    public static bool wwwLoad = true;
   
    public static T[,] Load2D<T>(string filename) where T : class
    {
        

            try
            {
            if ((filename.Contains("://") || filename.Contains(":///")) && Serializer.wwwLoad)
            {
                using (WebClient client = new WebClient())
                {
                    byte[] s = client.DownloadData(filename);

                    using (Stream stream = new MemoryStream(s))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return formatter.Deserialize(stream) as T[,];
                    }
                }
            }else{

                using (Stream stream = File.OpenRead(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as T[,];
                }
            }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        return default(T[,]);
    }

    public static void Save2D<T>(string filename, T[,] data) where T : class
    {
        using (Stream stream = File.OpenWrite( filename))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
    }
    public static T[] Load<T>(string filename) where T : class
    {
        if (File.Exists(filename))
        {
            try
            {
 
                using (Stream stream = File.OpenRead(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as T[];
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        return default(T[]);
    }

    public static void Save<T>(string filename, T[] data) where T : class
    {
        using (Stream stream = File.OpenWrite(filename))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
    }
   
}
//Properties of tile, including collision, and others
[System.Serializable]
public class Tile {
    public int layer;
    public string sprite;
    public string tag;
    public bool isAnimated;
   
    public Tile(int l, string s, string t, bool isanim){
        layer = l;
        sprite = s;
        tag = t;
        isAnimated = isanim;
    }

}
[System.Serializable]
public class MapData{
    public int mapNum;
    public long headerAddr;
    public int bank;
    public int tilesetNumber;
    public int originalHeight, originalWidth;
    public int height, width;
    public int mapPointer;
    public int textPointer;
    public int scriptPointer;
    public int connection;
    public int[,] connectionData;
    public int objectDataPointer;
    public int[] tileMap;
 

}
[System.Serializable]
public class Tileset
{
    //Tilesets contain 32x32 blocks made up of 4 tiles, which are used to make the map
    public  Texture2D[] blocks = new Texture2D[256];
}

public class MapEditor : MonoBehaviour
{
    public GameObject container, grasscontainer;
    public int currentTileIndex = 0;
    public GameObject template;
    public GameObject currenttile;
    public GameObject hoveredtile;
    public List<Tile> tilepool = new List<Tile>();
    public string MapSaveName;
    public GridTile[,] savedtiles = new GridTile[800, 800];
    public int currentMap;
    public Tileset[] tilesets = new Tileset[24];
    public MapData[] maps = new MapData[248];
    public static Color[] colors = {
        new Color(1, 1, 1, 1),
    new Color(.5625f, .5625f, .5625f, 1),
    new Color(.25f, .25f, .25f, 1),
    new Color(0,0,0,1)
};

    int xiterations = 0, yiterations = 0;
    // Use this for initialization
    void Awake()
    {

            container.SetActive(false);
            grasscontainer.SetActive(false);
        
                                                
	}
   
    void OnDrawGizmosSelected()
    {

            Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(800, 0, 0));
            Gizmos.DrawLine(new Vector3(0, 800, 0), new Vector3(800, 800, 0));
            Gizmos.DrawLine(new Vector3(800, 0, 0), new Vector3(800, 800, 0));
            Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 800, 0));

    }
	
    public void SpawnTile(Vector2 snappos){
        GameObject gameObject;
        if (tilepool[currentTileIndex].tag != "TallGrass")
        {
            gameObject = Instantiate(template, container.transform, true);
        }
        else
        {
            gameObject = Instantiate(template, grasscontainer.transform, true);
        }
                gameObject.transform.position = snappos;
                gameObject.tag = tilepool[currentTileIndex].tag;
                gameObject.layer = tilepool[currentTileIndex].layer;
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("interiortiles/" + tilepool[currentTileIndex].sprite);
        if (gameObject.tag == "TallGrass")
        {

            gameObject.transform.Translate(0, 0, -1);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            DestroyImmediate(gameObject.GetComponent<TallGrass>());
        }
                if (tilepool[currentTileIndex].isAnimated)
                {


                    gameObject.GetComponent<AnimatedTile>().tileanimsprites = Resources.LoadAll<Sprite>("interiortiles/" + tilepool[currentTileIndex].sprite);
        }else DestroyImmediate(gameObject.GetComponent<AnimatedTile>());
                

    }

  
    public int FetchIndex(string sprite){
        foreach (Tile tile in tilepool){
            if (tile.sprite == sprite) return tilepool.IndexOf(tile);
        }
        return 0;
    }

    public void setCurrentMap(int index){
        currentMap = index;
       
    } 

}
#if UNITY_EDITOR
[CustomEditor(typeof(MapEditor))]
public class MapTileEditor : Editor
{
   
    private static bool m_editMode = false;
    private static bool m_editMode2 = false;
    public int curmap = 0;
    public Vector2 atlasSize = new Vector2(1024, 512);
void OnSceneGUI()
    {
       
        MapEditor me = (MapEditor)target;
        Vector3 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        //if 'E' pressed, spawn the selected prefab
        if (Event.current.type == EventType.KeyDown)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Mathf.Round(spawnPosition.x), Mathf.Round(spawnPosition.y)), Vector2.zero);
            switch (Event.current.keyCode)
            {
                case KeyCode.Comma:
                    if ((hit.collider != null && me.tilepool[me.currentTileIndex].tag == "TallGrass" && hit.collider.tag != "TallGrass")){
                        me.SpawnTile(new Vector2(Mathf.Round(spawnPosition.x), Mathf.Round(spawnPosition.y)));
                        break;
                    }
                    if (hit.collider == null)
                        me.SpawnTile(new Vector2(Mathf.Round(spawnPosition.x), Mathf.Round(spawnPosition.y)));
                    break;
                case KeyCode.Period:
                    if(hit.collider != null)
                        DestroyImmediate(hit.collider.gameObject);
                    break;
                case KeyCode.Slash:
                    if (hit.collider != null)
                    {
                        int index = 0;
                        foreach (Tile tile in me.tilepool)
                        {
                            string tempname;
                            tempname = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name;
                            if (tempname.Contains("_"))
                            {
                                tempname = tempname.Substring(0, tempname.IndexOf('_'));
                            }
                            if (Resources.Load<Sprite>("interiortiles/" + tile.sprite) == Resources.Load<Sprite>((tile.sprite.Contains("Water") ? "interiortiles/Water/" : tile.sprite.Contains("Buildings") ? "interiortiles/Buildings/" : "interiortiles/") + tempname))
                            {

                                me.currentTileIndex = index;
                                break;
                            }
                            index++;
                        }
                    }
                    break;
        }
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.W)
        {
            me.currentTileIndex += 10;
            me.currentTileIndex %= me.tilepool.Count - 1;

        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S)
        {
            me.currentTileIndex -= 10;
            if (me.currentTileIndex < 0) me.currentTileIndex = me.tilepool.Count - 1 + me.currentTileIndex;

        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
        {
            me.currentTileIndex--;
            if (me.currentTileIndex < 0) me.currentTileIndex = me.tilepool.Count - 1;

        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D)
        {
            me.currentTileIndex++;
            if (me.currentTileIndex == me.tilepool.Count) me.currentTileIndex = 0;

        }
    
    }


public override void OnInspectorGUI()
{
  DrawDefaultInspector();
  MapEditor me = (MapEditor)target;
        Texture2D showntex =  Instantiate(Resources.Load<Texture2D>("interiortiles/" + me.tilepool[me.currentTileIndex].sprite));
        Texture2D newtex = new Texture2D(showntex.width, showntex.height, TextureFormat.ARGB32,true);
        newtex.SetPixels(0, 0, 16, 16, showntex.GetPixels());
            TextureScale.Point(newtex, 64, 64);

        GUILayout.Box(newtex,GUILayout.Width(64), GUILayout.Height(64));
  if (GUILayout.Button("Add New Tile Entry"))
  {
          me.tilepool.Add(new Tile(0,null,null,false));
    
  }   
        if (GUILayout.Button("Save Tile Pool from Save Data"))
  {
            Serializer.Save<Tile>("tilepool.txt", me.tilepool.ToArray());

    
  } 
        if (GUILayout.Button("Load Tile Pool from Save Data"))
  {
           
            me.tilepool = new List<Tile>(Serializer.Load<Tile>("tilepool.txt"));
    
  } 
        if(GUILayout.Button("Save Map to file")){
            Get get = new Get();

            List<GameObject> foundtiles = new List<GameObject>();
            List<GameObject> foundgrass = new List<GameObject>();

            int index = 0;
            for (int i = 0; i < me.container.transform.childCount; i++){
                
                foundtiles.Add(me.container.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < me.grasscontainer.transform.childCount; i++)
            {

                foundgrass.Add(me.grasscontainer.transform.GetChild(i).gameObject);
            }
            index = 0;
            Sprite tileSprite = null;
            foreach(GameObject child in foundtiles){
              
                me.savedtiles[(int)child.transform.position.x, (int)child.transform.position.y] = new GridTile(null, null, null, false,0,0,  "", false,false,false,1);
               GridTile grid =  me.savedtiles[(int)child.transform.position.x, (int)child.transform.position.y];
                string tempname;
                tempname = child.GetComponent<SpriteRenderer>().sprite.name;
                if (tempname.Contains("_"))
                {
                    tempname = tempname.Substring(0, tempname.IndexOf('_'));
                }
                grid.mainSprite = tempname;
                grid.tag = child.tag;
                if (grid.tag.Contains("Wall")) grid.isWall = true;
                grid.isWarp = child.GetComponent<TileWarp>() ? true : false;
                grid.isAnimated = child.GetComponent<AnimatedTile>() ? true : false;
                if (grid.isAnimated){
                    tempname = AssetDatabase.GetAssetPath(child.GetComponent<SpriteRenderer>().sprite);
                    if (tempname.Contains("_"))
                    {
                        tempname = tempname.Substring(0, tempname.IndexOf('_'));
                    }
                    Sprite[] tiles = Resources.LoadAll<Sprite>(tempname.Replace("Assets/Resources/", "").Replace(".png",""));
                    grid.frames = tiles.Length;
                   
                }

                GameObject currentGrass = null;
                foreach(GameObject grassObj in foundgrass){
                    if(grassObj.transform.position.x == child.transform.position.x && grassObj.transform.position.y == child.transform.position.y){
                        currentGrass = grassObj;
                        break;
                    }
                }
                TallGrass grass = null;
                if(currentGrass != null){
                    grass = currentGrass.GetComponent<TallGrass>();
                }

                if (grass != null)
                {
                    grid.hasGrass = true;
                    grid.tallGrass = grass.info;
                    tempname = currentGrass.GetComponent<SpriteRenderer>().sprite.name;
                    if (tempname.Contains("_"))
                    {
                        tempname = tempname.Substring(0, tempname.IndexOf('_'));
                    }

                    grid.grassSprite = tempname;
                   }

                else
                {
                    grid.tallGrass = null;
                    grid.hasGrass = false;
                }
                grid.tileWarp = child.GetComponent<TileWarp>() ? child.GetComponent<TileWarp>().info : null;
                grid.tiledata = child.GetComponent<itemData>() ? child.GetComponent<itemData>().data : null;
                grid.posx = (int)child.transform.position.x;
                grid.posy = (int)child.transform.position.y;
                index++;
            }

            Serializer.Save2D<GridTile>(Application.streamingAssetsPath + "/map.txt", me.savedtiles);
           
        }
        if (GUILayout.Button("Add all building tiles"))
        {
            for (int i = 126; i <= 273; i++){
                foreach (Tile tile in me.tilepool)
                {
                   
                    if (Resources.Load<Sprite>("interiortiles/" + tile.sprite) == Resources.Load<Sprite>("interiortiles/Buildings/tile" + i.ToString()))
                    {

                        goto SkipCreatingSlot;
                    }

                }

                me.tilepool.Add(new Tile(5,"Buildings/tile" + i.ToString() , "WallObject", false));
                SkipCreatingSlot:
                ;
            }
        }
      
        curmap = EditorGUILayout.IntSlider("Current Map", curmap, 0, 247);
        if (GUILayout.Button("Set Current Map"))
        {
            me.setCurrentMap(curmap);
        }
        if (GUILayout.Button("Load Map Data from file"))
        {
            me.maps =  Serializer.Load<MapData>(Application.streamingAssetsPath + "/romMaps.txt");
        }
        if (GUILayout.Button("Load Tileset Data from file"))
        {
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    if (File.Exists("Assets/Game/Blocks/tileset" + i + "/block" + j + ".png"))
                    {
                        me.tilesets[i].blocks[j] = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Game/Blocks/tileset" + i + "/block" + j + ".png");
                    }
                    else me.tilesets[i].blocks[j] = null;
                }
            }
        }

     
  }


}
#endif
public class TextureScale
{
    public class ThreadData
    {
        public int start;
        public int end;
        public ThreadData(int s, int e)
        {
            start = s;
            end = e;
        }
    }

    private static Color[] texColors;
    private static Color[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;
    private static int finishCount;
    private static Mutex mutex;

    public static void Point(Texture2D tex, int newWidth, int newHeight)
    {
        ThreadedScale(tex, newWidth, newHeight, false);
    }

    public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
    {
        ThreadedScale(tex, newWidth, newHeight, true);
    }

    private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
    {
        texColors = tex.GetPixels();
        newColors = new Color[newWidth * newHeight];
        if (useBilinear)
        {
            ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
            ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
        }
        else
        {
            ratioX = ((float)tex.width) / newWidth;
            ratioY = ((float)tex.height) / newHeight;
        }
        w = tex.width;
        w2 = newWidth;
        var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
        var slice = newHeight / cores;

        finishCount = 0;
        if (mutex == null)
        {
            mutex = new Mutex(false);
        }
        if (cores > 1)
        {
            int i = 0;
            ThreadData threadData;
            for (i = 0; i < cores - 1; i++)
            {
                threadData = new ThreadData(slice * i, slice * (i + 1));
                ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                Thread thread = new Thread(ts);
                thread.Start(threadData);
            }
            threadData = new ThreadData(slice * i, newHeight);
            if (useBilinear)
            {
                BilinearScale(threadData);
            }
            else
            {
                PointScale(threadData);
            }
            while (finishCount < cores)
            {
                Thread.Sleep(1);
            }
        }
        else
        {
            ThreadData threadData = new ThreadData(0, newHeight);
            if (useBilinear)
            {
                BilinearScale(threadData);
            }
            else
            {
                PointScale(threadData);
            }
        }

        tex.Resize(newWidth, newHeight);
        tex.SetPixels(newColors);
        tex.Apply();

        texColors = null;
        newColors = null;
    }

    public static void BilinearScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        for (var y = threadData.start; y < threadData.end; y++)
        {
            int yFloor = (int)Mathf.Floor(y * ratioY);
            var y1 = yFloor * w;
            var y2 = (yFloor + 1) * w;
            var yw = y * w2;

            for (var x = 0; x < w2; x++)
            {
                int xFloor = (int)Mathf.Floor(x * ratioX);
                var xLerp = x * ratioX - xFloor;
                newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                       y * ratioY - yFloor);
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    public static void PointScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        for (var y = threadData.start; y < threadData.end; y++)
        {
            var thisY = (int)(ratioY * y) * w;
            var yw = y * w2;
            for (var x = 0; x < w2; x++)
            {
                newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
    {
        return new Color(c1.r + (c2.r - c1.r) * value,
                          c1.g + (c2.g - c1.g) * value,
                          c1.b + (c2.b - c1.b) * value,
                          c1.a + (c2.a - c1.a) * value);
    }
}
