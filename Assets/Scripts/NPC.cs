using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Enum for all the different types of NPCS,
which are static NPC's, ones that change direction,
and ones that can move.
*/
public enum NPCType{
    Static,
    Direction,
    MovingOneDirection,
    Moving
}

//for Single direction NPCS
public enum NPCDirection{
Vertical,
Horizontal
}
public class NPC : MonoBehaviour
{
    public Texture2D[] moveUpSprites,moveDownSprites,moveLeftSprites,moveRightSprites;
    public Sprite[] idleUpSprites, idleDownSprites, idleLeftSprites, idleRightSprites;
    public bool[] cannotMove = new bool[4], objectExists = new bool[4]; //follows the enum order
    public Direction direction;
    public Direction staticDirection;
    public NPCType npcType;
    public int movementDelay; //set to a random number from 0 to 127
    public int frameTimer; //timer for decrementing the movementDelay every 2 frames;
    public SpriteRenderer topRenderer, bottomRenderer;
    public bool isMoving;
    public float movingTimer; //moving takes 0.55 seconds (33 frames)
    public NPCDirection npcDirection;
    public Vector3 homePos;
    public bool isTrainer;
    public int trainerSpriteIndex;
    public int textIndex;
    public GameObject movingHitbox, exclamationBox;
    public int framesSinceMoving;
    public Vector2 playerDistance;
    public PokemonObject pokemonObject;
    public NPCTileDialogue npcDialogue;

