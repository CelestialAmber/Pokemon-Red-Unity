﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour {
	private string str;
	public float scrollequation;
	public static string Name, opponentName;
	public bool nextIsCont, nextisPara;
	public GameObject DialogueBox;
	public string stringToReveal;
	public bool cantscroll, displaysimmediate;
	//public Text dialoguetext;
	public CustomText dialoguetext;
	public GameObject indicator;
	public bool deactivated;
	public GameObject subdialogue;
	public int taskType;
	public int textSpeed;
    Get get = new Get();
	public bool finishedWithTextOverall;
	public bool finishedCurrentTask;
	public Image box;
	public bool finishedThePrompt;
	public Image theindicator;
	public GameObject cursor;
	public int selectedOption;
    public bool displayimmediatenoscroll;
	public Slots lots;
	public GameObject yesnomenu, slotsmenu, buycoinsmenu;
	public GameObject[] options;
	public Player play;
    public CustomText[] buycoinstext;
    string laststring;
	void Start(){
        subdialogue.SetActive(true);
		finishedThePrompt = true;
		Name = "RED";
		finishedCurrentTask = true;
		finishedWithTextOverall = true;
		box.enabled = false;
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);
		theindicator.enabled = false;

	}


	IEnumerator AnimateText(string strComplete){
        
		box.enabled = true;
		dialoguetext.enabled = true;
        dialoguetext.gameObject.SetActive(true);
		theindicator.enabled = true;
		indicator.SetActive (false);

		int i = 0;
		if (taskType == 3) {
			str = stringToReveal + "\n" +  "";
		} if(taskType != 5 && taskType != 3){
			str = "";
		}
		if (taskType == 5) {
			str = stringToReveal;

		}
        if(taskType == 4){
            str = laststring + "\n" + "";
        }
            laststring = strComplete;

		
		dialoguetext.text = str;

		if(taskType != 5){
            if(displaysimmediate){
                str = strComplete;
                dialoguetext.text = str;
                i = strComplete.Length;
            }
		while( i < strComplete.Length ){
				
		
					yield return new WaitForSeconds (0.001f);
				
			str += strComplete[i++];
			dialoguetext.text = str;
				if (!displayimmediatenoscroll && !displaysimmediate && !cantscroll) {
                    if (Inputs.held("a")) {
						yield return new WaitForSeconds (scrollequation  / 2);
					
					}
					if (!Inputs.held("a")) {
						yield return new WaitForSeconds (scrollequation / Time.deltaTime);

					}
				}

			}
		}

		if((nextIsCont || taskType == 5 || nextisPara)){
            while (!Inputs.pressed("a")) {
				
				yield return new WaitForSeconds (0.001f);
				if (i == strComplete.Length || taskType == 5) {
					indicator.SetActive (true);
				}
                if (Inputs.pressed("a")) {
					break;
				}

			}

		}
		if (taskType == 5) {
			box.enabled = false;
			dialoguetext.enabled = false;
            dialoguetext.gameObject.SetActive(false);
			theindicator.enabled = false;
			finishedWithTextOverall = true;
			play.WaitToInteract ();
			stringToReveal = "";
		}
		stringToReveal = str;
		if (!cantscroll) {
			finishedCurrentTask = true;	
		}
        if (displayimmediatenoscroll)
        {
            finishedWithTextOverall = true;
        }
        displayimmediatenoscroll = false;
		nextIsCont = false;
			cantscroll = false;
		displaysimmediate = false;
		nextisPara = false;

	}
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if (deactivated) {
			Player.disabled = true;

		}
		buycoinstext [0].text = SaveData.money.ToString ();
		buycoinstext [1].text = SaveData.coins.ToString ();
		yesnomenu.SetActive (!finishedThePrompt);
		slotsmenu.SetActive (!finishedThePrompt);
		if(taskType != 6){
			yesnomenu.SetActive(false);
		}
		if(taskType != 7){
			slotsmenu.SetActive(false);
		}
		if (!finishedThePrompt) {
			if (taskType == 6) {
                if (Inputs.pressed("a")) {
					finishedThePrompt = true;
					StopAllCoroutines ();
					dialoguetext.text = "";

					finishedCurrentTask = true;
				}
                if (Inputs.pressed("b")) {
					selectedOption = 1;
					StopAllCoroutines ();

					play.WaitToInteract ();
					dialoguetext.text = "";
					finishedThePrompt = true;
					finishedCurrentTask = true;

				}
				options = new GameObject[yesnomenu.transform.childCount];

				for (int i = 0; i < yesnomenu.transform.childCount; i++) {


					options [i] = yesnomenu.transform.GetChild (i).gameObject;
				}

				cursor.transform.position = options [selectedOption].transform.position;

				cursor.SetActive (true);

                if (Inputs.pressed("down")) {
					selectedOption++;
				}
                if (Inputs.pressed("up")) {
					selectedOption--;
				}
				if (selectedOption < 0) {
					selectedOption = 0;

				}
				if (selectedOption == options.Length) {
					selectedOption = options.Length - 1;

				}
			}
			if (taskType == 7) {
                if (Inputs.pressed("a")) {
					finishedThePrompt = true;
					StopAllCoroutines ();
					dialoguetext.text = "";
					finishedCurrentTask = true;
					finishedWithTextOverall = true;
				}
                if (Inputs.pressed("b")) {
					finishedThePrompt = true;
					Deactivate ();
					StopAllCoroutines ();
					play.WaitToInteract ();
					dialoguetext.text = "";
					finishedCurrentTask = true;
					finishedWithTextOverall = true;
					lots.Exit ();


				}
				options = new GameObject[slotsmenu.transform.childCount];

				for (int i = 0; i < slotsmenu.transform.childCount; i++) {


					options [i] = slotsmenu.transform.GetChild (i).gameObject;
				}

				cursor.transform.position = options [selectedOption].transform.position;

				cursor.SetActive (true);

                if (Inputs.pressed("down")) {
					selectedOption++;
				}
                if (Inputs.pressed("up")) {
					selectedOption--;
				}
				if (selectedOption < 0) {
					selectedOption = 0;

				}
				if (selectedOption == options.Length) {
					selectedOption = options.Length - 1;

				}
			}

			} else if(get.menu().currentmenu == null) {
				cursor.SetActive (false);
			}
		 
		if (textSpeed == 1) {
            scrollequation = 1 * Time.deltaTime;
		}
		if (textSpeed == 2) {
            scrollequation = 0.75f * Time.deltaTime;
		}
		if (textSpeed == 3) {
            scrollequation = .5f * Time.deltaTime;
		}
	}

	public IEnumerator para(string text){
		finishedCurrentTask = false;
		taskType = 1;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}
