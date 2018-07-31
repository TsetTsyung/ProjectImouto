using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObjectBaseClass : MonoBehaviour{

    protected bool allowedToDisplay;

    public abstract void DisplayText();

    public abstract void HideText();
    
    public abstract void Interact();

    public abstract void DisableText();
}
