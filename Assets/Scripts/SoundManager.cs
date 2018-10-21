using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfx,music;

public static SoundManager instance;

void Awake(){
instance = this;
}
    
}
