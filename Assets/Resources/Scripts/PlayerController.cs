using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

    //Variables
    public static PlayerController p;

    //GameObjects and Scripts
    public Camera MainCam;
    private CharacterController C_Controller;

    //Lock Player
    public bool IsLocked;

    //Movement
    public Vector3 Movement, MovDir;
    public Vector2 PlayerVelocity;

    //MoveSpeed
    public float Move_Speed = 4f, MoveSpeed_Lerp;
    public bool IsRunning = false;
    public enum MoveState { Stopped, Walking, Running, Sneaking, Sneaking_Stopped }
    public MoveState Current_MoveState;

    //CameraRotation
    public Vector3 Rotation;
    public float Rot_Speed = 100f, X_Clamp = 80f;

    //Sneak mechanics
    public bool IsLeaning;
    private GameObject Leaning_RaycastObject;

    //FlashLight Variables
    public bool HasFlashLight;
    public FlashLightScript p_FlashLight;
    public int BatteryCount;

    //Camera Animator
    private Animator C_Animator;

    //Audio
    public AudioSource HeartSource;
    public AudioSource S_Source;
    public AudioClip WalkSound, InhalerSound, HurtSound;
    public float DefaultWalkVolume;

    //Volume
    public float Heart_Volume = 0f;

    //Grounded
    public bool KeepGrounded = true;
    public float GroundDistance_Margin = 0.2f;

    //Check Ground Material
    private RaycastHit GroundCheck;

    //Is player in a area Light?
    public bool IsInAreaLight;

    //Timer
    public float PlayTimer;
    public float PlayTimer_Max = 0.5f;
    
    //Player Health
    public int Health, Health_Max = 2;
    private float HealthRecovery_Timer, HealthRecovery_TimerMax = 4f;
    private DeathHandler.DeathFrom DeathType;

    //Attacked Effect
    private float AttackEffect_Timer;

    //Asthma
    public bool Asthma_Immunity;
    public bool Asthma_Attack;

    public float Asthma_Level = 0f;
    public float Asthma_Debuf, Asthma_DebufOverride;

    private int Asthma_Devider = 56;
    public float Asthma_subtractFade;

    //Screen Shake
    private float ShakeDuration, ShakeLerp, ShakeLerp_Fade;
    private Vector3 ShakeDirection, ShakeDir_Current, ShakeDir_Target, ShakeDir_Intensity;
    private bool Shake_Randomize;

    private void Awake() {
        //Prevent Duplicates
        if (FindObjectsOfType<PlayerController>().Length > 1) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this);
            p = this;
        }
        try {
            //Set Instances
            MainCam = Camera.main;
            p_FlashLight = this.GetComponentInChildren<FlashLightScript>();
            C_Controller = this.GetComponent<CharacterController>();
            Rotation = this.transform.rotation.eulerAngles;
            S_Source = this.GetComponent<AudioSource>();
            C_Animator = MainCam.GetComponent<Animator>();
        }
        catch { print("o-oh, big oopsie doopise. ran into some issues finding/setting instances");  }
        //Set Health to HealthMax
        Health = Health_Max;

        //Set walk volume
        DefaultWalkVolume = S_Source.volume;
    }


        private void Update() {
        if (!IsLocked) {
            P_Controller();
            FlashLight_Code();
        } else if(C_Animator) {
            C_Animator.SetBool("Walking", false); //Stop Bobbing
        }
    }

    private void P_Controller() {
        //Use Inhaler
        if (Input.GetKeyDown(ControlsHandler.get.InhalerKey) && !Asthma_Immunity && Asthma_Level >= 200f && UI_Inhalers.set.InhalerAmount > 0) {
            SoundSource.s.Play(InhalerSound, 0.2f, 20, true);
            Asthma_Immunity = true;
            UI_Inhalers.set.InhalerFunction(false, false);
        }


        //Animator
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) {
            PlayWalkSound(); //Play Walk sound effect
            C_Animator.SetBool("Walking", true); //View Bobbing
        } else {
            C_Animator.SetBool("Walking", false); //Stop View Bobbing
        }

        //cant run if asthma is over 100f
        if (Asthma_Level < 400f) {
            //Running state true
            IsRunning = Input.GetKey(ControlsHandler.get.SprintKey);
       } else {
            //Running State false
            IsRunning = false;
        }

        //Move Speed
        if(Current_MoveState == MoveState.Sneaking) {
            if (Asthma_Level < 500f) {
                Move_Speed = 2f;
            } else if (Asthma_Level > 500f) {
                Move_Speed = 2f / (Asthma_Level / 310f);
            } else if (Asthma_Level > 990f) {
                Move_Speed = 0.5f;
            }
        } else {
            switch (IsRunning) {
                case false:
                    if(Asthma_Level < 500f) {
                        Move_Speed = 4f;
                    } else if(Asthma_Level > 500f) {
                        Move_Speed = 4f / (Asthma_Level / 310f);
                    } else if (Asthma_Level > 990f) {
                        Move_Speed = 1f;
                    }
                    break;
                case true:
                    Move_Speed = 7f;
                    break;
            }
        }
        //Lerp MoveSpeed
        MoveSpeed_Lerp = Mathf.MoveTowards(MoveSpeed_Lerp, Move_Speed, 0.1f);

        //Movement Input
        Movement = new Vector3(Input.GetAxis("Horizontal"), -1f, Input.GetAxis("Vertical")) * MoveSpeed_Lerp * Time.deltaTime;
        Movement = transform.TransformDirection(Movement);
        //MovePlayer
        C_Controller.Move(Movement);

        //Rotation
        Rotation += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Rot_Speed * Time.deltaTime;
        Rotation.x = Mathf.Clamp(Rotation.x, -X_Clamp, X_Clamp);

        //Calculate XY movement
        PlayerVelocity = new Vector2(Movement.x, Movement.z);

        //GroundCheck
        if(KeepGrounded && Physics.Raycast(this.transform.position - new Vector3(0, this.transform.localScale.y - 0.1f, 0), -transform.up, out GroundCheck, Mathf.Infinity, Physics.AllLayers)) {
            if(GroundCheck.distance > this.transform.localScale.y + GroundDistance_Margin) {
                float _NewY = (this.transform.position.y - GroundCheck.distance) + this.transform.localScale.y;
                float _Applied_Y = Mathf.Lerp(this.transform.position.y, _NewY, 0.1f);
                this.transform.position = new Vector3(transform.position.x, _Applied_Y, transform.position.z);
            }
        }
        //Set Movement States
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) {
            switch(IsRunning) {
                case false:
                    Current_MoveState = MoveState.Walking;
                    break;
                case true:
                    Current_MoveState = MoveState.Running;
                    break;
            }
        } else {
            Current_MoveState = MoveState.Stopped;
        }

        //Stealth
        if(Input.GetKey(ControlsHandler.get.SneakKey)) {
            if(Current_MoveState == MoveState.Stopped) {
                Current_MoveState = MoveState.Sneaking_Stopped;
            } else {
                Current_MoveState = MoveState.Sneaking;
            }
            //update methodgroup
            Sneak();
        }
    }

    private void LateUpdate() {
        if(!IsLocked) {
            //Rotate Player
            this.transform.rotation = Quaternion.Euler(0, Rotation.y, 0);
            //Rotate Camera
            MainCam.transform.rotation = Quaternion.Euler(Rotation + ShakeDirection);
        }
    }

    private void FixedUpdate() {
        if(!IsLocked) {
            Asthma_GamePlay(); //Update Asthma GamePlay
            if (!Asthma_Immunity) {
                Asthma_Level += Asthma_Debuf;
                //Has Immunity
            } else if(Asthma_Immunity) {
                Asthma_Level = Mathf.Lerp(Asthma_Level, 0f, 0.05f);
                Asthma_Attack = false;
                if (Asthma_Level < 1f) { //Check if Asthma Level is ~0
                    Asthma_Immunity = false;
                }
            }
        }
    }

    private void Asthma_GamePlay() {
        //Clamp Asthma Level
        Asthma_Level = Mathf.Clamp(Asthma_Level, 0f, 1000f);

        //Player Health
        if(HealthRecovery_Timer > 0f) {
            HealthRecovery_Timer -= Time.deltaTime;
        } else {
            HealthRecovery_Timer = HealthRecovery_TimerMax;
            if(Health < Health_Max) {
                Health++;
            }
        }
        //Attacked Effects
        if(AttackEffect_Timer > 0f) {
            BlackOverlay.set.BloodOverlay(true, 0.7f, 0.04f, 0.01f, 0.5f);
            AttackEffect_Timer -= Time.deltaTime;
        }
        //Health States
        if (Health <= 0) { //Dead
            DeathHandler.set.Death(DeathType);
        }

        //Moved inhaler code to Update due to not being able to use on some frames
        
        //make sure the player isnt having a asthma attack

            //make sure asthma debuf isn't being overriden
            if (Asthma_DebufOverride == 0 && !Asthma_Attack) {
                //Movement Asthma Debuf
                if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) {
                    //Asthma debuf levels
                    if(Current_MoveState == MoveState.Sneaking) {
                    Asthma_Debuf = 0.01f;
                } else if (!IsRunning && Asthma_Level < 500f) {
                        Asthma_Debuf = 0.03f;
                    } else if (IsRunning) {
                        Asthma_Debuf = 0.1f;
                    } else if (Asthma_Level > 500f && Asthma_Level < 700f) {
                        Asthma_Debuf = 0.2f;
                    } else if (Asthma_Level > 700f) {
                        Asthma_Debuf = 0.3f;
                    }
                    //Not Moving
                } else if (Asthma_Level < 300f) {
                    Asthma_Debuf = -0.03f;
                } else if (Asthma_Level > 300f && Asthma_Level < 500f) {
                    Asthma_Debuf = 0.01f;
                } else if (Asthma_Level > 500f && Asthma_Level < 700f) {
                    Asthma_Debuf = 0.02f;
                } else if (Asthma_Level > 700f && Asthma_Level < 800f) {
                    Asthma_Debuf = 0.04f;
                } else if (Asthma_Level > 800f) {
                    Asthma_Debuf = 0.1f;
                }
            } else if (Asthma_Attack) { //Player is Experiencing a Asthma attack
                    if (Asthma_Level < 800f) {
                        Asthma_Level = Mathf.Lerp(Asthma_Level, 850f, 0.03f); //Lerp Asthma level to 600~
                    } else { //Asthma Attack Effects
                        Asthma_Debuf = 0.03f;
                    }
                    BlackOverlay.set.BloodOverlay(true, 0.1f, 0.01f, 0.01f, 0f); //Blood effect on screen
            } else { //Asthma debuf is being overriden
                Asthma_Debuf = Asthma_DebufOverride; //Apply Overriden Value
                Asthma_DebufOverride = 0f; //Reset Override
            }

        //HeartBeat sound effect
        float default_heartvolume = (Asthma_Level - 500f) / 600f;
        if (AttackEffect_Timer > 0f) {
            if(default_heartvolume < 0.5f) {
                Heart_Volume = Mathf.Lerp(Heart_Volume, 0.5f, 0.1f);
            } else {
                Heart_Volume = Mathf.Lerp(Heart_Volume, 1f, 0.1f);
            }
        } else {
            Heart_Volume = Mathf.Lerp(Heart_Volume, default_heartvolume, 0.03f);
        }

        //ScreenShake
        if (ShakeDuration > 0f) {
            //Lerp Values
            ShakeDir_Target = Vector3.MoveTowards(ShakeDir_Target, ShakeDir_Intensity, ShakeLerp_Fade);
            ScreenShake_NewDir(Shake_Randomize);
            //Decrease timer
            ShakeDuration -= Time.deltaTime;
        } else if(ShakeDir_Target != Vector3.zero) {
            ShakeDir_Target = Vector3.MoveTowards(ShakeDir_Target, Vector3.zero, ShakeLerp_Fade);
            ScreenShake_NewDir(Shake_Randomize);
        }
        //Apply
        ShakeDirection = Vector3.Lerp(ShakeDirection, ShakeDir_Current, ShakeLerp);

        //Visual Asthma Levels
        Asthma_subtractFade = Mathf.RoundToInt(Asthma_Level / Asthma_Devider);

        BlackOverlay.set.Black0_Alpha = (Asthma_Level - 800f) / 200f;

        //Audiotory Asthma effects
        SoundSource.s.Bass_source.volume = (Asthma_Level - 300f) / 5000f;

        //Set HeartBeat Volume
        HeartSource.volume = Heart_Volume;

        //Has player died from an asthma attack?
        if (Asthma_Level >= 1000f) {
            DeathHandler.set.Death(DeathHandler.DeathFrom.AsthmaAttack);
        }

    }

    //Override Asthma Debuf
    public void Override_DebufLevel(float NewDebuf) {
        Asthma_DebufOverride = NewDebuf;
    }

    //PlayerAttacked
    public void HealthDebuf(int damage, float asthma, DeathHandler.DeathFrom death_type) {
        SoundSource.s.Play(HurtSound, 0.2f, 10, true); //Play Sound effect

        Health -= damage; //Remove "a" amount of health
        Asthma_Level += asthma; //Give player asthma
        HealthRecovery_Timer = HealthRecovery_TimerMax; //Set Timer
        AttackEffect_Timer = HealthRecovery_TimerMax; //Set Effect Timer
        SetScreenShake(0.3f, new Vector3(0, 0f, 1f), 1f, 1f); //shake screen
        DeathType = death_type; //Death
    }

    //ScreenShake
    public void SetScreenShake(float duration, Vector3 intensity, float lerp, float lerpfade = 0.1f, bool randomize = true) {
        ShakeDuration = duration;
        ShakeDir_Intensity = intensity;
        ShakeLerp = lerp;
        ShakeLerp_Fade = lerpfade;
        Shake_Randomize = randomize;
    }
    //ScreenShake Direction Math
    private void ScreenShake_NewDir(bool randomize) {
        //New Dir
        if(randomize) {
        ShakeDir_Current = new Vector3(Random.Range(-ShakeDir_Target.x, ShakeDir_Target.x), Random.Range(-ShakeDir_Target.y, ShakeDir_Target.y), Random.Range(-ShakeDir_Target.z, ShakeDir_Target.z));
        } else {
            ShakeDir_Current = ShakeDir_Target;
        }
    }

    //FlashLight
    private void FlashLight_Code() {
        if(HasFlashLight && Input.GetKeyDown(ControlsHandler.get.FlashLightKey)) {
            p_FlashLight.SwitchOnState();
        }
    }

    public bool BatteryFunction(bool add) {
        if(!add && BatteryCount > 0 || add && BatteryCount < 6) {
            if (add) {
                BatteryCount++;
            } else {
                BatteryCount--;
            }
            return true;
        } else {
            return false;
        }
    }

    //Sneak Mechanics
    private void Sneak() {
        //Lean
        if(!IsLeaning) {
            RaycastHit leancheck_hit;
            //Raycast
            if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out leancheck_hit, 0.5f)) {
                //Set object
                Leaning_RaycastObject = leancheck_hit.collider.gameObject;
                //

            }
        }
    }

    //Play SFX
    public void PlayWalkSound() {
        //PlaySpeed
        if(Current_MoveState == MoveState.Sneaking) {
            //Animator PlayBack Speed
            C_Animator.speed = 0.4f;
            //Walk SFX Timer
            //PlayTimer
            PlayTimer_Max = 1f;
        } else //Isnt sneaking
        if (IsRunning) {
            //Animator PlayBack Speed
            C_Animator.speed = 1.4f;
            //Walk SFX Timer
            PlayTimer_Max = 0.4f;
        } else if(Asthma_Level < 500f) {
            //Animator PlayBack Speed
            C_Animator.speed = 1f;
            //Walk SFX Timer
            PlayTimer_Max = 0.5f;
        } else if (Asthma_Level > 500f && Asthma_Level < 700f) {
            //Animator PlayBack Speed
            C_Animator.speed = 0.8f;
            //Walk SFX Timer
            PlayTimer_Max = 0.8f;
        } else {
            //Animator PlayBack Speed
            C_Animator.speed = 0.5f;
            //Walk SFX Timer
            //PlayTimer
            PlayTimer_Max = 1f;
        }

        if(PlayTimer < PlayTimer_Max) {
            PlayTimer += Time.deltaTime;
        } else {
            //Check Material
            if (Physics.Raycast(this.transform.position, -transform.up, out GroundCheck, Mathf.Infinity, Physics.AllLayers)) {
                if (GroundCheck.collider.gameObject.GetComponent<AudioMaterial>()) {

                    //    print("Material:" + GroundCheck.collider.gameObject.GetComponent<AudioMaterial>().SoundType);

                    this.WalkSound = SoundSource.s.Get_StepSound(GroundCheck.collider.gameObject.GetComponent<AudioMaterial>().SoundType);
                }
            }
            PlayTimer = 0f; //Reset Timer
            try {
                //Set volume
                if(Current_MoveState == MoveState.Sneaking) {
                    S_Source.volume = DefaultWalkVolume / 2f;
                } else {
                    S_Source.volume = DefaultWalkVolume;
                }

                S_Source.clip = WalkSound;
                S_Source.pitch = 1f + Random.Range(-0.1f, 0.1f);
                S_Source.Play();
            } catch {
                print("No Audio Clip Found!");
            }
        }
    }
}
