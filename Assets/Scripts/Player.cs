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



public class Player : MonoBehaviour
{
    public enum MovementState{
        Walk,
        Bike,
        Surf
    }


 public Animator playerAnim;
    public bool holdingDirection;
    public bool inBattle;
    public bool manuallyWalking;
    public bool walkedfromwarp;
    public MovementState walkSurfBikeState;
    public Direction direction;
    public GameObject top, bottom;
    public static bool disabled = true;
    public bool isDisabled;
    public bool startMenuActive, menuActive;
    public bool displayingEmotion;
    public Sprite[] bubbles;
    public SpriteRenderer emotionbubble;
    public MainMenu mainMenu;
    public ViewBio viewBio;
    public bool isMoving;
    public bool ledgejumping;
    //public TileBase facedtile;
    public int numberOfNoRandomBattleStepsLeft;
	public bool isWarping;
	public AudioClip collisionClip, ledgeJumpClip, openStartMenuClip, cutClip;
	public float collisionSoundTimer;
    //1 up, 2down, 3 left, 4 right
    public bool cannotMoveLeft, cannotMoveRight, cannotMoveUp, cannotMoveDown;
    public bool canUseBike;
    public float speed = 0.0f;
    public Vector3 pos;
    public int holdFrames;
    public Map currentArea;
    public bool areaHasWaterEncounters;
    public EncounterData currentAreaTable;

    public GameObject facedObject;
    public bool facingTree;
    private bool[] objectExists = new bool[4];
    public GameObject movingHitbox;
        public UnityEvent onEncounterTrainer;

        private float baseMovementSpeed = 2f;

        public int moveFrame;

        
    // Use this for initialization

    public MapTile facedTile;

    public static Player instance;
    public List<string> doorSprites = new List<string>(new string[]{ "tile137", "tile149", "tile249", "tile480", "tile760" });

    public bool forcePlayerBikeDownwards; //used when on Cycling Road

    public bool onWarpTile;
    public TileWarp currentWarpTile;

    public bool noCollision, disableEncounters;
    void Awake()
    {

        disabled = false;
 
        pos = transform.position;
    }

    void Start()
    {
       GameData.instance.AddPokemonToParty("Mew",35);
        GameData.instance.party[0].SetMove("Cut",0);
        GameData.instance.party[0].SetMove("Surf",1);
       // GameData.instance.party[0].SetMove("Softboiled",2);
        GameData.instance.trainerID = Random.Range(0, 65536);
        direction = Direction.Down;
        pos = transform.position;
         CheckMapCollision();
    }

 

