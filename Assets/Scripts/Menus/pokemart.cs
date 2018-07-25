using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class pokemart : MonoBehaviour {
	public GameObject currentMenu;
	public Cursor cursor;
	public GameObject buysellwindow, martwindow, itemwindow,   quantitymenu;
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
    public UnityEvent onBuyItem;
	public bool withdrawing;


	void Start() {


	}



    void UpdateBuyScreen(){
        for (int i = 0; i < 4; i++)
        {
            int currentItem = offscreenindexup + 1 + i;
            if (currentItem > offscreenindexup && currentItem < id.items.Count)
            {
                ItemsToBuy[i].GetComponent<shopitemslotinfo>().mode = SlotMode.Item;
                ItemsToBuy[i].GetComponent<shopitemslotinfo>().name = martlist[currentItem];
            }
            else if (currentItem == id.items.Count)
            {
                Items[i].GetComponent<shopitemslotinfo>().mode = SlotMode.Cancel;

            }
            else
            {
                Items[i].GetComponent<shopitemslotinfo>().mode = SlotMode.Empty;

            }
        }
        cursor.SetPosition(40, 108 - 16 * (currentBagPosition - offscreenindexup - 1));
    }
    void UpdateSellScreen(){
        for (int i = 0; i < 4; i++)
        {
            int currentItem = offscreenindexup + 1 + i;
            if (currentItem > offscreenindexup && currentItem < id.items.Count)
            {
                Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Item;
                Items[i].GetComponent<itemslotinformation>().Name = id.items[currentItem].name;
                Items[i].GetComponent<itemslotinformation>().intquantity = id.items[currentItem].quantity;
                Items[i].GetComponent<itemslotinformation>().isKeyItem = id.items[currentItem].isKeyItem;
            }
            else if (currentItem == id.items.Count)
            {
                Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Cancel;

            }
            else
            {
                Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Empty;

            }
        }
        cursor.SetPosition(40, 108 - 16 * (currentBagPosition - offscreenindexup - 1));
    }




	// Update is called once per frame
	void Update () {
        if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 3;
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
                //Set the selling price of the selected item.

				maximumItem = Items [currentBagPosition].GetComponent<itemslotinformation> ().intquantity;

			}
			fullPrice = amountToTask * ItemPrice;
            if (Inputs.pressed("down")) {
				amountToTask--;
                MathE.Wrap(ref amountToTask, 1, maximumItem);
			}
            if (Inputs.pressed("up")) {
				amountToTask++;
                MathE.Wrap(ref amountToTask, 1, maximumItem);
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

                }
                MathE.Clamp(ref currentBagPosition, 0, martlist.Count - 1);
                UpdateBuyScreen();
            }
            if (Inputs.pressed("up"))
            {
                currentBagPosition--;
                if (currentBagPosition == offscreenindexup && offscreenindexup > -1)
                {
                    offscreenindexup--;
                    offscreenindexdown--;

                }
                MathE.Clamp(ref currentBagPosition, 0, martlist.Count - 1);
                UpdateBuyScreen();

            }


            if (!didFirstRunthrough)
            {

                UpdateBuyScreen();
                didFirstRunthrough = true;
            }
			
		


		}
		if (currentMenu == itemwindow) {


            if (Inputs.pressed("down"))
            {
                currentBagPosition++;
                if (currentBagPosition == offscreenindexdown && offscreenindexdown != id.items.Count + 1)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                }
                MathE.Clamp(ref currentBagPosition, 0, id.items.Count + 1);
                UpdateSellScreen();
            }
            if (Inputs.pressed("up"))
            {
                currentBagPosition--;
                if (currentBagPosition == offscreenindexup && offscreenindexup > -1)
                {
                    offscreenindexup--;
                    offscreenindexdown--;
                }
                MathE.Clamp(ref currentBagPosition, 0, id.items.Count + 1);
                UpdateSellScreen();
            }




            if (currentBagPosition != id.items.Count) {
				maximumItem = Items [currentBagPosition].GetComponent<itemslotinformation> ().intquantity;
			} else {
				maximumItem = 0;

			}

		}
		if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow)) {

		} else {
			if (currentMenu == buysellwindow) {

				cursor.SetPosition(8, 128 - 16 * selectedOption);

				cursor.SetActive (true);

                if (Inputs.pressed("down")) {
					selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 2);
				}
                if (Inputs.pressed("up")) {
					selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 2);
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
                        UpdateBuyScreen();

						}
						if (selectedOption == 1) {
							currentMenu = itemwindow;
                        UpdateSellScreen();
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
								StartCoroutine (CannotSellKeyItem ());


							}
						}

						if (ItemMode == 1) {
							if (SaveData.money >= fullPrice) {
								SaveData.money -= fullPrice;
								AddItem (ItemsToBuy [currentBagPosition].GetComponent<shopitemslotinfo> ().name, amountToTask);
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
	IEnumerator CannotSellKeyItem(){

		mylog.Deactivate();
		yield return StartCoroutine(mylog.text ("I can't put a"));
        yield return StartCoroutine(mylog.line("price on that."));
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
