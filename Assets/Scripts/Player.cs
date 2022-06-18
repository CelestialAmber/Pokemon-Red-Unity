using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


public enum Direction{
Up,
Down,
Left,
Right,
Null
}

public class Player : Singleton<Player> {

    public enum MovementState{
        Walk,
        Bike,
        Surf
    }


    public Animator playerAnim;
    public MovementState walkSurfBikeState;
    //1 up, 2 down, 3 left, 4 right
    public Direction direction;
    public GameObject top, bottom;
    public Sprite[] bubbles;
    public SpriteRenderer emotionbubble;
    public ViewBio viewBio;
    //public TileBase facedtile;
    public int numberOfNoRandomBattleStepsLeft;
	public AudioClip collisionClip, ledgeJumpClip, openStartMenuClip, cutClip;
	public float collisionSoundTimer;
    public float speed = 0.0f;
    public Vector3 targetPos;
    public int holdFrames;
    public Map currentArea;
    public EncounterData currentAreaTable;
    public GameObject facedObject;
    private bool[] objectExists = new bool[4];
    public GameObject movingHitbox;
    public UnityEvent onEncounterTrainer;
    private float baseMovementSpeed = 8f; //2 with regular Update
    public int moveFrame;
    public bool holdingDirection;
    public bool inBattle;
    public bool manuallyWalking;
    public bool walkedfromwarp;
    public bool isDisabled = true;
    public bool startMenuActive, menuActive;
    public bool displayingEmotion;
    public bool isMoving;
    public bool ledgejumping;
    public bool areaHasWaterEncounters;
    public bool facingTree;
    public bool isWarping;
    public bool cannotMoveLeft, cannotMoveRight, cannotMoveUp, cannotMoveDown;
    public bool canUseBike;
    public MapTile facedTile;
    List<string> doorSprites = new List<string>(new string[]{ "tile137", "tile149", "tile249", "tile480", "tile760" });
    public bool forcePlayerBikeDownwards; //used when on Cycling Road
    public bool onWarpTile;
    public TileWarp currentWarpTile;
    public bool noCollision, disableEncounters;
    public GameCursor cursor;
    public Bag bag;


    void Awake()
    {
        isDisabled = false;
        targetPos = transform.position;
    }

    void Start()
    {
        GameData.instance.AddPokemonToParty(PokemonEnum.Mew,35);
        GameData.instance.party[0].SetMove(Moves.Cut,0);
        GameData.instance.party[0].SetMove(Moves.Surf,1);
        GameData.instance.party[0].SetMove(Moves.Softboiled,2);
        GameData.instance.trainerID = Random.Range(0, 65536);
        direction = Direction.Down;
        targetPos = transform.position;
        CheckMapCollision();
    }

 

    List<string> downLedgeSprites = new List<string>(new string[]{"Tileset_73","Tileset_74","Tileset_75","Tileset_76","Tileset_77","Tileset_78","Tileset_117","Tileset_120","Tileset_123"});
    List<string> leftLedgeSprites = new List<string>(new string[]{"Tileset_93","Tileset_96","Tileset_99"});
    List<string> rightLedgeSprites = new List<string>(new string[]{"Tileset_95","Tileset_98","Tileset_101"});


