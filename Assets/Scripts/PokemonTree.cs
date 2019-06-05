using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PokemonTree : MonoBehaviour
{
    public Animator animator;
    public void Deactivate(){
        this.gameObject.SetActive(false);
    }
    public void Cut(){
     animator.SetTrigger("cutTree");

    }
}

