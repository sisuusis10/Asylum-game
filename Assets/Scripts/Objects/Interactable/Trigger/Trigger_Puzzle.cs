using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Puzzle : Trigger {

    //Variables
    public bool Locked = true;

    //Triggers
    public Trigger[] Puzzle_Triggers;

    public override void ActivateTrigger() {
        //Check if Unlockable
        bool r = true;
        foreach (Trigger t in Puzzle_Triggers) {
            if(!t.IsActivated) {
                r = false;
                break;
            }
        }
        Locked = r;

        base.ActivateTrigger();
    }

}