    IEnumerator MovementUpdate(){
        if(!ledgejumping){
            switch (walkSurfBikeState){
                case MovementState.Walk: //Walk
                    speed = baseMovementSpeed;
                    break;
                case MovementState.Bike: //Bicycle is 2x faster than walking/surfing
                    speed = baseMovementSpeed * 2f;
                    break;
                case MovementState.Surf: //Surf
                    speed = baseMovementSpeed;
                    break;
                //if on cycling road, add extra speed boost
            }
        }
        

        if (Dialogue.instance.finishedText && !isDisabled && !menuActive && !startMenuActive && !inBattle && !manuallyWalking && !GameData.instance.atTitleScreen){
         
            if(Inputs.released("left") || Inputs.released("right") || Inputs.released("up") || Inputs.released("down")){
                if(holdFrames <= 2) holdFrames = 0;
            }

            //If we're not ledge jumping already, the adjacent tile is a ledge, and we're exactly on a tile, ledge jump
            if (Inputs.held("down") && !isDisabled && !ledgejumping && facedTile.hasTile && facedTile.isLedge && downLedgeSprites.Contains(facedTile.tileName) && transform.position == targetPos && direction == Direction.Down && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Down;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());

            }
            if (Inputs.held("left") && !isDisabled && !ledgejumping && facedTile.hasTile && facedTile.isLedge && leftLedgeSprites.Contains(facedTile.tileName) && transform.position == targetPos && direction == Direction.Left && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Left;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());


            }
            if (Inputs.held("right") && !isDisabled && !ledgejumping && facedTile.hasTile && facedTile.isLedge && rightLedgeSprites.Contains(facedTile.tileName) && transform.position == targetPos && direction == Direction.Right && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Right;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());


            }
            if(holdingDirection && !isWarping && onWarpTile && currentWarpTile.warpType == WarpType.WallWarp && direction == currentWarpTile.wallDirection && transform.position == targetPos){
                        SoundManager.instance.PlayGoOutsideSound();
                        DoWarp(currentWarpTile);

            }
          
            if (!ledgejumping)
            {                
                if (Inputs.held("up")||Inputs.held("down")||Inputs.held("left")||Inputs.held("right"))
                {
                    holdFrames++;
                     if(!holdingDirection){
                collisionSoundTimer += 0.3f;
                }
                    if (isMoving && transform.position == targetPos)
                    {
                        walkedfromwarp = true;
                    }
                    Direction inputDir = Inputs.held("up") ? Direction.Up : Inputs.held("down") ? Direction.Down : Inputs.held("left") ? Direction.Left: Inputs.held("right") ? Direction.Right : 0;
                     if (transform.position == targetPos && direction != inputDir)
                    {
                        direction = inputDir;
                        UpdateFacedTile();
                        CheckCollision();
                        playerAnim.SetFloat("movedir", (int)direction + 1);
                    }

                    holdingDirection = true;
                    if (transform.position == targetPos && holdFrames > 2)
                    {
                        
                        if(Inputs.held("up") ? !cannotMoveUp : Inputs.held("down") ? !cannotMoveDown : Inputs.held("left") ? !cannotMoveLeft: Inputs.held("right") ? !cannotMoveRight : false){ 
                         if(walkSurfBikeState == MovementState.Surf && facedTile.hasTile && !facedTile.isWater && !facedTile.isWall && !facedTile.isLedge) {
                        walkSurfBikeState = MovementState.Walk;
                        PlayCurrentAreaSong();
                        
                          }
                        targetPos += Inputs.held("up") ? Vector3.up : Inputs.held("down") ? Vector3.down : Inputs.held("left") ? Vector3.left: Inputs.held("right") ? Vector3.right : Vector3.zero;
                        isMoving = true;
                        }
                    }

                }
                else if (transform.position == targetPos)
                {
                    isMoving = false;
                }
                else holdFrames = 0;
                UpdateMovement();
                if(facingWall()) isMoving = false;
                if (transform.position == targetPos)
                {

                    if (isMoving) //did we move onto a new tile?
                    {
                        //The player moved onto a tile, run any checks needed;
                        CheckCollision(); //update the tile in front of us
                        CheckMapCollision();
                        UpdateFacedTile();
                        if (!walkedfromwarp)
                            walkedfromwarp = true;
                        MapTile currentTile = new MapTile(new Vector3Int((int)transform.position.x,(int)transform.position.y,0));
                        if (currentTile.hasTile)
                        {

                            if (onWarpTile && currentWarpTile.warpType == WarpType.WalkOnWarp)
                            {

                                if (doorSprites.Contains(currentTile.tileName)) SoundManager.instance.PlayGoInsideSound();
                                else SoundManager.instance.PlayGoOutsideSound();
                                DoWarp(currentWarpTile);
                            }
                            
                            if(numberOfNoRandomBattleStepsLeft > 0) numberOfNoRandomBattleStepsLeft--;
                            if (((currentTile.hasGrass && currentAreaTable != null) || (currentTile.isWater && areaHasWaterEncounters)) && !disableEncounters){ 
                               
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
                                        //Debug.Log(table.slots[chosenIndex].Item1.Length);
                                isMoving = false;
                                holdingDirection = false;
                                StartCoroutine(StartWildBattle(table.slots[chosenIndex]));
                                }
                                }                              
                            }                          
                        }                       
                    }
                }
                 playerAnim.SetFloat("movingfloat",holdingDirection||isMoving ? 1f : 0);
                if (Inputs.held("up") || Inputs.held("left") ||Inputs.held("right") || Inputs.held("down")) holdingDirection = true;

               
                if (transform.position == targetPos) playerAnim.SetFloat("movedir", (int)direction + 1);
                collisionSoundTimer += Time.deltaTime;                
                if(collisionSoundTimer >= 0.3f && (holdingFacingDirection() && facingWall()) && !ledgejumping && holdFrames > 2){
                    SoundManager.instance.sfx.PlayOneShot(collisionClip);
                    collisionSoundTimer = 0;
                }              
                if(!holdingDirection) collisionSoundTimer = 0;
            }
        }

        if(manuallyWalking){
            UpdateMovement();
            playerAnim.SetFloat("movingfloat", isMoving ? 1 : 0);                 
            if (transform.position == targetPos)
            {
                isMoving = false;
                holdingDirection = false;
                manuallyWalking = false;
                if(!walkedfromwarp) walkedfromwarp = true;
            }
        }
        yield return 0;
    }
   

