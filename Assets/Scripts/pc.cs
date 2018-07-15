using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pc : MonoBehaviour  {
    public GameObject currentMenu;
    public GameObject cursor;
    public GameObject mainwindow, itemwindow,   quantitymenu;
    public GameObject[] menuSlots, itemSelectSlots;
    public Dialogue mylog;
    public Player play;
    public int selectedOption;
    public GameObject[] allMenus;
    public int ItemMode;
    //1 is withdraw;
    //2 is deposit;
    //3 is toss;
    public List<GameObject> Items = new List<GameObject>(51);
    public itemdatabase id;
    public int currentBagPosition;
    public GameObject cancel;
    public int selectBag;
    public int amountToTask;
    public bool didFirstRunthrough;
    public int maximumItem;
    public CustomText amountText;
    public bool donewaiting;
    public GameObject last;
    public GameObject container;
    public bool alreadyInBag;
    public bool alreadydidtext;

    public bool withdrawing;
    public int offscreenindexup, offscreenindexdown;
    public GameObject content;
    public int usedcap;
    void Start() {
        
    
    }
    private void Awake()
    {
        foreach(Transform child in container.transform){
            Items.Add(child.gameObject); 
        }
    }






    void UpdateBagScreen(){
        usedcap = ItemMode == 2 ? id.items.Count : id.pcitems.Count;
        if (ItemMode == 2)
        {
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
            Items[50].SetActive(true);

        }
        if (ItemMode == 1 || ItemMode == 3)
        {
            foreach (GameObject slot in Items)
            {
                slot.SetActive(false);
            }
            for (int i = 0; i < id.pcitems.Count; i++)
            {
                Items[i].SetActive(true);
                Items[i].GetComponent<itemslotinformation>().Name = id.pcitems[i].name;
                Items[i].GetComponent<itemslotinformation>().intquantity = id.pcitems[i].quantity;
                Items[i].GetComponent<itemslotinformation>().isKeyItem = id.pcitems[i].isKeyItem;
            }
            Items[50].SetActive(true);

        }

        didFirstRunthrough = true;
        itemSelectSlots = new GameObject[usedcap + 1];

        for (int i = 0; i < usedcap; i++)
        {

            itemSelectSlots[i] = Items[i].transform.GetChild(0).gameObject;
        }
        itemSelectSlots[itemSelectSlots.Length - 1] = Items[50].transform.GetChild(0).gameObject;
    }
    public IEnumerator Initialize(){
        
        mylog.displayimmediatenoscroll = true;
        yield return StartCoroutine(mylog.text ("What do you want"));
        yield return StartCoroutine(mylog.line("to do?"));
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

        usedcap = ItemMode == 2 ? id.items.Count : id.pcitems.Count;
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
            }
            if (Inputs.pressed("up"))
            {
                amountToTask++;
            }
            if (amountToTask < 1)
            {
                amountToTask = maximumItem;

            }
            if (amountToTask > maximumItem)
            {
                amountToTask = 1;

            }


        }
        if (currentMenu == itemwindow)
        {
            if (Inputs.pressed("down"))
            {
                currentBagPosition++;
                if (currentBagPosition == offscreenindexdown && offscreenindexdown != usedcap + 1)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));
                }
            }
            if (Inputs.pressed("up"))
            {
                currentBagPosition--;
                if (currentBagPosition == offscreenindexup && offscreenindexup > -1)
                {
                    offscreenindexup--;
                    offscreenindexdown--;
                    content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));
                }

            }
            if (currentBagPosition < 0)
            {

                currentBagPosition = 0;
                if (offscreenindexdown < 0)
                {

                }


            }
            if (currentBagPosition == usedcap + 1)
            {

                currentBagPosition = usedcap;

                if (offscreenindexdown == usedcap + 1)
                {
                }

            }


            if (!didFirstRunthrough)
            {
                UpdateBagScreen();
            }
            cursor.SetActive(true);
            cursor.transform.position = itemSelectSlots[currentBagPosition].transform.position;

            if (currentBagPosition != ((ItemMode == 2) ? id.items.Count : id.pcitems.Count))
            {
                maximumItem = Items[currentBagPosition].GetComponent<itemslotinformation>().intquantity;
            }
            else
            {
                maximumItem = 0;

            }


        }
        if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow))
        {
            cursor.SetActive(false);
        }
        else
        {
            menuSlots = new GameObject[currentMenu.transform.childCount];

            for (int i = 0; i < currentMenu.transform.childCount; i++)
            {


                menuSlots[i] = currentMenu.transform.GetChild(i).gameObject;
            }
            if (currentMenu == mainwindow)
            {


                cursor.SetActive(true);
                cursor.transform.position = menuSlots[selectedOption].transform.position;

                if (Inputs.pressed("down"))
                {
                    selectedOption++;
                }
                if (Inputs.pressed("up"))
                {
                    selectedOption--;
                }
                if (selectedOption < 0)
                {
                    selectedOption = 0;

                }
                if (selectedOption == menuSlots.Length)
                {
                    selectedOption = menuSlots.Length - 1;

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
                    Item item = id.pcitems[selectBag];
                    id.pcitems[selectBag] = id.pcitems[currentBagPosition];
                    id.pcitems[currentBagPosition] = item;

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
                            if (currentBagPosition == ((ItemMode == 2) ? id.items.Count : id.pcitems.Count))
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
                                if (id.pcitems.Count > 0)
                                {
                                    for (int i = 0; i < id.pcitems.Count; i++)
                                    {
                                        Items[i].GetComponent<itemslotinformation>().intquantity = id.pcitems[i].quantity;
                                        Items[i].GetComponent<itemslotinformation>().isKeyItem = id.pcitems[i].isKeyItem;
                                        Items[i].GetComponent<itemslotinformation>().Name = id.pcitems[i].name;


                                    }
                                }
                                StartCoroutine(ItemMode1());


                            }
                            if (selectedOption == 1)
                            {
                                if (id.items.Count > 0)
                                {
                                    for (int i = 0; i < id.items.Count; i++)
                                    {
                                        Items[i].GetComponent<itemslotinformation>().intquantity = id.items[i].quantity;
                                        Items[i].GetComponent<itemslotinformation>().isKeyItem = id.items[i].isKeyItem;
                                        Items[i].GetComponent<itemslotinformation>().Name = id.items[i].name;


                                    }
                                }
                                StartCoroutine(ItemMode2());

                            }
                            if (selectedOption == 2)
                            {
                                if (id.pcitems.Count > 0)
                                {
                                    for (int i = 0; i < id.pcitems.Count; i++)
                                    {
                                        Items[i].GetComponent<itemslotinformation>().intquantity = id.pcitems[i].quantity;
                                        Items[i].GetComponent<itemslotinformation>().isKeyItem = id.pcitems[i].isKeyItem;
                                        Items[i].GetComponent<itemslotinformation>().Name = id.pcitems[i].name;


                                    }
                                }
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
        Item  withdrawnitem = id.pcitems[currentBagPosition];
        string DisplayString =  withdrawnitem.name + ".";
        yield return StartCoroutine(mylog.text("Withdrew"));
        yield return StartCoroutine(mylog.line(DisplayString));

        mylog.finishedWithTextOverall = true;
        Item inbagItem = new Item("", 0);
        foreach (Item item in id.items)
        {
            if (item.name == withdrawnitem.name)
            {
                inbagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.items[id.items.IndexOf(inbagItem)].quantity += amountToTask;
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
        Item inbagItem = new Item("", 0);
        foreach(Item item in id.pcitems){
            if (item.name == depositeditem.name)
            {
                inbagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.pcitems[id.pcitems.IndexOf(inbagItem)].quantity += amountToTask;
        else if (id.pcitems.Count < 50) id.pcitems.Add(new Item(depositeditem.name, amountToTask));
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
        Item tosseditem = id.pcitems[currentBagPosition];
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
            id.pcitems[currentBagPosition].quantity -= amount;
            if (id.pcitems[currentBagPosition].quantity == 0 || id.pcitems[currentBagPosition].isKeyItem) id.pcitems.RemoveAt(currentBagPosition);
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
