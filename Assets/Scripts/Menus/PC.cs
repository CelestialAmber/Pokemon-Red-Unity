using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PC : MonoBehaviour
{
    public enum Menu
    {
        MainWindow,
        ItemWindow,
        QuantityMenu
    }

    public enum Mode{
        Withdraw,
        Deposit,
        Toss
    }

    public Menu currentMenu;
    public GameCursor cursor;
    public GameObject mainwindow, itemwindow, quantitymenu;
    public GameObject indicator;
    public int selectedOption;
    public GameObject[] allMenus;
    public Mode itemMode;
    public List<ItemSlot> itemSlots = new List<ItemSlot>(4);
    public int currentBagPosition;
    public int selectBag;
    public int amountToTask;
    public int maximumItem;
    public CustomText amountText;
    public RectTransform selectCursor;
    //the index of the top item on screen
    public int topItemIndex;
    public bool switchingItems;
        public bool alreadyInBag;
    public int numberOfItems;

    public IEnumerator Initialize()
    {
        StartCoroutine(WhatDoText());
        cursor.SetActive(true);
        cursor.SetPosition(8, 120 - 16 * selectedOption);
        currentMenu = Menu.MainWindow;
        yield return 0;
    }

    void UpdateBagScreen(){
        numberOfItems = itemMode == Mode.Deposit ? Items.instance.items.Count : Items.instance.pcItems.Count;
       
        if (currentBagPosition == 0)
        {
            topItemIndex = 0;
        }

        for (int i = 0; i < 4; i++){
            int currentItem = topItemIndex + i;
            if (currentItem < numberOfItems)
            {
                itemSlots[i].mode = SlotMode.Item;
                itemSlots[i].item = itemMode == Mode.Deposit ? Items.instance.items[currentItem].item : Items.instance.pcItems[currentItem].item;
                itemSlots[i].quantity = itemMode == Mode.Deposit ? Items.instance.items[currentItem].quantity : Items.instance.pcItems[currentItem].quantity;
                itemSlots[i].isKeyItem = itemMode == Mode.Deposit ? Items.instance.items[currentItem].isKeyItem : Items.instance.pcItems[currentItem].isKeyItem;
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
        //If there are still more items off screen at the bottom, keep the cursor active
        if (topItemIndex + 3 < Items.instance.items.Count) indicator.SetActive(true);
        else indicator.SetActive(false);
       
        if (switchingItems)
        {
            selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (selectBag - topItemIndex)) + new Vector2(4, 4);
            if (selectCursor.anchoredPosition.y > 112 || selectCursor.anchoredPosition.y < 50) selectCursor.gameObject.SetActive(false);
            else selectCursor.gameObject.SetActive(true);
        }
    }

    void UpdateMainScreen() //update the withdraw/deposit screen
    {
        cursor.SetPosition(8, 120 - 16 * selectedOption);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MainUpdate());
    }

    IEnumerator MainUpdate()
    {
        if (currentMenu == Menu.QuantityMenu && Dialogue.instance.finishedText)
        {

            if (InputManager.Pressed(Button.Down))
            {
                amountToTask--;
            }
            if (InputManager.Pressed(Button.Up))
            {
                amountToTask++;
            }
            if (amountToTask < 1){
                amountToTask = maximumItem;
            }
            if (amountToTask > maximumItem){
                amountToTask = 1;
            }

            amountText.text = amountToTask.ToString();
        }

        if (currentMenu == Menu.ItemWindow && Dialogue.instance.finishedText)
        {
            if (InputManager.Pressed(Button.Down))
            {
                currentBagPosition++;
                 MathE.Clamp(ref currentBagPosition, 0, numberOfItems);

                if (currentBagPosition == topItemIndex + 3 && numberOfItems > 3)
                {
                    topItemIndex++;
                }

                UpdateBagScreen();
            }

            if (InputManager.Pressed(Button.Up))
            {
                currentBagPosition--;

                 MathE.Clamp(ref currentBagPosition, 0, Items.instance.items.Count);
                

                if (currentBagPosition >= 0 && currentBagPosition < topItemIndex)
                {
                    topItemIndex--;
                }
                
                UpdateBagScreen();
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

        if (currentMenu == Menu.MainWindow && Dialogue.instance.finishedText)
        {
            if (InputManager.Pressed(Button.Down))
            {
                selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 3);
                UpdateMainScreen();
            }
            if (InputManager.Pressed(Button.Up))
            {
                selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 3);
                UpdateMainScreen();
            }
        }

        if (InputManager.Pressed(Button.Select) && Dialogue.instance.finishedText && currentBagPosition != numberOfItems)
        {
            if (currentMenu == Menu.ItemWindow)
            {
                if (!switchingItems)
                {
                    switchingItems = true;
                    selectCursor.gameObject.SetActive(true);
                    selectBag = currentBagPosition;
                    UpdateBagScreen();
                }
                else
                {
                    //our bag
                    if (itemMode == Mode.Deposit)
                    {
                        selectCursor.gameObject.SetActive(false);
                        Item item = Items.instance.items[selectBag];
                        Items.instance.items[selectBag] = Items.instance.items[currentBagPosition];
                        Items.instance.items[currentBagPosition] = item;
                        switchingItems = false;
                    }

                    if (itemMode == Mode.Withdraw || itemMode == Mode.Toss)
                    {
                        Item item = Items.instance.pcItems[selectBag];
                        Items.instance.pcItems[selectBag] = Items.instance.pcItems[currentBagPosition];
                        Items.instance.pcItems[currentBagPosition] = item;
                        switchingItems = false;
                    }

                    UpdateBagScreen();
                }
            }
        }

        if (Dialogue.instance.finishedText)
        {
        if (InputManager.Pressed(Button.A))
        {
            SoundManager.instance.PlayABSound();
            if (currentMenu == Menu.ItemWindow)
            {

                if (currentBagPosition == numberOfItems)
                {
                    Dialogue.instance.Deactivate();
                    Dialogue.instance.fastText = true;
                    switchingItems = false;
                    selectCursor.gameObject.SetActive(false);
                    Dialogue.instance.keepTextOnScreen = true;
                    Dialogue.instance.needButtonPress = false;
                    yield return Dialogue.instance.text("What do you want&lto do?");
                    currentMenu = Menu.MainWindow;
                    UpdateMainScreen();
                }
                else
                {
                    if (!itemSlots[currentBagPosition - topItemIndex].isKeyItem && itemMode != Mode.Toss)
                    {
                        amountToTask = 1;
                        Dialogue.instance.Deactivate();
                        Dialogue.instance.fastText = true;
                        Dialogue.instance.keepTextOnScreen = true;
                        Dialogue.instance.needButtonPress = false;
                        yield return Dialogue.instance.text("How much?");
                        currentMenu = Menu.QuantityMenu;
                        amountText.text = amountToTask.ToString();
                    }
                    else if (itemSlots[currentBagPosition - topItemIndex].isKeyItem)
                    {
                        switch (itemMode)
                        {
                            case Mode.Withdraw:
                                StartCoroutine(WithdrawItem());
                                break;
                            case Mode.Deposit:
                                StartCoroutine(DepositItem());
                                break;
                        }
                    }
                    else if (itemMode == Mode.Toss)
                    {
                        amountToTask = 1;
                        Dialogue.instance.Deactivate();
                        Dialogue.instance.fastText = true;
                        Dialogue.instance.keepTextOnScreen = true;
                        Dialogue.instance.needButtonPress = false;
                        yield return Dialogue.instance.text("How much?");
                        currentMenu = Menu.QuantityMenu;
                        amountText.text = amountToTask.ToString();
                    }
                }
                if (currentBagPosition != numberOfItems && itemSlots[currentBagPosition - topItemIndex].isKeyItem && itemMode == Mode.Toss)
                {
                    StartCoroutine(TooImportantToToss());
                }

            }
            else if (currentMenu == Menu.MainWindow)
            {
                if (selectedOption == 0)
                {
                    UpdateBagScreen();
                    StartCoroutine(SetItemMode(Mode.Withdraw));
                }
                if (selectedOption == 1)
                {
                    UpdateBagScreen();
                    StartCoroutine(SetItemMode(Mode.Deposit));
                }
                if (selectedOption == 2)
                {
                    UpdateBagScreen();
                    StartCoroutine(SetItemMode(Mode.Toss));
                }
                if (selectedOption == 3)
                {
                    Close();
                }
            }
            else if (currentMenu == Menu.QuantityMenu)
            {
                if (itemMode == Mode.Toss)
                {
                    if (!itemSlots[currentBagPosition - topItemIndex].isKeyItem)
                    {
                        StartCoroutine(TossItem());
                    }
                }
                if (itemMode == Mode.Withdraw)
                {
                    StartCoroutine(WithdrawItem());
                }

                if (itemMode == Mode.Deposit)
                {
                    StartCoroutine(DepositItem());
                }
            }
        }

        if (InputManager.Pressed(Button.B))
        {
            SoundManager.instance.PlayABSound();
            if (currentMenu == Menu.MainWindow)
            {
                Close();
            }
            else if (currentMenu == Menu.ItemWindow)
            {


                switchingItems = false;
                selectCursor.gameObject.SetActive(false);
                StartCoroutine(WhatDoText());

                currentMenu = Menu.MainWindow;
                UpdateMainScreen();

            }
            else if (currentMenu == Menu.QuantityMenu)
            {
                if (itemMode == Mode.Withdraw)
                {
                    currentBagPosition = 0;
                    selectBag = -1;
                    StartCoroutine(WhatWithdrawText());
                    currentMenu = Menu.ItemWindow;

                }
                if (itemMode == Mode.Deposit)
                {
                    currentBagPosition = 0;
                    selectBag = -1;
                    StartCoroutine(WhatDepositText());
                    currentMenu = Menu.ItemWindow;

                }
                if (itemMode == Mode.Toss)
                {
                    currentBagPosition = 0;
                    selectBag = -1;
                    StartCoroutine(WhatTossText());
                    currentMenu = Menu.ItemWindow;

                }
            }

        }
        }

        foreach (GameObject menu in allMenus)
        {
            if (menu != allMenus[(int)currentMenu]){
                menu.SetActive(false);
            }
            else{
                menu.SetActive(true);
            }

            if (menu == mainwindow && (currentMenu == Menu.ItemWindow || currentMenu == Menu.QuantityMenu)){
                menu.SetActive(true);
            }
            if (menu == quantitymenu && (currentMenu == Menu.ItemWindow || currentMenu == Menu.MainWindow)){
                menu.SetActive(false);
            }
            if (menu == itemwindow && currentMenu == Menu.QuantityMenu){
                menu.SetActive(true);
            }

        }
        yield return 0;
    }

    IEnumerator WithdrawItem(){
        alreadyInBag = false;
        Item withdrawnItem = Items.instance.pcItems[currentBagPosition];
        string DisplayString = PokemonData.GetItemName(withdrawnItem.item) + ".";
        yield return Dialogue.instance.text("Withdrew&l" + DisplayString);
        Item inBagItem = new Item(ItemsEnum.None, 0, false);

        foreach (Item item in Items.instance.items)
        {
            if (item.item == withdrawnItem.item)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }
        }

        if (alreadyInBag) Items.instance.items[Items.instance.items.IndexOf(inBagItem)].quantity += amountToTask;
        else if (Items.instance.items.Count < 20) Items.instance.items.Add(new Item(withdrawnItem.item, amountToTask, withdrawnItem.isKeyItem));
        yield return StartCoroutine(RemoveItem(amountToTask));


        StartCoroutine(WhatDoText());
        itemMode = 0;
        UpdateMainScreen();
        currentMenu = Menu.MainWindow;
    }

    IEnumerator DepositItem()
    {
        alreadyInBag = false;
        Item depositedItem = Items.instance.items[currentBagPosition];
        yield return Dialogue.instance.text(PokemonData.GetItemName(depositedItem.item) + " was&lstored via PC.");

        Item inBagItem = new Item(ItemsEnum.None, 0, false);

        foreach (Item item in Items.instance.pcItems)
        {
            if (item.item == depositedItem.item)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }
        }

        if (alreadyInBag) Items.instance.pcItems[Items.instance.pcItems.IndexOf(inBagItem)].quantity += amountToTask;
        else if (Items.instance.pcItems.Count < 50) Items.instance.pcItems.Add(new Item(depositedItem.item, amountToTask, depositedItem.isKeyItem));
        yield return StartCoroutine(RemoveItem(amountToTask));

        StartCoroutine(WhatDoText());
        itemMode = 0;
        UpdateMainScreen();
        currentMenu = Menu.MainWindow;
    }

    IEnumerator TossItem()
    {
        Item tossedItem = Items.instance.pcItems[currentBagPosition];
        yield return Dialogue.instance.text("Threw away " + PokemonData.GetItemName(tossedItem.item) + ".");
        yield return StartCoroutine(RemoveItem(amountToTask));

        StartCoroutine(WhatDoText());
        currentMenu = Menu.MainWindow;
        UpdateMainScreen();
        itemMode = 0;
    }

    public IEnumerator RemoveItem(int amount)
    {
        if (itemMode == Mode.Withdraw || itemMode == Mode.Toss)
        {
            Items.instance.RemoveItemPC(amount, currentBagPosition);
        }
        if (itemMode == Mode.Deposit)
        {
            Items.instance.RemoveItem(amount, currentBagPosition);

        }
        yield return null;
    }
    IEnumerator SetItemMode(Mode mode)
    {
        currentBagPosition = 0;
        itemMode = mode;
        selectBag = -1;

        switch(itemMode){
            case Mode.Withdraw:
                StartCoroutine(WhatWithdrawText());
                break;
            case Mode.Deposit:
                StartCoroutine(WhatDepositText());
                break;
            case Mode.Toss:
                StartCoroutine(WhatTossText());
                break;
        }

        currentMenu = Menu.ItemWindow;
        UpdateBagScreen();
        yield return null;
    }

    IEnumerator TooImportantToToss()
    {

        Dialogue.instance.Deactivate();
        yield return Dialogue.instance.text("That's too impor-&ltant to toss!");
        selectCursor.gameObject.SetActive(false);
        UpdateBagScreen();
        currentMenu = Menu.ItemWindow;

    }
    IEnumerator WhatDoText()
    {
        Dialogue.instance.Deactivate();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text("What do you want&lto do?");

    }
    IEnumerator WhatWithdrawText()
    {
        Dialogue.instance.Deactivate();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text("What do you want&lto withdraw?");

    }
    IEnumerator WhatDepositText()
    {
        Dialogue.instance.Deactivate();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text("What do you want&lto deposit?");

    }
    IEnumerator WhatTossText()
    {
        Dialogue.instance.Deactivate();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text("What do you want&lto toss?");

    }
    void Close()
    {
        cursor.SetActive(false);
        Dialogue.instance.fastText = false;
        Dialogue.instance.Deactivate();
        Player.instance.menuActive = false;
        InputManager.Enable(Button.Start);
        this.gameObject.SetActive(false);
    }
}
