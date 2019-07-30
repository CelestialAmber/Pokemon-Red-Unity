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
    public Material paletteEffect, invertEffect, waveEffect, ssAnneScrollEffect;
    public RawImage sgbScreen;
    public RenderTexture texture, outputScreen;
    
    public bool invert, wave;
    public Palette usedPalette;
    private Palette lastPalette;
    public List<PaletteSet> palettes = new List<PaletteSet>();
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
    public IEnumerator ScrollShip() //function to scroll the S.S. Anne Ship off screen.
    {
        WaitForSeconds wait = new WaitForSeconds(8f/160f); //the ship moves every 8 frames
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
