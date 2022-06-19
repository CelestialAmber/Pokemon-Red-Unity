using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenEffects : MonoBehaviour {
    public Material paletteEffect, invertEffect, waveEffect, ssAnneScrollEffect;
    public RawImage sgbScreen;
    public RenderTexture texture, outputScreen;
    public bool invert, wave;
    public bool screenEffectDebug = false;
    public VersionManager versionManager;
    public static ScreenEffects instance;
    [Range(-3,3)]
    public static int flashLevel;
    public Vector2 screenPos;
    public int shipScrollOffset;
    private RenderTexture[] passTex = new RenderTexture[3]; //RenderTextures for blitting

    // Use this for initialization
    void Awake(){
        instance = this;
    }
	void Start () {
        texture = GameDataManager.instance.mainRender;
        outputScreen = GameDataManager.instance.postRender;
        for(int i = 0; i < passTex.Length; i++) //create duplicates for pass textures
        {
            passTex[i] = new RenderTexture(GameDataManager.instance.templateRenderTexture);
        }
        sgbScreen.texture = outputScreen;
	}
   
	// Update is called once per frame
    void Update () {
        if(screenEffectDebug){
        if(Input.GetKeyDown(KeyCode.A)){
            StartCoroutine(SlowLongShakeHorizontal());
        }
        if(Input.GetKeyDown(KeyCode.S)){
            StartCoroutine(SlowShortShakeHorizontal());
        }
        if(Input.GetKeyDown(KeyCode.D)){
            StartCoroutine(FastHeavyShakeHorizontal());
        }
        if(Input.GetKeyDown(KeyCode.F)){
            StartCoroutine(FastLightShakeHorizontal());
        }
        if(Input.GetKeyDown(KeyCode.G)){
            StartCoroutine(ShakeVertical());
        }
        if(Input.GetKeyDown(KeyCode.H)){
            StartCoroutine(ScrollShip());
        }
        }

        paletteEffect.SetInt("flashLevel",flashLevel);
        ssAnneScrollEffect.SetFloat("shipScrollOffset", shipScrollOffset);
        Graphics.Blit(texture,passTex[0], paletteEffect,0);

        int passIndex = 0;

        if (invert){
             Graphics.Blit(passTex[passIndex], passTex[passIndex + 1], invertEffect, 0);
            passIndex++;
        }

        if (wave){
        	Graphics.Blit(passTex[passIndex], passTex[passIndex + 1], waveEffect, 0);
        	passIndex++;
        }

        Graphics.Blit(passTex[passIndex], outputScreen, ssAnneScrollEffect, 0);
        paletteEffect.SetVector("screenPos",screenPos);
	}
    public IEnumerator ScrollShip() //Function to scroll the S.S. Anne Ship off screen.
    {
        WaitForSeconds wait = new WaitForSeconds(8f/60f); //the ship moves every 8 frames
        for(int i = 0; i < 144; i++) //Scroll from 0 to 144 pixels until the ship is offscreen.
        {
            shipScrollOffset = i + 1;
            yield return wait;
        }
        shipScrollOffset = 0;
    }
    public IEnumerator SlowShortShakeHorizontal()
    {

        WaitForSeconds wait = new WaitForSeconds(0.032f);
        for (int j = 0; j < 4; j++)
        {
            screenPos.x += 0.00625f;
            yield return wait;
        }
        for (int j = 0; j < 8; j++)
        {
            screenPos.x -= 0.00625f;
            yield return wait;
        }
        for (int j = 0; j < 4; j++)
        {
            screenPos.x += 0.00625f;
            yield return wait;
        }
        screenPos.x = 0;

        yield return 0;
    }
    public IEnumerator SlowLongShakeHorizontal()
    {

        WaitForSeconds wait = new WaitForSeconds(0.032f);
        for (int j = 0; j < 6; j++)
        {
            screenPos.x -= 0.00625f;
            yield return wait;
        }
        for (int j = 0; j < 6; j++)
        {
            screenPos.x += 0.00625f;
            yield return wait;
        }
        for (int j = 0; j < 6; j++)
        {
            screenPos.x -= 0.00625f;
            yield return wait;
        }
        for (int j = 0; j < 6; j++)
        {
            screenPos.x += 0.00625f;
            yield return wait;
        }
        screenPos.x = 0;

        yield return 0;
    }
    public IEnumerator FastLightShakeHorizontal()
    {
        WaitForSeconds wait = new WaitForSeconds(0.032f);
        WaitForSeconds wait1 = new WaitForSeconds(0.256f);
        screenPos.x += 0.0125f;
        yield return wait;
        screenPos.x -= 0.0125f;
        yield return wait1;
        screenPos.x += 0.00625f;
        yield return wait;
        screenPos.x -= 0.00625f;



    }
    public IEnumerator FastHeavyShakeHorizontal()
    {
        float multiplier = 7f;
        for (int i = 0; i < 7; i++)
        {
            screenPos.x -= 0.00625f * multiplier;
            yield return new WaitForSeconds(0.032f);
            screenPos.x += 0.00625f * multiplier;
            yield return new WaitForSeconds(0.032f * 5f);
            multiplier--;
        }

    }
    public IEnumerator ShakeVertical()
    {
        float multiplier = 7f;
        for (int i = 0; i < 7; i++)
        {
            screenPos.y -= 0.00625f * multiplier;
            yield return new WaitForSeconds(0.032f);
            screenPos.y += 0.00625f * multiplier;
            yield return new WaitForSeconds(0.032f * 3f);
            multiplier--;
        }
    }


}