public bool facingWall() {
    return (direction == Direction.Up && cannotMoveUp) || (direction == Direction.Down && cannotMoveDown)  || (direction == Direction.Left && cannotMoveLeft) || (direction == Direction.Right && cannotMoveRight);
}



public bool holdingFacingDirection() {
    return  (direction == Direction.Up && Inputs.held("up")) || (direction == Direction.Down && Inputs.held("down")) || (direction == Direction.Left && Inputs.held("left")) || (direction == Direction.Right && Inputs.held("right"));
}




public Vector3 DirectionToVector(Direction dir) {
    return (dir == Direction.Up ? Vector3.up : dir == Direction.Down ? Vector3.down : dir == Direction.Left ? Vector3.left : dir == Direction.Right ? Vector3.right : Vector3.zero);
}



public IEnumerator MovePlayerOneTile(Direction dir)
{
        if(!manuallyWalking){
            direction = dir;
            holdingDirection = true;

            if (transform.position == targetPos)
            {
                playerAnim.SetFloat("movedir", (int)direction + 1);
                targetPos += DirectionToVector(dir);
                 isMoving = true;
            }
}
    manuallyWalking = true;
    while(manuallyWalking) yield return new WaitForEndOfFrame();
    CheckCollision();
    UpdateFacedTile();
    CheckMapCollision();
    yield return 0;
}

