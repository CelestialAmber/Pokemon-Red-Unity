using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PC : MonoBehaviour  {
    public GameObject currentMenu;
    public Cursor cursor;
    public GameObject mainwindow, itemwindow,   quantitymenu;
    public Dialogue mylog;
    public Player play;
    public int selectedOption;
    public GameObject[] allMenus;
    public int ItemMode;
    //1 is withdraw;
    //2 is deposit;
    //3 is toss;
    public List<GameObject> Items = new List<GameObject>(4);
    public Items id;
    public int currentBagPosition;
    public int selectBag;
    public int amountToTask;
    public bool didFirstRunthrough;
    public int maximumItem;
    public CustomText amountText;
    public bool donewaiting;
    public GameObject last;
    public bool alreadyInBag;
    public bool alreadydidtext;
    public UnityEvent onAddItem;
    public bool withdrawing;
    public int offscreenindexup, offscreenindexdown;
    public int usedcap;

    void UpdateBagScreen(){
        usedcap = ItemMode == 2 ? id.items.Count : id.pcItems.Count;
        if (ItemMode == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                int currentItem = offscreenindexup + 1 + i;
                if (currentItem > offscreenindexup && currentItem < usedcap)
                {
                    Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Item;
                    Items[i].GetComponent<itemslotinformation>().Name = id.items[currentItem].name;
                    Items[i].GetComponent<itemslotinformation>().intquantity = id.items[currentItem].quantity;
                    Items[i].GetComponent<itemslotinformation>().isKeyItem = id.items[currentItem].isKeyItem;
                }
                else if (currentItem == usedcap)
                {
                    Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Cancel;

                }
                else
                {
                    Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Empty;

                }
            }
        }
        if (ItemMode == 1 || ItemMode == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                int currentItem = offscreenindexup + 1 + i;
                if (currentItem > offscreenindexup && currentItem < usedcap)
                {
                    Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Item;
                    Items[i].GetComponent<itemslotinformation>().Name = id.pcItems[currentItem].name;
                    Items[i].GetComponent<itemslotinformation>().intquantity = id.pcItems[currentItem].quantity;
                    Items[i].GetComponent<itemslotinformation>().isKeyItem = id.pcItems[currentItem].isKeyItem;
                }
                else if (currentItem == usedcap)
                {
                    Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Cancel;

                }
                else
                {
                    Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Empty;

                }
            }
        }
        if (currentBagPosition != usedcap && Items[currentBagPosition - offscreenindexup].GetComponent<itemslotinformation>().mode == SlotMode.Item)
            maximumItem = Items[currentBagPosition - offscreenindexup].GetComponent<itemslotinformation>().intquantity;
        else maximumItem = 0;
        cursor.SetPosition(40, 108 - 16 * (currentBagPosition - offscreenindexup - 1));
    }
    public IEnumerator Initialize(){
        
        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text ("What do you want"));
        yield return StartCoroutine(mylog.line("to do?"));
        cursor.SetActive(true);
        cursor.SetPosition(8, 120 - 16 * selectedOption);
        currentMenu = mainwindow;
        play.PCactive = true;

    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MainUpdate());
    }
    IEnumerator MainUpdate()
    {

        usedcap = ItemMode == 2 ? id.items.Count : id.pcItems.Count;
        if (currentBagPosition == 0)
        {
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }
        amountText.text = amountToTask.ToString();
        if (currentMenu == quantitymenu)
        {

            if (Inputs.pressed("down"))
            {
                amountToTask--;
                MathE.Wrap(ref amountToTask, 1, maximumItem);
            }
            if (Inputs.pressed("up"))
            {
                amountToTask++;
                MathE.Wrap(ref amountToTask, 1, maximumItem);
            }



        }
        if (currentMenu == itemwindow)
        {
            if (Inputs.pressed("down"))
            {
                currentBagPosition++;

                if (currentBagPosition == offscreenindexdown && currentBagPosition <= usedcap && usedcap > 3)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                }
                MathE.Clamp(ref currentBagPosition, 0, usedcap);
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
                MathE.Clamp(ref currentBagPosition, 0, usedcap);
                UpdateBagScreen();
               

            }
           

            if (!didFirstRunthrough)
            {
                UpdateBagScreen();
                cursor.SetActive(true);
            }






        }
        if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow))
        {
        }
        else
        {
            if (currentMenu == mainwindow)
            {

                if (Inputs.pressed("down"))
                {
                    selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 3);
                    cursor.SetPosition(8, 120 - 16 * selectedOption);
                }
                if (Inputs.pressed("up"))
                {
                    selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 3);
                    cursor.SetPosition(8, 120 - 16 * selectedOption);
                }
            }
        }
        if (Inputs.pressed("select"))
        {
            if (selectBag == -1)
            {
                selectBag = currentBagPosition;
            }
            else
            {

                //our bag
                if (ItemMode == 2)
                {
                    Item item = id.items[selectBag];
                    id.items[selectBag] = id.items[currentBagPosition];
                    id.items[currentBagPosition] = item;
                    selectBag = -1;
                }


                if (ItemMode == 1 || ItemMode == 3)
                {
                    Item item = id.pcItems[selectBag];
                    id.pcItems[selectBag] = id.pcItems[currentBagPosition];
                    id.pcItems[currentBagPosition] = item;

                    selectBag = -1;
                }

                UpdateBagScreen();

            }


        }
        if (mylog.finishedWithTextOverall)
        {
            if (Inputs.pressed("a"))
            {
                if (donewaiting)
                {
                    if (currentMenu == itemwindow)
                    {
                        if (donewaiting)
                        {
                            if (currentBagPosition == ((ItemMode == 2) ? id.items.Count : id.pcItems.Count))
                            {
                                mylog.Deactivate();
                                mylog.displayimmediatenoscroll = true;
                                yield return StartCoroutine(mylog.text("What do you want"));
                                yield return StartCoroutine(mylog.line("to do?"));
                                currentMenu = mainwindow;
                            }
                            else
                            {
                                if (!Items[currentBagPosition].GetComponent<itemslotinformation>().isKeyItem  && ItemMode != 3)
                                {
                                    amountToTask = 1;
                                    mylog.Deactivate();
                                    mylog.displayimmediatenoscroll = true;
                                    yield return StartCoroutine(mylog.text("How much?"));
                                    currentMenu = quantitymenu;
                                }else if(Items[currentBagPosition].GetComponent<itemslotinformation>().isKeyItem){
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
                                    mylog.Deactivate();
                                    mylog.displayimmediatenoscroll = true;
                                    yield return StartCoroutine(mylog.text("How much?"));
                                    currentMenu = quantitymenu;
                                }


                            }
                            if (currentBagPosition != usedcap && Items[currentBagPosition].GetComponent<itemslotinformation>().isKeyItem && ItemMode == 3)
                            {
                                StartCoroutine(TooImportantToToss());
                            }
                        }
                        StartCoroutine(Wait());
                    }

                    if (currentMenu == mainwindow)
                    {
                        if (donewaiting)
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
                                play.overrideRenable = false;
                                mylog.Deactivate();
                                mylog.finishedWithTextOverall = true;
                                play.PCactive = false;
                                play.WaitToInteract();
                                this.gameObject.SetActive(false);

                            }

                        }
                        StartCoroutine(Wait());
                    }
                    if (currentMenu == quantitymenu)
                    {
                        if (donewaiting)
                        {
                            if (ItemMode == 3)
                            {
                                if (!Items[currentBagPosition].GetComponent<itemslotinformation>().isKeyItem)
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
                        StartCoroutine(Wait());
                    }
                }
            }


            if (Inputs.pressed("b"))
            {
                if (currentMenu == mainwindow)
                {
                    play.overrideRenable = false;
                    mylog.Deactivate();
                    play.PCactive = false;
                    mylog.finishedWithTextOverall = true;
                    play.WaitToInteract();
                    this.gameObject.SetActive(false);

                }
                if (currentMenu == itemwindow)
                {
                    didFirstRunthrough = false;
                    mylog.Deactivate();
                    mylog.displayimmediatenoscroll = true;
                    yield return StartCoroutine(mylog.text("What do you want"));
                    yield return StartCoroutine(mylog.line("to do?"));
                    currentMenu = mainwindow;

                }
                if (currentMenu == quantitymenu)
                {
                    if (ItemMode == 1)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        mylog.Deactivate();
                        mylog.displayimmediatenoscroll = true;
                        yield return StartCoroutine(mylog.text("What do you want"));
                        yield return StartCoroutine(mylog.line("to withdraw?"));
                        currentMenu = itemwindow;

                    }
                    if (ItemMode == 2)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        mylog.Deactivate();
                        mylog.displayimmediatenoscroll = true;
                        yield return StartCoroutine(mylog.text("What do you want"));
                        yield return StartCoroutine(mylog.line("to deposit?"));
                        currentMenu = itemwindow;

                    }
                    if (ItemMode == 3)
                    {
                        currentBagPosition = 0;
                        selectBag = -1;
                        mylog.Deactivate();
                        mylog.displayimmediatenoscroll = true;
                        yield return StartCoroutine(mylog.text("What do you want"));
                        yield return StartCoroutine(mylog.line("to toss?"));
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
        mylog.Deactivate();
        mylog.cantscroll = false;
        mylog.finishedCurrentTask = true;
        Item  withdrawnitem = id.pcItems[currentBagPosition];
        string DisplayString =  withdrawnitem.name + ".";
        yield return StartCoroutine(mylog.text("Withdrew"));
        yield return StartCoroutine(mylog.line(DisplayString));

        mylog.finishedWithTextOverall = true;
        Item inBagItem = new Item("", 0);
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
        else if (id.items.Count < 20) id.items.Add(new Item(withdrawnitem.name, amountToTask));
        yield return StartCoroutine(RemoveItem(amountToTask));


        mylog.displayimmediatenoscroll = true;
       yield return StartCoroutine(mylog.text("What do you want"));
        yield return StartCoroutine(mylog.line("to do?"));
        ItemMode = 0;
        UpdateBagScreen();
        currentMenu = mainwindow;




    }

    //deposit
    IEnumerator DepositItem(){
        alreadyInBag = false;
        mylog.Deactivate ();
        mylog.cantscroll = false;
        mylog.finishedCurrentTask = true;
        Item depositeditem = id.items[currentBagPosition];
        yield return StartCoroutine(mylog.text (depositeditem.name + " was"));
        yield return StartCoroutine(mylog.line("stored via PC."));

        mylog.finishedWithTextOverall = true;
        Item inBagItem = new Item("", 0);
        foreach(Item item in id.pcItems){
            if (item.name == depositeditem.name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.pcItems[id.pcItems.IndexOf(inBagItem)].quantity += amountToTask;
        else if (id.pcItems.Count < 50) id.pcItems.Add(new Item(depositeditem.name, amountToTask));
        yield return StartCoroutine(RemoveItem(amountToTask));

        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text("What do you want"));
        yield return StartCoroutine(mylog.line("to do?"));
        ItemMode = 0;
        UpdateBagScreen();
        currentMenu = mainwindow;




    }
    IEnumerator TossItem(){

         
        mylog.Deactivate ();
        mylog.cantscroll = false;
        mylog.finishedCurrentTask = true;
        Item tosseditem = id.pcItems[currentBagPosition];
        string DisplayString = "Threw away " + tosseditem.name + ".";
        yield return StartCoroutine(mylog.text(DisplayString));
        mylog.done ();
        yield return StartCoroutine(RemoveItem (amountToTask));
    
        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text("What do you want"));
        yield return StartCoroutine(mylog.line("to do?"));
        currentMenu = mainwindow;
        UpdateBagScreen();
        ItemMode = 0;
    }
    IEnumerator Wait(){
        donewaiting = false;
        yield return new WaitForSeconds (0.1f);
        donewaiting = true;
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
        mylog.Deactivate ();
        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text("What do you want"));
        yield return StartCoroutine(mylog.line("to withdraw?"));
        currentMenu = itemwindow;
        UpdateBagScreen();
    }
    IEnumerator ItemMode2(){
        currentBagPosition = 0;
        ItemMode = 2;
        selectBag = -1;
        mylog.Deactivate ();
        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text("What do you want"));
        yield return StartCoroutine(mylog.line("to deposit?"));
        currentMenu = itemwindow;
        UpdateBagScreen();

    }
    IEnumerator ItemMode3(){
        currentBagPosition = 0;
        ItemMode = 3;
        selectBag = -1;
        mylog.Deactivate ();
        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text("What do you want"));
        yield return StartCoroutine(mylog.line("to toss?"));
        currentMenu = itemwindow;
        UpdateBagScreen();

    }
        IEnumerator TooImportantToToss(){

            mylog.Deactivate();
        yield return StartCoroutine(mylog.text ("That's too impor-"));
        yield return StartCoroutine(mylog.line("tant to toss!"));
        yield return StartCoroutine(mylog.done());
        currentMenu = itemwindow;

    }
}
