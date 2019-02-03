using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Song{
    public AudioClip mainClip, loopClip;
   
}
public class SoundManager : MonoBehaviour
{
    public AudioSource sfx,music;
public Song[] songs;
/*
0:Gym Leader Battle
1:Trainer Battle
2:Wild Pokemon Battle
3:Casino
4:Celadon City
5:Cerulean City, Fuchsia City 
6:Cinnabar Island
7:Cycling
8:Ending
9:Evolution
10:Guide
11:Hall of Fame
12:Jigglypuff Song
13:Champion Rival Battle
14:Lavender Town
15:Mt. Moon
16:Nugget Bridge, Route 24/25
17:Oak's Lab
18:Ocean
19:Opening part 1
20:Opening part 2
21:Pallet Town
22:Pewter City, Viridian City, Saffron City
23:Pokemon Center
24:Gym
25:Pokemon Mansion
26:Pokemon Tower
27:Oak
28:Rival
29:St. Anne
30:Sylph Co.
31:Team Rocket Hideout
32:Victory Road
33:Route 4
34:Route 11
35:Route 1
36:Team Rocket Encounter
37:Boy Encounter
38:Girl Encounter
39:Vermillion City
40:Gym Leader Win
41:Trainer Win
42:Wild Pokemon Win
43:Viridian Forest
*/
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
void Awake(){
instance = this;
pokemonCrySounds = Resources.LoadAll<AudioClip>("Pokemon Cries");
}

void Update(){
if(isMusicPlaying && !music.isPlaying && songs[currentSong].loopClip != null && !music.loop){
music.Stop();
music.clip = songs[currentSong].loopClip;
music.loop = true;
music.Play();
}
if(isMusicPlaying && !music.isPlaying && !music.loop) isMusicPlaying = false;
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
    music.volume = maxMusicVolume;
    music.loop = false;
    currentSong = index;
    isMusicPlaying = true;
    music.clip = songs[currentSong].mainClip;
    if(songs[currentSong].loopClip == null){
        music.loop = true;
    }
    music.Play();
}
public void PlaySongNoLoop(int index){
    music.Stop();
    music.volume = maxMusicVolume;
    music.loop = false;
    currentSong = index;
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
yield return wait;
}
music.Stop();
music.volume = maxMusicVolume;
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
            yield return wait;
        }
        music.Stop();
        music.volume = maxMusicVolume;
        isFadingSong = false;
    }

    public void StopMusic(){
music.Stop();
isMusicPlaying = false;


}
public void PlayABSound(){
sfx.PlayOneShot(abSound,0.16f);
}

public void PlayCry(int index){
StartCoroutine(PlayCryCoroutine(index));
}
    public AudioClip[] itemGetSounds;
public IEnumerator PlayItemGetSound(int index)
    {
        music.Pause();
        sfx.PlayOneShot(itemGetSounds[index]);
        yield return new WaitForSeconds(itemGetSounds[index].length);
        music.UnPause();
    }
public IEnumerator PlayCryCoroutine(int index){
sfx.PlayOneShot(pokemonCrySounds[index]);
isPlayingCry = true; //Check for when cry is playing for functions that wait for the cry to end
yield return new WaitForSeconds(pokemonCrySounds[index].length);
isPlayingCry = false;
}

public void SetMusicLow(){
    music.volume = maxMusicVolume / 3f;
}
public void SetMusicNormal(){
    music.volume = maxMusicVolume;
}
public static int[] MapSongs = 
{
21,
17,//Oak's Lab
35,//Route 1
22,//ViridianCity
23, //Pokemon Center
23, //Pokemart
24, //Gym
33, //Route 22
32, //Route 23
15, //Victory Road
15, //-
15, //-
32, //Indigo Plateau
24, //Lorelei
31, //Bruno
26, //Agatha
11, //Hall of Fame
35, //Route 2
43, //Viridian Forest
43, //Diglett Cave
22, //Pewter City
33, //Route 3
15, //Mt Moon
15, //-
15, //-
33, //Route 4
5, //Cerulean City
16, //Route 24
16, //Route 25
33, //Route 5
35, //UndergroundRoad
33, //Route 6
39, //Vermillion City
29, //S.S. Anne
34, //Route 11
33, //Route 9
15, //RockTunnel
15, //-
43, //Power Plant
14, //Lavender Town
33, //Route 7
26, //Pokemon Tower
26, //-
26, //-
26, //-
26, //-
26, //-
26, //-
4, //Celadon City
3, //Game Corner
36, //Rocket Hideout
33, //Route 16
33,  //Route 17
33,//Route 18
5, //Fuchsia City
9, //Safari Zone
9, //-
9, //-
9, //-
9, //-
34, //Route 15
34, //Route 14
34, //Route 13
34, //Route 12
22, //Saffron City
30, //Silph Co.
33, //Route 19
43, //Seafoam Islands
43, //-
43, //-
43, //-
43, //-
33, //Route 20
6, //Cinnabar Island
25, //Mansion
25, //-
25, //-
25, //-
33, //Route21
43, //Unknown
43, //-
43, //-
4, //Trade Center
4, //Colloseum
4 //Bill's House
};
}

