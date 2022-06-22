using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]


public class NameSelection : MonoBehaviour {
	public int currentXselection, currentYselection;
	public GameCursor cursor;
	public int maxNameSize;
	public string futureName;
	public Sprite uppercase, lowercase;
	public Image nameselectscreen;
	public bool isLowerCase;
	public  CustomText displaytext;

	public string[,] characters = {
	{"A","B","C","D","E","F","G","H","I"},
	{"J","K","L","M","N","O","P","Q","R"},
	{"S","T","U","V","W","X","Y","Z"," "},
	{"*","(",")",":",";","[","]","<PK>","<MN>"},
	{"-","?","!","♂","♀","/",".",",",""}
	};

	// Update is called once per frame
	void Update () {
		if (isLowerCase) {
			nameselectscreen.sprite = lowercase;
		} else {
			nameselectscreen.sprite = uppercase;
		}
		displaytext.text = futureName;
		if (OakIntroCutsceneHandler.instance.givingRedAName || OakIntroCutsceneHandler.instance.givingGaryAName) {
			maxNameSize = 7;
		} else {
			maxNameSize = 10;
		}
		if (currentYselection == 5) {
			currentXselection = 0;
		}

		cursor.SetPosition(8 + 16 * currentXselection, 96 - 16 * currentYselection);
        if (InputManager.Pressed(Button.Right)) {
			if (currentYselection != 5) {
				currentXselection++;
			}
		}
        if (InputManager.Pressed(Button.Left)) {
			if (currentYselection != 5) {
				currentXselection--;
			}
		}
        if (InputManager.Pressed(Button.Up)) {

			currentYselection--;
		}
        if (InputManager.Pressed(Button.Down)) {

			currentYselection++;
		}
        MathE.Wrap(ref currentXselection, 0, 8);
        MathE.Wrap(ref currentYselection, 0, 5);

		if (InputManager.Pressed(Button.B)) {
			if(futureName.Length > 0) futureName = futureName.Remove (futureName.Length - 1);
		}
        if (InputManager.Pressed(Button.Start)) {

			if (futureName.Length != 0) {
				if (OakIntroCutsceneHandler.instance.givingRedAName) {
					GameData.instance.playerName = futureName;
                    OakIntroCutsceneHandler.instance.tutanim.SetTrigger("transition");
                    Dialogue.instance.enabled = true;
                    OakIntroCutsceneHandler.instance.givingRedAName = false;
					this.gameObject.SetActive (false);
				}
				if (OakIntroCutsceneHandler.instance.givingGaryAName) {
                    GameData.instance.rivalName = futureName;
                    OakIntroCutsceneHandler.instance.tutanim.SetTrigger("transition");
                    Dialogue.instance.enabled = true;
                    OakIntroCutsceneHandler.instance.givingGaryAName = false;
					this.gameObject.SetActive (false);
				}

                OakIntroCutsceneHandler.instance.currentmenu = null;
			}


		}
		if (InputManager.Pressed(Button.A)) {
            if (currentXselection == 8 && currentYselection == 4)
            {
                if (futureName.Length != 0)
                {
                    if (OakIntroCutsceneHandler.instance.givingRedAName)
                    {
                        GameData.instance.playerName = futureName;
                        OakIntroCutsceneHandler.instance.tutanim.SetTrigger("transition");
                        Dialogue.instance.enabled = true;
                        OakIntroCutsceneHandler.instance.givingRedAName = false;
						cursor.SetActive(false);
                        this.gameObject.SetActive(false);
                    }
                    if (OakIntroCutsceneHandler.instance.givingGaryAName)
                    {
                        GameData.instance.rivalName = futureName;
                        OakIntroCutsceneHandler.instance.tutanim.SetTrigger("transition");
                        Dialogue.instance.enabled = true;
                        OakIntroCutsceneHandler.instance.givingGaryAName = false;
						cursor.SetActive(false);
                        this.gameObject.SetActive(false);
                    }

                    OakIntroCutsceneHandler.instance.currentmenu = null;
                }
            }
			else{			
				if (currentXselection == 0 && currentYselection == 5){
					isLowerCase = !isLowerCase; //Toggle lower/upper case if the player selects the case toggle option
				} else if (futureName.Length == maxNameSize) return;
				else futureName += currentYselection < 3 && isLowerCase ? characters[currentYselection,currentXselection].ToLower() : characters[currentYselection,currentXselection];
			}
		}
	}
}
