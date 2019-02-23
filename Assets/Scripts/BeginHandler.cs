using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BeginHandler : MonoBehaviour {
	public GameObject  rednamemenu, garynamemenu,nameselectionmenu, currentmenu, oak, gary, red;
    public GameCursor cursor;
	public Animator tutanim;
	public int selectedOption;
	public GameObject[] allmenus;
	public GameObject overworld, white;
    public bool givingRedAName, givingGaryAName;
	public Player play;
	public NameSelection names;
    public Image playerNameMenuImage, rivalNameMenuImage;
    public Sprite redPlayerMenu, bluePlayerMenu;

    public static BeginHandler instance;
    public void Start()
    {
        Init();
    }
    // Use this for initialization
    public void InitVersion()
    {
        switch (VersionManager.instance.version)
        {
            case Version.Red:
                playerNameMenuImage.sprite = redPlayerMenu;
                rivalNameMenuImage.sprite = bluePlayerMenu;
                break;
            case Version.Blue:
                playerNameMenuImage.sprite = bluePlayerMenu;
                rivalNameMenuImage.sprite = redPlayerMenu;
                break;
        }
    }
    public void Init()
    {
        GameData.atTitleScreen = true;
        tutanim.SetTrigger("reset");
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

        if (Inputs.pressed("a"))
        {

            if (currentmenu == rednamemenu){
                if(selectedOption == 0)
            {
                currentmenu = nameselectionmenu;
                nameselectionmenu.SetActive(true);
                names.futureName = "";
            }
            else
            {
                currentmenu = null;
                switch (VersionManager.instance.version)
                {
                    case Version.Red:
                        switch (selectedOption)
                        {
                            case 1:
                                GameData.playerName = "RED";
                                break;
                            case 2:
                                GameData.playerName = "ASH";
                                break;
                            case 3:
                                GameData.playerName = "JACK";
                                break;
                        }
                        break;
                    case Version.Blue:
                        switch (selectedOption)
                        {
                            case 1:
                                GameData.playerName = "BLUE";
                                break;
                            case 2:
                                GameData.playerName = "GARY";
                                break;
                            case 3:
                                GameData.playerName = "JOHN";
                                break;
                        }
                        break;

                }
                tutanim.SetTrigger("transition");
                Dialogue.instance.enabled = true;
                givingRedAName = false;
            }
        }

            if (currentmenu == garynamemenu) {
                if(selectedOption == 0) {
                    currentmenu = nameselectionmenu;
                    nameselectionmenu.SetActive(true);
                    names.futureName = "";
                }
			else {
                    currentmenu = null;
                    switch (VersionManager.instance.version)
                    {
                        case Version.Blue:
                            switch (selectedOption)
                            {
                                case 1:
                                    GameData.rivalName = "RED";
                                    break;
                                case 2:
                                    GameData.rivalName = "ASH";
                                    break;
                                case 3:
                                    GameData.rivalName = "JACK";
                                    break;
                            }
                            break;
                        case Version.Red:
                            switch (selectedOption)
                            {
                                case 1:
                                    GameData.rivalName = "BLUE";
                                    break;
                                case 2:
                                    GameData.rivalName = "GARY";
                                    break;
                                case 3:
                                    GameData.rivalName = "JOHN";
                                    break;
                            }
                            break;

                    }
                    tutanim.SetTrigger("transition");
                    Dialogue.instance.enabled = true;
                    givingRedAName = false;
                }
            }
        }
	}

	public IEnumerator FirstOakDialogue(){
		play.menuActive = true;
		Dialogue.instance.deactivated = true;
		currentmenu = null;
		yield return Dialogue.instance.text ("Hello there!\nWelcome to the");
		yield return Dialogue.instance.cont ("world of POKeMON!");
		yield return Dialogue.instance.text ("My name is OAK!\nPeople call me");
		yield return Dialogue.instance.cont ("the POKeMON PROF!");
       tutanim.SetTrigger("transition");

    }
	public IEnumerator SecondOakDialogue(){
		yield return Dialogue.instance.text ("This world is\ninhabited by");
		yield return Dialogue.instance.cont ("creatures called");
		yield return Dialogue.instance.cont ("POKeMON!",true);
        yield return SoundManager.instance.PlayCryCoroutine(29);
		yield return Dialogue.instance.text("For some people,\nPOKéMON are");
		yield return Dialogue.instance.cont ("pets. Others use");
		yield return Dialogue.instance.cont ("them for fights.");
		yield return Dialogue.instance.text("Myself...");
		yield return Dialogue.instance.text ("I study POKeMON\nas a profession.");
        tutanim.SetTrigger("transition");

    }
	public IEnumerator ThirdOakDialogue(){
		
		yield return Dialogue.instance.text ("First, what is\nyour name?");
        tutanim.SetTrigger("transition");
        Dialogue.instance.enabled = false;

		currentmenu = rednamemenu;
		selectedOption = 0;
		givingRedAName = true;
}
	public IEnumerator FourthOakDialogue(){

		yield return Dialogue.instance.text ("Right! So your\nname is " + GameData.playerName + "!");
        tutanim.SetTrigger("transition");

    }
	public IEnumerator FifthOakDialogue(){

		yield return Dialogue.instance.text ("This is my grand-\nson. He's been");
		yield return Dialogue.instance.cont ("your rival since");
		yield return Dialogue.instance.cont ("you were a baby.");
		yield return Dialogue.instance.text ("...Erm, what is\nhis name again?");
        tutanim.SetTrigger("transition");
        Dialogue.instance.enabled = false;

		currentmenu = garynamemenu;
		selectedOption = 0;
		givingGaryAName = true;
	}
	public IEnumerator SixthOakDialogue(){

		yield return Dialogue.instance.text ("That's right! I\nremember now! His");
        yield return Dialogue.instance.cont ("name is " + GameData.rivalName + "!");
        tutanim.SetTrigger("transition");
    }
	public IEnumerator SeventhOakDialogue(){

        yield return Dialogue.instance.text (GameData.playerName + "!");
		yield return Dialogue.instance.text ("Your very own\nPOKéMON legend is");
		yield return Dialogue.instance.cont("about to unfold!");
        yield return Dialogue.instance.text("A world of dreams\nand adventures");
		yield return Dialogue.instance.cont ("with POKeMON");
		yield return Dialogue.instance.cont ("awaits! Let's go!");
        SoundManager.instance.FadeSong();
        tutanim.SetTrigger("transition");
    }

	void GotoOverworld(){
		overworld.SetActive (true);
		white.SetActive (false);
		play.menuActive = false;
Player.instance.GetComponent<BoxCollider2D>().enabled = true;
		Dialogue.instance.deactivated = false;
			Player.disabled = false;
        Inputs.Enable("start");
        GameData.atTitleScreen = false;
        Player.instance.FadeToCurrentAreaSong();
		this.gameObject.SetActive (false);

	}

}
