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
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (isLowerCase) {
			nameselectscreen.sprite = lowercase;
		} else {
			nameselectscreen.sprite = uppercase;
		}
		displaytext.text = futureName;
		if (BeginHandler.instance.givingRedAName || BeginHandler.instance.givingGaryAName) {
			maxNameSize = 7;
		} else {
			maxNameSize = 10;
		}
		if (currentYselection == 5) {
			currentXselection = 0;

		}

		cursor.SetPosition(8 + 16 * currentXselection, 96 - 16 * currentYselection);
        if (Inputs.pressed("right")) {
			if (currentYselection != 5) {
				currentXselection++;
			}
		}
        if (Inputs.pressed("left")) {
			if (currentYselection != 5) {
				currentXselection--;
			}
		}
        if (Inputs.pressed("up")) {

			currentYselection--;
		}
        if (Inputs.pressed("down")) {

			currentYselection++;
		}
        MathE.Wrap(ref currentXselection, 0, 8);
        MathE.Wrap(ref currentYselection, 0, 5);

		if (Inputs.pressed("b")) {
			if(futureName.Length > 0) futureName = futureName.Remove (futureName.Length - 1);
		}
        if (Inputs.pressed("start")) {

			if (futureName.Length != 0) {
				if (BeginHandler.instance.givingRedAName) {
					GameData.instance.playerName = futureName;
                    BeginHandler.instance.tutanim.SetTrigger("transition");
                    Dialogue.instance.enabled = true;
                    BeginHandler.instance.givingRedAName = false;
					this.gameObject.SetActive (false);
				}
				if (BeginHandler.instance.givingGaryAName) {
                    GameData.instance.rivalName = futureName;
                    BeginHandler.instance.tutanim.SetTrigger("transition");
                    Dialogue.instance.enabled = true;
                    BeginHandler.instance.givingGaryAName = false;
					this.gameObject.SetActive (false);
				}

                BeginHandler.instance.currentmenu = null;
			}


		}
		if (Inputs.pressed("a")) {
            if (currentXselection == 8 && currentYselection == 4)
            {
                if (futureName.Length != 0)
                {
                    if (BeginHandler.instance.givingRedAName)
                    {
                        GameData.instance.playerName = futureName;
                        BeginHandler.instance.tutanim.SetTrigger("transition");
                        Dialogue.instance.enabled = true;
                        BeginHandler.instance.givingRedAName = false;
						cursor.SetActive(false);
                        this.gameObject.SetActive(false);
                    }
                    if (BeginHandler.instance.givingGaryAName)
                    {
                        GameData.instance.rivalName = futureName;
                        BeginHandler.instance.tutanim.SetTrigger("transition");
                        Dialogue.instance.enabled = true;
                        BeginHandler.instance.givingGaryAName = false;
						cursor.SetActive(false);
                        this.gameObject.SetActive(false);
                    }

                    BeginHandler.instance.currentmenu = null;
                }
            }
			else{
            
				
				if (currentXselection == 0 && currentYselection == 5) {
					isLowerCase = !isLowerCase;
				}else if (futureName.Length == maxNameSize) return;
				else futureName += currentYselection < 3 && isLowerCase ? characters[currentYselection,currentXselection].ToLower() : characters[currentYselection,currentXselection];

			}
				
				

			

		}
	}
}
