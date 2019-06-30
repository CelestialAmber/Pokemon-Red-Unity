using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Song{
    public AudioClip mainClip, loopClip;
}


public class SoundManager : MonoBehaviour
{
    public enum Music{
    GymLeaderBattle,
    TrainerBattle,
    WildPokemonBattle,
    Casino,
    CeladonCity,
    CeruleanCity,
    CinnabarIsland,
    Cycling,
    Ending,
    Evolution,
    Guide,
    HallOfFame,
    Pokeflute,
    LavenderTown,
    MtMoon,
    NuggetBridge,
    OaksLab,
    Ocean,
    Opening1,
    Opening2,
    PalletTown,
    PewterCity,
    PokemonCenter,
    PokemonGym,
    Mansion,
    PokemonTower,
    Oak,
    Rival,
    ChampionBattle,
    Routes1,
    Routes2,
    Routes3,
    SilphCo,
    SSAnne,
    RocketHideout,
    VictoryRoad,
    TrainerBoy,
    TrainerGirl,
    TrainerTeamRocket,
    VermillionCity,
    VictoryGymLeader,
    VictoryTrainer,
    VictoryWildPokemon,
    ViridianForest


}
    public AudioSource sfx,music,musicLoop;
public Song[] songs;

public int currentSong;
public static SoundManager instance;
public bool isMusicPlaying;
public AudioClip[] pokemonCrySounds;
[Range(0,1)]
public float maxMusicVolume;
public bool isFadingSong;
public int switchIndex;
public AudioClip abSound;
public bool isPlayingCry;

    public AudioClip[] itemGetSounds;
    public AudioClip goInsideSound, goOutsideSound;

    void Awake(){
instance = this;
pokemonCrySounds = Resources.LoadAll<AudioClip>("Pokemon Cries");
}
public int debugSongIndex;
void Update(){
    if(Input.GetKeyDown(KeyCode.R)) PlaySong(debugSongIndex);
// if(isMusicPlaying && !music.isPlaying && songs[currentSong].loopClip != null && !music.loop){
// music.clip = songs[currentSong].loopClip;
// music.loop = true;
// music.Play();
// }
if(isMusicPlaying && !music.isPlaying && !musicLoop.isPlaying && !music.loop) isMusicPlaying = false;
if(Player.instance.inBattle){
if(isFadingSong){
    StopCoroutine("FadeToSongFunction");
    isFadingSong = false;
}

} 

}


public void StopFadeSong(){
    StopCoroutine("SwitchSongFade");
}
public void PlaySong(int index){
    StopAllCoroutines();
    music.Stop();
    musicLoop.Stop();
    music.volume = maxMusicVolume;
    musicLoop.volume = maxMusicVolume;
    currentSong = index;
    isMusicPlaying = true;
    music.loop = false;

    
        music.clip = songs[currentSong].mainClip;
        music.Play();
    
    if(songs[currentSong].loopClip == null) music.loop = true;
    else{
         musicLoop.clip = songs[currentSong].loopClip;
         float clipLength = (float)songs[currentSong].mainClip.samples/songs[currentSong].mainClip.frequency;
         musicLoop.PlayDelayed(clipLength);
    }
    
}

public void PlaySongNoLoop(int index){
    music.Stop();
    music.volume = maxMusicVolume;
    musicLoop.volume = music.volume;
    currentSong = index;
    music.loop = false;
    isMusicPlaying = true;
    music.clip = songs[currentSong].mainClip;
    music.Play();
}
public void FadeToSong(int index){
switchIndex = index;
StartCoroutine("FadeToSongFunction");
}
    public void FadeSong()
    {
        StartCoroutine("FadeSongFunction");
    }

    public IEnumerator FadeToSongFunction(){
    currentSong = switchIndex;
    if(isFadingSong) yield return 0;
    isFadingSong = true;
float fadeTimer = maxMusicVolume - music.volume;
WaitForSeconds wait = new WaitForSeconds(0.02f);
while(fadeTimer <= 1){ 
fadeTimer += Time.deltaTime;
music.volume = Mathf.Lerp(maxMusicVolume,0,fadeTimer);
musicLoop.volume = music.volume;
yield return wait;
}
music.Stop();
music.volume = maxMusicVolume;
musicLoop.volume = maxMusicVolume;
if(Player.instance.inBattle) yield return 0;
PlaySong(switchIndex);
isFadingSong = false;
}
    public IEnumerator FadeSongFunction()
    {
        if (isFadingSong) yield return 0;
        isFadingSong = true;
        float fadeTimer = maxMusicVolume - music.volume;
        WaitForSeconds wait = new WaitForSeconds(0.02f);
        while (fadeTimer <= 1)
        {
            fadeTimer += Time.deltaTime;
            music.volume = Mathf.Lerp(maxMusicVolume, 0, fadeTimer);
            musicLoop.volume = music.volume;
            yield return wait;
        }
        music.Stop();
        music.volume = maxMusicVolume;
        musicLoop.volume = maxMusicVolume;
        isFadingSong = false;
    }

