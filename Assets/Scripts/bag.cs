using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class bag : MonoBehaviour  {
	
	public GameObject currentMenu;
	public GameObject cursor;
	public GameObject usetossmenu, itemwindow,   quantitymenu, content;
	public GameObject[] menuSlots, itemSelectSlots;
	public Dialogue mylog;
	public Player play;
	public int selectedOption;
	public GameObject[] allMenus;
	public int ItemMode;
	public int selecteditem;
	//1 is withdraw;
	//2 is deposit;
	//3 is toss;
	public List<GameObject> Items = new List<GameObject>(21);
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
        Items[20].SetActive(true);

        itemSelectSlots = new GameObject[id.items.Count + 1];

        for (int i = 0; i < id.items.Count; i++)
        {

            itemSelectSlots[i] = Items[i].transform.GetChild(0).gameObject;
        }
        itemSelectSlots[itemSelectSlots.Length - 1] = Items[20].transform.GetChild(0).gameObject;
        content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 16 * (offscreenindexup + 1));
    }
		
	


	
	public IEnumerator UseItem(string whatItem){
        
		if (whatItem == "BIKE VOUCHER") {

			play.Warp (new Vector2(16, -5));

		}
        if (whatItem == "BICYCLE")
        {
            switch (play.walkSurfBikeState)
            {
                case 0:
                    yield return StartCoroutine(mylog.text(Dialogue.Name + " got on the"));
                    yield return StartCoroutine(mylog.line("BICYCLE!"));
                    yield return StartCoroutine(mylog.done());
                    play.walkSurfBikeState = 1;
                    break;
                case 1:

                    yield return StartCoroutine(mylog.text(Dialogue.Name + " got off"));
                    yield return StartCoroutine(mylog.line("the BICYCLE."));
                    yield return StartCoroutine(mylog.done());
                    play.walkSurfBikeState = 0;
                    break;
            }

            currentMenu = itemwindow;
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
                if (currentBagPosition == offscreenindexdown && offscreenindexdown != id.items.Count + 1)
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
            if (currentBagPosition == id.items.Count + 1)
            {

                currentBagPosition = id.items.Count;

                if (offscreenindexdown == id.items.Count + 1)
                {

                }

            }
            if (currentBagPosition < 0) currentBagPosition = 0;
            if (!didFirstRunthrough)
            {
                UpdateBagScreen();
              




                didFirstRunthrough = true;
               
            }
            if (didFirstRunthrough)
            {
                cursor.transform.position = itemSelectSlots[currentBagPosition].transform.position;
            }
			cursor.SetActive (true);

			if (currentBagPosition != id.items.Count) {
				maximumItem = id.items [currentBagPosition].quantity;
			} else {
				maximumItem = 0;

			}
		

		}
		if (currentMenu == null && (currentMenu != quantitymenu || currentMenu != itemwindow)) {
			
		} else {
			menuSlots = new GameObject[currentMenu.transform.childCount];

			for (int i = 0; i < currentMenu.transform.childCount; i++) {
				 

				menuSlots [i] = currentMenu.transform.GetChild (i).gameObject;
			}
            if (currentMenu == usetossmenu && mylog.finishedCurrentTask) {
				cursor.transform.position = menuSlots [selectedOption].transform.position;

				cursor.SetActive (true);

                if (Inputs.pressed("down")) {
					selectedOption++;
				}
                if (Inputs.pressed("up")) {
					selectedOption--;
				}
				if (selectedOption < 0) {
					selectedOption = 0;

				}
				if (selectedOption == menuSlots.Length) {
					selectedOption = menuSlots.Length - 1;

				}
			}
		
		}
        if (Inputs.pressed("select") && mylog.finishedCurrentTask && currentBagPosition != id.items.Count) {
			if (selectBag == -1) {
				selectBag = currentBagPosition;
			} else {
                //our bag
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
						
							if (!Items [currentBagPosition].GetComponent<itemslotinformation> ().isKeyItem) {
                                yield return StartCoroutine(mylog.text("Is it OK to toss "));
                                yield return StartCoroutine(mylog.line(Items[currentBagPosition].GetComponent<itemslotinformation>().Name + "?"));
                                yield return StartCoroutine(mylog.prompt());
                                if(mylog.selectedOption == 0){
                                    yield return StartCoroutine(mylog.text("Threw away " + Items[currentBagPosition].GetComponent<itemslotinformation>().Name + "."));
                                    yield return StartCoroutine(mylog.done());
                                    StartCoroutine(TossItem());

                                }else{
                                    mylog.Deactivate();
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


public IEnumerator AddItem(string name, int quantity){

		alreadyInBag = false;

        Item inbagItem = new Item("", 0);
        foreach (Item item in id.items)
        {
            if (item.name == name)
            {
                inbagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        if (alreadyInBag) id.items[id.items.IndexOf(inbagItem)].quantity += amountToTask;
        else if (id.items.Count < 20) id.items.Add(new Item(name, quantity));


		ItemMode = 0;
		currentMenu = itemwindow;
        onGetItem.Invoke();
        yield return null;


       
		}

	//deposit

	public IEnumerator TossItem(){

	
		 
		mylog.Deactivate ();
		mylog.cantscroll = false;
		mylog.finishedCurrentTask = true;
		StartCoroutine(RemoveItem (amountToTask));
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


	public IEnumerator RemoveItem(int amount){
		
			id.items [currentBagPosition].quantity -= amount;
        if (id.items[currentBagPosition].quantity == 0) id.items.RemoveAt(currentBagPosition);

		
		yield return null;
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
