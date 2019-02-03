using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Palette{
    Grayscale,
    PokemonRedMinecraft,
    gbcUp,
    gbcUpA,
    gbcUpB,
    gbcLeft,
    gbcLeftA,
    gbcLeftB,
    gbcDown,
    gbcDownA,
    gbcDownB,
    gbcRight,
    gbcRightA,
    gbcRightB,
    GameboyColors

}

[System.Serializable]
public class PaletteSet{
    public Color bg1 = new Color(1,1,1,1);
    public Color bg2 = new Color(.564f,.564f,.564f,1);
    public Color bg3 = new Color(.25f, .25f, .25f, 1);
    public Color bg4 = new Color(0, 0, 0, 1);

}
public class ScreenEffects : MonoBehaviour {
    public Material paletteEffect, invertEffect, waveEffect, noEffect;
    public RawImage sgbScreen, nintendoSwitchScreen;
    private RenderTexture texture, outputScreen, pass1, pass2;
    public bool invert, wave;
    public Palette usedPalette;
    private Palette lastPalette;
    public List<PaletteSet> palettes = new List<PaletteSet>();
    public VersionManager versionManager;
    public static ScreenEffects instance;
    [Range(-3,3)]
    public static int flashLevel;
    public Vector2 screenPos;
	// Use this for initialization
    void Awake(){
        instance = this;
    }
	void Start () {
        texture = GameDataManager.instance.mainRender;
        outputScreen = GameDataManager.instance.postRender;
        pass1 = GameDataManager.instance.pass1;
        pass2 = GameDataManager.instance.pass2;
        sgbScreen.texture = outputScreen;
        nintendoSwitchScreen.texture = outputScreen;
	}
   
	// Update is called once per frame
    void Update () {
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
        paletteEffect.SetColor("color1", palettes[(int)usedPalette].bg1);
        paletteEffect.SetColor("color2", palettes[(int)usedPalette].bg2);
        paletteEffect.SetColor("color3", palettes[(int)usedPalette].bg3);
        paletteEffect.SetColor("color4", palettes[(int)usedPalette].bg4);
        paletteEffect.SetInt("flashLevel",flashLevel);
        Graphics.Blit(texture,pass1, paletteEffect,0);
        if (invert) Graphics.Blit(pass1, pass2, invertEffect, 0);
        else Graphics.Blit(pass1, pass2, noEffect, 0);
        if (wave) Graphics.Blit(pass2, outputScreen, waveEffect, 0);
        else Graphics.Blit(pass2, outputScreen, noEffect, 0);

        paletteEffect.SetVector("screenPos",screenPos);
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
