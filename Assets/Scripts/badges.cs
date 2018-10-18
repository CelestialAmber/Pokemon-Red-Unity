using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Badges : MonoBehaviour {
	public Image[] allbadges = new Image[8];
	public Sprite[] notobtainedimages = new Sprite[8];
	public Sprite[] obtainedimages = new Sprite[8];
    public CustomText nameText, moneyText, timeText;

	// Use this for initialization
	void Start () {

	}
    public void Init(){
        nameText.text = GameData.playerName;
        moneyText.text = GameData.money.ToString();
        timeText.text =  GameData.hours.SpaceFormat(3) + " " + GameData.minutes.ZeroFormat("0x");
        for (int i = 0; i < 8; i++)
        {
            if (GameData.hasBadge[i])
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
	void Update () {

	}
}
