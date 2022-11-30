using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Variables
    public static PlayerController p;
    public bool IsLocked = true;

    //Components
    private Camera Cam;
    private Animator Anim;
    private Rigidbody _rb;
    private CharacterController CharController;
    private AudioSource _Audio;
    //Movement
    private Vector3 MovementVector;
    public float WalkSpeed = 5f, RunSpeed = 10f, SneakSpeed = 3f;
    private float SpeedMultiplier = 10f, _speed;
    public bool IsRunning = false;

    //Camera
    private Vector3 RotVector, FinalRotVector, ShakeDirection;
    private float RotationMultiplier = 100f;
    private float Rot_Clamp = 80f, RotLerp = 0.3f;
    // Start is called before the first frame update
    void Start() {
        //Set components
        p = this;
        Cam = Camera.main;
        _rb = this.GetComponent<Rigidbody>();
        CharController = this.GetComponent<CharacterController>();
        Anim = this.GetComponent<Animator>();
        _Audio = this.GetComponent<AudioSource>();
        //Unlock
        IsLocked = false;
    }

    // Update is called once per frame
    public void Update() {
        if(!IsLocked) {
            Mov();
            WalkAnimSfx();
        }
    }

    public void Mov() {
        IsRunning = (!Input.GetKey(KeyCode.LeftShift)) ? false : true;
        SpeedMultiplier = (!IsRunning) ? WalkSpeed : RunSpeed;

        _speed = Mathf.Lerp(_speed, SpeedMultiplier, 0.05f);
        //Movement
        MovementVector.x = Input.GetAxisRaw("Horizontal");
        MovementVector.z = Input.GetAxisRaw("Vertical");

        //Rotation
        RotVector.y += Input.GetAxis("Mouse X") * RotationMultiplier * Time.deltaTime;
        RotVector.x -= Input.GetAxis("Mouse Y") * RotationMultiplier * Time.deltaTime;

        //Clamp Rotation
        RotVector.x = Mathf.Clamp(RotVector.x, -Rot_Clamp, Rot_Clamp);

        //Apply
        Vector3 dir = this.transform.TransformDirection(MovementVector.normalized);
        CharController.Move((dir) * _speed * Time.deltaTime);
        
    }
    private void LateUpdate() {
        if(!IsLocked) {
            //Apply
            FinalRotVector = Vector3.Lerp(FinalRotVector, RotVector, RotLerp);
            //Rotate Player
            this.transform.rotation = Quaternion.Euler(0, FinalRotVector.y, 0);
            //Rotate Camera
            Cam.transform.rotation = Quaternion.Euler(FinalRotVector + ShakeDirection);
        }
    }


    //Walk SFX & anim variables
    private float timer, timer_max;
    private AudioClip stepsfx;
    public void WalkAnimSfx() {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(this.transform.position, Vector3.down, out hit);
        //Fix bug where player can float in air.
        if (hit.distance > 1f) {
            this.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, transform.position.y - (hit.distance - 1f), 0.8f), transform.position.z);
        }

        if (MovementVector != Vector3.zero) {
            timer += Time.deltaTime;
            Anim.SetBool("IsWalking", true);
            if (!IsRunning) {
                timer_max = 0.5f;
            }
            else {
                timer_max = 0.4f;
            }

            if(timer >= timer_max) {
                timer = 0;
                if(hit.collider != null) {
                    SoundMaterial s = hit.collider.gameObject.GetComponent<SoundMaterial>();
                    if (s != null) {
                        _Audio.clip = SoundSource.s.Get_StepSound(s.Material);
                    }

                    _Audio.pitch = Random.Range(0.9f, 1.1f);

                    _Audio.Play();
                }
            }
        }
        else {
            Anim.SetBool("IsWalking", false);
        }

    }

}
