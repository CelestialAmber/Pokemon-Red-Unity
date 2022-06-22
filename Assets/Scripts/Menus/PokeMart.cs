using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PokeMart : MonoBehaviour
{
    public enum Menu {
        BuySellWindow,
        ItemsWindow,
        QuantityMenu
    }

    public enum Mode{
        Buy,
        Sell
    }

    public Menu currentMenu;
    public GameCursor cursor;
    public GameObject buysellwindow, itemwindow, quantitymenu;
    public int selectedOption;
    public GameObject[] allMenus;
    public Mode itemMode;
    public int itemPrice;
    public int fullPrice;
    public List<ItemSlot> itemSlots = new List<ItemSlot>(4);
    public int currentBagPosition;
    public int menuId;
    public List<ItemsEnum> martItemsList;
    public int selectBag;
    public int amountToTask;
    public int maximumItem;
    public CustomText amountText, moneytext, pricetext;
    //the index of the top item on screen
    public int topItemIndex;
    public GameObject indicator;
    public bool switchingItems;
    public RectTransform selectCursor;
    int numberOfItems;

    public void Init()
    {
       indicator.SetActive(false);
    }

    void UpdateItemScreen()
    {
        if (currentBagPosition == 0)
        {
            topItemIndex = 0;
        }
        for (int i = 0; i < 4; i++)
        {
            int currentItem = topItemIndex + i;
            if (currentItem < numberOfItems)
            {
                if(itemMode == Mode.Buy){
                    itemSlots[i].mode = SlotMode.MartItem;
                    itemSlots[i].item = martItemsList[currentItem];
                    itemSlots[i].UpdatePrice();
                }else{
                    itemSlots[i].mode = SlotMode.Item;
                    itemSlots[i].item = Items.instance.items[currentItem].item;
                    itemSlots[i].quantity = Items.instance.items[currentItem].quantity;
                    itemSlots[i].isKeyItem = Items.instance.items[currentItem].isKeyItem;
                }
            }
            else if (currentItem == numberOfItems)
            {
                itemSlots[i].mode = SlotMode.Cancel;
            }
            else
            {
                itemSlots[i].mode = SlotMode.Empty;
            }
        }
        cursor.SetPosition(40, 104 - 16 * (currentBagPosition - topItemIndex));

        if (topItemIndex + 3 < numberOfItems) indicator.SetActive(true);
        else indicator.SetActive(false);

        if(itemMode == Mode.Sell){
            if (switchingItems){
            selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (selectBag - topItemIndex)) + new Vector2(4, 4);
            if (selectCursor.anchoredPosition.y > 112 || selectCursor.anchoredPosition.y < 50) selectCursor.gameObject.SetActive(false);
            else selectCursor.gameObject.SetActive(true);
            }
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
        selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (currentBagPosition - topItemIndex)) + new Vector2(4, 4);
    }



    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MainUpdate());
    }

    IEnumerator MainUpdate(){
        if (Dialogue.instance.finishedText)
        {
            if (currentMenu == Menu.QuantityMenu)
            {
                fullPrice = amountToTask * itemPrice;
                if (InputManager.Pressed(Button.Down))
                {
                    amountToTask--;
                    //UpdateQuantityScreen();
                }
                if (InputManager.Pressed(Button.Up))
                {
                    amountToTask++;
                    //UpdateQuantityScreen();
                }
                if (amountToTask < 1){
                amountToTask = maximumItem;
                }
                if (amountToTask > maximumItem){
                amountToTask = 1;
                }

                if (itemMode == Mode.Buy){
                    itemPrice = itemSlots[currentBagPosition - topItemIndex].price;
                    maximumItem = 99;
                }
                if (itemMode == Mode.Sell){
                    //Set the selling price of the selected item.
                    itemPrice = PokemonData.itemData[(int)Items.instance.items[currentBagPosition].item].price / 2;
                    maximumItem = Items.instance.items[currentBagPosition].quantity;
                }

                amountText.text = amountToTask.ToString();
            }

            if (currentMenu == Menu.ItemsWindow)
            {
                if (InputManager.Pressed(Button.Down))
                {
                    currentBagPosition++;
                    MathE.Clamp(ref currentBagPosition, 0, numberOfItems);

                    if (currentBagPosition == topItemIndex + 3 && numberOfItems > 3)
                    {
                        topItemIndex++;
                    }
                    
                    UpdateItemScreen();
                }
                if (InputManager.Pressed(Button.Up))
                {
                    currentBagPosition--;
                    MathE.Clamp(ref currentBagPosition, 0, numberOfItems);

                    if (currentBagPosition >= 0 && currentBagPosition < topItemIndex)
                    {
                        topItemIndex--;
                    }

                    UpdateItemScreen();
                }
                
                if(itemMode == Mode.Sell){
                    if (currentBagPosition != numberOfItems){
                        maximumItem = Items.instance.items[currentBagPosition].quantity;
                    }
                    else maximumItem = 0;
                }
            }

            if (currentMenu == Menu.BuySellWindow)
            {

                cursor.SetPosition(8, 128 - 16 * selectedOption);

                cursor.SetActive(true);

                if (InputManager.Pressed(Button.Down))
                {
                    selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 2);
                }
                if (InputManager.Pressed(Button.Up))
                {
                    selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 2);
                }
                if (InputManager.Pressed(Button.B))
                {
                    Player.instance.menuActive = false;
                    cursor.SetActive(false);
                    InputManager.Enable(Button.Start);
                    this.gameObject.SetActive(false);
                }
            }


            if (InputManager.Pressed(Button.Select))
            {
                if (currentMenu == Menu.ItemsWindow && itemMode == Mode.Sell)
                {
                    if (!switchingItems)
                    {
                        switchingItems = true;
                        selectBag = currentBagPosition;
                    }
                    else if (currentBagPosition != numberOfItems)
                    {
                        //our Bag
                        Item item = Items.instance.items[selectBag];
                        Items.instance.items[selectBag] = Items.instance.items[currentBagPosition];
                        Items.instance.items[currentBagPosition] = item;
                        switchingItems = false;
                        selectCursor.gameObject.SetActive(false);

                        UpdateItemScreen();

                    }
                }
            }


            if (InputManager.Pressed(Button.A))
            {
                SoundManager.instance.PlayABSound();
                if (currentMenu == Menu.BuySellWindow)
                {
                    if (selectedOption == 0)
                    {
                        currentBagPosition = 0;
                        currentMenu = Menu.ItemsWindow;
                         SetItemMode(Mode.Buy);
                        numberOfItems = martItemsList.Count;
                        UpdateItemScreen();
                    }
                    if (selectedOption == 1)
                    {
                        currentBagPosition = 0;
                        currentMenu = Menu.ItemsWindow;
                        SetItemMode(Mode.Sell);
                        numberOfItems = Items.instance.items.Count;
                        UpdateItemScreen();

                    }
                    if (selectedOption == 2)
                    {
                        Player.instance.menuActive = false;
                        InputManager.Enable(Button.Start);
                        cursor.SetActive(false);
                        this.gameObject.SetActive(false);
                        yield return 0;
                    }
                }
                else if (currentMenu == Menu.ItemsWindow)
                {
                    //If cancel was selected, go back to the buy/sell window
                    if (currentBagPosition == numberOfItems)
                    {
                        indicator.SetActive(false);
                        currentMenu = Menu.BuySellWindow;

                        if(itemMode == Mode.Sell){
                            switchingItems = false;
                            selectCursor.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        if(itemMode == Mode.Buy){
                            amountToTask = 1;
                            //UpdateQuantityScreen();
                            amountText.text = amountToTask.ToString();
                            cursor.SetActive(false);
                            currentMenu = Menu.QuantityMenu;
                        }else{
                            if (!Items.instance.items[currentBagPosition].isKeyItem && PokemonData.itemData[(int)Items.instance.items[currentBagPosition].item].price > 0){
                                amountToTask = 1;
                                //UpdateQuantityScreen();
                                amountText.text = amountToTask.ToString();
                                cursor.SetActive(false);
                                currentMenu = Menu.QuantityMenu;
                            }else{
                                switchingItems = false;
                                StartCoroutine(UnsellableItem());
                            }
                        }
                    }
                }
                else if (currentMenu == Menu.QuantityMenu)
                {
                    if (itemMode == Mode.Sell)
                    {
                        if (!Items.instance.items[currentBagPosition].isKeyItem)
                        {
                            //Give the player half of the item's price back
                            GameData.instance.money += fullPrice;
                            Items.instance.RemoveItem(amountToTask, currentBagPosition);
                            currentMenu = Menu.ItemsWindow;
                            cursor.SetActive(true);
                            selectCursor.gameObject.SetActive(false);
                            UpdateItemScreen();
                        }
                    }

                    if (itemMode == Mode.Buy)
                    {
                        if (GameData.instance.money >= fullPrice)
                        {
                            GameData.instance.money -= fullPrice;
                            Items.instance.AddItem(martItemsList[currentBagPosition], amountToTask);
                            currentMenu = Menu.ItemsWindow;
                            cursor.SetActive(true);
                            selectCursor.gameObject.SetActive(false);
                            UpdateItemScreen();
                        }
                        else StartCoroutine(NotEnoughMoney());
                    }
                }
            }

            if (InputManager.Pressed(Button.B))
            {
                SoundManager.instance.PlayABSound();
                if (currentMenu == Menu.ItemsWindow)
                {
                    indicator.SetActive(false);
                    currentMenu = Menu.BuySellWindow;

                }
                else if (currentMenu == Menu.ItemsWindow)
                {
                    switchingItems = false;
                     indicator.SetActive(false);
                    selectCursor.gameObject.SetActive(false);
                    currentMenu = Menu.BuySellWindow;

                }
                else if (currentMenu == Menu.QuantityMenu)
                {
                    selectBag = -1;
                    cursor.SetActive(true);
                    selectCursor.gameObject.SetActive(false);
                    currentMenu = Menu.ItemsWindow;
                }
            }
        }

        foreach (GameObject menu in allMenus)
        {
            if (menu != allMenus[(int)currentMenu])
            {
                menu.SetActive(false);
            }
            else
            {
                menu.SetActive(true);
            }
            if (menu == allMenus[0] && (currentMenu == Menu.QuantityMenu || currentMenu == Menu.ItemsWindow))
            {
                menu.SetActive(true);
            }
            if (menu == allMenus[1] && currentMenu == Menu.QuantityMenu)
            {
                menu.SetActive(true);
            }
            if (menu == allMenus[2] && (currentMenu ==  Menu.ItemsWindow))
            {
                menu.SetActive(false);
            }

        }
    }

    void SetItemMode(Mode mode)
    {
        itemMode = mode;
        selectBag = -1;
    }

    IEnumerator UnsellableItem()
    {

        Dialogue.instance.Deactivate();
        selectCursor.gameObject.SetActive(true);
        cursor.SetActive(false);
        UpdateSelectItemCursorPos();
        yield return Dialogue.instance.text("I can't put a&lprice on that.");
        selectCursor.gameObject.SetActive(false);
        UpdateItemScreen();
        cursor.SetActive(true);
        currentMenu = Menu.ItemsWindow;
    }


    IEnumerator NotEnoughMoney()
    {
        Dialogue.instance.Deactivate();
        selectCursor.gameObject.SetActive(true);
        cursor.SetActive(false);
        UpdateSelectItemCursorPos();
        yield return Dialogue.instance.text("You don't have&lenough money.");
        selectCursor.gameObject.SetActive(false);
        UpdateItemScreen();
        cursor.SetActive(true);
        currentMenu = Menu.ItemsWindow;
    }
}
