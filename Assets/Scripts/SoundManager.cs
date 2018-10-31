using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfx,music;
public Song[] songs;
public int currentSong;
public static SoundManager instance;
public bool isMusicPlaying;
void Awake(){
instance = this;
}

void Update(){
if(isMusicPlaying && !music.isPlaying && songs[currentSong].loopClip != null && !music.loop){
music.Stop();
music.clip = songs[currentSong].loopClip;
music.loop = true;
music.Play();
}
if(isMusicPlaying && !music.isPlaying && !music.loop) isMusicPlaying = false;
if(Input.GetKeyDown(KeyCode.A)) currentSong--;
if(Input.GetKeyDown(KeyCode.D)) currentSong++;
if(currentSong < 0) currentSong = songs.Length - 1;
if(currentSong == songs.Length) currentSong = 0;
if(Input.GetKeyDown(KeyCode.P)) PlaySong(currentSong);

}
void PlaySong(int index){
    music.Stop();
    music.loop = false;
    currentSong = index;
    isMusicPlaying = true;
    music.clip = songs[currentSong].mainClip;
    if(songs[currentSong].loopClip == null){
        music.loop = true;
    }
    music.Play();
}
void PlaySongNoLoop(int index){
    music.Stop();
    music.loop = false;
    currentSong = index;
    isMusicPlaying = true;
    music.clip = songs[currentSong].mainClip;
    music.Play();
}

void StopMusic(){
music.Stop();
isMusicPlaying = false;


}
}
[System.Serializable]
public class Song{
    public AudioClip mainClip, loopClip;
   
}
