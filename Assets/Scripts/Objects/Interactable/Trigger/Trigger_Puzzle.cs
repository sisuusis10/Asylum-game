using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Puzzle : Trigger {

    //Variables
    
    //Triggers
    public Trigger[] Puzzle_Pieces;

    public override void ActivateTrigger() {
        //Check if Unlockable
        bool r = true;
        foreach (Trigger t in Puzzle_Pieces) {
            if(!t.IsActivated) {
                r = false;
                return;
            }
        }
        //Trigger active state
        if(r) {
            if(TriggerOnce) {
                RequiresPlayerInput = false;
                IsActivated = true;
            }
        }
        OnActive(r);
    }
    
    public virtual void OnActive(bool b) {

    }

}
