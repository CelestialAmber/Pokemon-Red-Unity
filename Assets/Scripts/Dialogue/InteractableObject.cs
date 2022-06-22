using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Interface for all interactable objects.
public interface InteractableObject {
    IEnumerator Interact();
}