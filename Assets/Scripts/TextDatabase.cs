using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDatabase : MonoBehaviour {
	public GameObject itemPCMenu, shopmenu, slotmenu;
    public GameCursor cursor;
	public Items itemDatabase;
	public PokeMart pokeMart;
    public Slots slots;
	public static TextDatabase instance;
    public IEnumerator[] enumerators;
	void Awake(){
		instance = this;
	}
	// Use this for initialization

	// Update is called once per frame
	void Update () {

	}
	public void PlayText(int ID){
		switch (ID) {
            case 1:
                StartCoroutine("Text1");
                break;
		case 2:
			StartCoroutine ("Text2");
			break;
		case 3:
			StartCoroutine ("Text3");
			break;
		case 4:
			StartCoroutine ("Text4");
			break;
		
		}




	}
    public void GetItem(string item){
            StartCoroutine(GetItemText(item));
    }

    public IEnumerator GetItemText(string item){
        itemDatabase.AddItem(item, 1,false);
        yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + " found \n"+item.ToUpper() + "!"));


    }
    IEnumerator Text1()
    {
        if (Player.instance.direction == Direction.Left || Player.instance.direction == Direction.Right)
        {
            if (GameData.coins > 0)
            {
                yield return StartCoroutine(Dialogue.instance.text("A slot machine!\nWant to play?",true));
                yield return StartCoroutine(Dialogue.instance.prompt());
                if (Dialogue.instance.selectedOption == 0)
                {
                    Dialogue.instance.Deactivate();
                    Player.disabled = true;
                    StartCoroutine(Player.instance.DisplayEmotiveBubble(1));
                    while (Player.instance.displayingEmotion)
                    {
                        yield return new WaitForSeconds(0.1f);
                        if (!Player.instance.displayingEmotion)
                        {
                            break;
                        }
                    }
                    Player.disabled = true;
                    slotmenu.SetActive(true);
                    Inputs.Disable("start");
                    StartCoroutine(Slots.instance.Initialize());

                }
                else
                {
                    Dialogue.instance.Deactivate();
                }

            }
            else
            {
                yield return StartCoroutine(Dialogue.instance.text("You don't have any\ncoins!"));


            }
        }
    }


    IEnumerator Text2(){
		Dialogue.instance.Deactivate ();
        yield return StartCoroutine(Dialogue.instance.text (GameData.playerName + " turned on\nthe PC!"));
		Player.instance.menuActive = true;
		itemPCMenu.SetActive (true);
        Inputs.Disable("start");
        StartCoroutine(itemPCMenu.GetComponent<PC> ().Initialize ());
	}
	IEnumerator Text3(){
		Dialogue.instance.Deactivate ();
		yield return StartCoroutine(Dialogue.instance.text ("Battle!"));
		Player.instance.StartTrainerBattle(0);


	}
	IEnumerator Text4(){
		Dialogue.instance.Deactivate ();
        yield return StartCoroutine(Dialogue.instance.text("Hi there!\nMay I help you?"));
		Player.instance.menuActive = true;
        cursor.SetActive(true);
        Inputs.Disable("start");
		pokeMart.martlist = itemDatabase.IndigoItems;
		pokeMart.Init();
		shopmenu.SetActive (true);
        pokeMart.currentMenu = pokeMart.buysellwindow;


	}





}
