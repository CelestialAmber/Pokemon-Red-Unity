using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPC : MonoBehaviour {
	public enum FacingDirection{ North, South, West, East};
	public FacingDirection currentdir;
	public Sprite[] upsprites, downsprites, leftsprites, rightsprites;
	public Player play;
	public SpriteRenderer top, bottom;
	public bool cannotMoveLeft, cannotMoveRight, cannotMoveDown, cannotMoveUp;
	public enum NPCType{Static, Directions, Moving  };
	public NPCType currentType;
	public Vector3 pos;
	Transform tr;
	bool faceddirection, moveddirection;
	public float facetimer, movetimer;
	public bool moving;
	public float speed = .1f;
	RaycastHit2D leftCheck ;
	RaycastHit2D rightCheck ;
	RaycastHit2D upcheck ;
	RaycastHit2D downCheck;

	public LayerMask checkmask;
	// Use this for initialization
	void Start () {
		tr = transform;

		pos = transform.localPosition;
		play = GameObject.Find ("Player").GetComponent<Player> ();
	}
	IEnumerator CheckTypes(){
		if (currentType == NPCType.Directions) {

			if (!faceddirection) {
				facetimer += Time.deltaTime;
				if (facetimer > 5) {
					facetimer = 0;
					if (play.dia.finishedWithTextOverall) {
						faceddirection = true;
					}

				}


			}
			if (faceddirection) {
				int randomface = Random.Range (0, 4);
				if (randomface == 0) {
					currentdir = FacingDirection.North;
				}
				if (randomface == 1) {
					currentdir = FacingDirection.South;
				}
				if (randomface == 2) {
					currentdir = FacingDirection.West;
				}
				if (randomface == 3) {
					currentdir = FacingDirection.East;
				}
				faceddirection = false;
			}



		}
		if (currentType == NPCType.Moving) {

			if (!moveddirection) {
				movetimer += Time.deltaTime;
				if (movetimer > 5) {
					movetimer = 0;
					if (play.dia.finishedWithTextOverall) {
						moveddirection = true;
					}


				}


			}
			if (moveddirection) {
				int randommove = Random.Range (1, 5);
				if (!moving) {
					StartCoroutine (MovePlayerOneTile (randommove));
				}
				while (moving) {
					yield return new WaitForSeconds (.1f);
					if (tr.localPosition == pos) {
						break;
					}
				}
				moveddirection = false;
			}






		}
	}
	// Update is called once per frame
	void Update () {
		CheckCollision ();
		if (play.dia.finishedWithTextOverall) {
			StartCoroutine (CheckTypes ());
		}

		if (currentdir == FacingDirection.East) {
			top.sprite = rightsprites [0];
			bottom.sprite = rightsprites [1];

		}
		if (currentdir == FacingDirection.West) {

			top.sprite = leftsprites [0];
			bottom.sprite = leftsprites [1];

		}
		if (currentdir == FacingDirection.North) {

			top.sprite = upsprites [0];
			bottom.sprite = upsprites [1];

		}
		if (currentdir == FacingDirection.South) {

			top.sprite = downsprites [0];
			bottom.sprite = downsprites [1];

		}
	}
	void CheckCollision(){
		
		 leftCheck = Physics2D.Raycast (transform.position, Vector2.left, 2,checkmask);
		rightCheck = Physics2D.Raycast (transform.position, Vector2.right, 2,checkmask);
		upcheck = Physics2D.Raycast (transform.position, Vector2.up, 2,checkmask);
		downCheck = Physics2D.Raycast (transform.position, Vector2.down, 2,checkmask);
		Debug.DrawRay (transform.position, Vector3.right, Color.red);
		if (upcheck.collider != null) {

			if (upcheck.collider.tag.Contains ("WallObject")) {
				//print (upcheck.distance);
				if (upcheck.distance <= 1) {
					cannotMoveUp = true;
				} else {
					cannotMoveUp = false;

				}

			}
		} else {
			
			cannotMoveUp = false;


		}
		if (downCheck.collider != null) {

			if (downCheck.collider.tag.Contains ("WallObject")) {
				//print (downCheck.distance);
				if (downCheck.distance <= 1) {
					cannotMoveDown = true;
				} else {
					cannotMoveDown = false;

				}
			}

		} else {

			cannotMoveDown = false;

		}
		if (leftCheck.collider != null) {

			if (leftCheck.collider.tag.Contains ("WallObject")) {
				//print (leftCheck.distance);
				if (leftCheck.distance <= 1) {
					cannotMoveLeft = true;
				} else {
					cannotMoveLeft = false;

				}

			}
		} else {

			cannotMoveLeft = false;

		}
		if (rightCheck.collider != null) {

			if (rightCheck.collider.tag.Contains ("WallObject")) {
				
				if (rightCheck.distance <= 1) {
					cannotMoveRight = true;
				} else {
					cannotMoveRight = false;

				}

			}
		} else {
			
			cannotMoveRight = false;


		}

	}
	public IEnumerator MovePlayerOneTile(int direction){
		moving = true;

		if (direction == 1) {
			currentdir = FacingDirection.North;
			if (cannotMoveUp) {
				moving = false;
				yield return 0;


			}


			if (!cannotMoveUp) {
				pos += (Vector3.up);
			}
			
		} else if (direction == 2) {
			currentdir = FacingDirection.East;
			if (cannotMoveRight) {
				moving = false;
				yield return 0;


			}
			if (!cannotMoveRight) {
				pos += (Vector3.right);
			}


		} else if (direction == 3) {
			currentdir = FacingDirection.South;
			if (cannotMoveDown) {
				moving = false;
				yield return 0;


			}
			if (!cannotMoveDown) {
				pos += (Vector3.down);
			}

		} else if (direction == 4) {
			currentdir = FacingDirection.West;
			if (cannotMoveLeft) {
				moving = false;
				yield return 0;


			}
			if (!cannotMoveLeft) {
				pos += (Vector3.left);
			}

		}


		while (moving) {
			yield return new WaitForSeconds (.1f);
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, pos, Time.deltaTime * speed);
			if (tr.localPosition == pos || !moving) {
				break;
			}
		}
		moving = false;

	}
}
