using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
public enum Direction{
Up,
Down,
Left,
Right,
Null

}
public class Player : MonoBehaviour
{

 public Animator playerAnim;
    public bool holdingDirection;
    public bool inBattle;
    public bool manuallyWalking;
    public bool walkedfromwarp;
    public GameObject credits;
    public int walkSurfBikeState;
    public Direction direction;
    public GameObject top, bottom;
    public TextDatabase textData;
    public static bool disabled = true;
    public bool isDisabled;
    public GridTile itemCheck;
    public bool startMenuActive, menuActive;
    public GameObject startmenu;
    public bool displayingEmotion;
    public Sprite[] bubbles;
    public SpriteRenderer emotionbubble;
    public MainMenu moon;
    public ViewBio viewBio;
    public bool isMoving;
    public bool ledgejumping;
    public GridTile facedtile;
    public int numberOfNoRandomBattleStepsLeft;
    public WarpInfo warpInfo;
	public bool isWarping;
	public AudioClip collisionClip, ledgeJumpClip, openStartMenuClip, cutClip;
	public float collisionSoundTimer;
    //1 up, 2down, 3 left, 4 right
    public bool cannotMoveLeft, cannotMoveRight, cannotMoveUp, cannotMoveDown;

    public float speed = 2.0f;
    public Vector3 pos;
    public int holdFrames;
    public Map currentArea;
    public bool areaHasWaterEncounters;
    public EncounterData currentAreaTable;

    public GameObject facedObject;
    public bool facingTree;
    private bool[] objectExists = new bool[4];
    public GameObject movingHitbox;
        public UnityEvent onHitWarp, onLoadMap, onEncounterTrainer;
    // Use this for initialization
   

    public static Player instance;

      void Awake()
    {
        instance = this;
        disabled = false;
 
        pos = transform.position;
    }

    void Start()
    {
       GameData.party.Add(new Pokemon("Nidoking",50));
       GameData.party.Add(new Pokemon("Ditto",50));
        GameData.party[0].moves[0].name = "Cut";
        GameData.party[0].moves[1].name = "Surf";
        GameData.party[0].moves[2].name = "Softboiled";
        emotionbubble.enabled = false;
        GameData.trainerID = Random.Range(0, 65536);
        UpdateFacedTile();
        direction = Direction.Down;
        pos = transform.position;
        StartCoroutine(CoreUpdate());
    }
    IEnumerator CoreUpdate()
    {
        while (true)
        {

            StartCoroutine(MovementUpdate());
            yield return new WaitForEndOfFrame();
        }

    }

    

