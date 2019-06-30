using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IntroHandler : MonoBehaviour {
	public Animator anim;
	public GameObject middle;
    public RuntimeAnimatorController redController, blueController;
    public AudioClip hipSound, hopSound, crashSound, lungeSound, starSound, raiseSound, whooshSound;
	// Use this for initialization

	void Start () {
		Init();
	}
    public void ActivateTitle()
    {
        middle.SetActive(true);
        Title.instance.Init();
        this.gameObject.SetActive(false);
    }
    public void InitVersion()
    {
        switch (GameData.instance.version)
        {
            case Version.Red:
                anim.runtimeAnimatorController = redController;
                break;
            case Version.Blue:
                anim.runtimeAnimatorController = blueController;
                break;

        }

    }
	public void Init(){
        GameData.instance.atTitleScreen = true;
        Inputs.Disable("start");
        Player.disabled = true;
        Title.instance.animationsFinished = false;
		Player.instance.GetComponent<BoxCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ResetGame(){
	anim.SetTrigger("resetGame");
        Init();
	}
    public IEnumerator FadeInScreen()
    {
        WaitForSeconds wait = new WaitForSeconds(8f/60f); //8 frames
        for(int i = 1; i < 4; i++)
        {
            ScreenEffects.flashLevel = i;
            yield return wait;
        }
        ScreenEffects.flashLevel = 0;
    }
    public void PlayHipSound()
    {
        SoundManager.instance.sfx.PlayOneShot(hipSound);
    }
    public void PlayHopSound()
    {
        SoundManager.instance.sfx.PlayOneShot(hopSound);
    }
    public void PlayCrashSound()
    {
        SoundManager.instance.sfx.PlayOneShot(crashSound);
    }
    public void PlayLungeSound()
    {
        SoundManager.instance.sfx.PlayOneShot(lungeSound);
    }
    public void PlayStarSound()
    {
        SoundManager.instance.sfx.PlayOneShot(starSound);
    }
    public void PlayRaiseSound()
    {
        SoundManager.instance.sfx.PlayOneShot(raiseSound);
    }
    public void PlayIntroSong()
    {
        SoundManager.instance.PlaySongNoLoop(18);
    }
}

