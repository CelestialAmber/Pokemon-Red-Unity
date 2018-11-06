using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour {
	private string str;
	public float scrollequation;
	public static string Name, opponentName;
	public bool needButtonPress;
	public GameObject DialogueBox;
	public string stringToReveal;
	public bool  displaysimmediate;
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
	public GameCursor cursor;
	public int selectedOption;
	public Slots lots;
	public GameObject yesnomenu, slotsmenu, buycoinsmenu;
	public Player play;
    public CustomText[] buycoinstext;
    MainMenu mainmenu;
    string laststring;
    public static Dialogue instance;
    private void Awake()
    {
        instance = this;
    }
	void Start(){
        mainmenu = Get.menu;
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
        Inputs.dialogueCheck = true;
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
                str += strComplete;
                dialoguetext.text = str;
                i = strComplete.Length;
            }
		while( i < strComplete.Length ){
				
		
					yield return new WaitForSeconds (0.001f);
				
			str += strComplete[i++];
			dialoguetext.text = str;
				if (!displaysimmediate) {
                    if (Inputs.held("a")) {
						yield return new WaitForSeconds (scrollequation  / 2);
					
					}
					if (!Inputs.held("a")) {
						yield return new WaitForSeconds (scrollequation / Time.deltaTime);

					}
				}

			}
		}

		if((needButtonPress || taskType == 5 )){
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
            Inputs.dialogueCheck = false;
			box.enabled = false;
            dialoguetext.text = "";
			dialoguetext.enabled = false;
            dialoguetext.gameObject.SetActive(false);
			theindicator.enabled = false;
			finishedWithTextOverall = true;
			finishedCurrentTask = true;
			stringToReveal = "";
		}
		stringToReveal = str;
		
      
		needButtonPress = false;
		
	

	}
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if (deactivated && !Player.disabled) {
			Player.disabled = true;

		}
		buycoinstext [0].text = GameData.money.ToString ();
		buycoinstext [1].text = GameData.coins.ToString ();
		yesnomenu.SetActive (!finishedThePrompt);
		slotsmenu.SetActive (!finishedThePrompt);
		if(taskType != 6 && yesnomenu.activeSelf){
			yesnomenu.SetActive(false);
		}
        if(taskType != 7 && slotsmenu.activeSelf){
			slotsmenu.SetActive(false);
		}
		if (!finishedThePrompt) {
			if (taskType == 6) {
                if (Inputs.pressed("a")) {
					finishedThePrompt = true;
					StopAllCoroutines ();
                    Inputs.dialogueCheck = false;
					dialoguetext.text = "";
                    cursor.SetActive(false);
					finishedCurrentTask = true;
				}
                if (Inputs.pressed("b")) {
					selectedOption = 1;
					StopAllCoroutines ();
                    Inputs.dialogueCheck = false;
					dialoguetext.text = "";
                    cursor.SetActive(false);
					finishedThePrompt = true;
					finishedCurrentTask = true;
				}

				


                if (Inputs.pressed("down")) {
					selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 1);
                    cursor.SetPosition(120, 72 - 16 * selectedOption);
                }
                if (Inputs.pressed("up")) {
					selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 1);
                    cursor.SetPosition(120, 72 - 16 * selectedOption);
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
					dialoguetext.text = "";
					finishedCurrentTask = true;
					finishedWithTextOverall = true;
					lots.Exit ();


				}

               

                if(!finishedWithTextOverall && !cursor.isActive){
                    cursor.SetActive(true);
                }

                if (Inputs.pressed("down")) {
					selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 2);
                    cursor.SetPosition(120, 40 - 16 * selectedOption);
				}
                if (Inputs.pressed("up")) {
					selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 2);
                    cursor.SetPosition(120, 40 - 16 * selectedOption);
				}
			}

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
public IEnumerator para(string text, bool buttonPress)
{
	if (buttonPress)
		needButtonPress = true;
				finishedCurrentTask = false;
		taskType = 1;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}

public IEnumerator text(string text, bool buttonPress)
{
	if (buttonPress)
		needButtonPress = true;
			finishedWithTextOverall = false;
		finishedCurrentTask = false;
		taskType = 2;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}
	public IEnumerator text(string text)
{
			finishedWithTextOverall = false;
		finishedCurrentTask = false;
		taskType = 2;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
	}

public IEnumerator line(string text, bool buttonPress)
{
	if (buttonPress)
		needButtonPress = true;
	finishedCurrentTask = false;
	taskType = 3;

	yield return StartCoroutine(AnimateText(text));
		}
		public IEnumerator line(string text){

	finishedCurrentTask = false;
	taskType = 3;

	yield return StartCoroutine(AnimateText(text));
		}


public IEnumerator cont(string text, bool buttonPress)
{
	if (buttonPress)
		needButtonPress = true;
	finishedCurrentTask = false;
	taskType = 4;
	stringToReveal = text;
	yield return StartCoroutine(AnimateText(text));
	}
	public IEnumerator cont(string text)
{

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
        cursor.SetActive(true);
        cursor.SetPosition(120, 72 - 16 * selectedOption);
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
	public IEnumerator slots(){
		selectedOption = 0;
		taskType = 7;
        cursor.SetActive(true);
        cursor.SetPosition(120, 40 - 16 * selectedOption);
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
cursor.SetActive(false);

	}
	public void Deactivate(){
		StopAllCoroutines ();
		finishedCurrentTask = true;
		finishedWithTextOverall = true;
        Inputs.dialogueCheck = false;
		stringToReveal = "";
		box.enabled = false;
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);
		theindicator.enabled = false;
	}

}
