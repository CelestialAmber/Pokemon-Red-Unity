using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WarpType{
WallWarp,
WalkOnWarp,
SabrinaWarp
}
[System.Serializable]
public class WarpInfo{
    public int warpposx, warpposy;
   public WarpType warpType;
   public bool forceMove; //Force the player to move if landing on doors
   public Direction wallDirection; //Direction the player has to hold for wall warps
}
public class TileWarp : MonoBehaviour {
    public WarpInfo info;

}
