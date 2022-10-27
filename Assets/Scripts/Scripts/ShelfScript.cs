using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfScript : MonoBehaviour {
    //Variables
    private Trigger t;
    private AudioSource s;
    private TempLoader l;

    //State
    public bool IsOpen = false;

    public Vector3 DefaultPos, OpenPos, TargetPos, FinalPos;
    private float OpenMargin = 0.5f;
    private float LerpSpeed = 0.1f;

    //Saving
    private bool Once;

    //Audio
    public float AudioVolume = 0.5f;
    private float RandomPitch;
    public AudioClip Open, Close;

    private void Start() {
        t = this.GetComponent<Trigger>();
        s = this.GetComponent<AudioSource>();
        l = this.GetComponent<TempLoader>();
        s.volume = AudioVolume;

        //Make sure children dont load pos
        TempLoader[] ChildTemp = this.GetComponentsInChildren<TempLoader>();
        foreach(TempLoader tl in ChildTemp) {
            tl.AutoLoad_pos = false;
        }

        DefaultPos = this.transform.position;
        OpenPos = this.transform.position + (this.transform.forward * OpenMargin);
        FinalPos = DefaultPos;
        TargetPos = DefaultPos;
    }

    private void FixedUpdate() {
        if(Once == false) {
            IsOpen = l.Active;
            SetState(IsOpen);
            Once = true;
        }

        if (t.IsActive) {
            IsOpen = (IsOpen) ? false : true;
            t.IsActive = false;
            l.Active = IsOpen;
            SetState(IsOpen);

            //Set Random pitch
            RandomPitch = Random.Range(1f, 1.2f);
            s.pitch = RandomPitch;
            s.Play();
        }
        FinalPos = Vector3.Lerp(FinalPos, TargetPos, LerpSpeed);
        this.transform.position = FinalPos;
    }

    private void SetState(bool _isopen) {
        switch (_isopen) {
            case true:
                TargetPos = OpenPos;
                t.Use_Text = Trigger.Use_Types.Close;
                s.clip = Open;
                break;
            case false:
                TargetPos = DefaultPos;
                t.Use_Text = Trigger.Use_Types.Open;
                s.clip = Close;
                break;
        }

    }

}