    List<string> downLedgeSprites = new List<string>(new string[]{"tile74","tile75","tile76","tile77","tile78","tile79","tile116","tile119","tile122","tile220"});
    List<string> leftLedgeSprites = new List<string>(new string[]{"tile92","tile95","tile98"});
    List<string> rightLedgeSprites = new List<string>(new string[]{"tile94","tile97","tile100","tile203"});
    IEnumerator MovementUpdate()
    {
        if(!ledgejumping)
        switch (walkSurfBikeState)
        {
            case MovementState.Walk: //Walk
                speed = baseMovementSpeed;
                break;
            case MovementState.Bike: //Bicycle is 2x faster than walking/surfing
                speed = baseMovementSpeed * 2f;
                break;
            case MovementState.Surf: //Surf
                speed = baseMovementSpeed;
                break;


        }
        

        if (Dialogue.instance.finishedText && !disabled && !menuActive && !startMenuActive && !inBattle && !manuallyWalking && !GameData.instance.atTitleScreen)
        {
             
              if(Inputs.released("left") || Inputs.released("right") || Inputs.released("up") || Inputs.released("down")){
            if(holdFrames <= 2) holdFrames = 0;
        }



            //If we're not ledge jumping already, the adjacent tile is a ledge, and we're exactly on a tile, ledge jump
            if (Inputs.held("down") && !disabled && !ledgejumping && facedTile.hasTile && facedTile.isLedge && downLedgeSprites.Contains(facedTile.tileName) && transform.position == pos && direction == Direction.Down && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Down;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());

            }
            if (Inputs.held("left") && !disabled && !ledgejumping && facedTile.hasTile && facedTile.isLedge && leftLedgeSprites.Contains(facedTile.tileName) && transform.position == pos && direction == Direction.Left && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Left;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());


            }
            if (Inputs.held("right") && !disabled && !ledgejumping && facedTile.hasTile && facedTile.isLedge && rightLedgeSprites.Contains(facedTile.tileName) && transform.position == pos && direction == Direction.Right && holdFrames > 2)
            {
                ledgejumping = true;
                direction = Direction.Right;
                playerAnim.SetFloat("movedir", (int)direction + 1);
                StartCoroutine(LedgeJump());


            }
            if(holdingDirection && !isWarping && onWarpTile && currentWarpTile.warpType == WarpType.WallWarp && direction == currentWarpTile.wallDirection && transform.position == pos){
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
                    if (isMoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }
                    Direction inputDir = Inputs.held("up") ? Direction.Up : Inputs.held("down") ? Direction.Down : Inputs.held("left") ? Direction.Left: Inputs.held("right") ? Direction.Right : 0;
                     if (transform.position == pos && direction != inputDir)
                    {
                        direction = inputDir;
                        UpdateFacedTile();
                        CheckCollision();
                        playerAnim.SetFloat("movedir", (int)direction + 1);
                    }

                    holdingDirection = true;
                    if (transform.position == pos && holdFrames > 2)
                    {
                        
                        if(Inputs.held("up") ? !cannotMoveUp : Inputs.held("down") ? !cannotMoveDown : Inputs.held("left") ? !cannotMoveLeft: Inputs.held("right") ? !cannotMoveRight : false){ 
                         if(walkSurfBikeState == MovementState.Surf && facedTile.hasTile && !facedTile.isWater && !facedTile.isWall && !facedTile.isLedge) {
                        walkSurfBikeState = MovementState.Walk;
                        PlayCurrentAreaSong();
                        
                          }
                        pos += Inputs.held("up") ? Vector3.up : Inputs.held("down") ? Vector3.down : Inputs.held("left") ? Vector3.left: Inputs.held("right") ? Vector3.right : Vector3.zero;
                        isMoving = true;
                        }
                    }

                }
                else if (transform.position == pos)
                {
                    isMoving = false;
                }
                else holdFrames = 0;
                moveFrame++;
                //only update movement every 2 frames
                if(moveFrame == 2) transform.position = Vector3.MoveTowards(transform.position, pos, 0.0625f * speed);
                moveFrame %= 2;
                if(facingWall()) isMoving = false;
                if (transform.position == pos)
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
                                        Debug.Log(table.slots[chosenIndex].Item1.Length);
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

             moveFrame++;
                //only update movement every 2 frames
                if(moveFrame == 2) transform.position = Vector3.MoveTowards(transform.position, pos, 0.0625f * speed);
                moveFrame %= 2;
        
        playerAnim.SetFloat("movingfloat", isMoving ? 1 : 0);
                    

