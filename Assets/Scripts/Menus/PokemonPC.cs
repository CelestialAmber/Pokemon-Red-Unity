using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonPcState
{
    Withdraw,
    Deposit,
    Release
}
public class PokemonPC : MonoBehaviour
{
    public int currentBoxNumber; //what Pokemon box number are we on?
    public GameObject[] monBoxIcons; //the pokeball indicators next to each box
    public GameObject boxIconObject;
    public GameObject[] menus;
    public GameObject currentMenu, generalPcMenu, monPcMenu, monWithdrawMenu, monDepositMenu;
    public GameCursor cursor;
    public bool usingPlayerPC; //are we using our PC?
    public bool usingOakPC; //are we using Oak's PC?
    public MonPcState currentState;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
