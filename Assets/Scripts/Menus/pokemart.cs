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
	public List<ItemSlot> ItemsToSell = new List<ItemSlot>(4);
    public List<ShopItemSlot> ItemsToBuy = new List<ShopItemSlot>(4);
	public int currentBagPosition;
	public int MartID;
	public List<string> martlist;
	public int selectBag;
	public int amountToTask;
	public int maximumItem;
	public CustomText amountText, moneytext, pricetext;
    public int offscreenindexup, offscreenindexdown;
    public GameObject indicator;
    public bool switching;
    public RectTransform selectCursor;

    public void Init() {
        UpdateBuySellScreen();
	}

   void UpdateBuySellScreen()
    {
        indicator.SetActive(false);
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
        if (offscreenindexdown < martlist.Count) indicator.SetActive(true);
        else indicator.SetActive(false);
        cursor.SetPosition(40, 104 - 16 * (currentBagPosition - offscreenindexup - 1));
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
            if (currentItem > offscreenindexup && currentItem < Items.instance.items.Count)
            {
                ItemsToSell[i].mode = SlotMode.Item;
                ItemsToSell[i].Name = Items.instance.items[currentItem].name;
                ItemsToSell[i].intquantity = Items.instance.items[currentItem].quantity;
                ItemsToSell[i].isKeyItem = Items.instance.items[currentItem].isKeyItem;
            }
            else if (currentItem == Items.instance.items.Count)
            {
                ItemsToSell[i].mode = SlotMode.Cancel;

            }
            else
            {
                ItemsToSell[i].mode = SlotMode.Empty;

            }
        }
        if (offscreenindexdown < Items.instance.items.Count) indicator.SetActive(true);
        else indicator.SetActive(false);
        cursor.SetPosition(40, 104 - 16 * (currentBagPosition - offscreenindexup - 1));
        if (switching)
        {
            selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (selectBag - offscreenindexup - 1)) + new Vector2(4, 4);
            if (selectCursor.anchoredPosition.y > 112 || selectCursor.anchoredPosition.y < 50) selectCursor.gameObject.SetActive(false);
            else selectCursor.gameObject.SetActive(true);
        }
    }
    void UpdateQuantityScreen()
    {
        cursor.SetActive(false);
        selectCursor.gameObject.SetActive(true);
        UpdateSelectItemCursorPos();
        amountText.text = amountToTask.ToString();
    }
    void UpdateSelectItemCursorPos()
    {
        selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (currentBagPosition - offscreenindexup - 1)) + new Vector2(4, 4);
    }






    // Update is called once per frame
    void Update () {
		pricetext.text = "$" + fullPrice.ToString ();
        moneytext.text = "$" + GameData.instance.money.ToString ();

        if (currentMenu == quantitymenu) {
            if (ItemMode == 1) {
                itemPrice = ItemsToBuy[currentBagPosition - offscreenindexup - 1].intPrice;
                maximumItem = 99;
            }
            if (ItemMode == 2) {
                //Set the selling price of the selected item.
                itemPrice = PokemonData.itemPrices[Items.instance.items[currentBagPosition].name] / 2;
                maximumItem = Items.instance.items[currentBagPosition].quantity;

            }
        }
        if (Dialogue.instance.finishedText)
        {
            if (currentMenu == quantitymenu)
            {
                fullPrice = amountToTask * itemPrice;
                if (Inputs.pressed("down"))
                {
                    amountToTask--;
                    MathE.Wrap(ref amountToTask, 1, maximumItem);
                    UpdateQuantityScreen();
                }
                if (Inputs.pressed("up"))
                {
                    amountToTask++;
                    MathE.Wrap(ref amountToTask, 1, maximumItem);
                    UpdateQuantityScreen();
                }


            }
            if (currentMenu == martwindow)
            {


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
            if (currentMenu == itemwindow)
            {


                if (Inputs.pressed("down"))
                {
                    currentBagPosition++;
                    if (currentBagPosition == offscreenindexdown && offscreenindexdown != Items.instance.items.Count + 1)
                    {
                        offscreenindexup++;
                        offscreenindexdown++;
                    }
                    MathE.Clamp(ref currentBagPosition, 0, Items.instance.items.Count);
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
                    MathE.Clamp(ref currentBagPosition, 0, Items.instance.items.Count);
                    UpdateSellScreen();
                }




                if (currentBagPosition != Items.instance.items.Count)
                {
                    maximumItem = Items.instance.items[currentBagPosition].quantity;
                }
                else
                {
                    maximumItem = 0;

                }

            }

            if (currentMenu == buysellwindow)
            {

                cursor.SetPosition(8, 128 - 16 * selectedOption);

                cursor.SetActive(true);

                if (Inputs.pressed("down"))
                {
                    selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 2);
                }
                if (Inputs.pressed("up"))
                {
                    selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 2);
                }
                if (Inputs.pressed("b"))
                {
                    play.menuActive = false;
                    cursor.SetActive(false);
                    Inputs.Enable("start");
                    this.gameObject.SetActive(false);


                }
            }


            if (Inputs.pressed("select"))
            {
                if (currentMenu == itemwindow)
                {
                    if (!switching)
                    {
                        switching = true;
                        selectBag = currentBagPosition;
                    }
                    else if(currentBagPosition != Items.instance.items.Count)
                    {
                        //our Bag
                        Item item = Items.instance.items[selectBag];
                        Items.instance.items[selectBag] = Items.instance.items[currentBagPosition];
                        Items.instance.items[currentBagPosition] = item;
                        switching = false;
                        selectCursor.gameObject.SetActive(false);

                        UpdateSellScreen();

                    }
                }

            }


            if (Inputs.pressed("a"))
            {
                SoundManager.instance.PlayABSound();
                if (currentMenu == buysellwindow)
                {

                    if (selectedOption == 0)
                    {
                        currentBagPosition = 0;
                        currentMenu = martwindow;
                        UpdateBuyScreen();

                    }
                    if (selectedOption == 1)
                    {
                        currentBagPosition = 0;
                        currentMenu = itemwindow;
                        UpdateSellScreen();

                    }
                    if (selectedOption == 2)
                    {
                        play.menuActive = false;
                        Inputs.Enable("start");
                        cursor.SetActive(false);
                        this.gameObject.SetActive(false);
                        return;


                    }



                }
                else if (currentMenu == martwindow)
                {
                    if (currentBagPosition == martlist.Count)
                    {

                        UpdateBuySellScreen();
                        currentMenu = buysellwindow;



                    }
                    else
                    {
                        amountToTask = 1;
                        UpdateQuantityScreen();
                        cursor.SetActive(false);
                        currentMenu = quantitymenu;
                        ItemMode1();

                    }


                }
                else if (currentMenu == itemwindow)
                {
                    if (currentBagPosition == Items.instance.items.Count)
                    {
                        UpdateBuySellScreen();
                        switching = false;
                        selectCursor.gameObject.SetActive(false);
                        currentMenu = buysellwindow;



                    }
                    else
                    {

                        if (!Items.instance.items[currentBagPosition].isKeyItem && PokemonData.itemPrices[Items.instance.items[currentBagPosition].name] > 0)
                        {
                            amountToTask = 1;
                            UpdateQuantityScreen();
                            cursor.SetActive(false);
                            currentMenu = quantitymenu;
                            ItemMode2();
                        }
                        else
                        {
                            switching = false;
                            StartCoroutine(UnsellableItem());
                        }

                    }


                }


                else if (currentMenu == quantitymenu)
                {


                    if (ItemMode == 2)
                    {
                        if (!Items.instance.items[currentBagPosition].isKeyItem)
                        {
                            GameData.instance.money += fullPrice;
                           Items.instance.RemoveItem(amountToTask, currentBagPosition);
                            currentMenu = itemwindow;
                            cursor.SetActive(true);
                            selectCursor.gameObject.SetActive(false);
                            UpdateSellScreen();
                        }
                    }

                    if (ItemMode == 1)
                    {
                        if (GameData.instance.money >= fullPrice)
                        {
                            GameData.instance.money -= fullPrice;
                            Items.instance.AddItem(martlist[currentBagPosition], amountToTask);
                            currentMenu = martwindow;
                            cursor.SetActive(true);
                            selectCursor.gameObject.SetActive(false);
                            UpdateBuyScreen();
                        }
                        else
                        {

                            StartCoroutine(NotEnoughMoney());

                        }

                    }

                }

            }

            if (Inputs.pressed("b"))
            {
                SoundManager.instance.PlayABSound();
                if (currentMenu == martwindow)
                {

                    UpdateBuySellScreen();
                    currentMenu = buysellwindow;

                }
                else if (currentMenu == itemwindow)
                {
                    switching = false;
                    UpdateBuySellScreen();
                    selectCursor.gameObject.SetActive(false);
                    currentMenu = buysellwindow;

                }
                else if (currentMenu == quantitymenu)
                {

                    if (ItemMode == 1)
                    {
                        selectBag = -1;
                        cursor.SetActive(true);
                        selectCursor.gameObject.SetActive(false);
                        currentMenu = martwindow;


                    }
                    if (ItemMode == 2)
                    {
                        selectBag = -1;
                        cursor.SetActive(true);
                        selectCursor.gameObject.SetActive(false);
                        currentMenu = itemwindow;


                    }

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
        selectCursor.gameObject.SetActive(true);
        cursor.SetActive(false);
        UpdateSelectItemCursorPos();
        yield return Dialogue.instance.text ("I can't put a\\lprice on that.");
        selectCursor.gameObject.SetActive(false);
        UpdateSellScreen();
        cursor.SetActive(true);
		currentMenu = itemwindow;
	}
	IEnumerator NotEnoughMoney(){

		Dialogue.instance.Deactivate();
        selectCursor.gameObject.SetActive(true);
        cursor.SetActive(false);
        UpdateSelectItemCursorPos();
        yield return Dialogue.instance.text("You don't have\\lenough money.");
        selectCursor.gameObject.SetActive(false);
        UpdateBuyScreen();
        cursor.SetActive(true);
        currentMenu = martwindow;
	}
}
