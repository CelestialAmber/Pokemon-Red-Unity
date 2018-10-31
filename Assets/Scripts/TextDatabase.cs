using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDatabase : MonoBehaviour {
	public GameObject itemPCMenu, shopmenu, slotmenu;
	public Bag bag;
    public GameCursor cursor;
	public Items itemDatabase;
	public PokeMart pokeMart;
	public Player player;
	public PokemonMenu pokemonData;
	public Slots slots;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void PlayText(int ID, int amount){
		switch (ID) {
		case 1:
		//	if (play.direction == 3 || play.direction == 4) {
				StartCoroutine (Text1 ());
		//	}
			break;
		case 2:
			StartCoroutine (Text2 ());
			break;
		case 3:
			StartCoroutine (Text3 ());
			break;
		case 4:
			StartCoroutine (Text4 ());
			break;
		
		}




	}
    public void GetItem(string item, int coinAmount){
        if (coinAmount == 0)
            StartCoroutine(GetItemText(item));
        else StartCoroutine(FoundCoinsText(coinAmount));
    }

    IEnumerator GetItemText(string item){
        itemDatabase.AddItem(item, 1,false);
        yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + " found "));
        yield return StartCoroutine(Dialogue.instance.line(item.ToUpper() + "!"));
        yield return StartCoroutine(Dialogue.instance.done());


    }
	IEnumerator Text1(){
		if (player.direction == 3 || player.direction == 4) {
			if (GameData.coins > 0) {
				yield return StartCoroutine(Dialogue.instance.text ("A slot machine!"));
				yield return StartCoroutine(Dialogue.instance.line ("Want to play?"));
                yield return StartCoroutine(Dialogue.instance.prompt ());
				if (Dialogue.instance.selectedOption == 0) {
					Dialogue.instance.Deactivate ();
					Player.disabled = true;
					StartCoroutine (player.DisplayEmotiveBubble (1));
					while (player.displayingEmotion) {
						yield return new WaitForSeconds (0.1f);
						if (!player.displayingEmotion) {
							break;
						}
					}
					Player.disabled = true;
					slotmenu.SetActive (true);
                    Inputs.Disable("start");
					StartCoroutine (slots.Initialize ());

				} else {
					Dialogue.instance.Deactivate ();
					player.WaitToInteract ();
				}

			} else {
				yield return StartCoroutine(Dialogue.instance.text ("You don't have any"));
				yield return StartCoroutine(Dialogue.instance.line ("coins!"));
				yield return StartCoroutine(Dialogue.instance.done ());


			}
		} else {
			player.canInteractAgain = true;
		}
	}

	IEnumerator Text2(){
		Dialogue.instance.Deactivate ();
        yield return StartCoroutine(Dialogue.instance.para (GameData.playerName + " turned on"));
		yield return StartCoroutine(Dialogue.instance.line ("the PC!"));
		yield return StartCoroutine(Dialogue.instance.done());
        Player.disabled = true;
		itemPCMenu.SetActive (true);
        Inputs.Disable("start");
        StartCoroutine(itemPCMenu.GetComponent<PC> ().Initialize ());
	}
	IEnumerator Text3(){
		Dialogue.instance.Deactivate ();
		yield return StartCoroutine(Dialogue.instance.para ("Battle!"));
		yield return StartCoroutine(Dialogue.instance.done());
		player.StartTrainerBattle(0);


	}
	IEnumerator Text4(){
		Dialogue.instance.Deactivate ();
        yield return StartCoroutine(Dialogue.instance.text("Hi there!"));
        yield return StartCoroutine(Dialogue.instance.line("May I help you?"));
		yield return StartCoroutine(Dialogue.instance.done());
		player.shopup = true;
        cursor.SetActive(true);
        Inputs.Disable("start");
		pokeMart.martlist = itemDatabase.IndigoItems;
		pokeMart.Init();
		shopmenu.SetActive (true);
        pokeMart.currentMenu = pokeMart.buysellwindow;


	}



IEnumerator FoundCoinsText(int coinamount)
{
	GameData.coins += coinamount;
	yield return StartCoroutine(Dialogue.instance.text("Found " + coinamount + " coins!"));
	yield return StartCoroutine(Dialogue.instance.done());



}

}
