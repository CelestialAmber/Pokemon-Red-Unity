//roll to 7 code
//CheckPositions ();
//if (middle1 != "7") {
// for (int i = 0; i < 4; i++) {
//	row1.transform.localPosition = new Vector3 (row1.transform.localPosition.x, row1.transform.localPosition.y - 16);
//	CheckIfIllegalPosition();
//	CheckPositions ();
//	if (middle1 == "7") {
//		return;
//	}
//
//}
//}
//



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slots : MonoBehaviour {
	public string CurrentMode;
	public bool canstopthereels;
	public GameObject row1, row2, row3;
	public bool rolledone, rolledtwo, rolledthree, canroll;
	private string above1, middle1, below1, above2, middle2, below2, above3, middle3, below3;
	public Player play;
	public int frames;
	public int payout;
	public bool stayingInModeSuper;
	public int GuaranteedModeGood;
    public CustomText credittext, payouttext;
	public Sprite[] numbers = new Sprite[10];
	public int betamount;
	// Use this for initialization
	IEnumerator DecideBet(){
		int RandomNumber;
		int ModeNumber;
		yield return StartCoroutine(Dialogue.instance.slots ());

		if (Player.disabled) {

			if (Dialogue.instance.selectedOption == 0) {
				if (GameData.coins < 3) {
                    yield return StartCoroutine(Dialogue.instance.text("Not enough"));
                    yield return StartCoroutine(Dialogue.instance.line("coins!"));
					StartCoroutine(DecideBet ());
					yield break;

				}
				RandomNumber = Random.Range (0, 256);
				if (RandomNumber <= 1) {

					CurrentMode = "SUPER";
				}
				if (RandomNumber <= 54 && RandomNumber > 1) {

					CurrentMode = "GOOD";
				}
				if (RandomNumber > 54) {

					CurrentMode = "BAD";

				}
				ModeNumber = Random.Range (0, 256);
				if (ModeNumber < 1) {

					GuaranteedModeGood = 60;
				}
				if (GuaranteedModeGood != 0) {

					CurrentMode = "GOOD";
				}
				if (stayingInModeSuper) {
					CurrentMode = "SUPER";
				}
				if (CurrentMode == "SUPER") {
					stayingInModeSuper = true;
				}
				print ("Betting 3 coins");
				betamount = 3;
				GameData.coins -= 3;
				UpdateCredit ();
				rolledone = false;
				rolledtwo = false;
				rolledthree = false;
				canstopthereels = false;
				canroll = true;
                Dialogue.instance.displaysimmediate = true;
				StartCoroutine(Dialogue.instance.text ("Start!"));
			}
			if (Dialogue.instance.selectedOption == 1) {
				if (GameData.coins < 2) {
					yield return StartCoroutine(Dialogue.instance.text ("Not enough"));
					yield return StartCoroutine(Dialogue.instance.line ("coins!"));
					StartCoroutine(DecideBet ());
					yield break;

				}
				RandomNumber = Random.Range (0, 256);
				if (RandomNumber <= 1) {

					CurrentMode = "SUPER";
				}
				if (RandomNumber <= 54 && RandomNumber > 1) {

					CurrentMode = "GOOD";
				}
				if (RandomNumber > 54) {

					CurrentMode = "BAD";

				}
				ModeNumber = Random.Range (0, 256);
				if (ModeNumber < 1) {

					GuaranteedModeGood = 60;
				}
				if (GuaranteedModeGood != 0) {

					CurrentMode = "GOOD";
				}
				if (stayingInModeSuper) {
					CurrentMode = "SUPER";
				}
				if (CurrentMode == "SUPER") {
					stayingInModeSuper = true;
				}
				print ("Betting 2 coins");
				betamount = 2;
				GameData.coins -= 2;
				UpdateCredit ();
				rolledone = false;
				rolledtwo = false;

				rolledthree = false;
				canstopthereels = false;
				canroll = true;
                Dialogue.instance.displaysimmediate = true;
				StartCoroutine(Dialogue.instance.text ("Start!"));
			}
			if (Dialogue.instance.selectedOption == 2) {
				if (GameData.coins < 1) {
					yield return StartCoroutine(Dialogue.instance.text ("Not enough"));
					yield return StartCoroutine(Dialogue.instance.line ("coins!"));
					StartCoroutine (DecideBet ());
					yield break;

				}
				RandomNumber = Random.Range (0, 256);
				if (RandomNumber <= 1) {

					CurrentMode = "SUPER";
				}
				if (RandomNumber <= 54 && RandomNumber > 1) {

					CurrentMode = "GOOD";
				}
				if (RandomNumber > 54) {

					CurrentMode = "BAD";

				}
				ModeNumber = Random.Range (0, 256);
				if (ModeNumber < 1) {

					GuaranteedModeGood = 60;
				}
				if (GuaranteedModeGood != 0) {

					CurrentMode = "GOOD";
				}
				if (stayingInModeSuper) {
					CurrentMode = "SUPER";
				}
				if (CurrentMode == "SUPER") {
					stayingInModeSuper = true;
				}
				print ("Betting 1 coin");
				betamount = 1;
				GameData.coins -= 1;
				UpdateCredit ();
				rolledone = false;
				rolledtwo = false;
				rolledthree = false;
				canstopthereels = false;
				canroll = true;
                Dialogue.instance.displaysimmediate = true;
				StartCoroutine(Dialogue.instance.text ("Start!"));
			}
		}


	}
	void UpdateCredit(){
		if (GameData.coins > 9999) {
			GameData.coins = 9999;
		}
        credittext.text = (GameData.coins > 999 ? "" : GameData.coins > 99 ? "0" : GameData.coins > 9 ? "00" : "000" ) + GameData.coins.ToString();
	}
	void UpdatePayout(){
        payouttext.text = (payout > 999 ? "" : payout > 99 ? "0" : payout > 9 ? "00" : "000") + payout.ToString();
	}
	public IEnumerator Initialize () {
		GuaranteedModeGood = 0;
		UpdateCredit ();
		UpdatePayout ();
		row1.transform.localPosition = new Vector3 (row1.transform.localPosition.x, -152, 0);
		row2.transform.localPosition = new Vector3 (row2.transform.localPosition.x, -152, 0);
		row3.transform.localPosition = new Vector3 (row3.transform.localPosition.x, -152, 0);
		canroll = false;
		yield return StartCoroutine(Dialogue.instance.text ("Bet how many"));
		yield return StartCoroutine(Dialogue.instance.line ("coins?"));
		StartCoroutine(DecideBet ());


	}
			public void Exit(){
        Inputs.Enable("start");
		Player.disabled = false;
		rolledone = false;
		rolledtwo = false;
		rolledthree = false;
		canroll = false;


		Dialogue.instance.Deactivate ();
		play.WaitToInteract();
		this.gameObject.SetActive (false);


			}
	
	// Update is called once per frame
	void Update () {

		frames++;
		HandleInput ();
		if (frames % 3 == 0) {
			
			Frame2Update ();
			frames = 0;
		}
		
	}
	void Frame2Update(){
		if (canroll) {
			//REST
			CheckIfIllegalPosition();
		

			if (!rolledone) {

				row1.transform.localPosition = new Vector3 (row1.transform.localPosition.x, row1.transform.localPosition.y - 8, 0);
				row2.transform.localPosition = new Vector3 (row2.transform.localPosition.x, row2.transform.localPosition.y - 8, 0);
				row3.transform.localPosition = new Vector3 (row3.transform.localPosition.x, row3.transform.localPosition.y - 8, 0);
				canstopthereels = true;
					


			}
			if (rolledone && !rolledtwo) {
				
				row2.transform.localPosition = new Vector3 (row2.transform.localPosition.x, row2.transform.localPosition.y - 8, 0);
				row3.transform.localPosition = new Vector3 (row3.transform.localPosition.x, row3.transform.localPosition.y - 8, 0);

					


			}
			if (rolledtwo && !rolledthree) {

				row3.transform.localPosition = new Vector3 (row3.transform.localPosition.x, row3.transform.localPosition.y - 8, 0);

					

			}
			CheckIfIllegalPosition();
			CheckPositions ();
		}
	}
	IEnumerator LinedUp(){
		if (GuaranteedModeGood != 0) {
			GuaranteedModeGood--;
		}
		string whatwaslinedup;
		whatwaslinedup = "";
		if ((above1 == above2 && above2 == above3) || (middle1 == middle2 && middle2 == middle3) || (below1 == below2 && below2 == below3) || (above1 == middle2 && middle2 == below3) || (below1 == middle2 && middle2 == above3)) {
			
			if (middle1 == middle2 && middle2 == middle3) {
				whatwaslinedup = middle1;
			}
			if (betamount != 1) {
				if (above1 == above2 && above2 == above3) {
					whatwaslinedup = above1;
				}

				if (below1 == below2 && below2 == below3) { 
					whatwaslinedup = below1;
				}
			}
			if (betamount == 3) {
				if (above1 == middle2 && middle2 == below3) { 
					whatwaslinedup = middle2;
				}

				if (below1 == middle2 && middle2 == above3) {
					whatwaslinedup = middle2;
				}
			}
			if (whatwaslinedup != "") {
				if (whatwaslinedup == "7") {
					payout = 300;
					int toendornotsuper = Random.Range (0, 2);
					if (toendornotsuper == 0) {
						stayingInModeSuper = false;

					}

				}
				if (whatwaslinedup == "BIRD") {
					payout = 15;

				}
				if (whatwaslinedup == "FISH") {
					payout = 15;

				}
				if (whatwaslinedup == "MOUSE") {
					payout = 15;

				}
				if (whatwaslinedup == "BAR") {
					payout = 100;
					stayingInModeSuper = false;
				}
				if (whatwaslinedup == "CHERRY") {
					payout = 8;

				}
                switch(whatwaslinedup){
                    case "CHERRY":
                        whatwaslinedup = "à";
                        break;
                    case "FISH":
                        whatwaslinedup = "á";
                        break;
                    case "BIRD":
                        whatwaslinedup = "â";
                        break;
                    case "MOUSE":
                        whatwaslinedup = "ä";
                        break;
                    case "BAR":
                        whatwaslinedup = "ã";
                        break;
                    case "7":
                        whatwaslinedup = "ā";
                        break;
                }
				yield return StartCoroutine(Dialogue.instance.text (whatwaslinedup + " lined up!"));
				yield return StartCoroutine(Dialogue.instance.line ("Scored " + payout + "!"));

				int payoutamount = payout;
				for (int i = 0; i < payoutamount; i++) {
					yield return new WaitForSeconds (0.01f);
					payout--;
					GameData.coins++;
					UpdateCredit ();
					UpdatePayout ();



				}
			} else {
				yield return StartCoroutine(Dialogue.instance.text ("Not this time!",1));
			}
			} else {
				if (GameData.coins > 0) {
				yield return StartCoroutine(Dialogue.instance.text ("Not this time!",1));
				} else {
				yield return StartCoroutine(Dialogue.instance.text ("Darn! Ran out of"));
				yield return StartCoroutine(Dialogue.instance.line ("coins!"));
				yield return StartCoroutine(Dialogue.instance.done ());
             
					Exit ();
					yield break;
				}

			}
			
		yield return StartCoroutine(Dialogue.instance.para ("One more go?"));
        yield return StartCoroutine(Dialogue.instance.prompt ());
			if (Dialogue.instance.selectedOption == 0) {
				canroll = false;
			yield return StartCoroutine(Dialogue.instance.text ("Bet how many"));
			yield return StartCoroutine(Dialogue.instance.line ("coins?"));
				StartCoroutine(DecideBet ());


			} else {

				Exit ();

			}





	}
	void CheckPositions(){
		if (row1.transform.localPosition.y == 56) {
			above1 = "BAR";
			middle1 = "FISH";
			below1 = "MOUSE";
		}
		if (row1.transform.localPosition.y == 40) {
			above1 = "CHERRY";
			middle1 = "BAR";
			below1 = "FISH";
		}
		if (row1.transform.localPosition.y == 24) {
			above1 = "7";
			middle1 = "CHERRY";
			below1 = "BAR";
		}
		if (row1.transform.localPosition.y == 8) {
			above1 = "FISH";
			middle1 = "7";
			below1 = "CHERRY";
		}
		if (row1.transform.localPosition.y == -8) {
			above1 = "BIRD";
			middle1 = "FISH";
			below1 = "7";
		}
		if (row1.transform.localPosition.y == -24) {
			above1 = "BAR";
			middle1 = "BIRD";
			below1 = "FISH";
		}
		if (row1.transform.localPosition.y == -40) {
			above1 = "CHERRY";
			middle1 = "BAR";
			below1 = "BIRD";
		}
		if (row1.transform.localPosition.y == -56) {
			above1 = "7";
			middle1 = "CHERRY";
			below1 = "BAR";
		}
		if (row1.transform.localPosition.y == -72) {
			above1 = "MOUSE";
			middle1 = "7";
			below1 = "CHERRY";
		}
		if (row1.transform.localPosition.y == -88) {
			above1 = "BIRD";
			middle1 = "MOUSE";
			below1 = "7";
		}
		if (row1.transform.localPosition.y == -104) {
			above1 = "BAR";
			middle1 = "BIRD";
			below1 = "MOUSE";
		}
		if (row1.transform.localPosition.y == -120) {
			above1 = "CHERRY";
			middle1 = "BAR";
			below1 = "BIRD";
		}
		if (row1.transform.localPosition.y == -136) {
			above1 = "7";
			middle1 = "CHERRY";
			below1 = "BAR";
		}
		if (row1.transform.localPosition.y == -152) {
			above1 = "MOUSE";
			middle1 = "7";
			below1 = "CHERRY";
		}
		if (row1.transform.localPosition.y == -168) {
			above1 = "FISH";
			middle1 = "MOUSE";
			below1 = "7";
		}
		//ROW2
		if (row2.transform.localPosition.y == 56) {
			above2 = "BIRD";
			middle2 = "CHERRY";
			below2 = "FISH";
		}
		if (row2.transform.localPosition.y == 40) {
			above2 = "MOUSE";
			middle2 = "BIRD";
			below2 = "CHERRY";
		}
		if (row2.transform.localPosition.y == 24) {
			above2 = "BAR";
			middle2 = "MOUSE";
			below2 = "BIRD";
		}
		if (row2.transform.localPosition.y == 8) {
			above2 = "CHERRY";
			middle2 = "BAR";
			below2 = "MOUSE";
		}
		if (row2.transform.localPosition.y == -8) {
			above2 = "FISH";
			middle2 = "CHERRY";
			below2 = "BAR";
		}
		if (row2.transform.localPosition.y == -24) {
			above2 = "BIRD";
			middle2 = "FISH";
			below2 = "CHERRY";
		}
		if (row2.transform.localPosition.y == -40) {
			above2 = "CHERRY";
			middle2 = "BIRD";
			below2 = "FISH";
		}
		if (row2.transform.localPosition.y == -56) {
			above2 = "BAR";
			middle2 = "CHERRY";
			below2 = "BIRD";
		}
		if (row2.transform.localPosition.y == -72) {
			above2 = "FISH";
			middle2 = "BAR";
			below2 = "CHERRY";
		}
		if (row2.transform.localPosition.y == -88) {
			above2 = "BIRD";
			middle2 = "FISH";
			below2 = "BAR";
		}
		if (row2.transform.localPosition.y == -104) {
			above2 = "CHERRY";
			middle2 = "BIRD";
			below2 = "FISH";
		}
		if (row2.transform.localPosition.y == -120) {
			above2 = "MOUSE";
			middle2 = "CHERRY";
			below2 = "BIRD";
		}
		if (row2.transform.localPosition.y == -136) {
			above2 = "7";
			middle2 = "MOUSE";
			below2 = "CHERRY";
		}
		if (row2.transform.localPosition.y == -152) {
			above2 = "FISH";
			middle2 = "7";
			below2 = "MOUSE";
		}
		if (row2.transform.localPosition.y == -168) {
			above2 = "CHERRY";
			middle2 = "FISH";
			below2 = "7";
		}
		//ROW3
		if (row3.transform.localPosition.y == 56) {
			above3 = "CHERRY";
			middle3 = "FISH";
			below3 = "BIRD";
		}
		if (row3.transform.localPosition.y == 40) {
			above3 = "MOUSE";
			middle3 = "CHERRY";
			below3 = "FISH";
		}
		if (row3.transform.localPosition.y == 24) {
			above3 = "BIRD";
			middle3 = "MOUSE";
			below3 = "CHERRY";
		}
		if (row3.transform.localPosition.y == 8) {
			above3 = "FISH";
			middle3 = "BIRD";
			below3 = "MOUSE";
		}
		if (row3.transform.localPosition.y == -8) {
			above3 = "CHERRY";
			middle3 = "FISH";
			below3 = "BIRD";
		}
		if (row3.transform.localPosition.y == -24) {
			above3 = "MOUSE";
			middle3 = "CHERRY";
			below3 = "FISH";
		}
		if (row3.transform.localPosition.y == -40) {
			above3 = "BIRD";
			middle3 = "MOUSE";
			below3 = "CHERRY";
		}
		if (row3.transform.localPosition.y == -56) {
			above3 = "FISH";
			middle3 = "BIRD";
			below3 = "MOUSE";
		}
		if (row3.transform.localPosition.y == -72) {
			above3 = "CHERRY";
			middle3 = "FISH";
			below3 = "BIRD";
		}
		if (row3.transform.localPosition.y == -88) {
			above3 = "MOUSE";
			middle3 = "CHERRY";
			below3 = "FISH";
		}
		if (row3.transform.localPosition.y == -104) {
			above3 = "BIRD";
			middle3 = "MOUSE";
			below3 = "CHERRY";
		}
		if (row3.transform.localPosition.y == -120) {
			above3 = "BAR";
			middle3 = "BIRD";
			below3 = "MOUSE";
		}
		if (row3.transform.localPosition.y == -136) {
			above3 = "7";
			middle3 = "BAR";
			below3 = "BIRD";
		}
		if (row3.transform.localPosition.y == -152) {
			above3 = "BIRD";
			middle3 = "7";
			below3 = "BAR";
		}
		if (row3.transform.localPosition.y == -168) {
			above3 = "FISH";
			middle3 = "BIRD";
			below3 = "7";
		}
	}
	void CheckIfIllegalPosition(){
		if (row1.transform.localPosition.y <= -168) {
			row1.transform.localPosition = new Vector3 (row1.transform.localPosition.x, 72, 0);

		}
		if (row2.transform.localPosition.y <= -168) {
			row2.transform.localPosition = new Vector3 (row2.transform.localPosition.x, 72, 0);

		}
		if (row3.transform.localPosition.y <= -168) {
			row3.transform.localPosition = new Vector3 (row3.transform.localPosition.x, 72, 0);

		}
	}
	void HandleInput(){
		if (canroll && canstopthereels) {

			if (rolledtwo && !rolledthree) {
				if (Inputs.pressed("a")) {
					if ((Mathf.Abs (row3.transform.localPosition.y)+8) % 16 == 8) {
							 row3.transform.Translate(0,-8,0);
							 CheckIfIllegalPosition();
					}
					Debug.Log(row3.transform.localPosition.y);
						 
		
						if (CurrentMode == "SUPER" || CurrentMode == "GOOD") {
							CheckPositions ();
							if (middle1 == middle2) {

								if (middle3 != middle2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (middle3 == middle2) {
											break;
										}

									}
								}
							}
							if (above1 == above2) {

								if (above3 != above2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (above3 == above2) {
											break;
										}

									}	
								}
							}
							if (below1 == below2) { 

								if (below3 != below2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (below3 == below2) {
											break;
										}

									}
								}
							}

							if (above1 == middle2) { 

								if (below3 != middle2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (below3 == middle2) {
											break;
										}

									}
								}
							}

							if (below1 == middle2) {

								if (above3 != middle2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (above3 == middle2) {
											break;
										}

									}
								}

							}


						}
						if (CurrentMode == "GOOD") {
							CheckPositions ();
							if (middle1 == middle2) {
								if (middle2 == "BAR" || middle2 == "7") {
									if (middle3 == middle2) {
										for (int i = 0; i < 4; i++) {
											row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
											CheckIfIllegalPosition ();
											CheckPositions ();
											if (middle3 != middle2) {
												break;
											}

										}
									}
								}
							}
							if (above1 == above2) {
								if (above2 == "BAR" || above2 == "7") {
									if (above3 == above2) {

										for (int i = 0; i < 4; i++) {
											row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
											CheckIfIllegalPosition ();
											CheckPositions ();
											if (above3 != above2) {
												break;
											}

										}	
									}
								}
							}
							if (below1 == below2) { 
								if (below2 == "BAR" || below2 == "7") {
									if (below3 == below2) {
										for (int i = 0; i < 4; i++) {
											row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
											CheckIfIllegalPosition ();
											CheckPositions ();
											if (below3 != below2) {
												break;
											}

										}
									}
								}
							}

							if (above1 == middle2) { 
								if (middle2 == "BAR" || middle2 == "7") {
									if (below3 == middle2) {
										for (int i = 0; i < 4; i++) {
											row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
											CheckIfIllegalPosition ();
											CheckPositions ();
											if (below3 != middle2) {
												break;
											}

										}
									}
								}
							}

							if (below1 == middle2) {
								if (middle2 == "BAR" || middle2 == "7") {
									if (above3 == middle2) {
										for (int i = 0; i < 4; i++) {
											row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
											CheckIfIllegalPosition ();
											CheckPositions ();
											if (above3 != middle2) {
												break;
											}

										}
									}

								}
							}


						}
						if (CurrentMode == "BAD") {
							CheckPositions ();
							if (middle1 == middle2) {

								if (middle3 == middle2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (middle3 != middle2) {
											break;
										}

									}
								}
							}

							if (above1 == middle2) { 

								if (below3 == middle2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (below3 != middle2) {
											break;
										}

									}
								}
							}

							if (below1 == middle2) {

								if (above3 == middle2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (above3 != middle2) {
											break;
										}

									}
								}

							}
							if (above1 == above2) {

								if (above3 == above2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (above3 != above2) {
											break;
										}

									}	
								}
							}
							if (below1 == below2) { 

								if (below3 == below2) {
									for (int i = 0; i < 4; i++) {
										row3.transform.localPosition = new Vector2 (row3.transform.localPosition.x, row3.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (below3 != below2) {
											break;
										}

									}
								}
							}



						}
						rolledthree = true;
						CheckPositions ();
						StartCoroutine (LinedUp ());
					
				}

			}
			else if (rolledone && !rolledtwo) {
				bool FoundASuperMatch = false;
				bool FoundAMatch = false;
				if (Inputs.pressed("a")) {
						if ((Mathf.Abs (row2.transform.localPosition.y)+ 8) % 16 == 8) {
					
					  row2.transform.Translate(0,-8,0);
					  CheckIfIllegalPosition();
						}
						
	
						if (CurrentMode == "SUPER") {
							CheckPositions ();
							if (middle1 == "BAR" || middle1 == "7") {
						
								if (middle2 != middle1) {
									for (int i = 0; i < 4; i++) {
										row2.transform.localPosition = new Vector2 (row2.transform.localPosition.x, row2.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (middle2 == middle1) {
											FoundASuperMatch = true;
											break;
										}

									}
								} else {
									FoundASuperMatch = true;

								}

							
							}
							if ((above1 == "BAR" || above1 == "7") && !FoundASuperMatch) {

								if (above2 != above1 && middle2 != above1) {
									for (int i = 0; i < 4; i++) {
										row2.transform.localPosition = new Vector2 (row2.transform.localPosition.x, row2.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (above2 == above1 || middle2 == above1) {
											FoundASuperMatch = true;
											break;
										}

									}	
								} else {
									FoundASuperMatch = true;

								}
							
							}
							if ((below1 == "BAR" || below1 == "7") && !FoundASuperMatch) { 

								if (below2 != below1 && middle2 != below1) {
									for (int i = 0; i < 4; i++) {
										row2.transform.localPosition = new Vector2 (row2.transform.localPosition.x, row2.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (below2 == below1 || middle2 == below1) {
											FoundASuperMatch = true;
											break;
										}

									}
								} else {
									FoundASuperMatch = true;

								}
							
							}


						} else {
							CheckPositions ();


								if (middle2 != middle1) {
									for (int i = 0; i < 4; i++) {
									row2.transform.localPosition = new Vector2 (row2.transform.localPosition.x, row2.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (middle2 == middle1) {
											FoundAMatch = true;
											break;
										}

									}
								} else {
									FoundAMatch = true;

								}


							
							if (!FoundAMatch) {

								if (above2 != above1 && middle2 != above1) {
									for (int i = 0; i < 4; i++) {
										row2.transform.localPosition = new Vector2 (row2.transform.localPosition.x, row2.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (above2 == above1 || middle2 == above1) {
											FoundAMatch = true;
											break;
										}

									}	
								} else {
									FoundAMatch = true;

								}

							}
							if (!FoundAMatch) { 

								if (below2 != below1 && middle2 != below1) {
									for (int i = 0; i < 4; i++) {
										row2.transform.localPosition = new Vector2 (row2.transform.localPosition.x, row2.transform.localPosition.y - 16);
										CheckIfIllegalPosition ();
										CheckPositions ();
										if (below2 == below1 || middle2 == below1) {
											FoundAMatch = true;
											break;
										}

									}
								} else {
									FoundAMatch = true;

								}

							}


						}


						rolledtwo = true;
					
				}


			}
			else if (!rolledone) {
				if (Inputs.pressed("a")) {
						if ((Mathf.Abs (row1.transform.localPosition.y) + 8) % 16 == 8) {
					 row1.transform.Translate(0,-8,0);
					  CheckIfIllegalPosition();
						}
						
						if (CurrentMode == "SUPER") {
							CheckPositions ();
							//oversight where it spins regardless in mode super.
							if (above1 != "7" && middle1 != "7" && below1 != "7") { //DISABLE FOR OVERSIGHT
								for (int i = 0; i < 4; i++) {
									row1.transform.localPosition = new Vector2 (row1.transform.localPosition.x, row1.transform.localPosition.y - 16);
									CheckIfIllegalPosition ();
									CheckPositions ();
									if (above1 == "7" || middle1 == "7" || below1 == "7") {
										break;
									}

								}
							} //DISABLE
						} else {
							CheckPositions ();
							if (middle1 == "CHERRY") {
								for (int i = 0; i < 4; i++) {
									row1.transform.localPosition = new Vector2 (row1.transform.localPosition.x, row1.transform.localPosition.y - 16);
									CheckIfIllegalPosition ();
									CheckPositions ();
									if (middle1 != "CHERRY") {
										break;
									}

								}
							}
						}
						rolledone = true;
					
				}



			}


		}
	}
}
