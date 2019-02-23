using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
public enum SwitchMenu {
    BootUp,
    StartupMenu,
    HomeMenu,
    GameBoot,
    InGame

}
public class NintendoSwitchManager : MonoBehaviour
{
    public GameObject gameIconRect, batterySymbol;
    public List<GameObject> gameIconObjs;
    public int selectedGameIndex;
    public SwitchMenu currentMenu;
    public GameObject[] menus;
    public Text timeText, ampmText, chargeText;
    public Image batteryChargeImage;
    void Awake(){
    gameIconObjs = new List<GameObject>();
    foreach(Transform t in gameIconRect.transform){
        gameIconObjs.Add(t.gameObject);
    }    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         switch(currentMenu){
             case SwitchMenu.HomeMenu:
            batterySymbol.SetActive(SystemInfo.batteryStatus == BatteryStatus.Charging);
            timeText.text = System.DateTime.Now.ToString("h:mm");
            ampmText.text = System.DateTime.Now.ToString("tt");
            chargeText.text = (SystemInfo.batteryLevel * 100).ToString();
            batteryChargeImage.fillAmount = SystemInfo.batteryLevel;
             break;
         }
    }
    void UpdateMenus(){
        for(int i = 0; i < menus.Length; i++){
            if((int)currentMenu == i)menus[i].SetActive(true);
            else menus[i].SetActive(false);
        }
    }
}
