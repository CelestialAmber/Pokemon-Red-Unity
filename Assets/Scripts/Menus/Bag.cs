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
	public int selecteditem;
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
    public RectTransform selectCursor;
	public bool alreadyInBag;
	public bool alreadydidtext;
	public int keep;
    public int offscreenindexup, offscreenindexdown;
    public bool switching;
	public bool withdrawing;

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
		for (int i = 0; i < id.items.Count; i++) {
			if (id.items[i].name == item) {
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
            if (currentItem > offscreenindexup && currentItem < id.items.Count)
            {
                Items[i].mode = SlotMode.Item;
                Items[i].Name = id.items[currentItem].name;
                Items[i].intquantity = id.items[currentItem].quantity;
                Items[i].isKeyItem = id.items[currentItem].isKeyItem;
            }else if(currentItem == id.items.Count){
                Items[i].mode = SlotMode.Cancel;
                 
            }else
            {
                Items[i].mode = SlotMode.Empty;

            }
        }
        cursor.SetPosition(40,104 - 16 * (currentBagPosition - offscreenindexup - 1));
        if (offscreenindexdown < id.items.Count) indicator.SetActive(true);
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

                if (currentBagPosition == offscreenindexdown && currentBagPosition <= id.items.Count && id.items.Count > 3)
                {
                    offscreenindexup++;
                    offscreenindexdown++;
                }
                MathE.Clamp(ref currentBagPosition, 0, id.items.Count);
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
                MathE.Clamp(ref currentBagPosition, 0, id.items.Count);
                UpdateBagScreen();
               

            }
           

			if (currentBagPosition != id.items.Count) {
				maximumItem = id.items [currentBagPosition].quantity;
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
        if (Inputs.pressed("select") && Dialogue.instance.finishedText && currentBagPosition != id.items.Count) {
			if (!switching) {
                switching = true;
                selectCursor.gameObject.SetActive(true);
				selectBag = currentBagPosition;
                UpdateBagScreen();
			} else {
                //our Bag
                selectCursor.gameObject.SetActive(false);
                Item item = id.items[selectBag];
                id.items[selectBag] = id.items[currentBagPosition];
                id.items[currentBagPosition] = item;
                switching = false;
                UpdateBagScreen();
                




			}


		}
		if (Dialogue.instance.finishedText) {
			
			if (Inputs.pressed("a")) {
			SoundManager.instance.PlayABSound();
				if (currentMenu == itemwindow) {
                   
					
                        
                        if (currentBagPosition == id.items.Count) {

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
                            if (id.items.Count > 0) {
								ItemMode1 ();
                                StartCoroutine(Player.instance.UseItem (id.items [currentBagPosition].name));
								
							}


						}
						if (selectedOption == 1) {
							if (id.items.Count > 0) {
							

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
						
							if (!id.items[currentBagPosition].isKeyItem) {
                                yield return StartCoroutine(Dialogue.instance.text("Is it OK to toss \n" + id.items[currentBagPosition].name + "?"));
                                yield return StartCoroutine(Dialogue.instance.prompt());
                                if(Dialogue.instance.selectedOption == 0){
                                    yield return StartCoroutine(Dialogue.instance.text("Threw away " + id.items[currentBagPosition].name + "."));
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

	}
	}
	}



	//deposit

	public IEnumerator TossItem(){

	
		 
		Dialogue.instance.Deactivate ();
		RemoveItem (amountToTask);
		cursor.SetActive (true);
        UpdateBagScreen();
		currentMenu = itemwindow;
		ItemMode = 0;
        yield return 0;
	}
	


	public void RemoveItem(int amount){
		
			id.items [currentBagPosition].quantity -= amount;
        if (id.items[currentBagPosition].quantity <= 0) id.items.RemoveAt(currentBagPosition);
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
		yield return StartCoroutine(Dialogue.instance.text ("That's too impor-\ntant to toss!"));
        selectCursor.gameObject.SetActive(false);
UpdateBagScreen();
	currentMenu = itemwindow;
}
}
