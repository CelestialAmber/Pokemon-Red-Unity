using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{
	public enum Menu {
        ItemWindow,
		UseTossMenu,
        QuantityMenu
    }

    public Menu currentMenu;
    public GameCursor cursor;
    public GameObject usetossmenu, itemwindow, quantitymenu;
    public GameObject indicator;
    public int selectedOption;
    public GameObject[] allMenus;
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



    public void Initialize()
    {
        UpdateBagScreen();
        currentMenu = Menu.ItemWindow;
    }

    void UpdateBagScreen(){
        if (currentBagPosition == 0)
        {
            topItemIndex = 0;
        }

        for (int i = 0; i < 4; i++)
        {
            int currentItem = topItemIndex + i;

            if (currentItem < Items.instance.items.Count)
            {
                itemSlots[i].mode = SlotMode.Item;
                itemSlots[i].item = Items.instance.items[currentItem].item;
                itemSlots[i].quantity = Items.instance.items[currentItem].quantity;
                itemSlots[i].isKeyItem = Items.instance.items[currentItem].isKeyItem;
            }
            else if (currentItem == Items.instance.items.Count)
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

    void UpdateUseTossScreen()
    {
        selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (currentBagPosition - topItemIndex)) + new Vector2(4, 4);
        cursor.SetPosition(112, 72 - 16 * selectedOption);
    }

    // Update is called once per frame
    private void Update()
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
            if (amountToTask < 1)
            {
                amountToTask = maximumItem;
            }
            if (amountToTask > maximumItem)
            {
                amountToTask = 1;
            }

            amountText.text = amountToTask.ToString();
        }
        if (currentMenu == Menu.ItemWindow && Dialogue.instance.finishedText)
        {

            if (InputManager.Pressed(Button.Down))
            {
                currentBagPosition++;
                MathE.Clamp(ref currentBagPosition, 0, Items.instance.items.Count);

                if (currentBagPosition == topItemIndex + 3 && Items.instance.items.Count > 3)
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

        if (currentMenu == Menu.UseTossMenu && Dialogue.instance.finishedText)
        {
            if (InputManager.Pressed(Button.Down))
            {
                selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 1);
                UpdateUseTossScreen();
            }
            if (InputManager.Pressed(Button.Up))
            {
                selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 1);
                UpdateUseTossScreen();
            }
        }

        if (InputManager.Pressed(Button.Select) && Dialogue.instance.finishedText && currentBagPosition != Items.instance.items.Count)
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
                selectCursor.gameObject.SetActive(false);
                Item item = Items.instance.items[selectBag];
                Items.instance.items[selectBag] = Items.instance.items[currentBagPosition];
                Items.instance.items[currentBagPosition] = item;
                switchingItems = false;
                UpdateBagScreen();
            }
        }

        if (Dialogue.instance.finishedText)
        {
            if (InputManager.Pressed(Button.A))
            {

                SoundManager.instance.PlayABSound();

                if (currentMenu == Menu.ItemWindow)
                {
                    if (currentBagPosition == Items.instance.items.Count)
                    {
                        MainMenu.instance.selectedOption = 0;
                        MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        amountToTask = 1;
                        //usetossmenu.SetActive(true);
                        UpdateUseTossScreen();
                        switchingItems = false;
                        selectCursor.gameObject.SetActive(true);
                        cursor.SetActive(true);
                        currentMenu = Menu.UseTossMenu;
                    }
                }
                else if (currentMenu == Menu.UseTossMenu)
                {
                    if (selectedOption == 0)
                    {
                        if (Items.instance.items.Count > 0)
                        {
                            Player.instance.UseItem(Items.instance.items[currentBagPosition].item);
                        }
                    }
                    if (selectedOption == 1)
                    {
                        if (Items.instance.items.Count > 0)
                        {
                            selectBag = -1;
                            //quantitymenu.SetActive(true);
                            currentMenu = Menu.QuantityMenu;
                        }
                    }
                }
                else if (currentMenu == Menu.QuantityMenu)
                {
                    if (!Items.instance.items[currentBagPosition].isKeyItem)
                    {
                        yield return Dialogue.instance.text("Is it OK to toss&l" + PokemonData.GetItemName(Items.instance.items[currentBagPosition].item) + "?");
                        yield return StartCoroutine(Dialogue.instance.prompt());
                        if (Dialogue.instance.selectedOption == 0)
                        {
                            yield return Dialogue.instance.text("Threw away&l" + PokemonData.GetItemName(Items.instance.items[currentBagPosition].item) + ".");
                            StartCoroutine(TossItem());
                        }
                        else
                        {
                            Dialogue.instance.Deactivate();
                            UpdateBagScreen();
                            cursor.SetActive(true);
                            currentMenu = Menu.ItemWindow;
                        }
                    }
                    else
                    {
                        StartCoroutine(TooImportantToToss());
                    }
                }
            }
            if (InputManager.Pressed(Button.B))
            {
                SoundManager.instance.PlayABSound();
                if (currentMenu == Menu.ItemWindow)
                {

                    switchingItems = false;
                    selectCursor.gameObject.SetActive(false);
                    MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
                    InputManager.Enable(Button.Start);
                    this.gameObject.SetActive(false);
                }
                else if (currentMenu == Menu.UseTossMenu)
                {
                    currentMenu = Menu.ItemWindow;
                    selectCursor.gameObject.SetActive(false);
                    UpdateBagScreen();
                }
                else if (currentMenu == Menu.QuantityMenu)
                {
                    selectCursor.gameObject.SetActive(false);
                    UpdateBagScreen();
                    currentMenu = Menu.ItemWindow;
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

                if (menu == usetossmenu && (currentMenu == Menu.QuantityMenu))
                {
                    menu.SetActive(true);
                }
                if (menu == quantitymenu && (currentMenu == Menu.ItemWindow))
                {
                    menu.SetActive(false);
                }
                if (menu == itemwindow && (currentMenu == Menu.QuantityMenu || currentMenu == Menu.UseTossMenu))
                {
                    menu.SetActive(true);
                }
            }
        }
    }

    public IEnumerator TossItem()
    {
        Dialogue.instance.Deactivate();
        Items.instance.RemoveItem(amountToTask, currentBagPosition);
        cursor.SetActive(true);
        UpdateBagScreen();
        currentMenu = Menu.ItemWindow;
        yield return 0;
    }


    IEnumerator TooImportantToToss()
    {
        Dialogue.instance.Deactivate();
        yield return Dialogue.instance.text("That's too impor-&ltant to toss!");
        selectCursor.gameObject.SetActive(false);
        UpdateBagScreen();
        currentMenu = Menu.ItemWindow;
    }


    public void Close()
    {
        indicator.SetActive(false);
        this.gameObject.SetActive(false);
    }

}