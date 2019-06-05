using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WarpType{
WallWarp,
WalkOnWarp,
SabrinaWarp
}

[RequireComponent(typeof(BoxCollider2D))]
public class TileWarp : MonoBehaviour {
   public int warpPosX, warpPosY;
   public WarpType warpType;
   public bool forceMove; //Force the player to move if landing on doors
   public Direction wallDirection; //Direction the player has to hold for wall warps

}
