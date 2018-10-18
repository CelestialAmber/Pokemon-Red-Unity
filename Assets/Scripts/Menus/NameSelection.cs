using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]


public class NameSelection : MonoBehaviour {
	public int currentXselection, currentYselection;
	public GameCursor cursor;
	public TutorialHandler tut;
	public int maxNameSize;
	public string futureName;
	public Sprite uppercase, lowercase;
	public Image nameselectscreen;
	public bool isLowerCase;
	public  CustomText displaytext;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isLowerCase) {
			nameselectscreen.sprite = lowercase;
		} else {
			nameselectscreen.sprite = uppercase;
		}
		displaytext.text = futureName;
		if (tut.givingRedAName || tut.givingGaryAName) {
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
			futureName = futureName.Remove (futureName.Length - 1);
		}
        if (Inputs.pressed("start")) {

			if (futureName.Length != 0) {
				if (tut.givingRedAName) {
					GameData.playerName = futureName;
					tut.tutanim.SetBool ("fourthpass", true);
                    Dialogue.instance.enabled = true;
					tut.givingRedAName = false;
					this.gameObject.SetActive (false);
				}
				if (tut.givingGaryAName) {
                    GameData.rivalName = futureName;
					tut.tutanim.SetBool ("seventhpass", true);
                    Dialogue.instance.enabled = true;
					tut.givingGaryAName = false;
					this.gameObject.SetActive (false);
				}

				tut.currentmenu = null;
			}


		}
		if (Inputs.pressed("a")) {
			
			if (isLowerCase) {
				if (currentXselection == 8 && currentYselection == 4) {
					if (futureName.Length != 0) {
						if (tut.givingRedAName) {
                            GameData.playerName = futureName;
							tut.tutanim.SetBool ("fourthpass", true);
                            Dialogue.instance.enabled = true;
							tut.givingRedAName = false;
							this.gameObject.SetActive (false);
						}
						if (tut.givingGaryAName) {
                            GameData.rivalName = futureName;
							tut.tutanim.SetBool ("seventhpass", true);
							Dialogue.instance.enabled = true;
							tut.givingGaryAName = false;
							this.gameObject.SetActive (false);
						}

						tut.currentmenu = null;
					}
				}
				if (currentXselection == 0 && currentYselection == 5) {
					isLowerCase = !isLowerCase;
				}

				if (futureName.Length == maxNameSize) {
					return;
				}
				if (currentXselection == 0 && currentYselection == 0) {
					futureName += "a";
				}

				if (currentXselection == 0 && currentYselection == 1) {
					futureName += "j";
				}


				if (currentXselection == 0 && currentYselection == 2) {
					futureName += "s";
				}

				if (currentXselection == 0 && currentYselection == 3) {
					futureName += "x";
				}

				if (currentXselection == 0 && currentYselection == 4) {
					futureName += "-";
				}


				if (currentXselection == 1 && currentYselection == 0) {
					futureName += "b";
				}

				if (currentXselection == 1 && currentYselection == 1) {
					futureName += "k";
				}


				if (currentXselection == 1 && currentYselection == 2) {
					futureName += "t";
				}

				if (currentXselection == 1 && currentYselection == 3) {
					futureName += "(";
				}

				if (currentXselection == 1 && currentYselection == 4) {
					futureName += "?";
				}

				
				
				
				if (currentXselection == 2 && currentYselection == 0) {
					futureName += "c";
				}

				if (currentXselection == 2 && currentYselection == 1) {
					futureName += "l";
				}


				if (currentXselection == 2 && currentYselection == 2) {
					futureName += "u";
				}

				if (currentXselection == 2 && currentYselection == 3) {
					futureName += ")";
				}

				if (currentXselection == 2 && currentYselection == 4) {
					futureName += "!";
				}

				


				if (currentXselection == 3 && currentYselection == 0) {
					futureName += "d";
				}

				if (currentXselection == 3 && currentYselection == 1) {
					futureName += "m";
				}


				if (currentXselection == 3 && currentYselection == 2) {
					futureName += "v";
				}

				if (currentXselection == 3 && currentYselection == 3) {
					futureName += ":";
				}

				if (currentXselection == 3 && currentYselection == 4) {
					futureName += "{"; //
				}

				


				if (currentXselection == 4 && currentYselection == 0) {
					futureName += "e";
				}

				if (currentXselection == 4 && currentYselection == 1) {
					futureName += "n";
				}


				if (currentXselection == 4 && currentYselection == 2) {
					futureName += "w";
				}

				if (currentXselection == 4 && currentYselection == 3) {
					futureName += ";";
				}

				if (currentXselection == 4 && currentYselection == 4) {
					futureName += "}"; //female 
				}

				


				if (currentXselection == 5 && currentYselection == 0) {
					futureName += "f";
				}

				if (currentXselection == 5 && currentYselection == 1) {
					futureName += "o";
				}


				if (currentXselection == 5 && currentYselection == 2) {
					futureName += "x";
				}

				if (currentXselection == 5 && currentYselection == 3) {
					futureName += "[";
				}

				if (currentXselection == 5 && currentYselection == 4) {
					futureName += "/";
				}

				


				if (currentXselection == 6 && currentYselection == 0) {
					futureName += "g";
				}

				if (currentXselection == 6 && currentYselection == 1) {
					futureName += "p";
				}


				if (currentXselection == 6 && currentYselection == 2) {
					futureName += "y";
				}

				if (currentXselection == 6 && currentYselection == 3) {
					futureName += "]";
				}

				if (currentXselection == 6 && currentYselection == 4) {
					futureName += ".";
				}


				if (currentXselection == 7 && currentYselection == 0) {
					futureName += "h";
				}

				if (currentXselection == 7 && currentYselection == 1) {
					futureName += "q";
				}


				if (currentXselection == 7 && currentYselection == 2) {
					futureName += "z";
				}

				if (currentXselection == 7 && currentYselection == 3) {
                    futureName += "Ê"; //pk
				}

				if (currentXselection == 7 && currentYselection == 4) {
					futureName += ",";
				}

				

				if (currentXselection == 8 && currentYselection == 0) {
					futureName += "i";
				}

				if (currentXselection == 8 && currentYselection == 1) {
					futureName += "r";
				}


				if (currentXselection == 8 && currentYselection == 2) {
					futureName += " ";
				}

				if (currentXselection == 8 && currentYselection == 3) {
                    futureName += "Ë"; //mn
				}

			
				

			} else {
				if (currentXselection == 8 && currentYselection == 4) {
					if (futureName.Length != 0) {
						if (tut.givingRedAName) {
                            GameData.playerName = futureName;
							tut.tutanim.SetBool ("fourthpass", true);
                            Dialogue.instance.enabled = true;
							tut.givingRedAName = false;
							this.gameObject.SetActive (false);
						}
						if (tut.givingGaryAName) {
                            GameData.rivalName = futureName;
							tut.tutanim.SetBool ("seventhpass", true);
                            Dialogue.instance.enabled = true;
							tut.givingGaryAName = false;
							this.gameObject.SetActive (false);
						}

						tut.currentmenu = null;
					}
				} 
			
				if(currentXselection == 0 && currentYselection == 5){
					isLowerCase = !isLowerCase;
				}
				if (futureName.Length == maxNameSize ){
					return;
				}
				if(currentXselection == 0 && currentYselection == 0){
					futureName += "A";
				}

				if(currentXselection == 0  && currentYselection == 1){
					futureName += "J";
				}


				if(currentXselection == 0 && currentYselection == 2){
					futureName += "S";
				}

				if(currentXselection == 0 && currentYselection == 3){
					futureName += "X";
				}

				if(currentXselection == 0 && currentYselection == 4){
					futureName += "-";
				}



				if(currentXselection == 1 && currentYselection == 0){
					futureName += "B";
				}

				if(currentXselection == 1 && currentYselection == 1){
					futureName += "K";
				}


				if(currentXselection == 1 && currentYselection == 2){
					futureName += "T";
				}

				if(currentXselection == 1 && currentYselection == 3){
					futureName += "(";
				}

				if(currentXselection == 1 && currentYselection == 4){
					futureName += "?";
				}




				if(currentXselection == 2 && currentYselection == 0){
					futureName += "C";
				}

				if(currentXselection == 2 && currentYselection == 1){
					futureName += "L";
				}


				if(currentXselection == 2 && currentYselection == 2){
					futureName += "U";
				}

				if(currentXselection == 2 && currentYselection == 3){
					futureName += ")";
				}

				if(currentXselection == 2 && currentYselection == 4){
					futureName += "!";
				}




				if(currentXselection == 3 && currentYselection == 0){
					futureName += "D";
				}

				if(currentXselection == 3 && currentYselection == 1){
					futureName += "M";
				}


				if(currentXselection == 3 && currentYselection == 2){
					futureName += "V";
				}

				if(currentXselection == 3 && currentYselection == 3){
					futureName += ":";
				}

				if(currentXselection == 3 && currentYselection == 4){
					futureName += "{"; //
				}




				if(currentXselection == 4 && currentYselection == 0){
					futureName += "E";
				}

				if(currentXselection == 4 && currentYselection == 1){
					futureName += "N";
				}


				if(currentXselection == 4 && currentYselection == 2){
					futureName += "W";
				}

				if(currentXselection == 4 && currentYselection == 3){
					futureName += ";";
				}

				if(currentXselection == 4 && currentYselection == 4){
					futureName += "}"; //female 
				}




				if(currentXselection == 5 && currentYselection == 0){
					futureName += "F";
				}

				if(currentXselection == 5 && currentYselection == 1){
					futureName += "O";
				}


				if(currentXselection == 5 && currentYselection == 2){
					futureName += "X";
				}

				if(currentXselection == 5 && currentYselection == 3){
					futureName += "[";
				}

				if(currentXselection == 5 && currentYselection == 4){
					futureName += "/";
				}




				if(currentXselection == 6 && currentYselection == 0){
					futureName += "G";
				}

				if(currentXselection == 6 && currentYselection == 1){
					futureName += "P";
				}


				if(currentXselection == 6 && currentYselection == 2){
					futureName += "Y";
				}

				if(currentXselection == 6 && currentYselection == 3){
					futureName += "]";
				}

				if(currentXselection == 6 && currentYselection == 4){
					futureName += ".";
				}


				if(currentXselection == 7 && currentYselection == 0){
					futureName += "H";
				}

				if(currentXselection == 7 && currentYselection == 1){
					futureName += "Q";
				}


				if(currentXselection == 7 && currentYselection == 2){
					futureName += "Z";
				}

				if(currentXselection == 7 && currentYselection == 3){
					futureName += "%"; //pk
				}

				if(currentXselection == 7 && currentYselection == 4){
					futureName += ",";
				}



				if(currentXselection == 8 && currentYselection == 0){
					futureName += "I";
				}

				if(currentXselection == 8 && currentYselection == 1){
					futureName += "R";
				}


				if(currentXselection == 8 && currentYselection == 2){
					futureName += " ";
				}

				if(currentXselection == 8 && currentYselection == 3){
					futureName += "$"; //mn
				}





			}

		}
	}
}
