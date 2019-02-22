using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class MapCollider : MonoBehaviour
{
    public bool cantUseBike; //can the bike not be used in this map?
    public Map mapArea;
}
