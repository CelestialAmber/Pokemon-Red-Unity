using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PokeMart : MonoBehaviour {
	public GameObject currentMenu;
	public GameCursor cursor;
	public GameObject buysellwindow, martwindow, itemwindow,   quantitymenu;
	public Player play;
	public int selectedOption;
	public GameObject[] allMenus;
	public int ItemMode;
	public int itemPrice;
	public int fullPrice;
	//1 is withdraw;
	//2 is deposit;
	//3 is toss;
	public List<itemslotinformation> Items = new List<itemslotinformation>(4);
    public List<shopitemslotinfo> ItemsToBuy = new List<shopitemslotinfo>(4);
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


	public void Init() {

     UpdateBuyScreen();
	}



    void UpdateBuyScreen(){
        if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }
        for (int i = 0; i < 4; i++)
        {
            int currentItem = offscreenindexup + 1 + i;
            if (currentItem > offscreenindexup && currentItem < martlist.Count)
            {
                ItemsToBuy[i].mode = SlotMode.Item;
                ItemsToBuy[i].name = martlist[currentItem];
            }
            else if (currentItem == martlist.Count)
            {
                ItemsToBuy[i].mode = SlotMode.Cancel;

            }
            else
            {
                ItemsToBuy[i].mode = SlotMode.Empty;

            }
            ItemsToBuy[i].UpdatePrice();
        }
        cursor.SetPosition(40, 108 - 16 * (currentBagPosition - offscreenindexup - 1));
    }
    void UpdateSellScreen(){
        if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }
        for (int i = 0; i < 4; i++)
        {
            int currentItem = offscreenindexup + 1 + i;
            if (currentItem > offscreenindexup && currentItem < id.items.Count)
            {
                Items[i].mode = SlotMode.Item;
                Items[i].Name = id.items[currentItem].name;
                Items[i].intquantity = id.items[currentItem].quantity;
                Items[i].isKeyItem = id.items[currentItem].isKeyItem;
            }
            else if (currentItem == id.items.Count)
            {
                Items[i].mode = SlotMode.Cancel;

            }
            else
            {
                Items[i].mode = SlotMode.Empty;

            }
        }
        cursor.SetPosition(40, 108 - 16 * (currentBagPosition - offscreenindexup - 1));
    }




	// Update is called once per frame
	void Update () {
		pricetext.text = "$" + fullPrice.ToString ();
        moneytext.text = "$" + GameData.money.ToString ();
		amountText.text = amountToTask.ToString ();
		if (currentMenu == quantitymenu) {
			if (ItemMode == 1) {
				itemPrice = ItemsToBuy [currentBagPosition - offscreenindexup - 1].intPrice;
				maximumItem = 99;
                if(maximumItem == 0){
                    StartCoroutine(NotEnoughMoney());
                }
			}
			if (ItemMode == 2) {
                //Set the selling price of the selected item.
                itemPrice = PokemonData.itemPrices[id.items[currentBagPosition].name] / 2;
				maximumItem = Items [currentBagPosition].intquantity;

			}
			fullPrice = amountToTask * itemPrice;
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
                MathE.Clamp(ref currentBagPosition, 0, martlist.Count);
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
                MathE.Clamp(ref currentBagPosition, 0, martlist.Count);
                UpdateBuyScreen();

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
                MathE.Clamp(ref currentBagPosition, 0, id.items.Count);
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
                MathE.Clamp(ref currentBagPosition, 0, id.items.Count);
                UpdateSellScreen();
            }




            if (currentBagPosition != id.items.Count) {
				maximumItem = Items [currentBagPosition].intquantity;
			} else {
				maximumItem = 0;

			}

		}
		
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
            if(Inputs.pressed("b")){
                play.shopup = false;
                cursor.SetActive(false);
                Inputs.Enable("start");
                this.gameObject.SetActive(false);


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


                    UpdateSellScreen();

				}
			}

		}
		if (Dialogue.instance.finishedWithTextOverall) {

			if (Inputs.pressed("a")) {
				
					if (currentMenu == buysellwindow) {
					
						if (selectedOption == 0) {
                        currentBagPosition = 0;
							currentMenu = martwindow;
                        UpdateBuyScreen();

						}
						if (selectedOption == 1) {
                        currentBagPosition = 0;
							currentMenu = itemwindow;
                        UpdateSellScreen();

						}
					if (selectedOption == 2) {
							play.shopup = false;
                        Inputs.Enable("start");
                        cursor.SetActive(false);
						this.gameObject.SetActive (false);


                    
					}
				
					StartCoroutine (Wait ());


					
				}
				if (currentMenu == martwindow) {
					if (donewaiting) {
						if (currentBagPosition == martlist.Count) {


							currentMenu = buysellwindow;



						} else {
                            amountToTask = 1;
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

                            if (!id.items[currentBagPosition].isKeyItem && PokemonData.itemPrices[id.items[currentBagPosition].name] > 0)
                            {
                                amountToTask = 1;
                                currentMenu = quantitymenu;
                                ItemMode2();
                            }
                            else StartCoroutine(UnsellableItem());

						}
						StartCoroutine (Wait ());
					}

				}


				if (currentMenu == quantitymenu) {
					if (donewaiting) {


						if (ItemMode == 2) {
							if (!id.items[currentBagPosition].isKeyItem) {
								GameData.money += fullPrice;
                              Get.items.RemoveItem (amountToTask,currentBagPosition);
                                currentMenu = itemwindow;
                                UpdateSellScreen();
							}
						}

						if (ItemMode == 1) {
							if (GameData.money >= fullPrice) {
								GameData.money -= fullPrice;
                                Get.items.AddItem (martlist[currentBagPosition], amountToTask);
                                currentMenu = martwindow;
                                UpdateBuyScreen();
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
					selectBag = -1;
					currentMenu = martwindow;

				
			}
				if (ItemMode == 2) {
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
	IEnumerator UnsellableItem(){

		Dialogue.instance.Deactivate();
		yield return StartCoroutine(Dialogue.instance.text ("I can't put a"));
        yield return StartCoroutine(Dialogue.instance.line("price on that."));
		yield return StartCoroutine(Dialogue.instance.done ());
		currentMenu = itemwindow;
	}
	IEnumerator NotEnoughMoney(){

		Dialogue.instance.Deactivate();
        yield return StartCoroutine(Dialogue.instance.text("You don't have"));
        yield return StartCoroutine(Dialogue.instance.line("enough money."));                            
		yield return StartCoroutine(Dialogue.instance.done ());
		currentMenu = martwindow;
	}
}
