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
        switch (GameData.instance.version)
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
        GameData.instance.atTitleScreen = true;
        tutanim.SetTrigger("reset");
        cursor.SetActive (false);
    }
	// Update is called once per frame
	void Update () {
		
		if (currentmenu == null && currentmenu != nameselectionmenu)  {
		
            if (Inputs.pressed("down")) {
				selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 3);
                 cursor.SetPosition(8,120 - 16 * selectedOption);
			}
            if (Inputs.pressed("up")) {
				selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 3);
                 cursor.SetPosition(8,120 - 16 * selectedOption);
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
                                GameData.instance.playerName = "RED";
                                break;
                            case 2:
                                GameData.instance.playerName = "ASH";
                                break;
                            case 3:
                                GameData.instance.playerName = "JACK";
                                break;
                        }
                        break;
                    case Version.Blue:
                        switch (selectedOption)
                        {
                            case 1:
                                GameData.instance.playerName = "BLUE";
                                break;
                            case 2:
                                GameData.instance.playerName = "GARY";
                                break;
                            case 3:
                                GameData.instance.playerName = "JOHN";
                                break;
                        }
                        break;

                }
                tutanim.SetTrigger("transition");
                Dialogue.instance.enabled = true;
                givingRedAName = false;
                 cursor.SetActive(false);
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
                    switch (GameData.instance.version)
                    {
                        case Version.Blue:
                            switch (selectedOption)
                            {
                                case 1:
                                    GameData.instance.rivalName = "RED";
                                    break;
                                case 2:
                                    GameData.instance.rivalName = "ASH";
                                    break;
                                case 3:
                                    GameData.instance.rivalName = "JACK";
                                    break;
                            }
                            break;
                        case Version.Red:
                            switch (selectedOption)
                            {
                                case 1:
                                    GameData.instance.rivalName = "BLUE";
                                    break;
                                case 2:
                                    GameData.instance.rivalName = "GARY";
                                    break;
                                case 3:
                                    GameData.instance.rivalName = "JOHN";
                                    break;
                            }
                            break;

                    }
                    tutanim.SetTrigger("transition");
                    Dialogue.instance.enabled = true;
                    givingRedAName = false;
                    cursor.SetActive(false);
                }
            }
        }
	}

	public IEnumerator FirstOakDialogue(){
		play.menuActive = true;
		Dialogue.instance.deactivated = true;
		currentmenu = null;
		yield return Dialogue.instance.text ("Hello there!\nWelcome to the");
		yield return Dialogue.instance.cont ("world of POKéMON!");
		yield return Dialogue.instance.text ("My name is OAK!\nPeople call me");
		yield return Dialogue.instance.cont ("the POKéMON PROF!");
       tutanim.SetTrigger("transition");

    }
	public IEnumerator SecondOakDialogue(){
		yield return Dialogue.instance.text ("This world is\ninhabited by");
		yield return Dialogue.instance.cont ("creatures called");
		yield return Dialogue.instance.cont ("POKéMON!",true);
        yield return SoundManager.instance.PlayCryCoroutine(29);
		yield return Dialogue.instance.text("For some people,\nPOKéMON are");
		yield return Dialogue.instance.cont ("pets. Others use");
		yield return Dialogue.instance.cont ("them for fights.");
		yield return Dialogue.instance.text("Myself...");
		yield return Dialogue.instance.text ("I study POKéMON\nas a profession.");
        tutanim.SetTrigger("transition");

    }
	public IEnumerator ThirdOakDialogue(){
		
		yield return Dialogue.instance.text ("First, what is\nyour name?");
        tutanim.SetTrigger("transition");
        Dialogue.instance.enabled = false;
        while(!tutanim.GetCurrentAnimatorStateInfo(0).IsName("moveredright")) yield return new WaitForEndOfFrame();
        cursor.SetActive(true);
		currentmenu = rednamemenu;
		selectedOption = 0;
		givingRedAName = true;
}
	public IEnumerator FourthOakDialogue(){

		yield return Dialogue.instance.text ("Right! So your\nname is " + GameData.instance.playerName + "!");
        tutanim.SetTrigger("transition");

    }
	public IEnumerator FifthOakDialogue(){

		yield return Dialogue.instance.text ("This is my grand-\nson. He's been");
		yield return Dialogue.instance.cont ("your rival since");
		yield return Dialogue.instance.cont ("you were a baby.");
		yield return Dialogue.instance.text ("...Erm, what is\nhis name again?");
        tutanim.SetTrigger("transition");
        Dialogue.instance.enabled = false;
        while(!tutanim.GetCurrentAnimatorStateInfo(0).IsName("movegaryright")) yield return new WaitForEndOfFrame();
        cursor.SetActive(true);
		currentmenu = garynamemenu;
		selectedOption = 0;
		givingGaryAName = true;
	}
	public IEnumerator SixthOakDialogue(){

		yield return Dialogue.instance.text ("That's right! I\nremember now! His");
        yield return Dialogue.instance.cont ("name is " + GameData.instance.rivalName + "!");
        tutanim.SetTrigger("transition");
    }
	public IEnumerator SeventhOakDialogue(){

        yield return Dialogue.instance.text (GameData.instance.playerName + "!");
		yield return Dialogue.instance.text ("Your very own\nPOKéMON legend is");
		yield return Dialogue.instance.cont("about to unfold!");
        yield return Dialogue.instance.text("A world of dreams\nand adventures");
		yield return Dialogue.instance.cont ("with POKéMON");
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
        GameData.instance.atTitleScreen = false;
        Player.instance.FadeToCurrentAreaSong();
		this.gameObject.SetActive (false);

	}

}
