using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialHandler : MonoBehaviour {
	public GameObject  rednamemenu, garynamemenu,nameselectionmenu, currentmenu, oak, gary, red;
    public GameCursor cursor;
	public Animator tutanim;
	public int selectedOption;
	public GameObject[] allmenus;
	public GameObject overworld, white;
    public bool givingRedAName, givingGaryAName;
	public Player play;
	public NameSelection names;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentmenu == null && currentmenu != nameselectionmenu) {
			cursor.SetActive (false);
		} else {
            cursor.SetPosition(8,120 - 16 * selectedOption);
		
			cursor.SetActive (true);
		
            if (Inputs.pressed("down")) {
				selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 3);
			}
            if (Inputs.pressed("up")) {
				selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 3);
			}
	
		}
			foreach (GameObject menu in allmenus) {
				if (menu != currentmenu) {
					menu.SetActive (false);
				} else {

					menu.SetActive (true);
				}

			
			}

		if (Inputs.pressed("a")) {
			
			if (currentmenu == rednamemenu && selectedOption == 	0) {
				currentmenu = nameselectionmenu;
				nameselectionmenu.SetActive (true);
				names.futureName = "";
			}
			if (currentmenu == rednamemenu && selectedOption == 	1) {
				currentmenu = null;
                GameData.playerName = "RED";
				tutanim.SetBool ("fourthpass", true);
				Dialogue.instance.enabled = true;
				givingRedAName = false;
			}
			if (currentmenu == rednamemenu && selectedOption == 	2) {
				currentmenu = null;
                GameData.playerName = "ASH";
				tutanim.SetBool ("fourthpass", true);
				Dialogue.instance.enabled = true;
				givingRedAName = false;

			}
			if (currentmenu == rednamemenu && selectedOption == 	3) {
				currentmenu = null;
                GameData.playerName = "JACK";
				tutanim.SetBool ("fourthpass", true);
				Dialogue.instance.enabled = true;
				givingRedAName = false;
			}
			if (currentmenu == garynamemenu && selectedOption == 	0) {
				currentmenu = nameselectionmenu;
				nameselectionmenu.SetActive (true);
				names.futureName = "";
			}
			if (currentmenu == garynamemenu && selectedOption == 	1) {
				currentmenu = null;
                Dialogue.opponentName = "BLUE";
				tutanim.SetBool ("seventhpass", true);
				Dialogue.instance.enabled = true;
				givingRedAName = false;
			}
			if (currentmenu == garynamemenu && selectedOption == 	2) {
				currentmenu = null;
                Dialogue.opponentName = "GARY";
				tutanim.SetBool ("seventhpass", true);
				Dialogue.instance.enabled = true;
				givingRedAName = false;

			}
			if (currentmenu == garynamemenu && selectedOption == 	3) {
				currentmenu = null;
                Dialogue.opponentName = "JOHN";
				tutanim.SetBool ("seventhpass", true);
				Dialogue.instance.enabled = true;
				givingRedAName = false;
			}
		}
	}

	public IEnumerator FirstOakDialogue(){
		play.amenuactive = true;
		Dialogue.instance.deactivated = true;
		currentmenu = null;
		yield return StartCoroutine(Dialogue.instance.text ("Hello there!"));
		yield return StartCoroutine(Dialogue.instance.line ("Welcome to the",0));
		yield return StartCoroutine(Dialogue.instance.cont ("world of POKeMON!",1));
		yield return StartCoroutine(Dialogue.instance.para ("My name is OAK!"));
		yield return StartCoroutine(Dialogue.instance.line("People call me",0));
		yield return StartCoroutine(Dialogue.instance.cont ("the POKeMON PROF!"));
		yield return StartCoroutine(Dialogue.instance.done ());
		tutanim.SetBool ("firstpass", true);

	}
	public IEnumerator SecondOakDialogue(){
		yield return StartCoroutine(Dialogue.instance.text ("This world is"));
		yield return StartCoroutine(Dialogue.instance.line ("inhabited by",0));
		yield return StartCoroutine(Dialogue.instance.cont ("creatures called",0));
		yield return StartCoroutine(Dialogue.instance.cont ("POKeMON!",1));
		yield return StartCoroutine(Dialogue.instance.para("For some people,"));
		yield return StartCoroutine(Dialogue.instance.line ("POKeMON are",0));
		yield return StartCoroutine(Dialogue.instance.cont ("pets. Others use",0));
		yield return StartCoroutine(Dialogue.instance.cont ("them for fights.",1));
		yield return StartCoroutine(Dialogue.instance.para("Myself...",1));
		yield return StartCoroutine(Dialogue.instance.para ("I study POKeMON"));
		yield return StartCoroutine(Dialogue.instance.line ("as a profession."));
		yield return StartCoroutine(Dialogue.instance.done ());
		tutanim.SetBool ("secondpass", true);

	}
	public IEnumerator ThirdOakDialogue(){
		
		yield return StartCoroutine(Dialogue.instance.text ("First, what is"));
		yield return StartCoroutine(Dialogue.instance.line ("your name?"));
		yield return StartCoroutine(Dialogue.instance.done ());
		tutanim.SetBool ("thirdpass", true);
		Dialogue.instance.enabled = false;

		currentmenu = rednamemenu;
		selectedOption = 0;
		givingRedAName = true;
}
	public IEnumerator FourthOakDialogue(){

		yield return StartCoroutine(Dialogue.instance.text ("Right! So your"));
        yield return StartCoroutine(Dialogue.instance.line ("name is " + GameData.playerName + "!"));
		yield return StartCoroutine(Dialogue.instance.done ());
		tutanim.SetBool ("fifthpass", true);

	}
	public IEnumerator FifthOakDialogue(){

		yield return StartCoroutine(Dialogue.instance.text ("This is my grand-"));
		yield return StartCoroutine(Dialogue.instance.line ("son. He's been",0));
		yield return StartCoroutine(Dialogue.instance.cont ("your rival since",0));
		yield return StartCoroutine(Dialogue.instance.cont ("you were a baby.",1));
		yield return StartCoroutine(Dialogue.instance.para ("...Erm, what is"));
		yield return StartCoroutine(Dialogue.instance.line ("his name again?"));
		yield return StartCoroutine(Dialogue.instance.done ());
		tutanim.SetBool ("sixthpass", true);
		Dialogue.instance.enabled = false;

		currentmenu = garynamemenu;
		selectedOption = 0;
		givingGaryAName = true;
	}
	public IEnumerator SixthOakDialogue(){

		yield return StartCoroutine(Dialogue.instance.text ("That's right! I"));
		yield return StartCoroutine(Dialogue.instance.line ("remember now! His",0));
        yield return StartCoroutine(Dialogue.instance.cont ("name is " + GameData.rivalName + "!"));
		yield return StartCoroutine(Dialogue.instance.done());
		tutanim.SetBool ("eighthpass", true);
	}
	public IEnumerator SeventhOakDialogue(){

        yield return StartCoroutine(Dialogue.instance.text (GameData.playerName + "!",1));
		yield return StartCoroutine(Dialogue.instance.para ("Your very own"));
		yield return StartCoroutine(Dialogue.instance.line ("POKeMON legend is",0));
		yield return StartCoroutine(Dialogue.instance.cont("about to unfold!",1));
		yield return StartCoroutine(Dialogue.instance.para ("A world of dreams"));
		yield return StartCoroutine(Dialogue.instance.line ("and adventures",0));
		yield return StartCoroutine(Dialogue.instance.cont ("with POKeMON",0));
		yield return StartCoroutine(Dialogue.instance.cont ("awaits! Let's go!"));
		yield return StartCoroutine(Dialogue.instance.done());
		tutanim.SetBool ("ninthpass", true);
	}
	void DisableOak(){

		oak.GetComponent<Image> ().enabled = false;
	}
	void DisableGary(){

		gary.GetComponent<Image> ().enabled = false;
	}
	void EnableGary(){

		gary.GetComponent<Image> ().enabled = true;
	}
	void DisableRed(){

		red.GetComponent<Image> ().enabled = false;
	}
	void EnableRed(){

		red.GetComponent<Image> ().enabled = true;
	}
	void GotoOverworld(){
		overworld.SetActive (true);
		white.SetActive (false);
		play.amenuactive = false;
		Dialogue.instance.deactivated = false;
			Player.disabled = false;
        Inputs.Enable("start");
		this.gameObject.SetActive (false);

	}
}
