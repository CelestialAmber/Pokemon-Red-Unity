using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Bag : MonoBehaviour  {
	
	public GameObject currentMenu;
	public GameCursor cursor;
	public GameObject usetossmenu, itemwindow,   quantitymenu;
	public int selectedOption;
	public GameObject[] allMenus;
	public int ItemMode;
	public int selecteditem;
	//1 is withdraw;
	//2 is deposit;
	//3 is toss;
	public List<GameObject> Items = new List<GameObject>(4);
	public Items id;
	public int currentBagPosition;
	public int selectBag;
	public int amountToTask;
	public int maximumItem;
	public CustomText amountText;

	public GameObject last;
	public bool alreadyInBag;
	public bool alreadydidtext;
	public int keep;
    public int offscreenindexup, offscreenindexdown;
    public UnityEvent onGetItem;

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
                Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Item;
                Items[i].GetComponent<itemslotinformation>().Name = id.items[currentItem].name;
                Items[i].GetComponent<itemslotinformation>().intquantity = id.items[currentItem].quantity;
                Items[i].GetComponent<itemslotinformation>().isKeyItem = id.items[currentItem].isKeyItem;
            }else if(currentItem == id.items.Count){
                Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Cancel;
                 
            }else
            {
                Items[i].GetComponent<itemslotinformation>().mode = SlotMode.Empty;

            }
        }
        cursor.SetPosition(40,104 - 16 * (currentBagPosition - offscreenindexup - 1));
    }
    void UpdateUseTossScreen(){
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
        if (currentMenu == quantitymenu && Dialogue.instance.finishedCurrentTask) {
			
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
        if (currentMenu == itemwindow && Dialogue.instance.finishedCurrentTask)
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
            if (currentMenu == usetossmenu && Dialogue.instance.finishedCurrentTask) {
				

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
        if (Inputs.pressed("select") && Dialogue.instance.finishedCurrentTask && currentBagPosition != id.items.Count) {
			if (selectBag == -1) {
				selectBag = currentBagPosition;
			} else {
                //our Bag
                Item item = id.items[selectBag];
                id.items[selectBag] = id.items[currentBagPosition];
                id.items[currentBagPosition] = item;
                UpdateBagScreen();
				selectBag = -1;




			}


		}
		if (Dialogue.instance.finishedWithTextOverall) {
			
			if (Inputs.pressed("a")) {
			
				if (currentMenu == itemwindow) {
                   
					
                        
                        if (currentBagPosition == id.items.Count) {

							Get.menu.selectedOption = 0;
							Get.menu.currentmenu = Get.menu.thismenu;
							this.gameObject.SetActive (false);
								
							
						} else {
							amountToTask = 1;
							usetossmenu.SetActive (true);
                            UpdateUseTossScreen();
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
                                yield return StartCoroutine(Dialogue.instance.text("Is it OK to toss "));
                                yield return StartCoroutine(Dialogue.instance.line(id.items[currentBagPosition].name + "?"));
                                yield return StartCoroutine(Dialogue.instance.prompt());
                                if(Dialogue.instance.selectedOption == 0){
                                    yield return StartCoroutine(Dialogue.instance.text("Threw away " + id.items[currentBagPosition].name + "."));
                                    yield return StartCoroutine(Dialogue.instance.done());
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
			if (currentMenu == itemwindow) {
				
			
				Get.menu.currentmenu = Get.menu.thismenu;
				Inputs.Enable("start");
				this.gameObject.SetActive (false);

			}
			else if (currentMenu == usetossmenu) {
				currentMenu = itemwindow;
				UpdateBagScreen();
			}
			else if (currentMenu == quantitymenu) {
							
				if (ItemMode == 2) {
					currentBagPosition = 0;
					selectBag = -1;
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
		Dialogue.instance.finishedCurrentTask = true;
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
		yield return StartCoroutine(Dialogue.instance.text ("That's too important to toss!"));
		yield return StartCoroutine(Dialogue.instance.done ());
UpdateBagScreen();
	currentMenu = itemwindow;
}
}