            if (transform.position == pos)
            {
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

public Vector3 directionToVector(Direction dir) => (dir == Direction.Up ? Vector3.up : dir == Direction.Down ? Vector3.down : dir == Direction.Left ? Vector3.left : dir == Direction.Right ? Vector3.right : Vector3.zero);
    public IEnumerator MovePlayerOneTile(Direction dir)
    {
if(!manuallyWalking){

            direction = dir;
            holdingDirection = true;

            if (transform.position == pos)
            {
                playerAnim.SetFloat("movedir", (int)direction + 1);
                pos += directionToVector(dir);
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
    IEnumerator LedgeJump()
    {
        holdingDirection = false;
        SoundManager.instance.sfx.PlayOneShot (ledgeJumpClip);
        playerAnim.SetBool("ledgejumping", ledgejumping);
        disabled = true;
        speed = baseMovementSpeed;
        ledgejumping = true;
        yield return MovePlayerOneTile(direction);
        yield return MovePlayerOneTile(direction);
	playerAnim.SetBool("ledgejumping", false);
	yield return new WaitForSeconds(0.1f);
        ledgejumping = false;
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
        transform.position = position;
        pos = transform.position;
        
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
 


    // Update is called once per frame
    void Update()
    {
        if (GameData.instance.atTitleScreen) return;
        StartCoroutine(MovementUpdate());
        movingHitbox.transform.position = pos;
        
       
        isDisabled = disabled;
		playerAnim.SetFloat("walkbikesurfstate", (int)walkSurfBikeState);
		if (viewBio.bioscreen.enabled) {

			disabled = true;
		}

       
		if (!disabled && !menuActive && !startMenuActive) {
            if (Inputs.pressed("start") && !isMoving) {
                SoundManager.instance.sfx.PlayOneShot(openStartMenuClip);
				startMenuActive = true;
                mainMenu.gameObject.SetActive(true);
				mainMenu.Initialize ();
			}
			top.SetActive (!disabled);
			bottom.SetActive (!disabled);

			playerAnim.SetInteger ("movedirection", (int)direction + 1);

            if (Inputs.released("down") || Inputs.released("right") || Inputs.released("left") || Inputs.released("up")) {
				if (!manuallyWalking) holdingDirection = false;
            }
        }
            

			if (facedObject != null) {
                NPC npc = null;
                if(facedObject != null){
                     npc = facedObject.GetComponent<NPC>();
                     
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
                        if(facedObject != null)
                        {
                            switch (facedObject.tag) //what tag does the interactable object have?
                            {
                                case "Slots":
                                    SlotsObject dialogueSlots = facedObject.GetComponent<SlotsObject>();
                                    StartCoroutine(dialogueSlots.PlayDialogue());
                                    return;
                                case "Pokeball":
                                    Pokeball pokeball = facedObject.GetComponent<Pokeball>();
                                    pokeball.GetItem(pokeball.item);
                                    return;



                            }
                        }
                       
                    }

							
						}
					}
				}
                
    CheckObjectCollision();
        
	}

	
	public IEnumerator DisplayEmotiveBubble(int type){
		disabled = true;
		displayingEmotion = true;
		emotionbubble.enabled = true;
		emotionbubble.sprite = bubbles [type];
		yield return new WaitForSeconds (1);
		emotionbubble.enabled = false;
		displayingEmotion = false;

        disabled = false;


	}
    public void UpdateFacedTile(){
        MapTile tileToCheck = null;
        Vector3 offset = directionToVector(direction);
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
        yield return Dialogue.instance.text(MonName + " hacked\\laway with CUT!");
        SoundManager.instance.sfx.PlayOneShot(cutClip);
         facedObject.GetComponent<PokemonTree>().Cut();
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
    walkSurfBikeState = MovementState.Surf;
   yield return MovePlayerOneTile(direction);
    disabled = false;
    
    }
     public BattleManager battleManager;

    public GameObject battlemenu;
 public IEnumerator StartWildBattle(System.Tuple<string,int> pokemon)
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
        battleManager.enemyMons = new List<Pokemon>(new Pokemon[]{new Pokemon(pokemon.Item1,pokemon.Item2,true)});
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
            bag.Close();
            cursor.SetActive(false);
            startMenuActive = false;
            mainMenu.selectedOption = 0;
           mainMenu.Close();
           
	}

    public void UseItem(string whatItem)
    {
        StartCoroutine(UseItemFunction(whatItem));
    }
    	public IEnumerator UseItemFunction(string whatItem){ //function called when using an item
        
        if (whatItem == "Bicycle")
        {
			CloseMenus ();
            switch (walkSurfBikeState)
            {
                case  MovementState.Walk:
                SoundManager.instance.PlaySong(7); //play the biking music
                    yield return Dialogue.instance.text(GameData.instance.playerName + " got on the\\lBICYCLE!");
                    
                    walkSurfBikeState =  MovementState.Bike;
                    break;
                case  MovementState.Bike:
                    PlayCurrentAreaSong();
                    yield return Dialogue.instance.text(GameData.instance.playerName + " got off\\lthe BICYCLE.");
                    walkSurfBikeState = MovementState.Walk;
                    break;
            }
		    mainMenu.gameObject.SetActive(false);
             bag.gameObject.SetActive(false);
            

        }






	}
    public void RunFromBattle(){
    StartCoroutine(Run());
    }
    public IEnumerator Run(){

	if(battleManager.battleType == BattleType.Wild){
		yield return Dialogue.instance.text("Ran away\\lsafely!");
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
        case  MovementState.Surf:  SoundManager.instance.FadeToSong(17); break;
        case  MovementState.Bike: SoundManager.instance.FadeToSong(7); break;
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
SoundManager.instance.PlaySong((int)SoundManager.MapSongs[(int)currentArea]);
}
public void FadeToCurrentAreaSong(){
SoundManager.instance.FadeToSong((int)SoundManager.MapSongs[(int)currentArea]);
}
public void EncounterTrainer(){


}
public void DoWarp(TileWarp tileWarp){
    holdingDirection = false;
        isWarping = true;
        if(tileWarp.warpType == WarpType.WallWarp) walkedfromwarp = true;
        if (walkedfromwarp && transform.position == pos)
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
int songIndex = (int)SoundManager.MapSongs[mapArea];
if(SoundManager.instance.currentSong != songIndex && walkSurfBikeState == MovementState.Walk && !inBattle && !CreditsHandler.instance.isPlayingCredits){
    if(SoundManager.instance.isFadingSong){
       SoundManager.instance.StopFadeSong();
    }
     SoundManager.instance.FadeToSong(songIndex);
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
