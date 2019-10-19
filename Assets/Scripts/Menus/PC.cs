using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PC : MonoBehaviour  {
    public GameObject currentMenu;
    public GameCursor cursor;
    public GameObject mainwindow, itemwindow,   quantitymenu;
    public int selectedOption;
    public GameObject[] allMenus;
    public int ItemMode;
    //1 is withdraw;
    //2 is deposit;
    //3 is toss;
    public List<ItemSlot> itemSlots = new List<ItemSlot>(4);
    public int currentBagPosition;
    public int selectBag;
    public int amountToTask;
    public int maximumItem;
    public CustomText amountText;
    public bool alreadyInBag;
    public int offscreenindexup, offscreenindexdown;
    public int numberOfItems;
    public RectTransform selectCursor;
    public bool switching;
    public GameObject indicator;
    public static PC instance;
    
    void UpdateBagScreen(){
        
        numberOfItems = ItemMode == 2 ? Items.instance.items.Count : Items.instance.pcItems.Count;
          if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }
        if (ItemMode == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                int currentItem = offscreenindexup + 1 + i;
                if (currentItem > offscreenindexup && currentItem < numberOfItems)
                {
                    itemSlots[i].mode = SlotMode.Item;
                    itemSlots[i].Name = Items.instance.items[currentItem].name;
                    itemSlots[i].intquantity = Items.instance.items[currentItem].quantity;
                    itemSlots[i].isKeyItem = Items.instance.items[currentItem].isKeyItem;
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
        }
        if (ItemMode == 1 || ItemMode == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                int currentItem = offscreenindexup + 1 + i;
                if (currentItem > offscreenindexup && currentItem < numberOfItems)
                {
                    itemSlots[i].mode = SlotMode.Item;
                    itemSlots[i].Name = Items.instance.pcItems[currentItem].name;
                    itemSlots[i].intquantity = Items.instance.pcItems[currentItem].quantity;
                    itemSlots[i].isKeyItem = Items.instance.pcItems[currentItem].isKeyItem;
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
        }
        if (currentBagPosition != numberOfItems && itemSlots[currentBagPosition - offscreenindexup - 1].mode == SlotMode.Item)
            maximumItem = itemSlots[currentBagPosition - offscreenindexup - 1].intquantity;
        else maximumItem = 0;
        if (offscreenindexdown < numberOfItems) indicator.SetActive(true);
        else indicator.SetActive(false);
        cursor.SetPosition(40, 104 - 16 * (currentBagPosition - offscreenindexup - 1));
        if (switching)
        {
            selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (selectBag - offscreenindexup - 1)) + new Vector2(4, 4);
            if (selectCursor.anchoredPosition.y > 112 || selectCursor.anchoredPosition.y < 50) selectCursor.gameObject.SetActive(false);
            else selectCursor.gameObject.SetActive(true);
        }
    }
    void UpdateMainScreen() //update the withdraw/deposit screen
    {
        cursor.SetPosition(8, 120 - 16 * selectedOption);
    }
    void UpdateQuantityScreen()
    {
        selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (currentBagPosition - offscreenindexup - 1)) + new Vector2(4, 4);
        amountText.text = amountToTask.ToString();
    }
    public IEnumerator Initialize(){
        
        StartCoroutine(WhatDoText());
        cursor.SetActive(true);
        UpdateBagScreen();
        cursor.SetPosition(8, 120 - 16 * selectedOption);
        currentMenu = mainwindow;
        yield return 0;

    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MainUpdate());
    }
    IEnumerator MainUpdate()
    {

        numberOfItems = ItemMode == 2 ? Items.instance.items.Count : Items.instance.pcItems.Count;
        if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }
       
        if (currentMenu == quantitymenu)
        {

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
        if (currentMenu == itemwindow)
        {
            if (Inputs.pressed("down"))
            {
                currentBagPosition++;

                if (currentBagPosition == offscreenindexdown && currentBagPosition <= numberOfItems && numberOfItems > 3)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                }
                MathE.Clamp(ref currentBagPosition, 0, numberOfItems);
                UpdateBagScreen();
               
            }
            if (Inputs.pressed("up"))
            {
                currentBagPosition--;

                if (currentBagPosition == offscreenindexup && offscreenindexup > -1)
                {
                    offscreenindexup--;
                    offscreenindexdown--;
                }
                MathE.Clamp(ref currentBagPosition, 0, numberOfItems);
                UpdateBagScreen();
               

            }
           

        




        }
       
            if (currentMenu == mainwindow)
            {

                if (Inputs.pressed("down"))
                {
                    selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 3);
                UpdateMainScreen();
                }
                if (Inputs.pressed("up"))
                {
                    selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 3);
                UpdateMainScreen();
                }
            }
        
        if (Inputs.pressed("select"))
        {
            if (currentMenu == itemwindow)
            {
                if (!switching)
                {
                    switching = true;
                    selectCursor.gameObject.SetActive(true);
                    selectBag = currentBagPosition;
                    UpdateBagScreen();
                }
                else if (currentBagPosition != numberOfItems)
                {

                    //our bag
                    if (ItemMode == 2)
                    {
                        selectCursor.gameObject.SetActive(false);
                        Item item = Items.instance.items[selectBag];
                        Items.instance.items[selectBag] = Items.instance.items[currentBagPosition];
                        Items.instance.items[currentBagPosition] = item;
                        switching = false;
                    }


                    if (ItemMode == 1 || ItemMode == 3)
                    {
                        Item item = Items.instance.pcItems[selectBag];
                        Items.instance.pcItems[selectBag] = Items.instance.pcItems[currentBagPosition];
                        Items.instance.pcItems[currentBagPosition] = item;

                        switching = false;
                    }

                    UpdateBagScreen();

                }
            }

        }
        
            if (Inputs.pressed("a"))
            {
                SoundManager.instance.PlayABSound();
                    if (currentMenu == itemwindow)
                    {
                        
                            if (currentBagPosition == numberOfItems)
                            {
                                Dialogue.instance.Deactivate();
                                Dialogue.instance.fastText = true;
                                switching = false;
                                selectCursor.gameObject.SetActive(false);
                                Dialogue.instance.keepTextOnScreen = true;
                                Dialogue.instance.needButtonPress = false;
                    yield return Dialogue.instance.text("What do you want\\lto do?");
                                currentMenu = mainwindow;
                    UpdateMainScreen();
                }
                            else
                            {
                                if (!itemSlots[currentBagPosition - offscreenindexup - 1].isKeyItem  && ItemMode != 3)
                                {
                                    amountToTask = 1;
                                    Dialogue.instance.Deactivate();
                                    Dialogue.instance.fastText = true;
                                    Dialogue.instance.keepTextOnScreen = true;
                                    Dialogue.instance.needButtonPress = false;
                                    yield return Dialogue.instance.text("How much?");
                        currentMenu = quantitymenu;
                                    UpdateQuantityScreen();
                                }else if(itemSlots[currentBagPosition - offscreenindexup - 1].isKeyItem){
                                    switch(ItemMode){
                                        case 1:
                                            StartCoroutine(WithdrawItem());
                                            break;
                                        case 2:
                                            StartCoroutine(DepositItem());
                                            break;
                                    }

                                }else if(ItemMode == 3){
                                    amountToTask = 1;
                                    Dialogue.instance.Deactivate();
                                    Dialogue.instance.fastText = true;
                                    Dialogue.instance.keepTextOnScreen = true;
                                    Dialogue.instance.needButtonPress = false;
                        yield return Dialogue.instance.text("How much?");
                                    currentMenu = quantitymenu;
                        UpdateQuantityScreen();
                                }


                            }
                            if (currentBagPosition != numberOfItems && itemSlots[currentBagPosition - offscreenindexup - 1].isKeyItem && ItemMode == 3)
                            {
                                StartCoroutine(TooImportantToToss());
                            }
                        
                    }

                    else if (currentMenu == mainwindow)
                    {
                       
                            if (selectedOption == 0)
                            {
                                UpdateBagScreen();
                                StartCoroutine(ItemMode1());


                            }
                            if (selectedOption == 1)
                            {
                                UpdateBagScreen();
                                StartCoroutine(ItemMode2());

                            }
                            if (selectedOption == 2)
                            {
                                UpdateBagScreen();
                                StartCoroutine(ItemMode3());

                            }
                            if (selectedOption == 3)
                            {
                                Close();

                            }

                    }
                    else if (currentMenu == quantitymenu)
                    {
                        
                            if (ItemMode == 3)
                            {
                                if (!itemSlots[currentBagPosition - offscreenindexup - 1].isKeyItem)
                                {
                                    StartCoroutine(TossItem());
                                }
                            }
                            if (ItemMode == 1)
                            {

                                StartCoroutine(WithdrawItem());


                            }

                            if (ItemMode == 2)
                            {
                                StartCoroutine(DepositItem());

                            }


                




                        
                    }
                
            }


            if (Inputs.pressed("b"))
            {
                SoundManager.instance.PlayABSound();
                if (currentMenu == mainwindow)
                {
                Close();

                }
                else if (currentMenu == itemwindow)
                {
                    

                    switching = false;
                selectCursor.gameObject.SetActive(false);
                StartCoroutine(WhatDoText());
                    
                    currentMenu = mainwindow;
                UpdateMainScreen();

            }
                else if (currentMenu == quantitymenu)
                {
                    if (ItemMode == 1)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                       StartCoroutine(WhatWithdrawText());
                        currentMenu = itemwindow;

                    }
                    if (ItemMode == 2)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                       StartCoroutine(WhatDepositText());
                        currentMenu = itemwindow;

                    }
                    if (ItemMode == 3)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        StartCoroutine(WhatTossText());
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
            if (menu == mainwindow && (currentMenu == itemwindow || currentMenu == quantitymenu)) {


                menu.SetActive (true);
            }
            if(menu == quantitymenu && (currentMenu == itemwindow || currentMenu == mainwindow)){
                menu.SetActive(false);

            }
            if(menu == itemwindow  && currentMenu == quantitymenu){
                menu.SetActive(true);

            }

        }
        yield return 0;

    }
    IEnumerator WithdrawItem()
    {

        alreadyInBag = false;
        Item  withdrawnitem = Items.instance.pcItems[currentBagPosition];
        string DisplayString =  withdrawnitem.name + ".";
        yield return Dialogue.instance.text("Withdrew\\l" + DisplayString);
        Item inBagItem = new Item("", 0,false);
        foreach (Item item in Items.instance.items)
        {
            if (item.name == withdrawnitem.name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) Items.instance.items[Items.instance.items.IndexOf(inBagItem)].quantity += amountToTask;
        else if (Items.instance.items.Count < 20) Items.instance.items.Add(new Item(withdrawnitem.name, amountToTask,withdrawnitem.isKeyItem));
        yield return StartCoroutine(RemoveItem(amountToTask));


        StartCoroutine(WhatDoText());
        ItemMode = 0;
        UpdateMainScreen();
        currentMenu = mainwindow;




    }

    //deposit
    IEnumerator DepositItem(){
        alreadyInBag = false;
        Item depositeditem = Items.instance.items[currentBagPosition];
        yield return Dialogue.instance.text (depositeditem.name + " was\\lstored via PC.");
        Item inBagItem = new Item("", 0,false);
        foreach(Item item in Items.instance.pcItems){
            if (item.name == depositeditem.name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) Items.instance.pcItems[Items.instance.pcItems.IndexOf(inBagItem)].quantity += amountToTask;
        else if (Items.instance.pcItems.Count < 50) Items.instance.pcItems.Add(new Item(depositeditem.name, amountToTask,depositeditem.isKeyItem));
        yield return StartCoroutine(RemoveItem(amountToTask));

        StartCoroutine(WhatDoText());
        ItemMode = 0;
        UpdateMainScreen();
        currentMenu = mainwindow;




    }
    IEnumerator TossItem(){

         
        Item tosseditem = Items.instance.pcItems[currentBagPosition];
        yield return Dialogue.instance.text("Threw away " + tosseditem.name + ".");
        yield return StartCoroutine(RemoveItem (amountToTask));
    
        StartCoroutine(WhatDoText());
        currentMenu = mainwindow;
        UpdateMainScreen();
        ItemMode = 0;
    }
    


    public IEnumerator RemoveItem(int amount){
        if (ItemMode == 1 || ItemMode == 3) {
            Items.instance.RemoveItemPC(amount,currentBagPosition);
        }
        if (ItemMode == 2) {
            Items.instance.RemoveItem(amount,currentBagPosition);

        }
        yield return null;
    }
    IEnumerator ItemMode1(){
        currentBagPosition = 0;
        ItemMode = 1;
        selectBag = -1;
        StartCoroutine(WhatWithdrawText());
        currentMenu = itemwindow;
        UpdateBagScreen();
        yield return null;
    }
    IEnumerator ItemMode2(){
        currentBagPosition = 0;
        ItemMode = 2;
        selectBag = -1;
       StartCoroutine(WhatDepositText());
        currentMenu = itemwindow;
        UpdateBagScreen();
        yield return null;
    }
    IEnumerator ItemMode3(){
        currentBagPosition = 0;
        ItemMode = 3;
        selectBag = -1;
        StartCoroutine(WhatTossText());
        currentMenu = itemwindow;
        UpdateBagScreen();
        yield return null;
    }
        IEnumerator TooImportantToToss(){

            Dialogue.instance.Deactivate();
        yield return Dialogue.instance.text ("That's too impor-\\ltant to toss!");
        selectCursor.gameObject.SetActive(false);
        UpdateBagScreen();
        currentMenu = itemwindow;

    }
    IEnumerator WhatDoText(){
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text ("What do you want\\lto do?");

    }
    IEnumerator WhatWithdrawText(){
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text ("What do you want\\lto withdraw?");

    }
    IEnumerator WhatDepositText(){
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text ("What do you want\\lto deposit");

    }
    IEnumerator WhatTossText(){
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text ("What do you want\\lto toss?");

    }
    void Close()
    {
        cursor.SetActive(false);
        Dialogue.instance.fastText = false;
        Dialogue.instance.Deactivate();
        Player.instance.menuActive = false;
        Inputs.Enable("start");
        this.gameObject.SetActive(false);
    }
}