    IEnumerator MovementUpdate()
    {



        switch (walkSurfBikeState)
        {
            case 0: //Walk
                speed = 3.7f;
                break;
            case 1: //Bicycle is 2x faster than walking/surfing
                speed = 7.4f;
                break;
            case 2: //Surf
                speed = 3.7f;
                break;


        }

        if (Dialogue.instance.finishedText && !disabled && !menuActive && !startMenuActive && !inBattle && !manuallyWalking && !GameData.atTitleScreen)
        {
             
              if(Inputs.released("left") || Inputs.released("right") || Inputs.released("up") || Inputs.released("down")){
            if(holdFrames <= 2) holdFrames = 0;
        }



            //If we're not ledge jumping already, the adjacent tile is a ledge, and we're exactly on a tile, ledge jump
            if (Inputs.held("down") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeDown" && transform.position == pos && direction == Direction.Down && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Down;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());

            }
            if (Inputs.held("left") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeLeft" && transform.position == pos && direction == Direction.Left && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Left;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());


            }
            if (Inputs.held("right") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeRight" && transform.position == pos && direction == Direction.Right && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Right;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());


            }
          
            if (!ledgejumping)
            {
                
                 
                if(Inputs.held("up")||Inputs.held("down")||Inputs.held("left")||Inputs.held("right")){
                if(!holdingDirection){
                collisionSoundTimer += 0.3f;
                }
                }
                if (Inputs.held("up"))
                {
                    holdFrames++;
                    if (isMoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    holdingDirection = true;
                    if (transform.position == pos)
                    {
                        direction = Direction.Up;
                        UpdateFacedTile();
                        playerAnim.SetFloat("movedir", (int)direction + 1);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(!cannotMoveUp){ 
                         if(walkSurfBikeState == 2 && facedtile != null && !facedtile.isWater) {
                        walkSurfBikeState = 0;
                        PlayCurrentAreaSong();
                        
                          }
                        pos += (Vector3.up);
                        isMoving = true;
                        }
                    }

                }
                else if (Inputs.held("right"))
                {
                    holdFrames++;
                    if (isMoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    holdingDirection = true;
                    if (transform.position == pos)
                    {
                        direction = Direction.Right;
                        UpdateFacedTile();
                        playerAnim.SetFloat("movedir", (int)direction + 1);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(!cannotMoveRight){
                      if(walkSurfBikeState == 2 && facedtile != null && !facedtile.isWater) {
                        walkSurfBikeState = 0;
                        PlayCurrentAreaSong();
                        
                          }
                             pos += (Vector3.right);
                        isMoving = true;
                    }
                    }

                }

                else if (Inputs.held("down"))
                {
                    holdFrames++;
                    if (isMoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }


                    holdingDirection = true;
                    if (transform.position == pos)
                    {

                        direction = Direction.Down;
                        UpdateFacedTile();
                        playerAnim.SetFloat("movedir", (int)direction + 1);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(!cannotMoveDown){ 
                         if(walkSurfBikeState == 2 && facedtile != null && !facedtile.isWater) {
                        walkSurfBikeState = 0;
                        PlayCurrentAreaSong();
                        
                          }
                            pos += (Vector3.down);
                        isMoving = true;
                        } 
                    }
                }
                else if (Inputs.held("left"))
                {
                    holdFrames++;
                    if (isMoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }
                    
                    holdingDirection = true;
                    if (transform.position == pos)
                    {
                        direction = Direction.Left;
                        UpdateFacedTile();
                        playerAnim.SetFloat("movedir", (int)direction + 1);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                       
                        if(!cannotMoveLeft){ 
                         if(walkSurfBikeState == 2 && facedtile != null && !facedtile.isWater) {
                        walkSurfBikeState = 0;
                        PlayCurrentAreaSong();
                        
                          }
                            pos += (Vector3.left);
                        isMoving = true;
                        }
                    }
                }
                else if (transform.position == pos)
                {
                    isMoving = false;
                }
                else holdFrames = 0;
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                if(facingWall()) isMoving = false;
                if (transform.position == pos)
                {

                    if (isMoving) //did we move onto a new tile?
                    {
                        //The player moved onto a tile, run any checks needed;
                        CheckCollision(); //update the tile in front of us
                        UpdateFacedTile();
                        if (!walkedfromwarp)
                            walkedfromwarp = true;
                        GridTile currentTile = MapManager.maptiles[(int)transform.position.x, (int)transform.position.y];
                        if (currentTile != null)
                        {
                            if (currentTile.isWarp && currentTile.tileWarp.warpType == WarpType.WalkOnWarp)
                            {
                                warpInfo = currentTile.tileWarp;
                                onHitWarp.Invoke();
                            }
                            if(numberOfNoRandomBattleStepsLeft > 0) numberOfNoRandomBattleStepsLeft--;
                            if ((currentTile.hasGrass && currentAreaTable != null) || (currentTile.isWater && areaHasWaterEncounters)){ 
                               
                                if(numberOfNoRandomBattleStepsLeft == 0) {
                                int rand = Random.Range(0,256);
                                EncounterData table = (currentTile.hasGrass ? currentAreaTable : PokemonData.encounters[55]);
                                if(rand < currentAreaTable.encounterChance){
                                rand = Random.Range(0,256);
                                Debug.Log("Wild encounter triggered. Choosing the encounter slot.");
                                int chosenIndex = (
                                rand <= 50 ? 0 : //51/256 = 19.9% chance of slot 0
                                rand <= 101 ? 1 : //51/256 = 19.9% chance of slot 1
                                rand <= 140 ? 2 : //39/256 = 15.2% chance of slot 2
                                rand <= 165 ? 3 : //25/256 =  9.8% chance of slot 3
                                rand <= 190 ? 4 : //25/256 =  9.8% chance of slot 4
                                rand <= 215 ? 5 : //25/256 =  9.8% chance of slot 5
                                rand <= 228 ? 6 : //13/256 =  5.1% chance of slot 6
                                rand <= 241 ? 7 : //13/256 =  5.1% chance of slot 7
                                rand <= 252 ? 8 : 9);//11/256 =  4.3% chance of slot 8
                                //3/256 =  1.2% chance of slot 9
                                Debug.Log("Chosen Pokemon: " + table.slots[chosenIndex].ToString());
                                isMoving = false;
                                holdingDirection = false;
                                StartCoroutine(StartWildBattle(table.slots[chosenIndex]));
                                }

                                }
                                
                            }
                            
    
                        }
                 
                        onLoadMap.Invoke();
                        
                    }
                 
                    if(holdingDirection && !isWarping && facedtile != null && facedtile.isWarp && facedtile.tileWarp.warpType == WarpType.WallWarp && direction == facedtile.tileWarp.wallDirection){
                         warpInfo = facedtile.tileWarp;
                        onHitWarp.Invoke();
                    }
                    

                }
                 playerAnim.SetFloat("movingfloat",holdingDirection||isMoving ? 1f : 0);
                if (Inputs.held("up") || Inputs.held("left") ||Inputs.held("right") || Inputs.held("down")) holdingDirection = true;

               
                if (transform.position == pos) playerAnim.SetFloat("movedir", (int)direction + 1);


                    collisionSoundTimer += Time.deltaTime;
                    
                    if(collisionSoundTimer >= 0.3f && (holdingFacingDirection() && facingWall()) && !ledgejumping && holdFrames > 2){

                   SoundManager.instance.sfx.PlayOneShot(collisionClip);
                    collisionSoundTimer = 0;
                    }
                    
                if(!holdingDirection) collisionSoundTimer = 0;


            }



        }
        if(manuallyWalking){

             transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        
        playerAnim.SetFloat("movingfloat", isMoving ? 1 : 0);
                    

            if (transform.position == pos)
            {
       
        onLoadMap.Invoke();
         isMoving = false;
        holdingDirection = false;
        manuallyWalking = false;
        if(!walkedfromwarp) walkedfromwarp = true;
            }
        }
       


 




        
        yield return 0;


    }
   

public bool facingWall() => (direction == Direction.Up && cannotMoveUp) || (direction == Direction.Down && cannotMoveDown)  || (direction == Direction.Left && cannotMoveLeft) || (direction == Direction.Right && cannotMoveRight);
public bool holdingFacingDirection() => (direction == Direction.Up && Inputs.held("up")) || (direction == Direction.Down && Inputs.held("down")) || (direction == Direction.Left && Inputs.held("left")) || (direction == Direction.Right && Inputs.held("right"));
    public IEnumerator MovePlayerOneTile(Direction dir)
    {
if(!manuallyWalking){
        if (dir == Direction.Up)
        {
            direction = Direction.Up;
            holdingDirection = true;

            if (transform.position == pos)
            {
                playerAnim.SetFloat("movedir", (int)direction + 1);
                pos += (Vector3.up);
                 isMoving = true;
            }
        }
        else if (dir == Direction.Down)
        {
            direction = Direction.Down;
            holdingDirection = true;
            if (transform.position == pos)
            {
                playerAnim.SetFloat("movedir", (int)direction + 1);
                pos += (Vector3.down);
                isMoving = true;
            }

        }
        else if (dir == Direction.Left)
        {
            direction = Direction.Left;
            holdingDirection = true;
            if (transform.position == pos)
            {
                playerAnim.SetFloat("movedir", (int)direction + 1);
                pos += (Vector3.left);
                 isMoving = true;
            }
        }
        else if (dir == Direction.Right)
        {
            direction = Direction.Right;
            holdingDirection = true;
            if (transform.position == pos)
            { 
                playerAnim.SetFloat("movedir", (int)direction + 1);
                pos += (Vector3.right);
                 isMoving = true;
            }
        }
}
  manuallyWalking = true;
yield return 0;
    }
    IEnumerator LedgeJump()
    {
        holdingDirection = false;
        SoundManager.instance.sfx.PlayOneShot (ledgeJumpClip);
        bool reachedMiddle = false;
        playerAnim.SetBool("ledgejumping", ledgejumping);
        pos += direction == Direction.Down ? new Vector3(0, -2, 0) : direction == Direction.Left ? new Vector3(-2, 0, 0) : new Vector3(2, 0, 0);
        disabled = true;
        Vector3 originalPos = transform.position;
        float ledgeJumpTime = 1.85f/2.775f; //divide the animation clip time over the correct number to get the same duration as the real game
        float  curTime = 0f;
        while (curTime < ledgeJumpTime)
        {
             
             curTime += Time.deltaTime;
             if(curTime > ledgeJumpTime) curTime = ledgeJumpTime;
            transform.position = Vector3.Lerp(originalPos, pos, curTime/ledgeJumpTime);
            if (curTime >= ledgeJumpTime/2f && !reachedMiddle)
            {
                reachedMiddle = true;
                onLoadMap.Invoke();

            }
            yield return new WaitForEndOfFrame();
            
        }
        onLoadMap.Invoke();
	playerAnim.SetBool("ledgejumping", false);
	yield return new WaitForSeconds(0.1f);
        ledgejumping = false;
        facedtile = null;
        disabled = false;
        
       



    }

    public IEnumerator Warp(Vector2 position)
    {
        Inputs.Disable("start");
        Inputs.disableDpad();
        isMoving = false;
         WaitForSeconds wait = new WaitForSeconds(0.1f);
        for(int i = 0; i < 3; i++){
            yield return wait;
            ScreenEffects.flashLevel--;
        }
yield return new WaitForSeconds(0.25f);
        transform.localPosition = position;
        pos = transform.position;
        
        onLoadMap.Invoke();
        ScreenEffects.flashLevel = 3;
        yield return wait;
        ScreenEffects.flashLevel = 0;
        
        Inputs.Enable("start");
        Inputs.enableDpad();
        isWarping = false;
        GridTile currentTile = MapManager.maptiles[(int)transform.position.x,(int)transform.position.y];
        if(currentTile != null && currentTile.isWarp && currentTile.tileWarp.forceMove){
        StartCoroutine(MovePlayerOneTile(direction));
        }
       

    }
    public void onWarp()
    {
        holdingDirection = false;
        isWarping = true;
        if(warpInfo.warpType == WarpType.WallWarp) walkedfromwarp = true;
        if (transform.position == pos && walkedfromwarp)
        {
            walkedfromwarp = false;
            WarpInfo warp = warpInfo;
            StartCoroutine(Warp(new Vector2(warp.warpposx, warp.warpposy)));

        }
    }



    // Update is called once per frame
    void Update()
    {
        if (GameData.atTitleScreen) return;
        movingHitbox.transform.position = pos;
        
       
        isDisabled = disabled;
		playerAnim.SetFloat("walkbikesurfstate", walkSurfBikeState);
		if (viewBio.bioscreen.enabled) {

			disabled = true;
		}

       
		if (!disabled && !menuActive && !startMenuActive) {
            if (Inputs.pressed("start") && !isMoving) {
                SoundManager.instance.sfx.PlayOneShot(openStartMenuClip);
				startMenuActive = true;
                moon.gameObject.SetActive(true);
				moon.Initialize ();
			}
			top.SetActive (!disabled);
			bottom.SetActive (!disabled);

			playerAnim.SetInteger ("movedirection", (int)direction + 1);

            if (Inputs.released("down") || Inputs.released("right") || Inputs.released("left") || Inputs.released("up")) {
				if (!manuallyWalking) holdingDirection = false;

			UpdateFacedTile();
            }
        }
            

			if (facedObject != null ||(facedtile != null && facedtile.isInteractable)) {
                NPC npc = null;
                Pokeball pokeball = null;
                if(facedObject != null){
                     npc = facedObject.GetComponent<NPC>();
                     pokeball = facedObject.GetComponent<Pokeball>();
                }
				if (!holdingDirection && transform.position == pos) {

					if (!holdingDirection && !isMoving && !disabled && Dialogue.instance.finishedText && !startMenuActive && !menuActive && !inBattle && !ledgejumping) {

                    if (Inputs.pressed("a"))
                    {
                        if (npc != null && !npc.isMoving)
                        {
                            npc.FacePlayer();
                            if (npc.isTrainer) npc.StartEncounter();
                            else StartCoroutine(npc.NPCText());
                            return;

                        }
                        if (pokeball != null)
                        {
                            pokeball.gameObject.SetActive(false);
                            textData.GetItem(pokeball.item);
                            return;
                        }
                        if (facedtile != null)
                        {
                            
                            if (facedtile.isInteractable)
                            {
                                
                                if (facedtile.tiledata.hasText)
                                {
                                    textData.PlayText(facedtile.tiledata.TextID);
                                    return;
                                }
                                if (facedtile.hasItem)
                                {
                                    facedtile.hasItem = false;
                                    textData.GetItem(facedtile.tiledata.itemName);
                                }
                            }


                        }
                    }

							
						}
					}
				}
                
        CheckCollision();
        
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
    public void UpdateFacedTile(){
        if (direction == Direction.Up) {
                itemCheck =  MapManager.maptiles[(int)transform.position.x, (int)transform.position.y + 1];

            }
            if (direction == Direction.Down)
            {


                itemCheck = MapManager.maptiles[(int)transform.position.x, (int)transform.position.y - 1];

            }
            if (direction == Direction.Left)
            {

                itemCheck = MapManager.maptiles[(int)transform.position.x - 1, (int)transform.position.y];
            }
            if (direction == Direction.Right)
            {

                itemCheck = MapManager.maptiles[(int)transform.position.x + 1, (int)transform.position.y];
            }
            if (itemCheck != null)
            {
                    facedtile = itemCheck;
                }
                else facedtile = null;
            
            
    }
    
    void CheckCollision()
    {
         CheckObjectCollision(); 
        if (transform.position == pos)
        {
            GridTile tileToCheck = null;
            if(transform.position.x > 0)
            tileToCheck = MapManager.maptiles[(int)transform.position.x - 1, (int)transform.position.y];
            if (tileToCheck != null)
            {
                cannotMoveLeft = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2) || objectExists[2];
            }
            else cannotMoveLeft = true;
            if(transform.position.x < GameData.mapWidth - 1)
            tileToCheck = MapManager.maptiles[(int)transform.position.x + 1, (int)transform.position.y];
            if (tileToCheck != null)
            {
                cannotMoveRight = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2) || objectExists[3];
            }
            else cannotMoveRight = true;
             if(transform.position.y < GameData.mapHeight - 1)
            tileToCheck = MapManager.maptiles[(int)transform.position.x, (int)transform.position.y + 1];
            if (tileToCheck != null)
            {
                cannotMoveUp = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2) || objectExists[0];
            }
            else cannotMoveUp = true;
             if(transform.position.y > 0)
            tileToCheck = MapManager.maptiles[(int)transform.position.x, (int)transform.position.y - 1];
            if (tileToCheck != null)
            {
                cannotMoveDown = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2) || objectExists[1];
            }
            else cannotMoveDown = true;
        }
        
    }
    public LayerMask layerMask, collisionMask;
    public void CheckObjectCollision(){
        //Use a raycast to check for objects such as trees, etc...
        RaycastHit2D ray =  Physics2D.Raycast(transform.position,(direction == Direction.Up ? Vector2.up : direction == Direction.Down ? Vector2.down : direction == Direction.Left ? Vector2.left : Vector2.right),1,layerMask); 
        if(ray.collider != null){
            if(ray.collider.tag == "Tree"){
                facingTree = true;
            }
            else facingTree = false;
           

        }else facingTree = false;
         ray =  Physics2D.Raycast(transform.position,(direction == Direction.Up ? Vector2.up : direction == Direction.Down ? Vector2.down : direction == Direction.Left ? Vector2.left : Vector2.right),1,collisionMask);
        if(ray.collider != null) facedObject = ray.collider.gameObject;
        else facedObject = null;
         ray =  Physics2D.Raycast(transform.position,Vector2.up,1,layerMask);
         if(ray.collider != null) objectExists[0] = true;
         else objectExists[0] = false;
         ray =  Physics2D.Raycast(transform.position,Vector2.down,1,layerMask);
         if(ray.collider != null) objectExists[1] = true;
         else objectExists[1] = false;
         ray =  Physics2D.Raycast(transform.position,Vector2.left,1,layerMask);
         if(ray.collider != null) objectExists[2] = true;
         else objectExists[2] = false;
         ray =  Physics2D.Raycast(transform.position,Vector2.right,1,layerMask);
         if(ray.collider != null) objectExists[3] = true;
         else objectExists[3] = false;

    }
    public void Cut(string MonName){
        CloseMenus();
        StartCoroutine(CutFunction(MonName));
    }
    public IEnumerator CutFunction(string MonName){
        yield return StartCoroutine(Dialogue.instance.text(MonName + " hacked\naway with CUT!"));
        SoundManager.instance.sfx.PlayOneShot(cutClip);
         facedObject.GetComponent<Tree>().Cut();
         disabled = true;
         yield return new WaitForSeconds(1);
         disabled = false;

    }
    public void Surf(){
        CloseMenus();
     StartCoroutine(SurfFunction());
    }
    public IEnumerator SurfFunction(){
    disabled = true;
   yield return MovePlayerOneTile(direction);
    disabled = false;
    walkSurfBikeState = 2;
    }
     public BattleManager battleManager;

    public GameObject battlemenu;
 public IEnumerator StartWildBattle(StrInt pokemon)
    {
        inBattle = true;
        disabled = true;
        SoundManager.instance.PlaySong(2);
        WaitForSeconds wait = new WaitForSeconds(1.8f/60f);
        for(int i = 0; i < 3; i++){
            ScreenEffects.flashLevel = 0;
            for(int j = 0; j < 3; j++){
            ScreenEffects.flashLevel--;
            yield return wait;
            }
            for(int j = 0; j < 6; j++){
            ScreenEffects.flashLevel++;
            yield return wait;
            }
            for(int j = 0; j < 3; j++){
            ScreenEffects.flashLevel--;
            yield return wait;
            }
        
        }
        battleManager.battleType = BattleType.Wild;
        battleManager.enemyMons = new List<Pokemon>(new Pokemon[]{new Pokemon(pokemon.Name,pokemon.Int)});
        battlemenu.SetActive(true);
        battleManager.battleoverlay.sprite = battleManager.blank;
        battleManager.Initialize();
        yield return 0;
    }
    public IEnumerator StartTrainerBattle(int battleID)
    {
        disabled = true;
        battleManager.battleType = BattleType.Trainer;
        inBattle = true;
        disabled = true;
        battlemenu.SetActive(true);
        battleManager.battleoverlay.sprite = battleManager.blank;
        battleManager.battleID = battleID;
        battleManager.Initialize();
         yield return 0;
    }

