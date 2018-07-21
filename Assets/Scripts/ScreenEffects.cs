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
    public Material paletteEffect, invertEffect, waveEffect;
    public RawImage screen;
    public RenderTexture texture, outputScreen;
    public bool invert, wave;
    public Palette usedPalette;
    private Palette lastPalette;
    public List<PaletteSet> palettes = new List<PaletteSet>();
    public VersionManager versionManager;
	// Use this for initialization
	void Start () {
        screen.texture = outputScreen;
        UpdatePalette();
	}
	
	// Update is called once per frame
    void Update () {
        if (usedPalette != lastPalette) UpdatePalette();
        Graphics.Blit(texture,outputScreen, paletteEffect,0);
        if (invert)
            Graphics.Blit(outputScreen, outputScreen, invertEffect);
        if(wave)
            Graphics.Blit(outputScreen, outputScreen, waveEffect);
        lastPalette = usedPalette;
	}
    void UpdatePalette(){
        paletteEffect.SetColor("color1", palettes[(int)usedPalette].bg1);
        paletteEffect.SetColor("color2", palettes[(int)usedPalette].bg2);
        paletteEffect.SetColor("color3", palettes[(int)usedPalette].bg3);
        paletteEffect.SetColor("color4", palettes[(int)usedPalette].bg4);
    }
}
