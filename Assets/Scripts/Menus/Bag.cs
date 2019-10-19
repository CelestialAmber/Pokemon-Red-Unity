using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Bag : MonoBehaviour  {
	
	public GameObject currentMenu;
	public GameCursor cursor;
	public GameObject usetossmenu, itemwindow,   quantitymenu;
    public GameObject indicator;
    public int selectedOption;
	public GameObject[] allMenus;
	public int ItemMode;
	public List<ItemSlot> itemSlots = new List<ItemSlot>(4);
	public int currentBagPosition;
	public int selectBag;
	public int amountToTask;
	public int maximumItem;
	public CustomText amountText;
    public RectTransform selectCursor;
    public int offscreenindexup, offscreenindexdown;
    public bool switching;

    public static Bag instance;
    private void Awake()
    {
        instance = this;
    }
    
    public void Initialize() {
		UpdateBagScreen();
		currentMenu = itemwindow;
		  
	}

	public bool itemInInventory(string item){
		for (int i = 0; i < Items.instance.items.Count; i++) {
			if (Items.instance.items[i].name == item) {
				return true;
			}



		}
		return false;



	}
    public void UpdateBagScreen(){
 if(currentBagPosition == 0){
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }


        for (int i = 0; i < 4; i++)
        {
            int currentItem = offscreenindexup + 1 + i;
            if (currentItem > offscreenindexup && currentItem < Items.instance.items.Count)
            {
                itemSlots[i].mode = SlotMode.Item;
                itemSlots[i].Name = Items.instance.items[currentItem].name;
                itemSlots[i].intquantity = Items.instance.items[currentItem].quantity;
                itemSlots[i].isKeyItem = Items.instance.items[currentItem].isKeyItem;
            }else if(currentItem == Items.instance.items.Count){
                itemSlots[i].mode = SlotMode.Cancel;
                 
            }else
            {
                itemSlots[i].mode = SlotMode.Empty;

            }
        }
        cursor.SetPosition(40,104 - 16 * (currentBagPosition - offscreenindexup - 1));
        if (offscreenindexdown < Items.instance.items.Count) indicator.SetActive(true);
        else indicator.SetActive(false);
        if (switching)
        {
            selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (selectBag - offscreenindexup - 1)) + new Vector2(4, 4);
            if (selectCursor.anchoredPosition.y > 112 || selectCursor.anchoredPosition.y < 50) selectCursor.gameObject.SetActive(false);
            else selectCursor.gameObject.SetActive(true);
        }
    }
    void UpdateUseTossScreen(){
        selectCursor.anchoredPosition = new Vector2(40, 104 - 16 * (currentBagPosition - offscreenindexup - 1)) + new Vector2(4, 4);
        cursor.SetPosition(112, 72 - 16 * selectedOption);
    }
		
	





    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(MainUpdate());
    }
    IEnumerator  MainUpdate () {
        if(currentBagPosition == 0){
            offscreenindexup = -1;
            offscreenindexdown = 3;
        }


		amountText.text = amountToTask.ToString ();
        if (currentMenu == quantitymenu && Dialogue.instance.finishedText) {
			
            if (Inputs.pressed("down")) {
				amountToTask--;
			}
            if (Inputs.pressed("up")) {
				amountToTask++;
			}
			if (amountToTask < 1) {
				amountToTask = maximumItem;

			}
			if (amountToTask > maximumItem) {
				amountToTask = 1;

			}


		}
        if (currentMenu == itemwindow && Dialogue.instance.finishedText)
        {

            if (Inputs.pressed("down"))
            {
                currentBagPosition++;

                if (currentBagPosition == offscreenindexdown && currentBagPosition <= Items.instance.items.Count && Items.instance.items.Count > 3)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                }
                MathE.Clamp(ref currentBagPosition, 0, Items.instance.items.Count);
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
                MathE.Clamp(ref currentBagPosition, 0, Items.instance.items.Count);
                UpdateBagScreen();
               

            }
           

			if (currentBagPosition != Items.instance.items.Count) {
				maximumItem = Items.instance.items [currentBagPosition].quantity;
			} else {
				maximumItem = 0;

			}
		

		}
		if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow)) {
			
		} else {
            if (currentMenu == usetossmenu && Dialogue.instance.finishedText) {
				

                if (Inputs.pressed("down")) {
					selectedOption++;
                    MathE.Clamp(ref selectedOption, 0, 1);
                    UpdateUseTossScreen();
				}
                if (Inputs.pressed("up")) {
					selectedOption--;
                    MathE.Clamp(ref selectedOption, 0, 1);
                    UpdateUseTossScreen();
				}
				
			}
		
		}
        if (Inputs.pressed("select") && Dialogue.instance.finishedText && currentBagPosition != Items.instance.items.Count) {
			if (!switching) {
                switching = true;
                selectCursor.gameObject.SetActive(true);
				selectBag = currentBagPosition;
                UpdateBagScreen();
			} else {
                //our Bag
                selectCursor.gameObject.SetActive(false);
                Item item = Items.instance.items[selectBag];
                Items.instance.items[selectBag] = Items.instance.items[currentBagPosition];
                Items.instance.items[currentBagPosition] = item;
                switching = false;
                UpdateBagScreen();
                




			}


		}
		if (Dialogue.instance.finishedText) {
			
			if (Inputs.pressed("a")) {
			SoundManager.instance.PlayABSound();
				if (currentMenu == itemwindow) {
                   
					
                        
                        if (currentBagPosition == Items.instance.items.Count) {

							MainMenu.instance.selectedOption = 0;
							MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
							this.gameObject.SetActive (false);
								
							
						} else {
							amountToTask = 1;
							usetossmenu.SetActive (true);
                            UpdateUseTossScreen();
                            switching = false;
                        selectCursor.gameObject.SetActive(true);
                            cursor.SetActive(true);
							currentMenu = usetossmenu;




						}
						
					
					
				}

				else if (currentMenu == usetossmenu) {
                    
				
                   
						if (selectedOption == 0) {
                            if (Items.instance.items.Count > 0) {
								ItemMode1 ();
                                Player.instance.UseItem (Items.instance.items [currentBagPosition].name);
								
							}


						}
						if (selectedOption == 1) {
							if (Items.instance.items.Count > 0) {
							

								ItemMode2 ();
								quantitymenu.SetActive (true);
								currentMenu = quantitymenu;
							}


						}

					
					


				}
				else if (currentMenu == quantitymenu) {
					
						
						if (ItemMode == 1) {


							//use item

						}
				
						if (ItemMode == 2) {
						
							if (!Items.instance.items[currentBagPosition].isKeyItem) {
                                yield return Dialogue.instance.text("Is it OK to toss \\l" + Items.instance.items[currentBagPosition].name + "?");
                                yield return StartCoroutine(Dialogue.instance.prompt());
                                if(Dialogue.instance.selectedOption == 0){
                                    yield return Dialogue.instance.text("Threw away " + Items.instance.items[currentBagPosition].name + ".");
                                    StartCoroutine(TossItem());
									

                                }else{
                                    Dialogue.instance.Deactivate();
                                    UpdateBagScreen();
                                    cursor.SetActive(true);
                                    currentMenu = itemwindow;
                                }
								
							} else {
								StartCoroutine (TooImportantToToss ());
							

						}




					}
					
				

			}
		}
		if (Inputs.pressed("b")) {
			SoundManager.instance.PlayABSound();
			if (currentMenu == itemwindow) {

                    switching = false;
                    selectCursor.gameObject.SetActive(false);
                   MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
				Inputs.Enable("start");
				this.gameObject.SetActive (false);

			}
			else if (currentMenu == usetossmenu) {
				currentMenu = itemwindow;
                    selectCursor.gameObject.SetActive(false);
				UpdateBagScreen();
			}
			else if (currentMenu == quantitymenu) {
							
				if (ItemMode == 2) {
                        selectCursor.gameObject.SetActive(false);
                        UpdateBagScreen();
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
			if(menu == usetossmenu &&( currentMenu ==  quantitymenu)){
				menu.SetActive (true);
			}
		
		if(menu == quantitymenu && (currentMenu == itemwindow)){
			menu.SetActive(false);

		}
			if(menu == itemwindow  && (currentMenu == quantitymenu || currentMenu == usetossmenu)){
			menu.SetActive(true);

		}
                if (currentMenu == null) indicator.SetActive(false);

	}
	}
	}



	//deposit

	public IEnumerator TossItem(){

	
		 
		Dialogue.instance.Deactivate ();
		Items.instance.RemoveItem(amountToTask, currentBagPosition);
		cursor.SetActive (true);
        UpdateBagScreen();
		currentMenu = itemwindow;
		ItemMode = 0;
        yield return 0;
	}
	

	void ItemMode1(){
		//code
	}
	void ItemMode2(){
		ItemMode = 2;
		selectBag = -1;

	}
	IEnumerator TooImportantToToss(){

		Dialogue.instance.Deactivate();
		yield return Dialogue.instance.text ("That's too impor-\\ltant to toss!");
        selectCursor.gameObject.SetActive(false);
UpdateBagScreen();
	currentMenu = itemwindow;
}
    public void Close()
    {
        indicator.SetActive(false);
        currentMenu = null;
        this.gameObject.SetActive(false);
    }
}
