using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePokemon : MonoBehaviour
{
    public Title title;
    public bool isMoving;
    public void SetSwitchingFalse()
    {
        title.switchingPokemon = false;
    }
    public void SetSwitchingTrue()
    {
        title.switchingPokemon = true;
    }
    public void SwitchMon()
    {
        title.SelectPokemon();
    }
}
