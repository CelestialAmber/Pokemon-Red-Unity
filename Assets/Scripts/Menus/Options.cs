using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MathE{
    public static void Clamp (ref int var, int min, int max){
       var = (int)Mathf.Clamp((float)var, (float)min, (float)max);
    }

    public static void Wrap(ref int var, int min, int max)
    {
        var = var < min ? max : var > max ? min : var;
    }
}
public class Options : MonoBehaviour {
	public GameCursor cursor;
	public GameObject[] textSlots, animSlots, battleSlots, menuSlots;
	public int selectedOption;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		menuSlots = new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++) {
			if (i == 4) {
				break;
			}

			menuSlots [i] = transform.GetChild (i).gameObject;
		}
        if (Inputs.pressed("left")) {
			if (selectedOption == 0) {
				GameData.textChoice--;
                MathE.Clamp(ref GameData.textChoice, 0, 2);
			}
			if (selectedOption == 1) {
                GameData.animationChoice--;
                MathE.Clamp(ref GameData.animationChoice, 0, 1);
			}
			if (selectedOption == 2) {
                GameData.battleChoice--;
                MathE.Clamp(ref GameData.battleChoice, 0, 1);
			}
		}

        if (Inputs.pressed("right")) {

			if (selectedOption == 0) {
                GameData.textChoice++;
                MathE.Clamp(ref GameData.textChoice, 0, 2);
			}
			if (selectedOption == 1) {
                GameData.animationChoice++;
                MathE.Clamp(ref GameData.animationChoice, 0, 1);
			}
			if (selectedOption == 2) {
                GameData.battleChoice++;
                MathE.Clamp(ref GameData.battleChoice,0, 1);
			}

		}

        if (Inputs.pressed("down")) {
			selectedOption++;
            MathE.Clamp(ref selectedOption, 0, 3);
		}
        if (Inputs.pressed("up")) {
			selectedOption--;
            MathE.Clamp(ref selectedOption, 0, 3);
		}
	
	
		textSlots = new GameObject[3];
		animSlots = new GameObject[2];
		battleSlots = new GameObject[2];
	
			for (int i = 0; i < transform.GetChild(0).childCount; i++) {

				textSlots [i] = transform.GetChild(0).GetChild (i).gameObject;
			}
			for (int i = 0; i < transform.GetChild(1).childCount; i++) {

				animSlots [i] = transform.GetChild(1).GetChild (i).gameObject;
			}
			for (int i = 0; i < transform.GetChild(2).childCount; i++) {

				battleSlots [i] = transform.GetChild(2).GetChild (i).gameObject;
			}


	
	if (menuSlots.Length != 0) {
			
		cursor.transform.position = menuSlots [selectedOption].transform.position;
		
			if (selectedOption == 0) {
				cursor.transform.position = textSlots [GameData.textChoice].transform.position;
			}
			if (selectedOption == 1) {
                cursor.transform.position = animSlots [GameData.animationChoice].transform.position;
			}
			if (selectedOption == 2) {
                cursor.transform.position = battleSlots [GameData.battleChoice].transform.position;
			}
		

	}
	}
}
