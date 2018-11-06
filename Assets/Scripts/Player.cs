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
Null,
Up,
Down,
Left,
Right
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
    public bool canInteractAgain;
    public bool PCactive;
    public static bool disabled = true;
    public bool isDisabled;
    public GridTile itemCheck;
    public bool startmenuup;
    public GameObject startmenu;
    public bool displayingEmotion;
    public bool amenuactive;
    public Sprite[] bubbles;
    public UnityEvent onHitWarp, onLoadMap;
    public SpriteRenderer emotionbubble;
    public MainMenu moon;
    public bool shopup;
    public ViewBio viewBio;
    public bool isMoving;
    public bool ledgejumping;
    public GridTile facedtile;
    public int numberOfNoRandomBattleStepsLeft;
    public WarpInfo warpInfo;
	public bool isWarping;
	public AudioClip collisionClip, ledgeJumpClip, openStartMenuClip;
	public float collisionSoundTimer;

    //1 up, 2down, 3 left, 4 right
    public bool cannotMoveLeft, cannotMoveRight, cannotMoveUp, cannotMoveDown;

    public float speed = 2.0f;
    public Vector3 pos;
    public int holdFrames;
    // Use this for initialization
    int mod(int a, int b){
        return a < 0 ? b + a % b : a % b;
    }

    public static Player instance;

      void Awake()
    {
        instance = this;
        disabled = false;

        onHitWarp = new UnityEvent();
        onHitWarp.AddListener(onWarp);
    }

    void Start()
    {
       GameData.party.Add(new Pokemon("Pikachu",50));
        emotionbubble.enabled = false;
        GameData.trainerID = Random.Range(0, 65536);
        startmenuup = false;
        canInteractAgain = true;
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

        if (Dialogue.instance.finishedWithTextOverall && !disabled && !startmenuup && !shopup && !inBattle && !manuallyWalking)
        {
             



            //If we're not ledge jumping already, the adjacent tile is a ledge, and we're exactly on a tile, ledge jump
            if (Inputs.held("down") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeDown" && transform.position == pos && direction == Direction.Down && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Down;
                playerAnim.SetFloat("movedir", (int)direction);
                StartCoroutine(LedgeJump());

            }
            if (Inputs.held("left") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeLeft" && transform.position == pos && direction == Direction.Left && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Left;
                playerAnim.SetFloat("movedir", (int)direction);
                StartCoroutine(LedgeJump());


            }
            if (Inputs.held("right") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeRight" && transform.position == pos && direction == Direction.Right && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Right;
                playerAnim.SetFloat("movedir", (int)direction);
                StartCoroutine(LedgeJump());


            }
          
            if (!ledgejumping)
            {
                 
                if(Inputs.held("up")||Inputs.held("down")||Inputs.held("left")||Inputs.held("right")){
                if(!holdingDirection){
                collisionSoundTimer += 0.3f;
                }
                }

                if (isMoving)
                {
                    if (transform.position == pos)
                    {
                        if (!walkedfromwarp)
                            walkedfromwarp = true;
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
                        playerAnim.SetFloat("movedir", (int)direction);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(!cannotMoveUp){ 
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
                        playerAnim.SetFloat("movedir", (int)direction);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(!cannotMoveRight){
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
                        playerAnim.SetFloat("movedir", (int)direction);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(!cannotMoveDown){ 
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
                        playerAnim.SetFloat("movedir", (int)direction);
                    }
                    if (transform.position == pos && holdFrames > 2)
                    {
                       
                        if(!cannotMoveLeft){ 
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

                    if (isMoving)
                    {
                        //The player moved onto a tile, run any checks needed;
                        GridTile currentTile = MapManager.maptiles[mod((int)transform.position.x, GameData.mapWidth), mod((int)transform.position.y, GameData.mapHeight)];
                        if (currentTile != null)
                        {
                            if (currentTile.isWarp && currentTile.tileWarp.warpType == WarpType.WalkOnWarp)
                            {
                                warpInfo = currentTile.tileWarp;
                                onHitWarp.Invoke();
                            }
                            if(numberOfNoRandomBattleStepsLeft > 0) numberOfNoRandomBattleStepsLeft--;
                            if (currentTile.hasGrass || currentTile.isWater){ 
                                
                                if(numberOfNoRandomBattleStepsLeft == 0) {
                               // int rand = Random.Range(0,256);
                               int rand = 255;
                                if(rand < PokemonData.encounters[0].encounterChance){
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
                                Debug.Log("Chosen Pokemon: " + PokemonData.encounters[0].slots[chosenIndex].ToString());
                                isMoving = false;
                                holdingDirection = false;
                                StartCoroutine(StartWildBattle(PokemonData.encounters[0].slots[chosenIndex]));
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

               
                if (transform.position == pos) playerAnim.SetFloat("movedir", (int)direction);


                    collisionSoundTimer += Time.deltaTime;
                    
                    if(collisionSoundTimer >= 0.3f && (holdingDirection && facingWall()) && !ledgejumping && holdFrames > 2){

                   SoundManager.instance.sfx.PlayOneShot(collisionClip,0.5f);
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

    public IEnumerator MovePlayerOneTile(Direction dir)
    {
if(!manuallyWalking){
        if (dir == Direction.Up)
        {
            direction = Direction.Up;
            holdingDirection = true;

            if (transform.position == pos)
            {
                playerAnim.SetFloat("movedir", (int)direction);
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
                playerAnim.SetFloat("movedir", (int)direction);
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
                playerAnim.SetFloat("movedir", (int)direction);
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
                playerAnim.SetFloat("movedir", (int)direction);
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
        ledgejumping = false;
        facedtile = null;
        disabled = false;
        playerAnim.SetBool("ledgejumping", ledgejumping);
       



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
        GridTile currentTile = MapManager.maptiles[mod((int)transform.position.x, GameData.mapWidth), mod((int)transform.position.y, GameData.mapHeight)];
        if(currentTile != null && currentTile.isWarp && currentTile.tileWarp.forceMove){
        StartCoroutine(MovePlayerOneTile(direction));
        }
       

    }
    public void onWarp()
    {
        holdingDirection = false;
        isWarping = true;
        Debug.Log("Detected player.");
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
       
        isDisabled = disabled;
		playerAnim.SetFloat("walkbikesurfstate", walkSurfBikeState);
		if (viewBio.bioscreen.enabled) {

			disabled = true;
		}

       
		startmenu.SetActive (startmenuup);
		if (!disabled && !amenuactive &&!startmenuup) {
            if (Inputs.pressed("start") && !isMoving) {
                SoundManager.instance.sfx.PlayOneShot(openStartMenuClip,0.5f);
				startmenuup = true;
				moon.Initialize ();
			}
			top.SetActive (!disabled);
			bottom.SetActive (!disabled);

			playerAnim.SetInteger ("movedirection", (int)direction);

            if (Inputs.released("down") || Inputs.released("right") || Inputs.released("left") || Inputs.released("up")) {
				if (!manuallyWalking) holdingDirection = false;

			}
			if (transform.position == pos) {
				transform.localPosition = new Vector3 (Mathf.Round (transform.localPosition.x), Mathf.Round (transform.localPosition.y), 0);
				pos = transform.position;
			}
			if (direction == Direction.Up) {
                itemCheck =  MapManager.maptiles[mod((int)transform.position.x, GameData.mapWidth), mod((int)transform.position.y + 1, GameData.mapHeight)];

            }
            if (direction == Direction.Down)
            {


                itemCheck = MapManager.maptiles[mod((int)transform.position.x, GameData.mapWidth), mod((int)transform.position.y - 1, GameData.mapHeight)];

            }
            if (direction == Direction.Left)
            {

                itemCheck = MapManager.maptiles[mod((int)transform.position.x - 1, GameData.mapWidth), mod((int)transform.position.y, GameData.mapHeight)];
            }
            if (direction == Direction.Right)
            {

                itemCheck = MapManager.maptiles[mod((int)transform.position.x + 1, GameData.mapWidth), mod((int)transform.position.y, GameData.mapHeight)];
            }
            if (itemCheck != null)
            {
                    facedtile = itemCheck;
                }
                else facedtile = null;
            }
            else facedtile = null;

			if (itemCheck != null) {
         
				if (!holdingDirection && transform.position == pos) {

					if (!holdingDirection && !isMoving && canInteractAgain && !PCactive && !shopup && !disabled && Dialogue.instance.finishedWithTextOverall && !startmenuup && !inBattle && !ledgejumping) {
						if (itemCheck.isInteractable) {
							if (Inputs.pressed("a")) {
                            if (itemCheck.tiledata.hasText || itemCheck.hasItem) {
									if (itemCheck.tiledata.hasItem) {
                                    if (itemCheck.hasItemBall)
                                    {
                                        itemCheck.hasItemBall = false;
                                        onLoadMap.Invoke();
                                    }
											canInteractAgain = false;
                                    textData.GetItem(itemCheck.tiledata.itemName, itemCheck.tiledata.coinamount);
                                    itemCheck.hasItem = false;
									}
										canInteractAgain = false;
										textData.PlayText (itemCheck.tiledata.TextID, itemCheck.tiledata.coinamount);
									}
								}
							}
						}
					}
				}


			//Check collision here?
//If we just started surfing, skip checking collision until we're in the water
//if(!startingSurf)...
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
    void CheckCollision()
    {
        if (transform.position == pos)
        {
            GridTile tileToCheck;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x - 1, GameData.mapWidth), mod((int)transform.position.y,GameData.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveLeft = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2);
            }
            else cannotMoveLeft = true;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x + 1, GameData.mapWidth), mod((int)transform.position.y, GameData.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveRight = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall  || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2);
            }
            else cannotMoveRight = true;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x, GameData.mapWidth), mod((int)transform.position.y + 1,GameData.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveUp = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2);
            }
            else cannotMoveUp = true;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x, GameData.mapWidth), mod((int)transform.position.y - 1,GameData.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveDown = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall || (tileToCheck.tag.Contains("Water") && walkSurfBikeState != 2);
            }
            else cannotMoveDown = true;
        }
    }
     public BattleManager battleManager;

    public GameObject battlemenu;

 public IEnumerator StartWildBattle(StrInt pokemon)
    {
        disabled = true;
        WaitForSeconds wait = new WaitForSeconds(2f/60f);
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
        inBattle = true;
        disabled = true;
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
            startmenuup = false;
            moon.selectedOption = 0;
            moon.currentmenu = null;
           
	}

    	public IEnumerator UseItem(string whatItem){
        
        if (whatItem == "Bicycle")
        {
			CloseMenus ();
            switch (walkSurfBikeState)
            {
                case 0:
                    yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + " got on the"));
                    yield return StartCoroutine(Dialogue.instance.line("BICYCLE!"));
                    yield return StartCoroutine(Dialogue.instance.done());
                    walkSurfBikeState = 1;
                    break;
                case 1:

                    yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + " got off"));
                    yield return StartCoroutine(Dialogue.instance.line("the BICYCLE."));
                    yield return StartCoroutine(Dialogue.instance.done());
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
		yield return StartCoroutine(Dialogue.instance.text("Ran away"));
		yield return StartCoroutine(Dialogue.instance.line("safely!"));
        yield return StartCoroutine(Dialogue.instance.done());
        battlemenu.SetActive(false);
        battleManager.Deactivate();
		ScreenEffects.flashLevel = 3;
        yield return new WaitForSeconds(1);
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        for(int i = 0; i < 3; i++){
            yield return wait;
            ScreenEffects.flashLevel--;
        }
        numberOfNoRandomBattleStepsLeft = 3;
        inBattle = false;
        disabled = false;
		
	}
}
}
