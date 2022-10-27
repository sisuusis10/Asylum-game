using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightScript : MonoBehaviour {

    //Components
    private MeshRenderer _MeshRenderer;
    private Light _Light;
    public AudioClip FlashLightClick;

    //Variables
    public bool IsOn = false;
    public bool IsFlicker = false;
    private bool ResetFlicker = false;

    //Animation
    private Animator _Animator;

    //Battery
    private float BatteryPower;

    //Flicker Chance
    [Range(0f, 1f)]
    public float FlickerChance;
    public float FlickerTimerMax = 10f, FlickerCoolDown = 2.5f;
    private float FlickerChanceMax = 1f, FlickerChanceRandom, FlickerTimer;
    
    //Flicker Effect
    private float Flicker_Intensity;
    public float FlickerClamp = 1f;
    private float Flicker_Lerp = 0.3f;
    public int Flicker_TimerMax = 3;
    [SerializeField]
    private int Flicker_Timer;
    private float Intensity_Default;
    [SerializeField]
    private float Intensity_final;


    private void Start() {
        _Animator = this.GetComponent<Animator>();
        _MeshRenderer = this.GetComponent<MeshRenderer>();
        _Light = this.GetComponentInChildren<Light>();
        Intensity_Default = _Light.intensity;
        Intensity_final = Intensity_Default;
    }

    public void SwitchOnState() {
        IsOn = (IsOn) ? false : true;
        SoundSource.s.Play(FlashLightClick, 0.5f, 1, false);
    }


    public void AddRange(float NewRange) {
        _Light.range = NewRange;
    }

    private void FixedUpdate() {

        //Handle Animation
        _Animator.SetBool("IsOn", IsOn);
        if(_Animator.GetCurrentAnimatorStateInfo(0).IsName("FlashLight_IdleHidden")) {
            _MeshRenderer.enabled = false;
        } else {
            _MeshRenderer.enabled = true;
        }
        //Light
        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("FlashLight_Idle") || _Animator.GetCurrentAnimatorStateInfo(0).IsName("FlashLight_Appear")) {
            _Light.enabled = true;
        } else {
            _Light.enabled = false;
        }

        //Flicker Chance
        if (IsOn) {
            if (FlickerTimer < FlickerTimerMax) {
                FlickerTimer += Time.deltaTime;
            } else if (FlickerTimer >= FlickerTimerMax) {
                FlickerTimer = 0f;
                FlickerChanceRandom = Random.Range(0f, FlickerChanceMax);

                if(!IsFlicker && FlickerChanceRandom <= FlickerChance) {
                    IsFlicker = true;
                    FlickerTimer = FlickerTimerMax - FlickerCoolDown;
                } else if(IsFlicker) {
                    IsFlicker = false;
                    ResetFlicker = true;
                    Intensity_final = -2f;
                }
            }

            if(ResetFlicker) {
                Intensity_final = Mathf.MoveTowards(Intensity_final, Intensity_Default, Flicker_Lerp / 2f);
                if(Intensity_final == Intensity_Default) {
                    ResetFlicker = false;
                }
            }

            //FlickerEffect
            if (IsFlicker) {
                if (Flicker_Timer < Flicker_TimerMax) {
                    Flicker_Timer++;
                } else {
                    //Reset Flicker timer
                    Flicker_Timer = Random.Range(0, Flicker_TimerMax);
                    //Set New intensity
                    Flicker_Intensity = Random.Range(0f, FlickerClamp);
                }
                Intensity_final = Mathf.Lerp(_Light.intensity, Flicker_Intensity, Flicker_Lerp);
            }
            _Light.intensity = Intensity_final;
        }
    }

}
