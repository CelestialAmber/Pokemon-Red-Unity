using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class title : MonoBehaviour {
	public bool HasData;
	public GameObject titlemenu, startmenu;
	public GameObject nodatamenu, datamenu, continuemenu, options;
    public Cursor cursor;
	public GameObject[] startmenus;
	public GameObject currentMenu;
	public GameObject[] menuSlots;
	public Animator titleanim;
	public bool isCursorDisabled;
	public int selectedOption;
	public GameObject tutorialmanager;
	public Dialogue mylog;
	public Options opt;

	// Use this for initialization
	void Start () {
		startmenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isCursorDisabled) {
			

		
            if (Inputs.pressed("down")) {
				selectedOption++;
			}
            if (Inputs.pressed("up")) {
				selectedOption--;
			}
			if (selectedOption < 0) {
				selectedOption = 0;

			}
			if (currentMenu != options) {
				if (selectedOption == menuSlots.Length) {
					selectedOption = menuSlots.Length - 1;

				}
			}
			if (currentMenu == options) {
				if (selectedOption == 4) {
					selectedOption = 3;

				}
			}
		}



			cursor.SetActive (!isCursorDisabled);
			if (!startmenu.activeSelf) {
				isCursorDisabled = true;

			} else {
				isCursorDisabled = false;
			}
		if (currentMenu != null) {
			if (currentMenu.transform.childCount == 0) {
				isCursorDisabled = true;

			} else {
				isCursorDisabled = false;
			}
			menuSlots = new GameObject[currentMenu.transform.childCount];
		
			for (int i = 0; i < currentMenu.transform.childCount; i++) {
				if (i == 4) {
					break;
				}
					
				menuSlots [i] = currentMenu.transform.GetChild (i).gameObject;
			}
		}
		
			if (Inputs.pressed("a")) {
			if (titlemenu.activeInHierarchy) {
				Invoke ("GotoStart", .2f);

			}
			if (currentMenu == nodatamenu && selectedOption == 0) {
				tutorialmanager.SetActive (true);
				startmenu.SetActive (false);
				this.gameObject.SetActive (false);

			}
			if (currentMenu == nodatamenu && selectedOption == 	1) {
				options.SetActive (true);
				currentMenu = options;

			}
			if (currentMenu == datamenu && selectedOption == 	2) {
				options.SetActive (true);
				currentMenu = options;

			}
			if (currentMenu == options && selectedOption == 	3) {
				opt.selectedOption = 0;
				options.SetActive (false);
				currentMenu = nodatamenu;
				selectedOption = 0;


			}
			}
		if (Inputs.pressed("b") && startmenu.activeInHierarchy) {
			if ((currentMenu == nodatamenu || currentMenu == datamenu)) {
				startmenu.SetActive (false);
				titlemenu.SetActive (true);
				currentMenu = null;
				titleanim.SetBool ("revisiting", true);
				Invoke ("NotRevisiting", 0.1f);

			}
			if (currentMenu == options) {
				currentMenu = nodatamenu;
				selectedOption = 0;
			}
		}
			foreach (GameObject menu in startmenus) {
				if (menu != currentMenu) {
					menu.SetActive (false);
				} else {

					menu.SetActive (true);
				}


			}
			
		if (menuSlots.Length != 0) {

			cursor.transform.position = menuSlots [selectedOption].transform.position;
		}

	}
	void NotRevisiting(){
		titleanim.SetBool ("revisiting", false);

	}
	void GotoStart(){
		titlemenu.SetActive (false);
		startmenu.SetActive (true);
		if (!HasData) {
			currentMenu = nodatamenu;

		} else {

			currentMenu = datamenu;
		}
	}



}
