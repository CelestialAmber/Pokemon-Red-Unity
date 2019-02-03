using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PC : MonoBehaviour  {
    public GameObject currentMenu;
    public GameCursor cursor;
    public GameObject mainwindow, itemwindow,   quantitymenu;
    public Player play;
    public int selectedOption;
    public GameObject[] allMenus;
    public int ItemMode;
    //1 is withdraw;
    //2 is deposit;
    //3 is toss;
    public List<ItemSlot> Items = new List<ItemSlot>(4);
    public Items id;
    public int currentBagPosition;
    public int selectBag;
    public int amountToTask;
    public int maximumItem;
    public CustomText amountText;
    public GameObject last;
    public bool alreadyInBag;
    public bool alreadydidtext;
    public bool withdrawing;
    public int offscreenindexup, offscreenindexdown;
    public int numberOfItems;
    public RectTransform selectCursor;
    public bool switching;
    public GameObject indicator;

    
    void UpdateBagScreen(){
        
        numberOfItems = ItemMode == 2 ? id.items.Count : id.pcItems.Count;
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
                    Items[i].mode = SlotMode.Item;
                    Items[i].Name = id.items[currentItem].name;
                    Items[i].intquantity = id.items[currentItem].quantity;
                    Items[i].isKeyItem = id.items[currentItem].isKeyItem;
                }
                else if (currentItem == numberOfItems)
                {
                    Items[i].mode = SlotMode.Cancel;

                }
                else
                {
                    Items[i].mode = SlotMode.Empty;

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
                    Items[i].mode = SlotMode.Item;
                    Items[i].Name = id.pcItems[currentItem].name;
                    Items[i].intquantity = id.pcItems[currentItem].quantity;
                    Items[i].isKeyItem = id.pcItems[currentItem].isKeyItem;
                }
                else if (currentItem == numberOfItems)
                {
                    Items[i].mode = SlotMode.Cancel;

                }
                else
                {
                    Items[i].mode = SlotMode.Empty;

                }
            }
        }
        if (currentBagPosition != numberOfItems && Items[currentBagPosition - offscreenindexup - 1].mode == SlotMode.Item)
            maximumItem = Items[currentBagPosition - offscreenindexup - 1].intquantity;
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
        
        Dialogue.instance.fastText = true;
        yield return StartCoroutine(Dialogue.instance.text ("What do you want\nto do?",true));
        cursor.SetActive(true);
        UpdateBagScreen();
        cursor.SetPosition(8, 120 - 16 * selectedOption);
        currentMenu = mainwindow;

    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MainUpdate());
    }
    IEnumerator MainUpdate()
    {

        numberOfItems = ItemMode == 2 ? id.items.Count : id.pcItems.Count;
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
                        Item item = id.items[selectBag];
                        id.items[selectBag] = id.items[currentBagPosition];
                        id.items[currentBagPosition] = item;
                        switching = false;
                    }


                    if (ItemMode == 1 || ItemMode == 3)
                    {
                        Item item = id.pcItems[selectBag];
                        id.pcItems[selectBag] = id.pcItems[currentBagPosition];
                        id.pcItems[currentBagPosition] = item;

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
                    yield return StartCoroutine(Dialogue.instance.text("What do you want\nto do?", true));
                                currentMenu = mainwindow;
                    UpdateMainScreen();
                }
                            else
                            {
                                if (!Items[currentBagPosition - offscreenindexup - 1].isKeyItem  && ItemMode != 3)
                                {
                                    amountToTask = 1;
                                    Dialogue.instance.Deactivate();
                                    Dialogue.instance.fastText = true;
                                    yield return StartCoroutine(Dialogue.instance.text("How much?", true));
                        currentMenu = quantitymenu;
                                    UpdateQuantityScreen();
                                }else if(Items[currentBagPosition - offscreenindexup - 1].isKeyItem){
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
                        yield return StartCoroutine(Dialogue.instance.text("How much?", true));
                                    currentMenu = quantitymenu;
                        UpdateQuantityScreen();
                                }


                            }
                            if (currentBagPosition != numberOfItems && Items[currentBagPosition - offscreenindexup - 1].isKeyItem && ItemMode == 3)
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
                                if (!Items[currentBagPosition - offscreenindexup - 1].isKeyItem)
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
                    
                    Dialogue.instance.Deactivate();
                    Dialogue.instance.fastText = true;
                    switching = false;
                selectCursor.gameObject.SetActive(false);
                    yield return StartCoroutine(Dialogue.instance.text("What do you want\nto do?", true));
                    
                    currentMenu = mainwindow;
                UpdateMainScreen();

            }
                else if (currentMenu == quantitymenu)
                {
                    if (ItemMode == 1)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        Dialogue.instance.Deactivate();
                        Dialogue.instance.fastText = true;
                        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto withdraw?", true));
                        currentMenu = itemwindow;

                    }
                    if (ItemMode == 2)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        Dialogue.instance.Deactivate();
                        Dialogue.instance.fastText = true;
                        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto deposit?", true));
                        currentMenu = itemwindow;

                    }
                    if (ItemMode == 3)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        Dialogue.instance.Deactivate();
                        Dialogue.instance.fastText = true;
                        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto toss?", true));
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
        Item  withdrawnitem = id.pcItems[currentBagPosition];
        string DisplayString =  withdrawnitem.name + ".";
        yield return StartCoroutine(Dialogue.instance.text("Withdrew\n" + DisplayString));
        Item inBagItem = new Item("", 0,false);
        foreach (Item item in id.items)
        {
            if (item.name == withdrawnitem.name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.items[id.items.IndexOf(inBagItem)].quantity += amountToTask;
        else if (id.items.Count < 20) id.items.Add(new Item(withdrawnitem.name, amountToTask,withdrawnitem.isKeyItem));
        yield return StartCoroutine(RemoveItem(amountToTask));


        Dialogue.instance.fastText = true;
       yield return StartCoroutine(Dialogue.instance.text("What do you want\nto do?", true));
        ItemMode = 0;
        UpdateMainScreen();
        currentMenu = mainwindow;




    }

    //deposit
    IEnumerator DepositItem(){
        alreadyInBag = false;
        Item depositeditem = id.items[currentBagPosition];
        yield return StartCoroutine(Dialogue.instance.text (depositeditem.name + " was\nstored via PC."));
        Item inBagItem = new Item("", 0,false);
        foreach(Item item in id.pcItems){
            if (item.name == depositeditem.name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.pcItems[id.pcItems.IndexOf(inBagItem)].quantity += amountToTask;
        else if (id.pcItems.Count < 50) id.pcItems.Add(new Item(depositeditem.name, amountToTask,depositeditem.isKeyItem));
        yield return StartCoroutine(RemoveItem(amountToTask));

        Dialogue.instance.fastText = true;
        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto do?", true));
        ItemMode = 0;
        UpdateMainScreen();
        currentMenu = mainwindow;




    }
    IEnumerator TossItem(){

         
        Item tosseditem = id.pcItems[currentBagPosition];
        yield return StartCoroutine(Dialogue.instance.text("Threw away " + tosseditem.name + "."));
        yield return StartCoroutine(RemoveItem (amountToTask));
    
        Dialogue.instance.fastText = true;
        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto do?", true));
        currentMenu = mainwindow;
        UpdateMainScreen();
        ItemMode = 0;
    }
    


    public IEnumerator RemoveItem(int amount){
        if (ItemMode == 1 || ItemMode == 3) {
            id.pcItems[currentBagPosition].quantity -= amount;
            if (id.pcItems[currentBagPosition].quantity == 0 || id.pcItems[currentBagPosition].isKeyItem) id.pcItems.RemoveAt(currentBagPosition);
        }
        if (ItemMode == 2) {
            id.items[currentBagPosition].quantity -= amount;
            if (id.items[currentBagPosition].quantity == 0 || id.items[currentBagPosition].isKeyItem) id.items.RemoveAt(currentBagPosition);

        }
        yield return null;
    }
    IEnumerator ItemMode1(){
        currentBagPosition = 0;
        ItemMode = 1;
        selectBag = -1;
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto withdraw?", true));
        currentMenu = itemwindow;
        UpdateBagScreen();
    }
    IEnumerator ItemMode2(){
        currentBagPosition = 0;
        ItemMode = 2;
        selectBag = -1;
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto deposit?", true));
        currentMenu = itemwindow;
        UpdateBagScreen();

    }
    IEnumerator ItemMode3(){
        currentBagPosition = 0;
        ItemMode = 3;
        selectBag = -1;
        Dialogue.instance.Deactivate ();
        Dialogue.instance.fastText = true;
        yield return StartCoroutine(Dialogue.instance.text("What do you want\nto toss?", true));
        currentMenu = itemwindow;
        UpdateBagScreen();

    }
        IEnumerator TooImportantToToss(){

            Dialogue.instance.Deactivate();
        yield return StartCoroutine(Dialogue.instance.text ("That's too impor-\ntant to toss!"));
        selectCursor.gameObject.SetActive(false);
        UpdateBagScreen();
        currentMenu = itemwindow;

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
