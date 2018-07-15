using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {
	public GameObject cursor;
	public GameObject[] textSlots, animSlots, battleSlots, menuSlots;
	public int textChoice, animationChoice, battleChoice;
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
				textChoice--;
			}
			if (selectedOption == 1) {
				animationChoice--;
			}
			if (selectedOption == 2) {
				battleChoice--;
			}
		}

        if (Inputs.pressed("right")) {

			if (selectedOption == 0) {
				textChoice++;
			}
			if (selectedOption == 1) {
				animationChoice++;
			}
			if (selectedOption == 2) {
				battleChoice++;
			}

		}
		if (textChoice == 3) {
			textChoice = 2;

		}
		if (textChoice == -1) {
			textChoice = 0;

		}
		if (animationChoice == 2) {
			animationChoice = 1;

		}
		if (animationChoice == -1) {
			animationChoice = 0;

		}
		if (battleChoice == 2) {
			battleChoice = 1;

		}
		if (battleChoice == -1) {
			battleChoice = 0;

		}
        if (Inputs.pressed("up")) {
			selectedOption++;
		}
        if (Inputs.pressed("up")) {
			selectedOption--;
		}
		if (selectedOption < 0) {
			selectedOption = 0;

		}
		if (selectedOption == 4) {
			selectedOption = 3;

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
				cursor.transform.position = textSlots [textChoice].transform.position;
			}
			if (selectedOption == 1) {
				cursor.transform.position = animSlots [animationChoice].transform.position;
			}
			if (selectedOption == 2) {
				cursor.transform.position = battleSlots [battleChoice].transform.position;
			}
		

	}
	}
}