public GameCursor cursor;
public Bag bag;

	public void CloseMenus(){

Inputs.Enable("start");
            bag.currentMenu = null;
            cursor.SetActive(false);
            startMenuActive = false;
            moon.selectedOption = 0;
           moon.Close();
           
	}

    	public IEnumerator UseItem(string whatItem){
        
        if (whatItem == "Bicycle")
        {
			CloseMenus ();
            switch (walkSurfBikeState)
            {
                case 0:
                SoundManager.instance.PlaySong(7);
                    yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + " got on the \nBICYCLE!"));
                    
                    walkSurfBikeState = 1;
                    break;
                case 1:
                    PlayCurrentAreaSong();
                    yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + "got off\nthe BICYCLE."));
                    walkSurfBikeState = 0;
                    break;
            }
		    moon.gameObject.SetActive(false);
             bag.gameObject.SetActive(false);
            

        }






	}
    public void RunFromBattle(){
    StartCoroutine(Run());
    }
    public IEnumerator Run(){

	if(battleManager.battleType == BattleType.Wild){
		yield return StartCoroutine(Dialogue.instance.text("Ran away\nsafely!"));
        battlemenu.SetActive(false);
        battleManager.Deactivate();
		ScreenEffects.flashLevel = 3;
        yield return new WaitForSeconds(1);
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        for(int i = 0; i < 3; i++){
            yield return wait;
            ScreenEffects.flashLevel--;
        }
       
        switch(walkSurfBikeState){
        case 2:  SoundManager.instance.FadeToSong(18); break;
        case 1: SoundManager.instance.FadeToSong(7); break;
        default: FadeToCurrentAreaSong(); break;
        }
        numberOfNoRandomBattleStepsLeft = 3;
       
		
	}
        Dialogue.instance.fastText = false;
    isMoving = false;
    inBattle = false;
    disabled = false;
}
public void PlayCurrentAreaSong(){
SoundManager.instance.PlaySong(SoundManager.MapSongs[(int)currentArea]);
}
public void FadeToCurrentAreaSong(){
SoundManager.instance.FadeToSong(SoundManager.MapSongs[(int)currentArea]);
}
public void EncounterTrainer(){


}

void OnTriggerEnter2D(Collider2D col){
if(col.gameObject.tag == "MapCollider"){
            if (GameData.atTitleScreen) return;
currentArea = col.gameObject.GetComponent<MapCollider>().mapArea;
int mapArea = (int)col.gameObject.GetComponent<MapCollider>().mapArea;
if(GameData.WaterEncounterMaps.Contains(currentArea)) areaHasWaterEncounters = true;
    else areaHasWaterEncounters = false;
if(GameData.MapGrassEncounterTableIndices[mapArea] != -1) currentAreaTable = PokemonData.encounters[GameData.MapGrassEncounterTableIndices[mapArea]];
else currentAreaTable = null;
int songIndex = SoundManager.MapSongs[mapArea];
if(SoundManager.instance.currentSong != songIndex && walkSurfBikeState == 0 && !inBattle){
    if(SoundManager.instance.isFadingSong){
       SoundManager.instance.StopFadeSong();
    }
     SoundManager.instance.FadeToSong(songIndex);
}

}

}

}
