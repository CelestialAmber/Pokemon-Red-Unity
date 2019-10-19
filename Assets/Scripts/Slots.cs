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

    public AudioClip startSlotsSound, payoutSound, stopReelSound;
    public Animator slotPointsAnimator;
    public GameObject flashObject;

	public bool handlingInput;

    // Use this for initialization

	private string[] reel1Slots = {
		"7",
		"MOUSE",
		"FISH",
		"BAR",
		"CHERRY",
		"7",
		"FISH",
		"BIRD",
		"BAR",
		"CHERRY",
		"7",
		"MOUSE",
		"BIRD",
		"BAR",
		"CHERRY",
		"7",
		"MOUSE",
		"FISH"
	};
	private string[] reel2Slots = {
		"7",
		"FISH",
		"CHERRY",
		"BIRD",
		"MOUSE",
		"BAR",
		"CHERRY",
		"FISH",
		"BIRD",
		"CHERRY",
		"BAR",
		"FISH",
		"BIRD",
		"CHERRY",
		"MOUSE",
		"7",
		"FISH",
		"CHERRY"
	};
	private string[] reel3Slots = {
		"7",
		"BIRD",
		"FISH",
		"CHERRY",
		"MOUSE",
		"BIRD",
		"FISH",
		"CHERRY",
		"MOUSE",
		"BIRD",
		"FISH",
		"CHERRY",
		"MOUSE",
		"BIRD",
		"BAR",
		"7",
		"BIRD",
		"FISH"
	};

    public void Init()
    {
        instance = this;
        if (GameData.instance.version == Version.Red)
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
		yield return StartCoroutine(Dialogue.instance.slots ());
        if (Dialogue.instance.selectedOption == 3)
        {
            Exit();
            yield break;
        }
        if (Player.disabled) {

            int amount = 3 - Dialogue.instance.selectedOption;
		
				if (GameData.instance.coins < amount) {
                    yield return Dialogue.instance.text("Not enough\\lcoins!");
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
				GameData.instance.coins -= amount;
				UpdateCredit ();
				rolledone = false;
				rolledtwo = false;
				rolledthree = false;
				canstopthereels = true;
				canroll = true;
            SoundManager.instance.sfx.PlayOneShot(startSlotsSound);
                slotPointsAnimator.SetBool("toggleStatus", true);
				Dialogue.instance.keepTextOnScreen = true;
				Dialogue.instance.needButtonPress = false;
                StartCoroutine(Dialogue.instance.text ("Start!"));
            slotPointsAnimator.SetFloat("betAmount", (float)amount);
			inputTimer = 20;
			
		}


	}
	void UpdateCredit(){
		if (GameData.instance.coins > 9999) {
			GameData.instance.coins = 9999;
		}
        credittext.text = (GameData.instance.coins > 999 ? "" : GameData.instance.coins > 99 ? "0" : GameData.instance.coins > 9 ? "00" : "000" ) + GameData.instance.coins.ToString();
	}
	void UpdatePayout(){
        payouttext.text = (payout > 999 ? "" : payout > 99 ? "0" : payout > 9 ? "00" : "000") + payout.ToString();
	}
	public IEnumerator Initialize () {
		GuaranteedModeGood = 0;
		UpdateCredit ();
		UpdatePayout ();
        row1Index = 0;
        row2Index = 0;
        row3Index = 0;
		canroll = false;
        slotPointsAnimator.SetBool("toggleStatus",false);
		Dialogue.instance.fastText = true;
		Dialogue.instance.keepTextOnScreen = true;
        yield return Dialogue.instance.text ("Bet how many\\lcoins?");
		Dialogue.instance.fastText = false;
		StartCoroutine(DecideBet ());


	}
			public void Exit(){
        Inputs.Enable("start");
		Player.disabled = false;
		rolledone = false;
		rolledtwo = false;
		rolledthree = false;
		canroll = false;
		handlingInput = false;
		Dialogue.instance.Deactivate ();
		this.gameObject.SetActive (false);


			}
	
	// Update is called once per frame
	void Update () {
		if(inputTimer == 0 && !handlingInput) StartCoroutine(HandleInput ());
		frames++;
		if (frames % (inputTimer > 0 ? 2 : 3) == 0) {
			if(canstopthereels)UpdatePositions (true);
			if(inputTimer > 0) inputTimer--;
			frames = 0;
		}
		
		
	}
	int inputTimer; //counter disabling inputs until it reaches 0

	void UpdatePositions(bool addHalf){
		if (canroll) {

			if (!rolledone) {
               if(addHalf) row1Half++;
                if (addHalf) row2Half++;
                if (addHalf) row3Half++;
			}
			if (rolledone && !rolledtwo) {
                if (addHalf) row2Half++;
                if (addHalf) row3Half++;
            }
			if (rolledtwo && !rolledthree) {
                if (addHalf) row3Half++;
            }
				if (row1Half > 1)
                {
                    row1Half = 0;
                    row1Index--;
                    if (row1Index < 0) row1Index = 14;
                }

				if (row2Half > 1)
                {
                    row2Half = 0;
                    row2Index--;
                    if (row2Index < 0) row2Index = 14;
                }

				if (row3Half > 1)
                {
                    row3Half = 0;
                    row3Index--;
                    if (row3Index < 0) row3Index = 14;
                }
				row1.transform.localPosition = new Vector3(row1.transform.localPosition.x, -152 + row1Index * 16 - row1Half * 8, 0);
				row2.transform.localPosition = new Vector3(row2.transform.localPosition.x, -152 + row2Index * 16 - row2Half * 8, 0);
                row3.transform.localPosition = new Vector3(row3.transform.localPosition.x, -152 + row3Index * 16 - row3Half * 8, 0);
			
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

	public string FindMatch(){
		string result = "";
		if (middle1 == middle2 && middle2 == middle3) {
				result = middle1;
			}
			if (betamount != 1) {
				if (above1 == above2 && above2 == above3) {
					result = above1;
				}

				if (below1 == below2 && below2 == below3) { 
					result = below1;
				}
			}
			if (betamount == 3) {
				if (above1 == middle2 && middle2 == below3) { 
					result = middle2;
				}

				if (below1 == middle2 && middle2 == above3) {
					result = middle2;
				}
			}
			return result;

	}
	IEnumerator RollWheelDownOne(int whatWheel){
		WaitForSeconds wait = new WaitForSeconds(1f/60f);
		for(int i = 0; i < 2; i++){
			switch(whatWheel){
				case 1:
				row1Half++;
				break;
				case 2:
				row2Half++;
				break;
				case 3:
				row3Half++;
				break;

			}
			UpdatePositions(false);
			yield return wait;
		}
		CheckPositions();
	}
	IEnumerator LinedUp(){
		canstopthereels = false;

		string whatwaslinedup;
		whatwaslinedup = FindMatch();
		
		if(CurrentMode != "BAD" && whatwaslinedup == ""){
			for(int i = 0; i < 4; i++){
			yield return RollWheelDownOne(3);
			whatwaslinedup = FindMatch();
			if(whatwaslinedup != "") break;

			}
		}

		if(CurrentMode != "SUPER" && (whatwaslinedup == "7" || whatwaslinedup == "BAR")){ //if the current mode isn't Super and there's a bar or 7 match, move the 3rd wheel down until there isn't a 7 or bar match
			while((whatwaslinedup == "7" || whatwaslinedup == "BAR")){
			yield return RollWheelDownOne(3);
			whatwaslinedup = FindMatch();
			}

		}
		
		if(CurrentMode == "BAD" && whatwaslinedup != ""){ //if the current mode is Bad and there's a match, move the 3rd wheel down until there isn't a match
			while(whatwaslinedup != ""){
			yield return RollWheelDownOne(3);
			whatwaslinedup = FindMatch();
			}
		}
		
		if (whatwaslinedup != "") {
			
			if(whatwaslinedup != "7" && whatwaslinedup != "BAR" && GuaranteedModeGood != 0)GuaranteedModeGood--;
			
		
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
					Dialogue.instance.keepTextOnScreen = true;
                    yield return Dialogue.instance.text("Yeah!");
                    StartCoroutine(SlotsFlash(8));
                    yield return StartCoroutine(SoundManager.instance.PlayItemGetSound(1));
                }else StartCoroutine(SlotsFlash(1));
                if (whatwaslinedup == "BAR") yield return StartCoroutine(SoundManager.instance.PlayItemGetSound(2));
                float timeToWait;
                if (whatwaslinedup == "7" || whatwaslinedup == "BAR") timeToWait = 0.016f * 3f;
                else timeToWait = 0.016f * 8f;
                whatwaslinedup = "<" + (whatwaslinedup == "7" ? "SEVEN" : whatwaslinedup) + ">";
				Dialogue.instance.keepTextOnScreen = true;
				Dialogue.instance.waitForButtonPress = true;
                yield return Dialogue.instance.text(whatwaslinedup + " lined up!\\lScored " + payout + " coins!");

                int payoutamount = payout;
                
                flashObject.SetActive(true);
                for (int i = 0; i < payoutamount; i++) {
			
					payout--;
                    SoundManager.instance.sfx.PlayOneShot(payoutSound);
					GameData.instance.coins++;
					UpdateCredit ();
					UpdatePayout ();
                    yield return new WaitForSeconds(timeToWait);


				}
                flashObject.SetActive(false);
			} else {
				if (GameData.instance.coins > 0) {
				yield return Dialogue.instance.text ("Not this time!");
				} else {
				yield return Dialogue.instance.text ("Darn! Ran out of\\lcoins!");
             
					Exit ();
					yield break;
				}

			}
			Dialogue.instance.keepTextOnScreen = true;
		yield return Dialogue.instance.text ("One more go?");
        yield return StartCoroutine(Dialogue.instance.prompt ());
			if (Dialogue.instance.selectedOption == 0) {
				canroll = false;
            slotPointsAnimator.SetBool("toggleStatus",false);
			Dialogue.instance.keepTextOnScreen = true;
			yield return Dialogue.instance.text ("Bet how many\\lcoins?");
				StartCoroutine(DecideBet ());


			} else {

				Exit ();

			}
	}
	void CheckPositions(){
		
		//Row 1
		above1 = reel1Slots[16 - row1Index];
		middle1 = reel1Slots[15 - row1Index];
		below1 = reel1Slots[14 - row1Index];
		
		
		//ROW2
		above2 = reel2Slots[16 - row2Index];
		middle2 = reel2Slots[15 - row2Index];
		below2 = reel2Slots[14 - row2Index];
		
		
		//ROW3
		above3 = reel3Slots[16 - row3Index];
		middle3 = reel3Slots[15 - row3Index];
		below3 = reel3Slots[14 - row3Index];
		
		
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
    }
	string checkWheel1Wheel2Matches(){
		return above1 == above2 || above1 == middle2 ? above1:  middle1 == middle2 ? middle1 : below1 == middle2 || below1 == below2 ? below1 : "";
	}
	IEnumerator HandleInput(){
		handlingInput = true;
		if (canroll && canstopthereels) {
			

			if (rolledtwo && !rolledthree) {
				if (Inputs.pressed("a")) {
					rolledthree = true;
				SoundManager.instance.sfx.PlayOneShot(stopReelSound);
				
					if (row3Half > 0) {
						row3Half++;
						UpdatePositions(false);
					}
						CheckPositions ();
						yield return LinedUp ();
					
				}

			}
			else if (rolledone && !rolledtwo) {
				if (Inputs.pressed("a")) {
					rolledtwo = true;
					SoundManager.instance.sfx.PlayOneShot(stopReelSound);
					
						if (row2Half > 0) {
						row2Half++;
						UpdatePositions(false);
                    }
						CheckPositions ();
						if (CurrentMode == "SUPER") {
							
							string match = checkWheel1Wheel2Matches();
							if(match == "" && (below2 == "7" || below2 == "BAR")){
							handlingInput = false;
							yield return 0;
							}
							if (match != "BAR" && match != "7") {
						
									for (int i = 0; i < 4; i++) {
                                    yield return RollWheelDownOne(2);
									match = checkWheel1Wheel2Matches();
										if (match == "BAR" && match == "7") {
											break;
										}

									}
								}
							
						} else {
						

							string match = checkWheel1Wheel2Matches();
								if (match == "") {
						
									for (int i = 0; i < 4; i++) {
                                     yield return RollWheelDownOne(2);
									match = checkWheel1Wheel2Matches();
										if (match != "") {
											break;
										}

									}
								}


						}
						
					
				}


			}
			else if (!rolledone) {
				if (Inputs.pressed("a")) {
					rolledone = true;
					SoundManager.instance.sfx.PlayOneShot(stopReelSound);
				
						if (row1Half > 0) {
                        row1Half++;
						UpdatePositions(false);
                    }
					CheckPositions ();
						if (CurrentMode == "SUPER") {

							
							//oversight where it spins regardless in mode super.
							/*
							if (above1 != "7" && middle1 != "7" && below1 != "7") {
								for (int i = 0; i < 4; i++) {
                                 yield return RollWheelDownOne(1);
									if (above1 == "7" || middle1 == "7" || below1 == "7") {
										break;
									}

								}
							}
							*/
						} else {
							
							if (middle1 == "CHERRY") {
								for (int i = 0; i < 4; i++) {
                                 yield return RollWheelDownOne(1);
									if (middle1 != "CHERRY") {
										break;
									}

								}
							}
						}
						
				}



			}


		}
		handlingInput = false;
	}
}