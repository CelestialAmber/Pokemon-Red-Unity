using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class CreditsHandler : MonoBehaviour {
	public CustomText top, middle, low, bottom;
	public int CreditIndex, MonIndex;
	public Image monimage;
	public Sprite[] mons = new Sprite[16];
    public Animator anim;
	string[,] creditsText;


    void OnEnable(){
		Init();
	}

	public void Init(){
		TextAsset creditsTextAsset = (TextAsset)Resources.Load("Text/creditsText");
		creditsText = JsonConvert.DeserializeObject<string[,]>(creditsTextAsset.text);
        Player.instance.isDisabled = true;
		GameData.instance.isPlayingCredits = true;
        SoundManager.instance.PlaySongNoLoop(Music.Ending);
        CreditIndex = -1;
        MonIndex = 1;
        anim.SetTrigger("startCredits");
    }
	
	// Update is called once per frame
	void Update(){	
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
        if (CreditIndex < 0 || CreditIndex >= creditsText.Length){
            top.text = "";
            middle.text = "";
            low.text = "";
            bottom.text = "";
        }else{
			top.text = creditsText[CreditIndex,0];
            middle.text = creditsText[CreditIndex,1];
            low.text = creditsText[CreditIndex,2];
            bottom.text = creditsText[CreditIndex,3];
		}

		CreditIndex++;
	}
}
