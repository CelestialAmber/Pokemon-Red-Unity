using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
[System.Serializable]
public class GridTile
{

    public TilesData tiledata;
    public WarpInfo tileWarp;
    public GrassInfo tallGrass;
    public bool isAnimated;
    public int posx, posy;
    public string tag;
    public bool isWarp;
    public bool hasGrass;
    public bool isWall;
    public int frames;
    public string mainSprite;
    public List<Vector2[]> mainUvs = new List<Vector2[]>();
    public GridTile(TilesData tiledata, WarpInfo warpInfo, GrassInfo tallGrass, bool isAnimated, int posx, int posy, string tag, bool isWarp, bool hasGrass, bool isWall, int frames, string mainSprite){
        this.tiledata = tiledata;
        this.tallGrass = tallGrass;
        this.isAnimated = isAnimated;
        this.posx = posx;
        this.posy = posy;
        this.tag = tag;
        this.isWarp = isWarp;
        this.hasGrass = hasGrass;
        this.isWall = isWall;
        this.frames = frames;
        this.mainSprite = mainSprite;

    }

}
public class Player : MonoBehaviour
{
    public int bionumber;
 public Animator playerAnim;
    public Dialogue dia;
    public bool moving;
    public bool inBattle;
    public bool HALLEVENT;
    public bool manuallyWalking;
    public bool walkedfromwarp;
    public GameObject credits;
    public int walkSurfBikeState;
    public int direction;
    public GameObject top, bottom;
    public ITEMTEXTDB IDB;
    public GameObject fameoak;
    public bool canInteractAgain;
    public bool overrideRenable;
    public bool PCactive;
    public static bool disabled = true;
    public bool isDisabled;
    public GridTile itemCheck, facingCheck;
    public bool startmenuup;
    public GameObject startmenu;
    public bool displayingEmotion;
    public bool amenuactive;
    public Sprite[] bubbles;
    public UnityEvent onHitWarp;
    public SpriteRenderer emotionbubble;
    public MainMenu moon;
    public bool shopup;
    public ViewBio BIO;
    public bool actuallymoving;
    public bool ledgejumping;
    public GridTile facedtile;
    public float tileanimtimer;
    public GridTile[,] maptiles = new GridTile[GameConstants.mapWidth, GameConstants.mapHeight];
    public bool doupdate;
    public MeshRenderer mainLayer, grassLayer;
    public SpriteAtlas tileAtlas;
    public Mesh mesh;
    public Material tileMat;
    public Vector2 atlasSize;
    public Vector2[] mainUv;
    public MeshFilter mainMesh, grassMesh;
    //1 up, 2down, 3 left, 4 right
    public bool cannotMoveLeft, cannotMoveRight, cannotMoveUp, cannotMoveDown;

    public float speed = 2.0f;
    public Vector3 pos, centerPos;
    Transform tr;

