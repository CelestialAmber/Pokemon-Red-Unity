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
	private GameObject row1, row2, row3;
	public bool rolledone, rolledtwo, rolledthree, canroll;
	private string above1, middle1, below1, above2, middle2, below2, above3, middle3, below3;
	public int frames;
	public int payout;
	public bool stayingInModeSuper;
	public int GuaranteedModeGood;
    public CustomText credittext, payouttext;
	public int betamount;
    public GameObject[] blueRows, redRows;
    public Sprite blueBG, redBG;
    public Image bg;
    public GameObject blueRowsObject, redRowsObject;
    public static Slots instance;
    public int row1Index, row2Index, row3Index, row1Half, row2Half, row3Half;
    public bool funMode;

    public AudioClip startSlotsSound, payoutSound, stopReelSound;
    public Animator slotPointsAnimator;
    public GameObject flashObject;
    // Use this for initialization

    public void Init()
    {
        instance = this;
        if (VersionManager.instance.version == Version.Red)
        {
            row1 = redRows[0];
            row2 = redRows[1];
            row3 = redRows[2];
            bg.sprite = redBG;
            blueRowsObject.SetActive(false);

        }
        else
        {
            row1 = blueRows[0];
            row2 = blueRows[1];
            row3 = blueRows[2];
            bg.sprite = blueBG;
            redRowsObject.SetActive(false);
        }
    }
    IEnumerator DecideBet(){
		int RandomNumber;
		int ModeNumber;
        if (funMode)
        {
            GuaranteedModeGood = 999;
            stayingInModeSuper = true;
        }
		yield return StartCoroutine(Dialogue.instance.slots ());
        if (Dialogue.instance.selectedOption == 3)
        {
            Exit();
            yield break;
        }
        if (Player.disabled) {

            int amount = 3 - Dialogue.instance.selectedOption;
		
				if (GameData.coins < amount) {
                    yield return Dialogue.instance.text("Not enough\ncoins!");
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
				print ("Betting " + amount + " coin(s)");
				betamount = amount;
				GameData.coins -= amount;
				UpdateCredit ();
				rolledone = false;
				rolledtwo = false;
				rolledthree = false;
				canstopthereels = false;
				canroll = true;
            SoundManager.instance.sfx.PlayOneShot(startSlotsSound);
                slotPointsAnimator.SetBool("toggleStatus", true);
                StartCoroutine(Dialogue.instance.text ("Start!",true));
            slotPointsAnimator.SetFloat("betAmount", (float)amount);
			
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
        Dialogue.instance.fastText = true;
		GuaranteedModeGood = 0;
		UpdateCredit ();
		UpdatePayout ();
        row1Index = 0;
        row2Index = 0;
        row3Index = 0;
		canroll = false;
        Dialogue.instance.fastText = false;
        slotPointsAnimator.SetBool("toggleStatus",false);
        yield return Dialogue.instance.text ("Bet how many\ncoins?",true);
        Dialogue.instance.fastText = true;
		StartCoroutine(DecideBet ());


	}
			public void Exit(){
        Inputs.Enable("start");
		Player.disabled = false;
		rolledone = false;
		rolledtwo = false;
		rolledthree = false;
		canroll = false;
		Dialogue.instance.fastText = false;
		Dialogue.instance.Deactivate ();
		this.gameObject.SetActive (false);


			}
	
	// Update is called once per frame
	void Update () {

		frames++;
		HandleInput ();
		if (frames % 3 == 0) {
			
			UpdatePositions (true);
			frames = 0;
		}
		
	}
	void UpdatePositions(bool addHalf){
		if (canroll) {
		

			if (!rolledone) {
               if(addHalf) row1Half++;
                if (row1Half > 1)
                {
                    row1Half = 0;
                    row1Index--;
                    if (row1Index < 0) row1Index = 14;
                }
                if (addHalf) row2Half++;
                if (row2Half > 1)
                {
                    row2Half = 0;
                    row2Index--;
                    if (row2Index < 0) row2Index = 14;
                }
                if (addHalf) row3Half++;
                if (row3Half > 1)
                {
                    row3Half = 0;
                    row3Index--;
                    if (row3Index < 0) row3Index = 14;
                }

                row1.transform.localPosition = new Vector3 (row1.transform.localPosition.x, -152 + row1Index * 16 - row1Half * 8, 0);
				row2.transform.localPosition = new Vector3(row2.transform.localPosition.x, -152 + row2Index * 16 - row2Half * 8, 0);
                row3.transform.localPosition = new Vector3(row3.transform.localPosition.x, -152 + row3Index * 16 - row3Half * 8, 0);
                canstopthereels = true;
					


			}
			if (rolledone && !rolledtwo) {
                if (addHalf) row2Half++;
                if (row2Half > 1)
                {
                    row2Half = 0;
                    row2Index--;
                    if (row2Index < 0) row2Index = 14;
                }
                if (addHalf) row3Half++;
                if (row3Half > 1)
                {
                    row3Half = 0;
                    row3Index--;
                    if (row3Index < 0) row3Index = 14;
                }
                row2.transform.localPosition = new Vector3(row2.transform.localPosition.x, -152 + row2Index * 16 - row2Half * 8, 0);
                row3.transform.localPosition = new Vector3(row3.transform.localPosition.x, -152 + row3Index * 16 - row3Half * 8, 0);




            }
			if (rolledtwo && !rolledthree) {
                if (addHalf) row3Half++;
                if (row3Half > 1)
                {
                    row3Half = 0;
                    row3Index--;
                    if (row3Index < 0) row3Index = 14;
                }

                row3.transform.localPosition = new Vector3(row3.transform.localPosition.x, -152 + row3Index * 16 - row3Half * 8, 0);



            }

			CheckPositions ();
		}
	}
    IEnumerator SlotsFlash(int times)
    {
        WaitForSeconds wait = new WaitForSeconds(0.016f * 5f);
        for (int i = 0; i < times; i++)
        {
       
            ScreenEffects.flashLevel = 1;
            yield return wait;
            ScreenEffects.flashLevel = 0;
            yield return wait;
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
                if (whatwaslinedup == "7")
                {
                    yield return Dialogue.instance.text("Yeah!", true);
                    StartCoroutine(SlotsFlash(8));
                    yield return StartCoroutine(SoundManager.instance.PlayItemGetSound(1));
                }else StartCoroutine(SlotsFlash(1));
                if (whatwaslinedup == "BAR") yield return StartCoroutine(SoundManager.instance.PlayItemGetSound(2));
                float timeToWait;
                if (whatwaslinedup == "7" || whatwaslinedup == "BAR") timeToWait = 0.016f * 3f;
                else timeToWait = 0.016f * 8f;
                switch (whatwaslinedup){
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
               
                yield return Dialogue.instance.text (whatwaslinedup + " lined up!\nScored " + payout + "!");
                yield return Dialogue.instance.text(whatwaslinedup + " lined up!\nScored " + payout + "!",true);

                int payoutamount = payout;
                
                flashObject.SetActive(true);
                for (int i = 0; i < payoutamount; i++) {
			
					payout--;
                    SoundManager.instance.sfx.PlayOneShot(payoutSound);
					GameData.coins++;
					UpdateCredit ();
					UpdatePayout ();
                    yield return new WaitForSeconds(timeToWait);


				}
                flashObject.SetActive(false);
			} else {
				yield return Dialogue.instance.text ("Not this time!");
			}
			} else {
				if (GameData.coins > 0) {
				yield return Dialogue.instance.text ("Not this time!");
				} else {
				yield return Dialogue.instance.text ("Darn! Ran out of\ncoins!");
             
					Exit ();
					yield break;
				}

			}
			
		yield return Dialogue.instance.text ("One more go?",true);
        yield return StartCoroutine(Dialogue.instance.prompt ());
			if (Dialogue.instance.selectedOption == 0) {
				canroll = false;
            Dialogue.instance.fastText = false;
            slotPointsAnimator.SetBool("toggleStatus",false);
			yield return Dialogue.instance.text ("Bet how many\ncoins?",true);
            Dialogue.instance.fastText = true;
				StartCoroutine(DecideBet ());


			} else {

				Exit ();

			}





	}
	void CheckPositions(){
		if (row1Index == 14) { //-168
			above1 = "FISH";
			middle1 = "MOUSE";
			below1 = "7";
		}
		if (row1Index == 13) { //56
			above1 = "BAR";
			middle1 = "FISH";
			below1 = "MOUSE";
		}
		if (row1Index == 12) {
			above1 = "CHERRY";
			middle1 = "BAR";
			below1 = "FISH";
		}
		if (row1Index == 11) {
			above1 = "7";
			middle1 = "CHERRY";
			below1 = "BAR";
		}
		if (row1Index == 10) {
			above1 = "FISH";
			middle1 = "7";
			below1 = "CHERRY";
		}
		if (row1Index == 9) {
			above1 = "BIRD";
			middle1 = "FISH";
			below1 = "7";
		}
		if (row1Index == 8) {
			above1 = "BAR";
			middle1 = "BIRD";
			below1 = "FISH";
		}
		if (row1Index == 7) {
			above1 = "CHERRY";
			middle1 = "BAR";
			below1 = "BIRD";
		}
		if (row1Index == 6) {
			above1 = "7";
			middle1 = "CHERRY";
			below1 = "BAR";
		}
		if (row1Index == 5) {
			above1 = "MOUSE";
			middle1 = "7";
			below1 = "CHERRY";
		}
		if (row1Index == 4) {
			above1 = "BIRD";
			middle1 = "MOUSE";
			below1 = "7";
		}
		if (row1Index == 3) {
			above1 = "BAR";
			middle1 = "BIRD";
			below1 = "MOUSE";
		}
		if (row1Index == 2) {
			above1 = "CHERRY";
			middle1 = "BAR";
			below1 = "BIRD";
		}
		if (row1Index == 1) {
			above1 = "7";
			middle1 = "CHERRY";
			below1 = "BAR";
		}
		if (row1Index == 0) {
			above1 = "MOUSE";
			middle1 = "7";
			below1 = "CHERRY";
		}
		
		//ROW2
		if (row2Index == 14) {
			above2 = "CHERRY";
			middle2 = "FISH";
			below2 = "7";
		}
		if (row2Index == 13) {
			above2 = "BIRD";
			middle2 = "CHERRY";
			below2 = "FISH";
		}
		if (row2Index == 12) {
			above2 = "MOUSE";
			middle2 = "BIRD";
			below2 = "CHERRY";
		}
		if (row2Index == 11) {
			above2 = "BAR";
			middle2 = "MOUSE";
			below2 = "BIRD";
		}
		if (row2Index == 10) {
			above2 = "CHERRY";
			middle2 = "BAR";
			below2 = "MOUSE";
		}
		if (row2Index == 9) {
			above2 = "FISH";
			middle2 = "CHERRY";
			below2 = "BAR";
		}
		if (row2Index == 8) {
			above2 = "BIRD";
			middle2 = "FISH";
			below2 = "CHERRY";
		}
		if (row2Index == 7) {
			above2 = "CHERRY";
			middle2 = "BIRD";
			below2 = "FISH";
		}
		if (row2Index == 6) {
			above2 = "BAR";
			middle2 = "CHERRY";
			below2 = "BIRD";
		}
		if (row2Index == 5) {
			above2 = "FISH";
			middle2 = "BAR";
			below2 = "CHERRY";
		}
		if (row2Index == 4) {
			above2 = "BIRD";
			middle2 = "FISH";
			below2 = "BAR";
		}
		if (row2Index == 3) {
			above2 = "CHERRY";
			middle2 = "BIRD";
			below2 = "FISH";
		}
		if (row2Index == 2) {
			above2 = "MOUSE";
			middle2 = "CHERRY";
			below2 = "BIRD";
		}
		if (row2Index == 1) {
			above2 = "7";
			middle2 = "MOUSE";
			below2 = "CHERRY";
		}
		if (row2Index == 0) {
			above2 = "FISH";
			middle2 = "7";
			below2 = "MOUSE";
		}
		
		//ROW3
		if (row3Index == 14) {
			above3 = "FISH";
			middle3 = "BIRD";
			below3 = "7";
		}
		if (row3Index == 13) {
			above3 = "CHERRY";
			middle3 = "FISH";
			below3 = "BIRD";
		}
		if (row3Index == 12) {
			above3 = "MOUSE";
			middle3 = "CHERRY";
			below3 = "FISH";
		}
		if (row3Index == 11) {
			above3 = "BIRD";
			middle3 = "MOUSE";
			below3 = "CHERRY";
		}
		if (row3Index == 10) {
			above3 = "FISH";
			middle3 = "BIRD";
			below3 = "MOUSE";
		}
		if (row3Index == 9) {
			above3 = "CHERRY";
			middle3 = "FISH";
			below3 = "BIRD";
		}
		if (row3Index == 8) {
			above3 = "MOUSE";
			middle3 = "CHERRY";
			below3 = "FISH";
		}
		if (row3Index == 7) {
			above3 = "BIRD";
			middle3 = "MOUSE";
			below3 = "CHERRY";
		}
		if (row3Index == 6) {
			above3 = "FISH";
			middle3 = "BIRD";
			below3 = "MOUSE";
		}
		if (row3Index == 5) {
			above3 = "CHERRY";
			middle3 = "FISH";
			below3 = "BIRD";
		}
		if (row3Index == 4) {
			above3 = "MOUSE";
			middle3 = "CHERRY";
			below3 = "FISH";
		}
		if (row3Index == 3) {
			above3 = "BIRD";
			middle3 = "MOUSE";
			below3 = "CHERRY";
		}
		if (row3Index == 2) {
			above3 = "BAR";
			middle3 = "BIRD";
			below3 = "MOUSE";
		}
		if (row3Index == 1) {
			above3 = "7";
			middle3 = "BAR";
			below3 = "BIRD";
		}
		if (row3Index == 0) {
			above3 = "BIRD";
			middle3 = "7";
			below3 = "BAR";
		}
		
	}
	void CheckIfIllegalPosition(){
		if (row1Index < 0) {
			row1Index = 14; //goes to 72 y

		}
        if (row2Index < 0)
        {
            row2Index = 14; //goes to 72 y

        }
        if (row3Index < 0)
        {
            row3Index = 14; //goes to 72 y

        }
        UpdatePositions(false);
    }
	void HandleInput(){
		if (canroll && canstopthereels) {

			if (rolledtwo && !rolledthree) {
				if (Inputs.pressed("a")) {
				SoundManager.instance.sfx.PlayOneShot(stopReelSound);
					if (row3Half > 0) {
                        row3Half++;
                        UpdatePositions(false);
						CheckIfIllegalPosition();
                        CheckPositions();
					}

						 
		
						if (CurrentMode == "SUPER" || CurrentMode == "GOOD") {
							CheckPositions ();
							if (middle1 == middle2) {

								if (middle3 != middle2) {
									for (int i = 0; i < 4; i++) {
										row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                        row3Index--;
                                        UpdatePositions(false);
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
                                        row3Index--;
                                        UpdatePositions(false);
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
                                        row3Index--;
                                        UpdatePositions(false);
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
                                        row3Index--;
                                        UpdatePositions(false);
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
                                        row3Index--;
                                        UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
                                    row3Index--;
                                    UpdatePositions(false);
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
					SoundManager.instance.sfx.PlayOneShot(stopReelSound);
						if (row2Half > 0) {

                        row2Half++;
                        UpdatePositions(false);
                        
                        CheckIfIllegalPosition();
                        CheckPositions();
                    }
						
	
						if (CurrentMode == "SUPER") {
							CheckPositions ();
							if (middle1 == "BAR" || middle1 == "7") {
						
								if (middle2 != middle1) {
									for (int i = 0; i < 4; i++) {
                                    row2Index--;
                                    UpdatePositions(false);
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
                                    row2Index--;
                                    UpdatePositions(false);
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
                                    row2Index--;
                                    UpdatePositions(false);
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
                                row2Index--;
                                UpdatePositions(false);
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
                                    row2Index--;
                                    UpdatePositions(false);
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
                                    row2Index--;
                                    UpdatePositions(false);
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
					SoundManager.instance.sfx.PlayOneShot(stopReelSound);
						if (row1Half > 0) {
                        row1Half++;
                        UpdatePositions(false);
                       
					  CheckIfIllegalPosition();
                        CheckPositions();
                    }
                    
						if (CurrentMode == "SUPER") {

							CheckPositions ();
							//oversight where it spins regardless in mode super.
							if (above1 != "7" && middle1 != "7" && below1 != "7") { //DISABLE FOR OVERSIGHT
								for (int i = 0; i < 4; i++) {
                                row1Index--;
                                UpdatePositions(false);
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
                                row1Index--;
                                UpdatePositions(false);
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