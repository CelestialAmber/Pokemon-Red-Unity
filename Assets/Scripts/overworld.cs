using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script for managing overworld variables, such as current area, etc.
public class overworld : MonoBehaviour {


    public static bool IsBikeRidingAllowed()
    {
        // The bike can be used on Route 23 and Indigo Plateau,
        // or maps with tilesets in BikeRidingTilesets.
        // Return carry if biking is allowed.
        return true;
    }
}