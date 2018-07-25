using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Bag : MonoBehaviour  {
	
	public GameObject currentMenu;
	public Cursor cursor;
	public GameObject usetossmenu, itemwindow,   quantitymenu;
	public Dialogue mylog;
	public Player play;
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
	public bool didFirstRunthrough;
	public int maximumItem;
	public CustomText amountText;
	public bool donewaiting;
	public GameObject last;
	public bool alreadyInBag;
	public bool alreadydidtext;
	public MainMenu moon;
	public int keep;
    public int offscreenindexup, offscreenindexdown;
    public UnityEvent onGetItem;

	public bool withdrawing;


	void Start() {
		
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
        cursor.SetPosition(40,108 - 16 * (currentBagPosition - offscreenindexup - 1));
    }
    void UpdateUseTossScreen(){
        cursor.SetPosition(112, 72 - 16 * selectedOption);
    }
		
	


	
	public IEnumerator UseItem(string whatItem){
        
        if (whatItem == "Bicycle")
        {
            switch (play.walkSurfBikeState)
            {
                case 0:
                    yield return StartCoroutine(mylog.text(SaveData.playerName + " got on the"));
                    yield return StartCoroutine(mylog.line("BICYCLE!"));
                    yield return StartCoroutine(mylog.done());
                    play.walkSurfBikeState = 1;
                    break;
                case 1:

                    yield return StartCoroutine(mylog.text(SaveData.playerName + " got off"));
                    yield return StartCoroutine(mylog.line("the BICYCLE."));
                    yield return StartCoroutine(mylog.done());
                    play.walkSurfBikeState = 0;
                    break;
            }

            currentMenu = itemwindow;
            cursor.SetActive(false);
            play.startmenuup = false;
            moon.selectedOption = 0;
            moon.currentmenu = null;
            moon.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            play.WaitToInteract(0.3f);

        }






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
        if (currentMenu == quantitymenu && mylog.finishedCurrentTask) {
			
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
        if (currentMenu == itemwindow && mylog.finishedCurrentTask)
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
           

            if (!didFirstRunthrough)
            {
                UpdateBagScreen();
                didFirstRunthrough = true;
               
            }
			

			if (currentBagPosition != id.items.Count) {
				maximumItem = id.items [currentBagPosition].quantity;
			} else {
				maximumItem = 0;

			}
		

		}
		if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow)) {
			
		} else {
            if (currentMenu == usetossmenu && mylog.finishedCurrentTask) {
				

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
        if (Inputs.pressed("select") && mylog.finishedCurrentTask && currentBagPosition != id.items.Count) {
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
		if (mylog.finishedWithTextOverall) {
			
			if (Inputs.pressed("a")) {
			
				if (currentMenu == itemwindow) {
                   
					if (donewaiting) {
                        StartCoroutine(Wait()); 
                        if (currentBagPosition == id.items.Count) {

							moon.selectedOption = 0;
							moon.currentmenu = moon.thismenu;
							this.gameObject.SetActive (false);
								
							
						} else {
							amountToTask = 1;
							usetossmenu.SetActive (true);
                            UpdateUseTossScreen();
                            cursor.SetActive(true);
							currentMenu = usetossmenu;




						}
						
					}
					
				}

				if (currentMenu == usetossmenu) {
                    
					if (donewaiting) {
                        StartCoroutine(Wait());
						if (selectedOption == 0) {
                            if (id.items.Count > 0) {
								ItemMode1 ();
                                StartCoroutine(UseItem (id.items [currentBagPosition].name));
								
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
					


				}
				if (currentMenu == quantitymenu) {
					if (donewaiting) {
						
						if (ItemMode == 1) {


							//use item

						}
				
						if (ItemMode == 2) {
						
							if (!id.items[currentBagPosition].isKeyItem) {
                                yield return StartCoroutine(mylog.text("Is it OK to toss "));
                                yield return StartCoroutine(mylog.line(id.items[currentBagPosition].name + "?"));
                                yield return StartCoroutine(mylog.prompt());
                                if(mylog.selectedOption == 0){
                                    yield return StartCoroutine(mylog.text("Threw away " + id.items[currentBagPosition].name + "."));
                                    yield return StartCoroutine(mylog.done());
                                    StartCoroutine(TossItem());

                                }else{
                                    mylog.Deactivate();
                                    UpdateBagScreen();
                                    cursor.SetActive(true);
                                    currentMenu = itemwindow;
                                }
								
							} else {
								StartCoroutine (TooImportantToToss ());
							

						}




					}
					StartCoroutine (Wait ());
				}

			}
		}
		if (Inputs.pressed("b")) {
			if (currentMenu == itemwindow) {
				
			
				moon.currentmenu = moon.thismenu;
				this.gameObject.SetActive (false);

			}
			if (currentMenu == usetossmenu) {
				didFirstRunthrough = false;
				currentMenu = itemwindow;

			}
			if (currentMenu == quantitymenu) {
							
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

	
		 
		mylog.Deactivate ();
		mylog.cantscroll = false;
		mylog.finishedCurrentTask = true;
		RemoveItem (amountToTask);
        UpdateBagScreen();
		currentMenu = itemwindow;
		ItemMode = 0;
        yield return 0;
	}
	public IEnumerator Wait(){
		donewaiting = false;
		yield return new WaitForSeconds (0.1f);
		donewaiting = true;
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

		mylog.Deactivate();
		yield return StartCoroutine(mylog.text ("That's too important to toss!"));
		yield return StartCoroutine(mylog.done ());

	currentMenu = itemwindow;
}
}
