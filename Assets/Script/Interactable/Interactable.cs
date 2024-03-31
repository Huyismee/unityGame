using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interacable : MonoBehaviour
{
    public bool useEvents;
    //message displayed when player look at
    [SerializeField]
    public String promptMessage;

    public void BaseInteract()
    {
        if(useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    // Start is called before the first frame update
    protected virtual void Interact()
    {

    }
}
