using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class MapCollider : MonoBehaviour
{
    
    public bool canUseBike = true; //can the player use the bike on this map?
    public Map mapArea;
}
