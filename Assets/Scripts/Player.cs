using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
public class Player : MonoBehaviour
{
    public int bionumber;
 public Animator playerAnim;
    public Dialogue dia;
    public bool moving;
    public bool inBattle;
    public bool manuallyWalking;
    public bool walkedfromwarp;
    public GameObject credits;
    public int walkSurfBikeState;
    public int direction;
    public GameObject top, bottom;
    public TextDatabase IDB;
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
    public UnityEvent onHitWarp, onLoadMap;
    public SpriteRenderer emotionbubble;
    public MainMenu moon;
    public bool shopup;
    public ViewBio BIO;
    public bool actuallymoving;
    public bool ledgejumping;
    public GridTile facedtile;
    public bool doupdate;
    //1 up, 2down, 3 left, 4 right
    public bool cannotMoveLeft, cannotMoveRight, cannotMoveUp, cannotMoveDown;

    public float speed = 2.0f;
    public Vector3 pos;

    // Use this for initialization
    int mod(int a, int b){
        return a < 0 ? b + a % b : a % b;
    }

      void Awake()
    {
        disabled = false;
       
        onHitWarp = new UnityEvent();
        onHitWarp.AddListener(onWarp);
    }
   
    void Start()
    {
        emotionbubble.enabled = false;
        SaveData.trainerID = Random.Range(0, 65536);
        startmenuup = false;
        canInteractAgain = true;
        direction = 2;
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
            if (Inputs.held("down") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeDown" && transform.position == pos && direction == 2)
            {
                ledgejumping = true;
                direction = 2;
                playerAnim.SetFloat("movedir", direction);
                StartCoroutine(LedgeJump());

            }
            if (Inputs.held("left") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeLeft" && transform.position == pos && direction == 3)
            {
                ledgejumping = true;
                direction = 3;
                playerAnim.SetFloat("movedir", direction);
                StartCoroutine(LedgeJump());


            }
            if (Inputs.held("right") && !disabled && !ledgejumping && facedtile != null && facedtile.tag == "LedgeRight" && transform.position == pos && direction == 4)
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
                    if (transform.position == pos)
                    {
                        if (!walkedfromwarp)
                            walkedfromwarp = true;
                    }

                }
                if (Inputs.held("up"))
                {
                    if (actuallymoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    moving = true;
                    if (transform.position == pos)
                    {
                        direction = 1;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (transform.position == pos && !cannotMoveUp)
                    {
                        onLoadMap.Invoke();
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
                    if (actuallymoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    moving = true;
                    if (transform.position == pos)
                    {
                        direction = 4;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (transform.position == pos && !cannotMoveRight)
                    {
                        onLoadMap.Invoke();
                        pos += (Vector3.right);
                        actuallymoving = true;
                    }
                    else if (cannotMoveRight)
                    {
                        actuallymoving = false;
                    }

                }
             
                else if (Inputs.held("down"))  {   
                    if (actuallymoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }
                   

                    moving = true;
                    if (transform.position == pos)
                    {
                        direction = 2;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (transform.position == pos && !cannotMoveDown)
                    {
                        onLoadMap.Invoke();
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
                    if (actuallymoving && transform.position == pos)
                    {
                        walkedfromwarp = true;
                    }

                    moving = true;
                    if (transform.position == pos)
                    {
                        direction = 3;
                        playerAnim.SetFloat("movedir", direction);
                    }
                    if (transform.position == pos && !cannotMoveLeft)
                    {
                        onLoadMap.Invoke();
                        pos += (Vector3.left);
                        actuallymoving = true;
                    }
                    else if (cannotMoveLeft)
                    {
                        actuallymoving = false;
                    }
                }
                else if (transform.position == pos)
                {
                    actuallymoving = false;
                }

                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);

                if (transform.position == pos)
                {
                    if (actuallymoving)
                    {
                        GridTile currentTile = MapManager.maptiles[mod((int)transform.position.x, GameConstants.mapWidth), mod((int)transform.position.y, GameConstants.mapHeight)];
                        if (currentTile != null)
                        {
                            if (currentTile.isWarp)
                            {
                                onHitWarp.Invoke();
                            }
                        }

                        onLoadMap.Invoke();
                    }
                    moving = false;

                }

                if (Inputs.held("up") || Inputs.held("left") ||Inputs.held("right") || Inputs.held("down"))
                    moving = true;

                playerAnim.SetFloat("movingfloat", actuallymoving ? 1 : 0);
                if (transform.position == pos)
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

            if (transform.position == pos)
            {
                onLoadMap.Invoke();
                pos += (Vector3.up);
            }
        }
        else if (direction == 2)
        {
            direction = 2;
            moving = true;
            if (transform.position == pos)
            {
                onLoadMap.Invoke();
                pos += (Vector3.right);
            }

        }
        else if (direction == 3)
        {
            direction = 3;
            moving = true;
            if (transform.position == pos)
            {
                onLoadMap.Invoke();
                pos += (Vector3.down);
            }
        }
        else if (direction == 4)
        {
            direction = 4;
            moving = true;
            if (transform.position == pos)
            {
                onLoadMap.Invoke();
                pos += (Vector3.left);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        while (moving)
        {
            yield return new WaitForSeconds(.1f);
            if (transform.position == pos)
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
        while (transform.position != pos)
        {
            yield return new WaitForSeconds(0.001f);
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 4);



            if (transform.position == pos)
            {
                onLoadMap.Invoke();
                break;
            }
            float midDistance = (direction == 2 ? transform.position.y - (pos.y + 1) : direction == 3 ? transform.position.x - (pos.x + 1) : transform.position.x - (pos.x - 1));
            if (Mathf.Abs(midDistance) < Time.deltaTime * 4 && !reachedMiddle)
            {
                reachedMiddle = true;
                Debug.Log("At middle");
                onLoadMap.Invoke();

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
        pos = transform.position;
        disabled = true;
        onLoadMap.Invoke();
        WaitToInteract();

    }
    public void onWarp()
    {

        Debug.Log("Detected player.");
        if (transform.position == pos && walkedfromwarp)
        {
            walkedfromwarp = false;
            WarpInfo warp = MapManager.maptiles[(int)transform.position.x, (int)transform.position.y].tileWarp;
            Warp(new Vector2(warp.warpposx, warp.warpposy));

        }
    }
   


    // Update is called once per frame
    void Update()
    {
        isDisabled = disabled;
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
			if (transform.position == pos) {
				transform.localPosition = new Vector3 (Mathf.Round (transform.localPosition.x), Mathf.Round (transform.localPosition.y), 0);
				pos = transform.position;
			}
			if (direction == 1) {
                itemCheck =  MapManager.maptiles[mod((int)transform.position.x, GameConstants.mapWidth), mod((int)transform.position.y + 1, GameConstants.mapHeight)];

            }
            if (direction == 2)
            {


                itemCheck = MapManager.maptiles[mod((int)transform.position.x, GameConstants.mapWidth), mod((int)transform.position.y - 1, GameConstants.mapHeight)];

            }
            if (direction == 3)
            {

                itemCheck = MapManager.maptiles[mod((int)transform.position.x - 1, GameConstants.mapWidth), mod((int)transform.position.y, GameConstants.mapHeight)];
            }
            if (direction == 4)
            {

                itemCheck = MapManager.maptiles[mod((int)transform.position.x + 1, GameConstants.mapWidth), mod((int)transform.position.y, GameConstants.mapHeight)];
            }
            if (itemCheck != null)
            {
                    facedtile = itemCheck;
                }
                else facedtile = null;
            }
            else facedtile = null;

			if (itemCheck != null) {
            //print (itemCheck.distance.ToString ());
				if (!moving && transform.position == pos) {
                    
					if (!moving && canInteractAgain && !PCactive && !shopup && !disabled && dia.finishedWithTextOverall && !startmenuup && !inBattle && !moving) {
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
                                    IDB.GetItem(itemCheck.tiledata.itemName, itemCheck.tiledata.coinamount);
                                    itemCheck.hasItem = false;
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
        if (transform.position == pos)
        {
            GridTile tileToCheck;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x - 1, GameConstants.mapWidth), mod((int)transform.position.y,GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveLeft = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall;
            }
            else cannotMoveLeft = false;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x + 1, GameConstants.mapWidth), mod((int)transform.position.y, GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveRight = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall;
            }
            else cannotMoveRight = false;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x, GameConstants.mapWidth), mod((int)transform.position.y + 1,GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveUp = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall;
            }
            else cannotMoveUp = false;
            tileToCheck = MapManager.maptiles[mod((int)transform.position.x, GameConstants.mapWidth), mod((int)transform.position.y - 1,GameConstants.mapHeight)];
            if (tileToCheck != null)
            {
                cannotMoveDown = tileToCheck.isWall || tileToCheck.tag.Contains("Ledge") || tileToCheck.hasItemBall;
            }
            else cannotMoveDown = false;
        }
    }
}