void UpdateMovement(){
    moveFrame++;
    //only update movement every 2 frames
    if(moveFrame == 2){
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        moveFrame = 0;
    }
}
    


    IEnumerator LedgeJump()
    {
        holdingDirection = false;
        SoundManager.instance.sfx.PlayOneShot (ledgeJumpClip);
        playerAnim.SetBool("ledgejumping", ledgejumping);
        isDisabled = true;
        speed = baseMovementSpeed;
        ledgejumping = true;
        targetPos += 2 * DirectionToVector(direction);

        while(targetPos != transform.position){
            UpdateMovement();
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(3f/60f);
        }
        
        CheckCollision();
        UpdateFacedTile();
        CheckMapCollision();
	    playerAnim.SetBool("ledgejumping", false);
	    yield return new WaitForSeconds(0.1f);
        ledgejumping = false;
        isDisabled = false;
        //add the move up/down part of ledge jump animation later
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
        transform.position = position;
        targetPos = transform.position;
        
        ScreenEffects.flashLevel = 3;
        yield return wait;
        ScreenEffects.flashLevel = 0;
        
        Inputs.Enable("start");
        Inputs.enableDpad();
        isWarping = false;
        CheckCollision();
        
        MapTile currentTile = new MapTile(new Vector3Int((int)transform.position.x,(int)transform.position.y,0));
        if(currentTile.hasTile && onWarpTile && currentWarpTile.forceMove){
        StartCoroutine(MovePlayerOneTile(direction));
        }
    }


    void FixedUpdate(){
        if (GameData.instance.atTitleScreen) return;
        StartCoroutine(MovementUpdate());
        movingHitbox.transform.position = targetPos;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameData.instance.atTitleScreen) return;
        //StartCoroutine(MovementUpdate());
        //movingHitbox.transform.position = targetPos;
        
		playerAnim.SetFloat("walkbikesurfstate", (int)walkSurfBikeState);
       
		if (!isDisabled && !menuActive && !startMenuActive) {
            if (Inputs.pressed("start") && !isMoving) {
                SoundManager.instance.sfx.PlayOneShot(openStartMenuClip);
				startMenuActive = true;
                MainMenu.instance.enabled = true;
				MainMenu.instance.Initialize();
			}
			top.SetActive (!isDisabled);
			bottom.SetActive (!isDisabled);

			playerAnim.SetInteger ("movedirection", (int)direction + 1);

            if (Inputs.released("down") || Inputs.released("right") || Inputs.released("left") || Inputs.released("up")) {
				if (!manuallyWalking) holdingDirection = false;
            }
        }
            

		if (facedObject != null) {
            NPC npc = facedObject.GetComponent<NPC>();

			if (!holdingDirection && transform.position == targetPos) {
                if (!holdingDirection && !isMoving && !isDisabled && Dialogue.instance.finishedText && !startMenuActive && !menuActive && !inBattle && !ledgejumping) {
                    if (Inputs.pressed("a")){

                        if (npc != null && !npc.isMoving){
                            npc.FacePlayer();
                            if (npc.isTrainer) npc.StartEncounter();
                            else StartCoroutine(npc.NPCText());
                            return;
                        }

                        switch(facedObject.tag){ //what tag does the interactable object have?
                            case "Slots":
                                SlotsObject dialogueSlots = facedObject.GetComponent<SlotsObject>();
                                StartCoroutine(dialogueSlots.PlayDialogue());
                                return;
                            case "Pokeball":
                                Pokeball pokeball = facedObject.GetComponent<Pokeball>();
                                pokeball.GetItem();
                                return;
                        }

                    }
				}
			}
		}

        CheckObjectCollision();      
	}

	
	public IEnumerator DisplayEmotiveBubble(int type){
		isDisabled = true;
		displayingEmotion = true;
		emotionbubble.enabled = true;
		emotionbubble.sprite = bubbles[type];
		yield return new WaitForSeconds(1);
		emotionbubble.enabled = false;
		displayingEmotion = false;
        isDisabled = false;
	}


    public void UpdateFacedTile(){
        MapTile tileToCheck = null;
        Vector3 offset = DirectionToVector(direction);
        tileToCheck =  new MapTile(new Vector3Int((int)transform.position.x + (int)offset.x,(int)transform.position.y+(int)offset.y,0));

           
            if (tileToCheck.hasTile)
            {
                facedTile = tileToCheck;
            }
            else facedTile.hasTile = false;       
    }

    public LayerMask mapMask;

    void CheckCollision()
    {    
         CheckObjectCollision(); 

            MapTile tileToCheck = null;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x-1,(int)transform.position.y,0));
            cannotMoveLeft = tileToCheck.isWall || tileToCheck.isLedge || (tileToCheck.isWater && walkSurfBikeState != MovementState.Surf) || objectExists[2] || !tileToCheck.hasTile;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x+1,(int)transform.position.y,0));
            cannotMoveRight = tileToCheck.isWall || tileToCheck.isLedge || (tileToCheck.isWater && walkSurfBikeState != MovementState.Surf) || objectExists[3] || !tileToCheck.hasTile;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x,(int)transform.position.y+1,0));
            cannotMoveUp = tileToCheck.isWall || tileToCheck.isLedge || (tileToCheck.isWater && walkSurfBikeState != MovementState.Surf) || objectExists[0] || !tileToCheck.hasTile;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x,(int)transform.position.y-1,0));
            cannotMoveDown = tileToCheck.isWall || tileToCheck.isLedge || (tileToCheck.isWater && walkSurfBikeState != MovementState.Surf) || objectExists[1] || !tileToCheck.hasTile;
        
        if(noCollision){
            cannotMoveDown = cannotMoveLeft = cannotMoveRight = cannotMoveUp = false;
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

    public void CheckMapCollision(){
        foreach(MapCollider mapCol in MapManager.instance.mapColliders){
            mapCol.tilemapObject.SetActive(false);
        }
        MapManager.instance.mapColliders.Clear();
        RaycastHit2D[] hitColliders;
        for(int i= 0; i < 5; i++){
            Vector2 dir = i == 0 ? Vector2.up : i == 1 ? Vector2.down : i == 2 ? Vector2.left : i == 3 ? Vector2.right : Vector2.zero;
            float rayDist = dir.y != 0 ? 5 : 6;
            
           // Debug.DrawLine(transform.position, transform.position + (Vector3)(dir * rayDist),Color.red,6f);
            hitColliders =  Physics2D.BoxCastAll((Vector2)transform.position + new Vector2(0.5f,0),new Vector2(11,9),0,Vector2.zero);
            foreach(RaycastHit2D ray in hitColliders){
                if(ray.collider.tag == "MapCollider"){
                    MapCollider mapCollider = ray.collider.GetComponent<MapCollider>();
                    if(!MapManager.instance.mapColliders.Contains(mapCollider)){
                        MapManager.instance.mapColliders.Add(mapCollider);
                        mapCollider.tilemapObject.SetActive(true);
                    }
                }
            }
        }
    }


    public void Cut(string MonName){
        CloseMenus();
        StartCoroutine(CutFunction(MonName));
    }

    public IEnumerator CutFunction(string MonName){
        yield return Dialogue.instance.text(MonName + " hacked&laway with CUT!");
        SoundManager.instance.sfx.PlayOneShot(cutClip);
        facedObject.GetComponent<PokemonTree>().Cut();
        //Implement cutting wild grass
        isDisabled = true;
        yield return new WaitForSeconds(1);
        isDisabled = false;
    }

    public void Surf(){
        CloseMenus();
        StartCoroutine(SurfFunction());
    }

    public IEnumerator SurfFunction(){
        isDisabled = true;
        walkSurfBikeState = MovementState.Surf;
        yield return MovePlayerOneTile(direction);
        isDisabled = false;
    }

    public BattleManager battleManager;
    public GameObject battlemenu;

    public IEnumerator StartWildBattle(System.Tuple<PokemonEnum,int> pokemon)
    {
        inBattle = true;
        isDisabled = true;
        SoundManager.instance.PlaySong(Music.WildPokemonBattle);

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
        battleManager.enemyMons = new List<Pokemon>(new Pokemon[]{new Pokemon(pokemon.Item1,pokemon.Item2,true)});
        battlemenu.SetActive(true);
        battleManager.battleoverlay.sprite = battleManager.blank;
        battleManager.Initialize();
        yield return 0;
    }
    public IEnumerator StartTrainerBattle(int battleID)
    {
        isDisabled = true;
        battleManager.battleType = BattleType.Trainer;
        inBattle = true;
        battlemenu.SetActive(true);
        battleManager.battleoverlay.sprite = battleManager.blank;
        battleManager.battleID = battleID;
        battleManager.Initialize();
         yield return 0;
    }


	public void CloseMenus(){
        Inputs.Enable("start");
        //Bag.instance.Close();
        bag.Close();
        cursor.SetActive(false);
        startMenuActive = false;
        MainMenu.instance.selectedOption = 0;
        MainMenu.instance.Close();      
	}

    public void UseItem(ItemsEnum whatItem)
    {
        StartCoroutine(UseItemFunction(whatItem));
    }
    
    public IEnumerator UseItemFunction(ItemsEnum whatItem){ //function called when using an item

        switch(whatItem){ 
            case ItemsEnum.Bicycle:
		    	CloseMenus();

                switch (walkSurfBikeState)
                {
                    case MovementState.Walk:
                    SoundManager.instance.PlaySong(Music.Cycling); //play the biking music
                        yield return Dialogue.instance.text(GameData.instance.playerName + " got on the&lBICYCLE!");
                        walkSurfBikeState =  MovementState.Bike;
                        break;
                    case MovementState.Bike:
                        PlayCurrentAreaSong();
                        yield return Dialogue.instance.text(GameData.instance.playerName + " got off&lthe BICYCLE.");
                        walkSurfBikeState = MovementState.Walk;
                        break;
                }

                //Bag.instance.gameObject.SetActive(false);
                bag.gameObject.SetActive(false);
                MainMenu.instance.Close();
                break;
        }
	}

    public void RunFromBattle(){
        StartCoroutine(Run());
    }

    public IEnumerator Run(){
	    if(battleManager.battleType == BattleType.Wild){
		    yield return Dialogue.instance.text("Ran away&lsafely!");
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
            case  MovementState.Surf: SoundManager.instance.FadeToSong(Music.Ocean); break;
            case  MovementState.Bike: SoundManager.instance.FadeToSong(Music.Cycling); break;
            default: FadeToCurrentAreaSong(); break;
            }
            numberOfNoRandomBattleStepsLeft = 3;		
	    }
        Dialogue.instance.fastText = false;
        isMoving = false;
        inBattle = false;
        isDisabled = false;
    }

    public void PlayCurrentAreaSong(){
    SoundManager.instance.PlaySong(SoundManager.MapSongs[(int)currentArea]);
    }

    public void FadeToCurrentAreaSong(){
    SoundManager.instance.FadeToSong(SoundManager.MapSongs[(int)currentArea]);
    }

    public void EncounterTrainer(){
    //disable movement, and have trainer walk to player
    //other stuff...
    //start battle
    }

    public void DoWarp(TileWarp tileWarp){
        holdingDirection = false;
        isWarping = true;
        if(tileWarp.warpType == WarpType.WallWarp) walkedfromwarp = true;
        if (walkedfromwarp && transform.position == targetPos)
        {
            walkedfromwarp = false;
            StartCoroutine(Warp(new Vector2(tileWarp.warpPosX, tileWarp.warpPosY)));
        }
    }

    public List<Map> forceBikeMaps = new List<Map>(new Map[]{
    Map.Route16,
    Map.Route18
    });

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "MapCollider"){
            if (GameData.instance.atTitleScreen) return;
            MapCollider mapCollider = col.gameObject.GetComponent<MapCollider>();
            currentArea = mapCollider.mapArea;
            int mapArea = (int)mapCollider.mapArea;
            MapManager.instance.currentMapGrassTilemap = mapCollider.grassTilemap.GetComponent<TilemapRenderer>();
            canUseBike = mapCollider.canUseBike; //set the bool for whether the player can use the bike
            if(forceBikeMaps.Contains(currentArea)) walkSurfBikeState = MovementState.Bike; //if the map forces the player to use the bike, set the movement state to the bike
            else if(!canUseBike && walkSurfBikeState == MovementState.Bike) walkSurfBikeState = MovementState.Walk;
            if(currentArea == Map.Route17) forcePlayerBikeDownwards = true; //if the player is on Route 17 (Cycling Road), force the player to move downwards
            else forcePlayerBikeDownwards = false;
            if(GameData.instance.WaterEncounterMaps.Contains(currentArea)) areaHasWaterEncounters = true;
                else areaHasWaterEncounters = false;
            if(GameData.instance.MapGrassEncounterTableIndices[mapArea] != -1) currentAreaTable = PokemonData.encounters[GameData.instance.MapGrassEncounterTableIndices[mapArea]];
            else currentAreaTable = null;
            if (currentArea == Map.House) return; //if the current area is a house, don't change the music
            Music song = SoundManager.MapSongs[mapArea];
            if(SoundManager.instance.currentSong != (int)song && walkSurfBikeState == MovementState.Walk && !inBattle && !GameData.instance.isPlayingCredits){
                if(SoundManager.instance.isFadingSong){
                   SoundManager.instance.StopFadeSong();
                }
                SoundManager.instance.FadeToSong(song);
            }
        }
        if(col.tag == "Warp"){
            TileWarp tileWarp = col.GetComponent<TileWarp>();
            onWarpTile = true;
            currentWarpTile = tileWarp;
        }
    }


    void OnTriggerExit2D(Collider2D col){
        if(col.tag == "Warp"){
        onWarpTile = false;   
        }
    }

}