/// <summary>
/// Initiates a paragraph text dialogue message signifying the next as a specific type. 0 for continue, 1 for paragraph.
/// </summary>
/// <returns>The text.</returns>
/// <param name="text">Text.</param>
/// <param name="nextypeindex">Nextypeindex.</param>
public IEnumerator para(string text, int nextypeindex)
{
	if (nextypeindex == 0)
		nextIsCont = true;
	else if (nextypeindex == 1)
		nextisPara = true;
				finishedCurrentTask = false;
		taskType = 1;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}
public IEnumerator text(string text){
		finishedWithTextOverall = false;
		finishedCurrentTask = false;
		taskType = 2;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}
	/// <summary>
	/// Initiates a starting text dialogue message signifying the next as a specific type. 0 for continue, 1 for paragraph.
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="text">Text.</param>
	/// <param name="nextypeindex">Nextypeindex.</param>
public IEnumerator text(string text, int nextypeindex)
{
	if (nextypeindex == 0)
		nextIsCont = true;
	else if (nextypeindex == 1)
		nextisPara = true;
			finishedWithTextOverall = false;
		finishedCurrentTask = false;
		taskType = 2;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}
public IEnumerator line(string text){
					finishedCurrentTask = false;
		taskType = 3;

		yield return StartCoroutine(AnimateText (text));
	}
/// <summary>
/// Initiates a generic text dialogue message signifying the next as a specific type. 0 for continue, 1 for paragraph.
/// </summary>
/// <returns>The text.</returns>
/// <param name="text">Text.</param>
/// <param name="nextypeindex">Nextypeindex.</param>
public IEnumerator line(string text, int nextypeindex)
{
	if (nextypeindex == 0)
		nextIsCont = true;
	else if (nextypeindex == 1)
		nextisPara = true;
	finishedCurrentTask = false;
	taskType = 3;

	yield return StartCoroutine(AnimateText(text));	}
public IEnumerator cont(string text){
					finishedCurrentTask = false;
		taskType = 4;
		stringToReveal =   text;
		yield return StartCoroutine(AnimateText (text));

	}
/// <summary>
/// Initiates a continuing text dialogue message signifying the next as a specific type. 0 for continue, 1 for paragraph.
/// </summary>
/// <returns>The text.</returns>
/// <param name="text">Text.</param>
/// <param name="nextypeindex">Nextypeindex.</param>
public IEnumerator cont(string text, int nextypeindex)
{
	if (nextypeindex == 0)
		nextIsCont = true;
	else if (nextypeindex == 1)
		nextisPara = true;
	finishedCurrentTask = false;
	taskType = 4;
	stringToReveal = text;
	yield return StartCoroutine(AnimateText(text));
	}
public IEnumerator done(){
		taskType = 5;
					finishedCurrentTask = false;
		yield return StartCoroutine(AnimateText (stringToReveal));

	}
	public IEnumerator prompt(){
		selectedOption = 0;
		taskType = 6;
		finishedCurrentTask = false;
		finishedWithTextOverall = false;
		finishedThePrompt = false;
        while (!finishedThePrompt)
        {
            yield return new WaitForSeconds(0.1f);
            if (finishedThePrompt)
            {
                break;
            }
        }
	}
	public void slots(){
		selectedOption = 0;
		taskType = 7;
		finishedCurrentTask = false;
		finishedWithTextOverall = false;
		finishedThePrompt = false;


	}
	public void Deactivate(){
		StopAllCoroutines ();
		finishedCurrentTask = true;
		finishedWithTextOverall = true;
		stringToReveal = "";
		box.enabled = false;
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);
		theindicator.enabled = false;

	}

}
