using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class pokemart : MonoBehaviour {
	public GameObject currentMenu;
	public Cursor cursor;
	public GameObject buysellwindow, martwindow, itemwindow,   quantitymenu;
	public GameObject[] menuSlots, itemSelectSlots;
	public Dialogue mylog;
	public Player play;
	public int selectedOption;
	public GameObject[] allMenus;
	public int ItemMode;
	public int ItemPrice;
	public int fullPrice;
	//1 is withdraw;
	//2 is deposit;
	//3 is toss;
	public List<GameObject> Items = new List<GameObject>(4);
	public List<GameObject> ItemsToBuy = new List<GameObject>(4);
	public Items id;
	public int currentBagPosition;
	public GameObject cancel, buycancel;
	public int MartID;
	public List<string> martlist;
	public int selectBag;
	public int amountToTask;
	public bool didFirstRunthrough;
	public int maximumItem;
	public CustomText amountText, moneytext, pricetext;
	public bool donewaiting;
	public GameObject last;
	public bool alreadyInBag;
	public bool alreadydidtext;
	public int keep;
    public int offscreenindexup, offscreenindexdown;
    public GameObject sellcontent, buycontent;
    public UnityEvent onBuyItem;
	public bool withdrawing;


	void Start() {


	}








	// Update is called once per frame
	void Update () {
        if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 4;
        }
		pricetext.text = "$" + fullPrice.ToString ();
        moneytext.text = "$" + SaveData.money.ToString ();
		amountText.text = amountToTask.ToString ();
		if (currentMenu == quantitymenu) {
			if (ItemMode == 1) {


				ItemPrice = ItemsToBuy [currentBagPosition].GetComponent<shopitemslotinfo> ().intPrice;
				maximumItem = 99;
			}
			if (ItemMode == 2) {

				if (Items [currentBagPosition].GetComponent<itemslotinformation> ().Name == "POTION") {

					ItemPrice = 100;
                }else{
                    ItemPrice = 0;
                }
				maximumItem = Items [currentBagPosition].GetComponent<itemslotinformation> ().intquantity;

			}
			fullPrice = amountToTask * ItemPrice;
            if (Inputs.pressed("down")) {
				amountToTask--;
			}
            if (Inputs.pressed("up")) {
				amountToTask++;
			}
			if (amountToTask < 1) {
				amountToTask = maximumItem;

			}
			if (amountToTask > maximumItem) {
				amountToTask = 1;

			}


		}
		if (currentMenu == martwindow) {


            if (Inputs.pressed("down"))
            {
                currentBagPosition++;
                if (currentBagPosition == offscreenindexdown && offscreenindexdown != martlist.Count + 1)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                    buycontent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));

                }
            }
            if (Inputs.pressed("up"))
            {
                currentBagPosition--;
                if (currentBagPosition == offscreenindexup && offscreenindexup > -1)
                {
                    offscreenindexup--;
                    offscreenindexdown--;
                    buycontent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));

                }

            }
            if (currentBagPosition < 0)
            {

                currentBagPosition = 0;


            }
            if (currentBagPosition == martlist.Count + 1)
            {

                currentBagPosition = martlist.Count;

            }


            if (!didFirstRunthrough)
            {

                foreach (GameObject slot in Items)
                {
                    slot.SetActive(false);
                }
                for (int i = 0; i < martlist.Count; i++)
                {
                    ItemsToBuy[i].SetActive(true);
                    ItemsToBuy[i].GetComponent<shopitemslotinfo>().Name = martlist[i];
                }
                ItemsToBuy[10].SetActive(true);






                didFirstRunthrough = true;
                itemSelectSlots = new GameObject[martlist.Count + 1];

                for (int i = 0; i < martlist.Count; i++)
                {

                    itemSelectSlots[i] = ItemsToBuy[i].transform.GetChild(0).gameObject;
                }
                itemSelectSlots[itemSelectSlots.Length - 1] = Items[10].transform.GetChild(0).gameObject;
            }
			cursor.SetActive (true);
			cursor.transform.position = itemSelectSlots [currentBagPosition].transform.position;
		


		}
		if (currentMenu == itemwindow) {


            if (Inputs.pressed("down"))
            {
                currentBagPosition++;
                if (currentBagPosition == offscreenindexdown && offscreenindexdown != id.items.Count + 1)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                    sellcontent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));
                }
            }
            if (Inputs.pressed("up"))
            {
                currentBagPosition--;
                if (currentBagPosition == offscreenindexup && offscreenindexup > -1)
                {
                    offscreenindexup--;
                    offscreenindexdown--;
                    sellcontent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));
                }

            }
            if (currentBagPosition < 0)
            {

                currentBagPosition = 0;
            }
            if (currentBagPosition == id.items.Count + 1)
            {

                currentBagPosition = id.items.Count;


            }



			
            foreach (GameObject slot in Items)
            {
                slot.SetActive(false);
            }
            for (int i = 0; i < id.items.Count; i++)
            {
                Items[i].SetActive(true);
                Items[i].GetComponent<itemslotinformation>().Name = id.items[i].name;
                Items[i].GetComponent<itemslotinformation>().intquantity = id.items[i].quantity;
                Items[i].GetComponent<itemslotinformation>().isKeyItem = id.items[i].isKeyItem;
            }
            Items[20].SetActive(true);






			didFirstRunthrough = true;
			itemSelectSlots = new GameObject[id.items.Count + 1];

            for (int i = 0; i < id.items.Count; i++) {

				itemSelectSlots [i] = Items [i].transform.GetChild (0).gameObject;
			}
            itemSelectSlots[itemSelectSlots.Length - 1] = Items[20].transform.GetChild(0).gameObject;


            if (currentBagPosition != id.items.Count) {
				maximumItem = Items [currentBagPosition].GetComponent<itemslotinformation> ().intquantity;
			} else {
				maximumItem = 0;

			}
			cursor.transform.position = itemSelectSlots [currentBagPosition].transform.position;
			cursor.SetActive (true);

		}
		if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow)) {

		} else {
			menuSlots = new GameObject[currentMenu.transform.childCount];

			for (int i = 0; i < currentMenu.transform.childCount; i++) {


				menuSlots [i] = currentMenu.transform.GetChild (i).gameObject;
			}
			if (currentMenu == buysellwindow) {

				cursor.transform.position = menuSlots [selectedOption].transform.position;

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
				if (selectedOption == menuSlots.Length) {
					selectedOption = menuSlots.Length - 1;

				}
			}

		}
        if (Inputs.pressed("select")) {
			if (currentMenu == itemwindow) {
				if (selectBag == -1) {
					selectBag = currentBagPosition;
				} else {
                    //our Bag
                    Item item = id.items[selectBag];
                    id.items[selectBag] = id.items[currentBagPosition];
                    id.items[currentBagPosition] = item;
					selectBag = -1;




				}
			}

		}
		if (mylog.finishedWithTextOverall) {

			if (Inputs.pressed("a")) {
				
					if (currentMenu == buysellwindow) {
					
						if (selectedOption == 0) {
							currentMenu = martwindow;

						}
						if (selectedOption == 1) {
							currentMenu = itemwindow;
						currentBagPosition = 0;

						}
					if (selectedOption == 2) {
							play.shopup = false;
						this.gameObject.SetActive (false);



					}
				
					StartCoroutine (Wait ());


					
				}
				if (currentMenu == martwindow) {
					if (donewaiting) {
						if (currentBagPosition == martlist.Count) {


							currentMenu = buysellwindow;



						} else {



							currentMenu = quantitymenu;


							ItemMode1 ();

						}
						StartCoroutine (Wait ());
					}
				
				}
				if (currentMenu == itemwindow) {
					if (donewaiting) {
                        if (currentBagPosition == id.items.Count) {


							currentMenu = buysellwindow;



						} else {
							


							currentMenu = quantitymenu;


							ItemMode2 ();

						}
						StartCoroutine (Wait ());
					}

				}


				if (currentMenu == quantitymenu) {
					if (donewaiting) {


						if (ItemMode == 2) {
							if (!Items [currentBagPosition].GetComponent<itemslotinformation> ().isKeyItem) {
								SaveData.money += fullPrice;
								StartCoroutine (TossItem ());
							} else {
								StartCoroutine (TooImportantToToss ());


							}
						}

						if (ItemMode == 1) {
							if (SaveData.money >= fullPrice) {
								SaveData.money -= fullPrice;
								AddItem (ItemsToBuy [currentBagPosition].GetComponent<shopitemslotinfo> ().Name, amountToTask);
							} else {

								StartCoroutine(NotEnoughMoney());

							}

						}
						}
						StartCoroutine (Wait ());
					}

				}
			}
			if (Inputs.pressed("b")) {
				if (currentMenu == martwindow) {


				currentMenu = buysellwindow;

				}
				if (currentMenu == itemwindow) {
					didFirstRunthrough = false;
					currentMenu = buysellwindow;

				}
			if (currentMenu == quantitymenu) {

				if (ItemMode == 1) {
					currentBagPosition = 0;
					selectBag = -1;
					currentMenu = martwindow;

				
			}
				if (ItemMode == 2) {
					currentBagPosition = 0;
					selectBag = -1;
					currentMenu = itemwindow;


					}

				}
			}




			foreach (GameObject menu in allMenus) {
				if (menu != currentMenu) {
					menu.SetActive (false);
				} else {

					menu.SetActive (true);
				}
			if(menu == martwindow  &&( currentMenu ==  quantitymenu && ItemMode == 1)){
				menu.SetActive (true);
			}
				if(menu == itemwindow &&( currentMenu ==  quantitymenu && ItemMode == 2)){
					menu.SetActive (true);
				}

				if(menu == quantitymenu && (currentMenu == itemwindow || currentMenu == martwindow)){
					menu.SetActive(false);

				}
				if(menu == buysellwindow  && (currentMenu == quantitymenu || currentMenu == itemwindow || currentMenu == martwindow)){
					menu.SetActive(true);

				}

			}
		}



    public void AddItem(string name, int quantity)
    {

        alreadyInBag = false;

        Item inBagItem = new Item("", 0);
        foreach (Item item in id.items)
        {
            if (item.name == name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.items[id.items.IndexOf(inBagItem)].quantity += amountToTask;
        else if (id.items.Count < 20) id.items.Add(new Item(name, quantity));


        ItemMode = 0;
        currentMenu = itemwindow;
        onBuyItem.Invoke();



    }
	//deposit
    public IEnumerator TossItem()
    {



        mylog.Deactivate();
        mylog.cantscroll = false;
        mylog.finishedCurrentTask = true;

        string DisplayString = "Threw away " + id.items[currentBagPosition].name + ".";
        mylog.cont(DisplayString);
        while (!mylog.finishedCurrentTask)
        {
            yield return new WaitForSeconds(0.1f);
            if (mylog.finishedCurrentTask)
            {
                break;

            }
        }
        mylog.done();
        StartCoroutine(RemoveItem(amountToTask));

        currentMenu = itemwindow;
        ItemMode = 0;
    }
	IEnumerator Wait(){
		donewaiting = false;
		yield return new WaitForSeconds (0.1f);
		donewaiting = true;
	}



    public IEnumerator RemoveItem(int amount)
    {

        id.items[currentBagPosition].quantity -= amount;
        if (id.items[currentBagPosition].quantity == 0) id.items.RemoveAt(currentBagPosition);


        yield return null;
    }
	void ItemMode1(){
		ItemMode = 1;
		selectBag = -1;
	}
	void ItemMode2(){
		ItemMode = 2;
		selectBag = -1;

	}
	IEnumerator TooImportantToToss(){

		mylog.Deactivate();
		yield return StartCoroutine(mylog.text ("That's too important to toss!"));
		yield return StartCoroutine(mylog.done ());
		currentMenu = itemwindow;
	}
	IEnumerator NotEnoughMoney(){

		mylog.Deactivate();
		yield return StartCoroutine(mylog.text ("You don't have enough money."));
		yield return StartCoroutine(mylog.done ());
		currentMenu = martwindow;
	}
}
