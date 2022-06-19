using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemMenuBase : MonoBehaviour {
    public GameCursor cursor;
    public int selectedOption;
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
}