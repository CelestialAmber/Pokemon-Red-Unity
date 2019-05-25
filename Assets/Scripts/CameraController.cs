using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public GameObject player;
	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		transform.position = player.transform.position + new Vector3(0.5f,0,-2);
	}
}
