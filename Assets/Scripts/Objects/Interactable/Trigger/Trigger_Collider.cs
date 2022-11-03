using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Collider : Trigger {

    //Variables
    public string Tag = "Player";
    private void OnTriggerEnter(Collider other) {
        if(other.tag == Tag)
            IsActivated = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == Tag)    
            IsActivated = false;
    }

}