    // Start is called before the first frame update
    void Start()
    {
        npcDialogue = GetComponent<NPCTileDialogue>();
        UpdateSprite();
        homePos = transform.position;
        frameTimer = 2;
        movementDelay = Random.Range(0,128);
        pokemonObject = GetComponent<PokemonObject>();
        pokemonObject.onDisabled.AddListener(OnDisableNPC);
    }
private bool dontUpdateDireciton;
    // Update is called once per frame
    void UpdateDirectionBool(){
         if(movementDelay == 1) dontUpdateDireciton = true; //moonwalking bug if exiting npc dialogue the frame before moving
             Dialogue.instance.onFinishText.RemoveListener(UpdateDirectionBool);
    }
    void Update()
    {
        CheckCollision();
        if (!Dialogue.instance.finishedText || Player.instance.menuActive || Player.instance.inBattle || GameData.instance.isPaused) return;
        if(isMoving) return;
        if (pokemonObject.isDisabled) return;
        
        frameTimer--;
        if(frameTimer == 0){
             movementDelay--;
             frameTimer = 2;
             if(movementDelay <= 0){
             DoMovement();
            movementDelay = Random.Range(0,128);
             }
        }
        

    }
    void DoMovement(){
Direction chosenDir;
        switch(npcType){
            case NPCType.Static:
            direction = staticDirection;
            UpdateSprite();
            break;
            case NPCType.Direction:
            int newDir = Random.Range(0,4);
            direction = (Direction)newDir;
            UpdateSprite();
            break;
            case NPCType.MovingOneDirection:
            switch(npcDirection){

                case NPCDirection.Horizontal:
                int random = Random.Range(0,2);
                if(random == 0){
                    chosenDir = Direction.Left;
                    StartCoroutine(MoveNPC(chosenDir));
                }else {
                    chosenDir = Direction.Right;
                    StartCoroutine(MoveNPC(chosenDir));
                }
                break;
                case NPCDirection.Vertical:
                random = Random.Range(0,2);
                if(random == 0){
                    chosenDir = Direction.Up;
                    StartCoroutine(MoveNPC(chosenDir));
                }else {
                    chosenDir = Direction.Down;
                    StartCoroutine(MoveNPC(chosenDir));
                }
                break;
            }
            break;
            case NPCType.Moving:
             newDir = Random.Range(0,4);
            chosenDir = (Direction)newDir;
            StartCoroutine(MoveNPC(chosenDir));
            break;



        }
    }
    public IEnumerator MoveNPC(Direction direction){
        movingTimer = 0;
        if(!dontUpdateDireciton) this.direction = direction;
        dontUpdateDireciton = false;
     UpdateSprite();
     if(cannotMove[(int)direction]){ 
         yield break;

     }
     isMoving = true;
     Vector2 initialPos = transform.position;
     Vector3 delta = (direction == Direction.Up ? Vector3.up : direction == Direction.Down ? Vector3.down : direction == Direction.Left ? Vector3.left : Vector3.right);
     Vector2 targetPos = transform.position + delta;
     movingHitbox.transform.position = targetPos;
     while(movingTimer < 0.55f){
        movingTimer += Time.deltaTime;
        UpdateSprite();
         transform.position = Vector2.Lerp(initialPos,targetPos,movingTimer/0.55f);
         movingHitbox.transform.position = targetPos;
         yield return new WaitForEndOfFrame();

     }
     isMoving = false;
     framesSinceMoving = 0;
     UpdateSprite();
     movingHitbox.transform.position = transform.position;
    }
    void UpdateSprite(){
        if(!isMoving){
            switch(direction){
                case Direction.Up:
                topRenderer.sprite = idleUpSprites[0];
                bottomRenderer.sprite = idleUpSprites[1];
                break;
                case Direction.Down:
                topRenderer.sprite = idleDownSprites[0];
                bottomRenderer.sprite = idleDownSprites[1];
                break;
                case Direction.Left:
                topRenderer.sprite = idleLeftSprites[0];
                bottomRenderer.sprite = idleLeftSprites[1];
                break;
                case Direction.Right:
                topRenderer.sprite = idleRightSprites[0];
                bottomRenderer.sprite = idleRightSprites[1];
                break;

            }
        }else{
            int frame;
            if(direction == Direction.Up || direction == Direction.Down)
             frame = Mathf.FloorToInt(movingTimer/0.55f * 4f) + 1;
             else frame = Mathf.FloorToInt(movingTimer/0.55f * 4f) % 2 + 1;
        switch(direction){
                case Direction.Up:
                topRenderer.sprite = Sprite.Create(moveUpSprites[0],new Rect(0f,64f - (float)frame/4f*64,16,16),new Vector2(0.5f,0.5f),16);
                bottomRenderer.sprite = Sprite.Create(moveUpSprites[1],new Rect(0f,64f - (float)frame/4f*64,16,16),new Vector2(0.5f,0.5f),16);
                break;
                case Direction.Down:
                topRenderer.sprite = Sprite.Create(moveDownSprites[0],new Rect(0f,64f - (float)frame/4f*64,16,16),new Vector2(0.5f,0.5f),16);
                bottomRenderer.sprite = Sprite.Create(moveDownSprites[1],new Rect(0f,64f - (float)frame/4f*64,16,16),new Vector2(0.5f,0.5f),16);
                break;
                case Direction.Left:
                topRenderer.sprite = Sprite.Create(moveLeftSprites[0],new Rect(0f,32f - (float)frame/2f*32,16,16),new Vector2(0.5f,0.5f),16);
                bottomRenderer.sprite = Sprite.Create(moveLeftSprites[1],new Rect(0f,32f - (float)frame/2f*32,16,16),new Vector2(0.5f,0.5f),16);
                break;
                case Direction.Right:
                topRenderer.sprite = Sprite.Create(moveRightSprites[0],new Rect(0f,32f - (float)frame/2f*32,16,16),new Vector2(0.5f,0.5f),16);
                bottomRenderer.sprite = Sprite.Create(moveRightSprites[1],new Rect(0f,32f - (float)frame/2f*32,16,16),new Vector2(0.5f,0.5f),16);
                break;

            }

        }


    }
    void CheckCollision(){
        CheckObjectCollision();
        if (!isMoving)
        {
            MapTile tileToCheck = null;

            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x-1,(int)transform.position.y,0));
                cannotMove[2] = tileToCheck.isWall || tileToCheck.isLedge || tileToCheck.isWater || objectExists[2] || !tileToCheck.hasTile;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x+1,(int)transform.position.y,0));
                cannotMove[3] = tileToCheck.isWall || tileToCheck.isLedge || tileToCheck.isWater || objectExists[3] || !tileToCheck.hasTile;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x,(int)transform.position.y+1,0));
                cannotMove[0] = tileToCheck.isWall || tileToCheck.isLedge || tileToCheck.isWater || objectExists[0] || !tileToCheck.hasTile;
            tileToCheck = new MapTile(new Vector3Int((int)transform.position.x,(int)transform.position.y-1,0));
                cannotMove[1] = tileToCheck.isWall || tileToCheck.isLedge || tileToCheck.isWater || objectExists[1] || !tileToCheck.hasTile;
        }
        
    if(transform.position.x - homePos.x > 3) cannotMove[3] = true;
    if(transform.position.x - homePos.x < -3) cannotMove[2] = true;
    if(transform.position.y - homePos.y > 3) cannotMove[0] = true;
    if(transform.position.y - homePos.y < -3) cannotMove[1] = true;

    }
    public LayerMask layerMask, playerMask;
    public void CheckObjectCollision(){
        //Use a raycast to check for objects such as trees, etc...
        RaycastHit2D ray; 
        ray =  Physics2D.Raycast(transform.position,(direction == Direction.Up ? Vector2.up : direction == Direction.Down ? Vector2.down : direction == Direction.Left ? Vector2.left : Vector2.right),1,playerMask);
        if(ray.collider != null && ray.collider.tag == "Player" && ray.distance <= 4.5 && isTrainer){ 
            StartEncounter();
            Debug.Log(ray.distance);
        }
         ray =  Physics2D.Raycast(transform.position + Vector3.up,Vector2.up,1,layerMask);
         if(ray.collider != null) objectExists[0] = true;
         else objectExists[0] = false;
         ray =  Physics2D.Raycast(transform.position + Vector3.down,Vector2.down,1,layerMask);
         if(ray.collider != null) objectExists[1] = true;
         else objectExists[1] = false;
         ray =  Physics2D.Raycast(transform.position + Vector3.left,Vector2.left,1,layerMask);
         if(ray.collider != null) objectExists[2] = true;
         else objectExists[2] = false;
         ray =  Physics2D.Raycast(transform.position + Vector3.right,Vector2.right,1,layerMask);
         if(ray.collider != null) objectExists[3] = true;
         else objectExists[3] = false;

    }
    public void StartEncounter(){
    Debug.Log("triggered an encounter");
    }
    public void FacePlayer(){
        if(Player.instance.direction == Direction.Up){
            direction = Direction.Down;
            UpdateSprite();
        }
        if(Player.instance.direction == Direction.Down){
            direction = Direction.Up;
            UpdateSprite();
        }
        if(Player.instance.direction == Direction.Left){
            direction = Direction.Right;
            UpdateSprite();
        }
        if(Player.instance.direction == Direction.Right){
            direction = Direction.Left;
            UpdateSprite();
        }
    }
   public IEnumerator NPCText(){
     Dialogue.instance.onFinishText.AddListener(UpdateDirectionBool);
        yield return npcDialogue.PlayDialogue(npcDialogue.dialogueArray);
    }
    public void OnDisableNPC()
    {
        movementDelay = Random.Range(0, 128);
    }
}
