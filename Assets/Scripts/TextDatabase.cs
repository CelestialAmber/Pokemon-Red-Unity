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
        yield return Dialogue.instance.text(GameData.playerName + " found \n"+item.ToUpper() + "!");


    }
    


    IEnumerator Text2(){
		Dialogue.instance.Deactivate ();
        yield return Dialogue.instance.text (GameData.playerName + " turned on\nthe PC!");
		Player.instance.menuActive = true;
		itemPCMenu.SetActive (true);
        Inputs.Disable("start");
        StartCoroutine(itemPCMenu.GetComponent<PC> ().Initialize ());
	}
	IEnumerator Text3(){
		Dialogue.instance.Deactivate ();
		yield return Dialogue.instance.text ("Battle!");
		Player.instance.StartTrainerBattle(0);


	}
	IEnumerator Text4(){
		Dialogue.instance.Deactivate ();
        yield return Dialogue.instance.text("Hi there!\nMay I help you?");
		Player.instance.menuActive = true;
        cursor.SetActive(true);
        Inputs.Disable("start");
		pokeMart.martlist = itemDatabase.IndigoItems;
		pokeMart.Init();
		shopmenu.SetActive (true);
        pokeMart.currentMenu = pokeMart.buysellwindow;


	}





}
