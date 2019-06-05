using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Badges : MonoBehaviour {
	public Image[] allbadges = new Image[8];
	public Sprite[] notobtainedimages = new Sprite[8];
	public Sprite[] obtainedimages = new Sprite[8];
    public CustomText nameText, moneyText, timeText;
    public GameCursor cursor;

	// Use this for initialization
	
    public void Init(){
        nameText.text = GameData.instance.playerName;
        moneyText.text = GameData.instance.money.ToString();
        timeText.text =  GameData.instance.hours.SpaceFormat(3) + " " + GameData.instance.minutes.ZeroFormat("0x");
        for (int i = 0; i < 8; i++)
        {
            if (GameData.instance.hasBadge[i])
            {
                allbadges[i].sprite = obtainedimages[i];

            }
            else
            {

                allbadges[i].sprite = notobtainedimages[i];
            }


        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Inputs.pressed("a"))
        {


            if (MainMenu.instance.currentmenu == MainMenu.instance.badgesmenu)
            {
                SoundManager.instance.PlayABSound();
                MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
                Inputs.Enable("start");
                cursor.SetActive(true);
                this.gameObject.SetActive(false);

            }
        }
    }
}
