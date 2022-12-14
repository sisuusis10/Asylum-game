using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Trigger {

    //Variables
    public bool IsOpen;
    public bool Locked;
    public Animator[] Anim;
    private AudioSource source;
    public AudioClip OpenSfx, CloseSfx, LockedSfx;
    private Transform Player;
    public Trigger Key;
    
    private float AnimSpeed = 1f;

    //Messages
    public string Locked_String = "Locked.";


    // Start is called before the first frame update
    void Start() {
        source = this.GetComponent<AudioSource>();
        Player = PlayerController.p.transform;
    }

    public override void ActivateTrigger() {
        if(Key != null && Key.IsActivated) { Locked = false; }
        if(!Locked) {
            if(!IsOpen) {
                TriggerOpen();
            } else {
                TriggerClose();
            }
        } else {
            GameManagerScript.game.textbox.SetState(Locked_String);
        }
        base.ActivateTrigger();
    }

    public void TriggerOpen() {
        SetChildDoorStuff(true);
        source.clip = OpenSfx;
        source.Play();
    }

    public void TriggerClose() {
        SetChildDoorStuff(false);
        source.clip = CloseSfx;
        source.Play();
    }

    private bool CheckOpenDirection() {
        if (Vector3.Dot(this.transform.forward, Player.forward) > 0) {
            return true;
        }
        else {
            return false;
        }
    }

    private void SetChildDoorStuff(bool _IsOpen) {
        IsOpen = _IsOpen;
        bool default_dir = CheckOpenDirection();
        foreach (Animator a in Anim) {
            a.speed = AnimSpeed;
            a.SetBool("DefaultDirection", default_dir);
            a.SetBool("Open", _IsOpen);
            default_dir = !default_dir;
        }
    }

}
