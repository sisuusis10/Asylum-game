using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Variables
    public bool IsLocked = true;

    private Camera Cam;
    private Rigidbody _rb;
    private CharacterController CharController;
    //Movement
    private Vector3 MovementVector;
    private float SpeedMultiplier = 10f, _speed;

    //Camera
    private Vector3 RotVector, FinalRotVector;
    private float RotationMultiplier = 100f;
    private float Rot_Clamp = 80f, RotLerp = 0.05f;
    // Start is called before the first frame update
    void Start() {
        //Set components
        Cam = Camera.main;
        _rb = this.GetComponent<Rigidbody>();
        CharController = this.GetComponent<CharacterController>();
        
        //Unlock
        IsLocked = false;
    }

    // Update is called once per frame
    public void Update() {
        if(!IsLocked) {
            Mov();
        }
    }

    public void Mov() {
        SpeedMultiplier = (!Input.GetKey(KeyCode.LeftShift)) ? 5f : 15f;
        _speed = Mathf.Lerp(_speed, SpeedMultiplier, 0.05f);
        //Movement
        MovementVector.x = Input.GetAxisRaw("Horizontal");
        MovementVector.z = Input.GetAxisRaw("Vertical");
        //Apply
        Vector3 dir = this.transform.rotation.normalized * MovementVector.normalized;
        CharController.Move((dir) * _speed * Time.deltaTime);
    
    }
    private void LateUpdate() {
        //Rotation
        RotVector.y += Input.GetAxis("Mouse X") * RotationMultiplier * Time.deltaTime;
        RotVector.x -= Input.GetAxis("Mouse Y") * RotationMultiplier * Time.deltaTime;

        //Clamp Rotation
        RotVector.x = Mathf.Clamp(RotVector.x, -Rot_Clamp, Rot_Clamp);
        //Apply
        FinalRotVector = Vector3.Lerp(FinalRotVector, RotVector, RotLerp);
        Cam.transform.rotation = Quaternion.Euler(RotVector.x, RotVector.y, RotVector.z);
        this.transform.rotation = Quaternion.Euler(0, RotVector.y, RotVector.z);
    }
}