using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    //Component
    public Trigger t_active, t_flicker;
    private Light Lamp_Light;
    private AudioSource a_source;
    //Does lamp flicker?
    public bool Flicker = false;
    //Flicker variables
    [SerializeField]
    private float Light_Intensity, Flicker_Intensity;
    public float FlickerClamp = 0.2f;
    private float Flicker_Lerp = 0.3f;
    public int Flicker_TimerMax = 20;
    [SerializeField]
    private int Flicker_Timer;

    private float Intensity_final;

    private void Awake() {
        Flicker_Timer = Flicker_TimerMax / 2;
        Lamp_Light = this.GetComponent<Light>();
        Light_Intensity = Lamp_Light.intensity;

        a_source = this.GetComponent<AudioSource>();
        if (Lamp_Light.enabled && a_source) {
            this.GetComponent<AudioSource>().Play();
        }
    }

    private void FixedUpdate() {
        if(t_active != null) {
            if(t_flicker == null) {
                OnState(t_active.IsActive, Flicker);
            } else {
                OnState(t_active.IsActive, t_flicker.IsActive);
            }
        }

        if(Flicker) {
            if (Flicker_Timer < Flicker_TimerMax) {
                Flicker_Timer++;
            } else {
                //Reset Flicker timer
                Flicker_Timer = Random.Range(0, Flicker_TimerMax);
                //Set New intensity
                Flicker_Intensity = Light_Intensity + Random.Range(-FlickerClamp, FlickerClamp);
            }
            Intensity_final = Mathf.Lerp(Lamp_Light.intensity, Flicker_Intensity, Flicker_Lerp);
            Lamp_Light.intensity = Intensity_final;
        } else {
            Lamp_Light.intensity = Light_Intensity;
        }

        if(a_source) {
            a_source.volume = Light_Intensity / 5f;
        }
    }

    public void OnState(bool IsOn, bool DoesFlicker) {
        Lamp_Light.enabled = IsOn;
        Flicker = DoesFlicker;
    }

}
