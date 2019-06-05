using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreditsHandler : MonoBehaviour {
	public CustomText top, middle, low, bottom;
	public int CreditIndex, MonIndex;
	public Image monimage;
	public Sprite[] mons = new Sprite[16];
	public GameObject textboxes;
    public static CreditsHandler instance;
    public Animator anim;
	
	public bool isPlayingCredits;
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void OnEnable(){
		Init();
	}
   public void Init()
    {
        Player.disabled = true;
		isPlayingCredits = true;
        SoundManager.instance.PlaySongNoLoop(8);
        CreditIndex = -2;
        MonIndex = 1;
        anim.SetTrigger("startCredits");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	void SetMonSprite(){
		if (MonIndex <= 0) {
			monimage.enabled = false;
		} else {
			monimage.enabled = true;
			monimage.sprite = mons [MonIndex];
		}
		MonIndex++;




	}
	void SetText(){
        if (CreditIndex == -2)
        {
            top.text = "";
            middle.text = "";
            low.text = "";
            bottom.text = "";

        }
		if (CreditIndex == -1) {
            top.text = "POKEMON RED UNITY";
            middle.text = "CREATED BY";
            low.text = "CELESTIALAMBER";
			bottom.text = "";

		}
        if (CreditIndex == 0)
        {
            top.text = "SPECIAL THANKS";
            middle.text = "MR.SQUISHY";
            low.text = "LEGENDOFLINQ";
            bottom.text = "MANY OTHERS";

        }
		if (CreditIndex == 1) {
            top.text = "ORIGINAL POKéMON";
				middle.text = "RED VERSION STAFF";
					low.text = "";
						bottom.text = "";

		}
		if (CreditIndex == 2) {
			top.text = "DIRECTOR";
			middle.text = "SATOSHI TAJIRI";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 3) {
			top.text = "PROGRAMMERS";
			middle.text = "TAKENORI OOTA";
			low.text = "SHIGEKI MORIMOTO";
			bottom.text = "";

		}
		if (CreditIndex == 4) {
			top.text = "PROGRAMMERS";
			middle.text = "TETSUYA WATANABE";
			low.text = "JUNICHI MASUDA";
			bottom.text = "SOUSUKE TAMADA";

		}
		if (CreditIndex == 5) {
			top.text = "CHARACTER DESIGN";
			middle.text = "KEN SUGIMORI";
			low.text = "ATSUKO NISHIDA";
			bottom.text = "";

		}
		if (CreditIndex == 6) {
			top.text = "MUSIC";
			middle.text = "JUNICHI MASUDA";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 7) {
			top.text = "SOUND EFFECTS";
			middle.text = "JUNICHI MASUDA";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 8) {
			top.text = "GAME DESIGN";
			middle.text = "SATOSHI TAJIRI";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 9) {
			top.text = "MONSTER DESIGN";
			middle.text = "KEN SUGIMORI";
			low.text = "ATSUKO NISHIDA";
			bottom.text = "MOTOFUMI FUZIWARA";

		}
		if (CreditIndex == 10) {
			top.text = "MONSTER DESIGN";
			middle.text = "SHIGEKI MORIMOTO";
			low.text = "SATOSHI OOTA";
			bottom.text = "RENA YOSHIKAWA";

		}
		if (CreditIndex == 11) {
			top.text = "GAME SCENARIO";
			middle.text = "SATOSHI TAJIRI";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 12) {
			top.text = "GAME SCENARIO";
			middle.text = "RYOHSUKE TANIGUCHI";
			low.text = "FUMIHIRO NONOMURA";
			bottom.text = "HIROYUKI ZINNAI";

		}
		if (CreditIndex == 13) {
			top.text = "PARAMETRIC DESIGN";
			middle.text = "KOHJI NISINO";
			low.text = "TAKEO NAKAMURA";
			bottom.text = "";

		}
		if (CreditIndex == 14) {
			top.text = "MAP DESIGN";
			middle.text = "SATOSHI TAJIRI";
			low.text = "KOHJI NISINO";
			bottom.text = "";

		}
		if (CreditIndex == 15) {
			top.text = "MAP DESIGN";
			middle.text = "KENJI MATSUSIMA";
			low.text = "FUMIHIRO NOMOMURA";
			bottom.text = "RYOHSUKE TANIGUCHI";

		}
		if (CreditIndex == 16) {
			top.text = "PRODUCT TESTING";
			middle.text = "AKIYOSHI KAKEI";
			low.text = "KAZUKI TSUCHIYA";
			bottom.text = "";

		}
		if (CreditIndex == 17) {
			top.text = "PRODUCT TESTING";
			middle.text = "TAKEO NAKAMURA";
			low.text = "MASAMITSU YUDA";
			bottom.text = "";

		}
		if (CreditIndex == 18) {
			top.text = "SPECIAL THANKS";
			middle.text = "TATSUYA HISHIDA";
			low.text = "YASUHIRO SAKAI";
			bottom.text = "";

		}
		if (CreditIndex == 19) {
			top.text = "SPECIAL THANKS";
			middle.text = "WATARU YAMAGUCHI";
			low.text = "KAZUYUKI YAMAMOTO";
			bottom.text = "";

		}
		if (CreditIndex == 20) {
			top.text = "SPECIAL THANKS";
			middle.text = "AKIHITO TOMISAWA";
			low.text = "HIROSHI KAWAMOTO";
			bottom.text = "TOMOMICHI OOTA";

		}
		if (CreditIndex == 21) {
			top.text = "PRODUCERS";
			middle.text = "SHIGERU MIYAMOTO";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 22) {
			top.text = "PRODUCERS";
			middle.text = "TAKASHI KAWAGUCHI";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 23) {
			top.text = "PRODUCERS";
			middle.text = "TSUNEKAZU ISHIHARA";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 24) {
			top.text = "US VERSION STAFF";
			middle.text = "";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 25) {
			top.text = "US COORDINATION";
			middle.text = "GAIL TILDEN";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 26) {
			top.text = "US COORDINATION";
			middle.text = "NAOKO KAWAKAMI";
			low.text = "HIRO NAKAMURA";
			bottom.text = "";

		}
		if (CreditIndex == 27) {
			top.text = "US COORDINATION";
			middle.text = "WILLIAM GIESE";
			low.text = "SARA OSBORNE";
			bottom.text = "";

		}
		if (CreditIndex == 28) {
			top.text = "TEXT TRANSLATION";
			middle.text = "NOB OGASAWARA";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 29) {
			top.text = "PROGRAMMERS";
			middle.text = "TERUKI MURAKAWA";
			low.text = "KOHTA FUKUI";
			bottom.text = "";

		}
		if (CreditIndex == 30) {
			top.text = "SPECIAL THANKS";
			middle.text = "SATORU IWATA";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 31) {
			top.text = "SPECIAL THANKS";
			middle.text = "TAKAHIRO HARADA";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 32) {
			top.text = "PRODUCT TESTING";
			middle.text = "PAAD TESTING";
			low.text = "NCL SUPER MARIO CLUB";
			bottom.text = "";

		}
		if (CreditIndex == 33) {
			top.text = "PRODUCER";
			middle.text = "TAKEHIRO IZUSHI";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 34) {
			top.text = "EXECUTIVE PRODUCER";
			middle.text = "HIROSHI YAMAUCHI";
			low.text = "";
			bottom.text = "";

		}
		if (CreditIndex == 35) {
			top.text = "";
			middle.text = "";
			low.text = "";
			bottom.text = "";

		}

		CreditIndex++;
	}
}