    // Use this for initialization
    int mod(int a, int b){
        return a < 0 ? b + a % b : a % b;
    }
    void GenerateUvs(){
        Rect rect;
        foreach(GridTile tile in maptiles){
            if(tile != null){
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
                }else{
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
    void GenerateMeshes(){
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
        mainMesh.mesh = new Mesh();
        mainMesh.mesh.vertices = mesh.vertices;
        mainMesh.mesh.triangles = mesh.triangles;
        grassMesh.mesh = new Mesh();
        grassMesh.mesh.vertices = mesh.vertices;
        grassMesh.mesh.triangles = mesh.triangles;

    }
      void Awake()
    {
        tileMat.mainTexture =  tileAtlas.GetSprite("tile7").texture;
        atlasSize = new Vector2(tileMat.mainTexture.width, tileMat.mainTexture.height);
        maptiles = new GridTile[GameConstants.mapWidth, GameConstants.mapHeight];
        GenerateMeshes();
        SaveData.Init();
        SaveData.money = 3000;
        SaveData.coins = 300;
        disabled = false;
       
        onHitWarp = new UnityEvent();
        onHitWarp.AddListener(onWarp);
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
    List<Vector2> mainUvs = new List<Vector2>();
    List<Vector2> grassUvs = new List<Vector2>();
     void LoadMap(){
        mainUvs.Clear();
        grassUvs.Clear();
        for (int y = 0; y < 11; y++)
        {
            for (int x = 0; x < 12; x++)
            {

                GridTile tileToUse = maptiles[mod(Mathf.RoundToInt(tr.position.x + (x - 5)) , GameConstants.mapWidth), mod(Mathf.RoundToInt(tr.position.y + (y - 5)) , GameConstants.mapHeight)];
               
                if (tileToUse != null)
                {
                    //Load the main layer.
                    GridTile loadedtile = tileToUse;
                    if (loadedtile.isAnimated)
                    {
                       int frame =  Mathf.FloorToInt(loadedtile.frames * (tileanimtimer / 2));
                        mainUvs.AddRange(loadedtile.mainUvs[frame]);
                        grassUvs.AddRange(new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
                    }
                    else
                    {
                        if (loadedtile.hasGrass)
                        { //Does the tile have grass?
                          //yes
                            mainUvs.AddRange(new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
                            grassUvs.AddRange(loadedtile.mainUvs[0]);
                        }
                        else
                        {
                            
                            mainUvs.AddRange(loadedtile.mainUvs[0]);
                            grassUvs.AddRange(new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
                        }
                    }

                }
                else {
                    mainUvs.AddRange(new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
                    grassUvs.AddRange(new Vector2[] { new Vector2(0.95f, 0.95f), new Vector2(0.96f, 0.95f), new Vector2(0.96f, 0.96f), new Vector2(0.95f, 0.96f) });
                }


            }
        }
        mainLayer.transform.position = tr.position - new Vector3(5.5f, 5.5f, -1);
        grassLayer.transform.position = tr.position - new Vector3(5.5f, 5.5f, -0.33f);
        mainUv = mainUvs.ToArray();
        grassMesh.mesh.uv = grassUvs.ToArray();
        centerPos = tr.position;
    }
    void LoadMapData(){
        maptiles = Serializer.Load2D<GridTile>(Application.streamingAssetsPath + "/map.txt");
    }
    void Start()
    {
        LoadMapData();
        GenerateUvs();
        emotionbubble.enabled = false;
        SaveData.trainerID = Random.Range(0, 65536);
        startmenuup = false;
        canInteractAgain = true;
        direction = 2;
        pos = transform.position;
        centerPos = transform.position;
        tr = transform;
        LoadMap();
        StartCoroutine(CoreUpdate());
    }
    IEnumerator CoreUpdate()
    {
        while (true)
        {
            
            StartCoroutine(MapUpdate());
            StartCoroutine(MovementUpdate());
            mainMesh.mesh.uv = mainUv;
            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator MapUpdate(){
        AnimateTiles();
        yield return 0;
    }

        

    IEnumerator MovementUpdate()
    {
        

        switch (walkSurfBikeState)
        {
            case 0:
                speed = 4f;
                break;
            case 1:
                speed = 8f;
                break;
            case 2:
                speed = 4f;
                break;


        }
       
        if (dia.finishedWithTextOverall && !disabled && !startmenuup && !shopup && !inBattle)
        {
            //If we're not ledge jumping already, the adjacent tile is a ledge, and we're exactly on a tile, ledge jump
            if (Inputs.held("down") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeDown" && tr.position == pos && direction == 2)
            {
                ledgejumping = true;
                direction = 2;
                playerAnim.SetFloat("movedir", direction);
                StartCoroutine(LedgeJump());

            }
            if (Inputs.held("left") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeLeft" && tr.position == pos && direction == 3)
            {
                ledgejumping = true;
                direction = 3;
                playerAnim.SetFloat("movedir", direction);
                StartCoroutine(LedgeJump());


            }
            if (Inputs.held("right") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeRight" && tr.position == pos && direction == 4)
            {
                ledgejumping = true;
                direction = 4;
                playerAnim.SetFloat("movedir", direction);
                StartCoroutine(LedgeJump());


            }
            if (!ledgejumping)
            {

                if (actuallymoving)
                {
                    if (tr.position == pos)
                    {
                        if (!walkedfromwarp)
                            walkedfromwarp = true;
                    }

                }
                if (Inputs.held("up"))
                {
                    if (actuallymoving && tr.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    moving = true;
                    if (tr.position == pos)
                    {
                        direction = 1;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (tr.position == pos && !cannotMoveUp)
                    {
                        LoadMap();
                        pos += (Vector3.up);
                        actuallymoving = true;
                    }
                    else if (cannotMoveUp)
                    {
                        actuallymoving = false;
                    }

                }
                else if (Inputs.held("right"))
                {
                    if (actuallymoving && tr.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    moving = true;
                    if (tr.position == pos)
                    {
                        direction = 4;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (tr.position == pos && !cannotMoveRight)
                    {
                        LoadMap();
                        pos += (Vector3.right);
                        actuallymoving = true;
                    }
                    else if (cannotMoveRight)
                    {
                        actuallymoving = false;
                    }

                }
             
                else if (Inputs.held("down"))  {   
                    if (actuallymoving && tr.position == pos)
                    {
                        walkedfromwarp = true;
                    }
                   

                    moving = true;
                    if (tr.position == pos)
                    {
                        direction = 2;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (tr.position == pos && !cannotMoveDown)
                    {
                        LoadMap();
                        pos += (Vector3.down);
                        actuallymoving = true;
                    }
                    else if (cannotMoveDown)
                    {
                        actuallymoving = false;
                    }
                }
                else if (Inputs.held("left"))
                {
                    if (actuallymoving && tr.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    moving = true;
                    if (tr.position == pos)
                    {
                        direction = 3;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (tr.position == pos && !cannotMoveLeft)
                    {
                        LoadMap();
                        pos += (Vector3.left);
                        actuallymoving = true;
                    }
                    else if (cannotMoveLeft)
                    {
                        actuallymoving = false;
                    }
                }
                else if (tr.position == pos)
                {
                    actuallymoving = false;
                }

                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);

                if (tr.position == pos)
                {
                    if (actuallymoving)
                    {
                        GridTile currentTile = maptiles[mod((int)tr.position.x, GameConstants.mapWidth), mod((int)tr.position.y, GameConstants.mapHeight)];
                        if (currentTile != null)
                        {
                            if (currentTile.isWarp)
                            {
                                onHitWarp.Invoke();
                            }
                        }

                        LoadMap();
                    }
                    moving = false;

                }

                if (Inputs.held("up") || Inputs.held("left") ||Inputs.held("right") || Inputs.held("down"))
                    moving = true;

                playerAnim.SetFloat("movingfloat", actuallymoving ? 1 : 0);
                if (tr.position == pos)
                    playerAnim.SetFloat("movedir", direction);

            }

        }
        yield return 0;
    }
    public IEnumerator MovePlayerOneTile(int direction)
    {
        manuallyWalking = true;

        if (direction == 1)
        {
            direction = 1;
            moving = true;

            if (tr.position == pos)
            {
                LoadMap();
                pos += (Vector3.up);
            }
        }
        else if (direction == 2)
        {
            direction = 2;
            moving = true;
            if (tr.position == pos)
            {
                LoadMap();
                pos += (Vector3.right);
            }

        }
        else if (direction == 3)
        {
            direction = 3;
            moving = true;
            if (tr.position == pos)
            {
                LoadMap();
                pos += (Vector3.down);
            }
        }
        else if (direction == 4)
        {
            direction = 4;
            moving = true;
            if (tr.position == pos)
            {
                LoadMap();
                pos += (Vector3.left);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        while (moving)
        {
            yield return new WaitForSeconds(.1f);
            if (tr.position == pos)
            {
                break;
            }
        }
        moving = false;
        manuallyWalking = false;
    }
    IEnumerator LedgeJump()
    {
        bool reachedMiddle = false;
        playerAnim.SetBool("ledgejumping", ledgejumping);
        pos += direction == 2 ? new Vector3(0, -2, 0) : direction == 3 ? new Vector3(-2, 0, 0) : new Vector3(2, 0, 0);
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 4);
        disabled = true;
        while (tr.position != pos)
        {
            yield return new WaitForSeconds(0.001f);
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 4);



            if (tr.position == pos)
            {
                LoadMap();
                break;
            }
            float midDistance = (direction == 2 ? transform.position.y - (pos.y + 1) : direction == 3 ? transform.position.x - (pos.x + 1) : transform.position.x - (pos.x - 1));
            if (Mathf.Abs(midDistance) < Time.deltaTime * 4 && !reachedMiddle)
            {
                reachedMiddle = true;
                Debug.Log("At middle");
                LoadMap();

            }
        }
        ledgejumping = false;
        facedtile = null;
        playerAnim.SetBool("ledgejumping", ledgejumping);
        WaitToInteract(0.2f);



    }

    public void Warp(Vector2 position)
    {
        actuallymoving = false;
        transform.localPosition = position;
        pos = tr.position;
        disabled = true;
        LoadMap();
        WaitToInteract();

    }
    public void onWarp()
    {

        Debug.Log("Detected player.");
        if (tr.position == pos && walkedfromwarp)
        {
            walkedfromwarp = false;
            WarpInfo warp = maptiles[(int)tr.position.x, (int)tr.position.y].tileWarp;
            Warp(new Vector2(warp.warpposx, warp.warpposy));

        }
    }
   


    // Update is called once per frame
    void Update()
    {
        isDisabled = disabled;
        tileanimtimer += Time.deltaTime;
        if (tileanimtimer >= 2) tileanimtimer = 0;
		playerAnim.SetFloat("walkbikesurfstate", walkSurfBikeState);
		if (BIO.bioscreen.enabled) {

			disabled = true;
		}
		
		startmenu.SetActive (startmenuup);
		if (!disabled && !amenuactive &&!startmenuup) {
            if (Inputs.pressed("start") && !moving) {
				startmenuup = true;
				moon.Initialize ();
			}
			top.SetActive (!disabled);
			bottom.SetActive (!disabled);

			playerAnim.SetBool ("moving", moving);
			playerAnim.SetInteger ("movedirection", direction);
	
            if (Inputs.released("down") || Inputs.released("right") || Inputs.released("left") || Inputs.released("up")) {
				moving = false;
			
			}
			if (tr.position == pos) {
				transform.localPosition = new Vector3 (Mathf.Round (transform.localPosition.x), Mathf.Round (transform.localPosition.y), 0);
				pos = tr.position;
			}




           
           

			if (direction == 1) {

                itemCheck =  maptiles[mod((int)tr.position.x, GameConstants.mapWidth), mod((int)tr.position.y + 1, GameConstants.mapHeight)];
                facingCheck = maptiles[mod((int)tr.position.x, GameConstants.mapWidth) , mod((int)tr.position.y + 1, GameConstants.mapHeight)];
			}
            if (direction == 2)
            {

                itemCheck = maptiles[mod((int)tr.position.x, GameConstants.mapWidth), mod((int)tr.position.y - 1,GameConstants.mapHeight)];
                facingCheck = maptiles[mod((int)tr.position.x, GameConstants.mapWidth), mod((int)tr.position.y - 1,GameConstants.mapHeight)];
            }
            if (direction == 3)
            {

                itemCheck = maptiles[mod((int)tr.position.x - 1,GameConstants.mapWidth),mod((int)tr.position.y,GameConstants.mapHeight)];
                facingCheck = maptiles[mod((int)tr.position.x - 1, GameConstants.mapWidth), mod((int)tr.position.y, GameConstants.mapHeight)];
            }
            if (direction == 4)
            {

                itemCheck = maptiles[mod((int)tr.position.x + 1, GameConstants.mapWidth), mod((int)tr.position.y, GameConstants.mapHeight)];
                facingCheck = maptiles[mod((int)tr.position.x + 1, GameConstants.mapWidth), mod((int)tr.position.y, GameConstants.mapHeight)];
            }
            if (facingCheck != null)
            {
                    facedtile = facingCheck;
                }
                else facedtile = null;
            }
            else facedtile = null;

			if (itemCheck != null) {
				//print (itemCheck.distance.ToString ());
				if (!moving && tr.position == pos) {
                    
					if (!moving && canInteractAgain && !PCactive && !shopup && !disabled && dia.finishedWithTextOverall && !startmenuup && !inBattle && !moving) {
						if (itemCheck.tag.Contains("Interact")) {
							if (Inputs.pressed("a")) {
                      if (itemCheck.tiledata.hasText) {
									if (itemCheck.tiledata.onlyonce) {
										if (!itemCheck.tiledata.triggered) {
											itemCheck.tiledata.triggered = true;
											canInteractAgain = false;
											IDB.PlayText (itemCheck.tiledata.TextID, itemCheck.tiledata.coinamount);
										}
									}
										canInteractAgain = false;
										IDB.PlayText (itemCheck.tiledata.TextID, itemCheck.tiledata.coinamount);


									}
								}


							}
						}
					}
				}

			
			//Check collision here?
		
        CheckCollision();

	}

	public void WaitToInteract(){
		Invoke ("ReenableInteracting", .1f);

	}
    public void WaitToInteract(float time)
    {
        disabled = true;
        Invoke("ReenableInteracting", time);

    }

	void ReenableInteracting(){
        if (!overrideRenable)
        {
            canInteractAgain = true;
            disabled = false;
        }
	}
	public IEnumerator DisplayEmotiveBubble(int type){
		disabled = true;
		displayingEmotion = true;
		emotionbubble.enabled = true;
		emotionbubble.sprite = bubbles [type];
		yield return new WaitForSeconds (2);
		emotionbubble.enabled = false;
		displayingEmotion = false;

        disabled = false;


	}
    void CheckCollision()
    {
        if (tr.position == pos)
        {
            GridTile tileToCheck;
            tileToCheck = maptiles[mod((int)tr.position.x - 1, GameConstants.mapWidth), mod((int)tr.position.y,GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveLeft = tileToCheck.tag.Contains("Wall") || tileToCheck.tag.Contains("Ledge");
            }
            else cannotMoveLeft = false;
            tileToCheck = maptiles[mod((int)tr.position.x + 1, GameConstants.mapWidth), mod((int)tr.position.y, GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveRight = tileToCheck.tag.Contains("Wall") || tileToCheck.tag.Contains("Ledge");
            }
            else cannotMoveRight = false;
            tileToCheck = maptiles[mod((int)tr.position.x, GameConstants.mapWidth), mod((int)tr.position.y + 1,GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveUp = tileToCheck.tag.Contains("Wall") || tileToCheck.tag.Contains("Ledge");
            }
            else cannotMoveUp = false;
            tileToCheck = maptiles[mod((int)tr.position.x, GameConstants.mapWidth), mod((int)tr.position.y - 1,GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveDown = tileToCheck.tag.Contains("Wall") || tileToCheck.tag.Contains("Ledge");
            }
            else cannotMoveDown = false;
        }
    }
}
