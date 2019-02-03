using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
[System.Serializable]
public class DialogueMessage {
public bool isContinue;
public string message;
}

public enum DialogueType
{
Text,
Continue,
Done
}
public class Dialogue : MonoBehaviour {
	private string str;
	public float scrollequation;
	public static string Name, opponentName;
	public GameObject DialogueBox;
	public string stringToReveal;
	public bool fastText;
	public CustomText dialoguetext;
	public GameObject indicator;
	public bool deactivated;
	public GameObject subdialogue;
	public int taskType;
	public bool finishedText;
	public Image box;
	public bool finishedThePrompt;
	public GameCursor cursor;
	public int selectedOption;
	public GameObject yesnomenu, slotsmenu, buycoinsmenu;
	public Player play;
    public CustomText[] buycoinstext;
    MainMenu mainmenu;
    string laststring;
    public static Dialogue instance;
    public bool keepTextOnScreen;
    public UnityEvent onFinishText;
    public DialogueType currentDialogueType;
    private void Awake()
    {
        instance = this;
    }
	void Start(){
        mainmenu = MainMenu.instance;
        subdialogue.SetActive(true);
		finishedThePrompt = true;
		Name = "RED";
		finishedText = true;
		box.enabled = false;
        indicator.SetActive(false);
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);

	}


	IEnumerator AnimateText(string strComplete){
        Inputs.dialogueCheck = true;
		box.enabled = true;
		dialoguetext.enabled = true;
        dialoguetext.gameObject.SetActive(true);
		indicator.SetActive (false);

	strComplete = strComplete.Replace("<player>",GameData.playerName).Replace("<rival>",GameData.rivalName).Replace("#MON","POKéMON").Replace("//","\n");
		int i = 0;
		 if(currentDialogueType != DialogueType.Done) str = "";
else str = stringToReveal;
        if(currentDialogueType == DialogueType.Continue)
        {
            str = laststring + "\n" + "";
        }
		if(currentDialogueType != DialogueType.Done)laststring = strComplete.Substring(strComplete.IndexOf('\n')+1);
       
		
		dialoguetext.text = str;

		if(currentDialogueType != DialogueType.Done)
        {
            if(fastText){
                str += strComplete;
                dialoguetext.text = str;
                i = strComplete.Length;
            }
		while( i < strComplete.Length ){
				
		
					yield return new WaitForSeconds (0.001f);
				
			str += strComplete[i++];
			dialoguetext.text = str;
				if (!fastText) {
						yield return new WaitForSeconds (scrollequation);
				}

			}
		}

		if(currentDialogueType == DialogueType.Done){
            indicator.SetActive(true);
            while (!Inputs.pressed("a")) {
				
				yield return new WaitForSeconds (0.001f);
                if (Inputs.pressed("a")) {
                    SoundManager.instance.PlayABSound();
					break;
				}

			}
            Inputs.dialogueCheck = false;
			
            if (!keepTextOnScreen)
            {
                box.enabled = false;
                dialoguetext.text = "";
                dialoguetext.enabled = false;
                dialoguetext.gameObject.SetActive(false);
            }
            keepTextOnScreen = false;
            indicator.SetActive(false);
			finishedText = true;
			stringToReveal = "";
		}

		stringToReveal = str;
		
	

	}
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        
		if (deactivated && !Player.disabled) {
			Player.disabled = true;

		}
		buycoinstext [0].text = GameData.money.ToString ();
		buycoinstext [1].text = GameData.coins.ToString ();
		if (!finishedThePrompt) {

                if (Inputs.pressed("a")) {
					finishedThePrompt = true;
					StopAllCoroutines ();
                    Inputs.dialogueCheck = false;
					dialoguetext.text = "";
                    cursor.SetActive(false);
				}
                if (Inputs.pressed("b")) {
                switch (taskType)
                {
                    case 0:
                        selectedOption = 1;
                        break;
                    case 1:
                        selectedOption = 3;
                        break;
                }
					StopAllCoroutines ();
                    Inputs.dialogueCheck = false;
					dialoguetext.text = "";
                    cursor.SetActive(false);
					finishedThePrompt = true;
				}

				


                if (Inputs.pressed("down")) {
					selectedOption++;
                switch (taskType)
                {
                    case 0:
                        MathE.Clamp(ref selectedOption, 0, 1);
                        cursor.SetPosition(120, 72 - 16 * selectedOption);
                        break;
                    case 1:
                        MathE.Clamp(ref selectedOption, 0, 2);
                        cursor.SetPosition(120, 40 - 16 * selectedOption);
                        break;
                }
                    
                }
                if (Inputs.pressed("up")) {
					selectedOption--;
                switch (taskType)
                {
                    case 0:
                        MathE.Clamp(ref selectedOption, 0, 1);
                        cursor.SetPosition(120, 72 - 16 * selectedOption);
                        break;
                    case 1:
                        MathE.Clamp(ref selectedOption, 0, 2);
                        cursor.SetPosition(120, 40 - 16 * selectedOption);
                        break;
                }
               
				}
				
			

			

			} 
		 
		if (GameData.textChoice == 2) {
            scrollequation = 3 * 0.016f;
		}
		if (GameData.textChoice == 1) {
            scrollequation = 2f * 0.016f;
		}
		if (GameData.textChoice == 0) {
            scrollequation = 1f * 0.016f;
		}
	}


public IEnumerator text(string text){
			finishedText = false;
        currentDialogueType = DialogueType.Text;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
		yield return StartCoroutine(done());
	}
	public IEnumerator text(string text,bool keepText){
			finishedText = false;
        currentDialogueType = DialogueType.Text;
		stringToReveal = "";
		yield return StartCoroutine(AnimateText (text));
		if(!keepText)yield return StartCoroutine(done());
	}
	public IEnumerator cont(string text){
	finishedText = false;
        currentDialogueType = DialogueType.Continue;
	stringToReveal = text;
	yield return StartCoroutine(AnimateText(text));
		yield return StartCoroutine(done());
	}
    public IEnumerator cont(string text, bool keepText)
    {
        finishedText = false;
        currentDialogueType = DialogueType.Continue;
        stringToReveal = text;
        keepTextOnScreen = true;
        yield return StartCoroutine(AnimateText(text));
        yield return StartCoroutine(done());
    }
    public IEnumerator done(){
        currentDialogueType = DialogueType.Done;
		yield return StartCoroutine(AnimateText (stringToReveal));

	}
	public IEnumerator prompt(){
		selectedOption = 0;
		taskType = 0;
        cursor.SetActive(true);
        cursor.SetPosition(120, 72 - 16 * selectedOption);
		finishedText = false;
		finishedThePrompt = false;
        yesnomenu.SetActive(true);
        while (!finishedThePrompt)
        {
            yield return new WaitForSeconds(0.1f);
            if (finishedThePrompt)
            {
                break;
            }
        }
        yesnomenu.SetActive(false);
    }
	public IEnumerator slots(){
		selectedOption = 0;
		taskType = 1;
        cursor.SetActive(true);
        cursor.SetPosition(120, 40 - 16 * selectedOption);
		finishedText = false;
		finishedThePrompt = false;
        slotsmenu.SetActive(true);
        while (!finishedThePrompt)
        {
            yield return new WaitForSeconds(0.1f);
            if (finishedThePrompt)
            {
                break;
            }
        }
        slotsmenu.SetActive(false);
        cursor.SetActive(false);

	}
	public void Deactivate(){
		StopAllCoroutines ();
		finishedText = true;
        Inputs.dialogueCheck = false;
		stringToReveal = "";
		box.enabled = false;
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);
        indicator.SetActive(false);


	}

}