    public void StopMusic(){
music.Stop();
musicLoop.Stop();
isMusicPlaying = false;


}
public void PlayABSound(){
sfx.PlayOneShot(abSound);
}
    public void PlayGoInsideSound()
    {
        sfx.PlayOneShot(goInsideSound);
    }
    public void PlayGoOutsideSound()
    {
        sfx.PlayOneShot(goOutsideSound);
    }

public void PlayCry(int index){
StartCoroutine(PlayCryCoroutine(index));
}

public IEnumerator PlayItemGetSound(int index)
    {
        music.Pause();
        musicLoop.Pause();
        sfx.PlayOneShot(itemGetSounds[index]);
        yield return new WaitForSeconds(itemGetSounds[index].length);
        music.UnPause();
        musicLoop.UnPause();
    }
public IEnumerator PlayCryCoroutine(int index){
sfx.PlayOneShot(pokemonCrySounds[index]);
isPlayingCry = true; //Check for when cry is playing for functions that wait for the cry to end
yield return new WaitForSeconds(pokemonCrySounds[index].length);
isPlayingCry = false;
}

public void SetMusicLow(){
    music.volume = maxMusicVolume / 3f;
    musicLoop.volume = music.volume;
}
public void SetMusicNormal(){
    music.volume = maxMusicVolume;
    musicLoop.volume = music.volume;
}
public static Music[] MapSongs = 
{
Music.PalletTown, //Pallet Town
Music.OaksLab,//Oak's Lab
Music.Routes3,//Route 1
Music.PewterCity,//ViridianCity 
Music.PokemonCenter, //Pokemon Center
Music.PokemonCenter, //Pokemart
Music.PokemonGym, //Gym
Music.Routes1, //Route 22
Music.VictoryRoad, //Route 23
Music.MtMoon, //Victory Road
Music.MtMoon, //-
Music.MtMoon, //-
Music.VictoryRoad, //Indigo Plateau
Music.PokemonGym, //Lorelei
Music.RocketHideout, //Bruno
Music.PokemonTower, //Agatha
Music.HallOfFame, //Hall of Fame
Music.Routes3, //Route 2
Music.ViridianForest, //Viridian Forest
Music.ViridianForest, //Diglett Cave
Music.PewterCity, //Pewter City
Music.Routes1, //Route 3
Music.MtMoon, //Mt Moon
Music.MtMoon, //-
Music.MtMoon, //-
Music.Routes1, //Route 4
Music.CeruleanCity, //Cerulean City
Music.NuggetBridge, //Route 24
Music.NuggetBridge, //Route 25
Music.Routes1, //Route 5
Music.Routes3, //UndergroundRoad
Music.Routes1, //Route 6
Music.VermillionCity, //Vermillion City
Music.SSAnne, //S.S. Anne
Music.Routes2, //Route 11
Music.Routes1, //Route 9
Music.Routes1, //Route 10
Music.MtMoon, //RockTunnel
Music.MtMoon, //-
Music.ViridianForest, //Power Plant
Music.LavenderTown, //Lavender Town
Music.PokemonTower, //Pokemon Tower
Music.PokemonTower, //-
Music.PokemonTower, //-
Music.PokemonTower, //-
Music.PokemonTower, //-
Music.PokemonTower, //-
Music.PokemonTower, //-
Music.Routes1, //Route 8
Music.Routes1, //Route 7
Music.CeladonCity, //Celadon City
Music.Casino, //Game Corner
Music.TrainerTeamRocket, //Rocket Hideout
Music.Routes1, //Route 16
Music.Routes1,  //Route 17
Music.Routes1,//Route 18
Music.CeruleanCity, //Fuchsia City
Music.Evolution, //Safari Zone
Music.Evolution, //-
Music.Evolution, //-
Music.Evolution, //-
Music.Evolution, //-
Music.Routes2, //Route 15
Music.Routes2, //Route 14
Music.Routes2, //Route 13
Music.Routes2, //Route 12
Music.PewterCity, //Saffron City
Music.SilphCo, //Silph Co.
Music.Routes1, //Route 19
Music.ViridianForest, //Seafoam Islands
Music.ViridianForest, //-
Music.ViridianForest, //-
Music.ViridianForest, //-
Music.ViridianForest, //-
Music.Routes1, //Route 20
Music.CinnabarIsland, //Cinnabar Island
Music.Mansion, //Mansion
Music.Mansion, //-
Music.Mansion, //-
Music.Mansion, //-
Music.Routes1, //Route21
Music.ViridianForest, //Unknown
Music.ViridianForest, //-
Music.ViridianForest, //-
Music.CeladonCity, //Trade Center
Music.CeladonCity, //Colloseum
Music.CeladonCity, //Bill's House
Music.CeladonCity, //Houses
Music.ViridianForest, //Victory Road Gate
Music.VictoryRoad //Indigo Plateau Lobby
};
}

