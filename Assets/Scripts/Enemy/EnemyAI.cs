using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    //Variables
    public bool IsActive = true;
    private NavMeshAgent NavAgent;
    //AI states
    public enum AI_States { Patrol, Investigate, Hunt };
    public AI_States CurrentAI_State = AI_States.Patrol;

    //Behaviour
    public float Fov = 30f;

    //Player stuff
    [SerializeField]
    private Vector3 Target_pos;
    [SerializeField]
    private Transform PlayerTarget;
    [SerializeField]
    private float PlayerSpotted_Timer = 0f, CoolDown = 0f, ReactionTime = 0f;
    // Start is called before the first frame update
    void Start() {
        NavAgent = this.GetComponent<NavMeshAgent>();
        PlayerTarget = PlayerController.p.transform;
    }

    // Update is called once per frame
    void Update() {
        if(IsActive) {
            stateMachine();
        }
    }

    void stateMachine() {
        switch(CurrentAI_State) {
            case AI_States.Patrol:
                AI_Patrol();
                return;
            case AI_States.Investigate:
                AI_Investigate();
                return;
            case AI_States.Hunt:
                AI_Hunt();
                return;
        }
    }

    void AI_Patrol() {
        if(IsInFov()) {
            float d = Vector3.Distance(this.transform.position, Target_pos) / 10f;
            ReactionTime += Time.deltaTime;

            //React on player being in sight
            if (ReactionTime >= d) {
                PlayerSpotted_Timer = 3f;
                CurrentAI_State = AI_States.Hunt;
            }
        } else {
            ReactionTime = 0f;
        }
    }
    void AI_Investigate() {
        CurrentAI_State = AI_States.Patrol;
    }

    void AI_Hunt() {
        //Player spotted
        if(PlayerSpotted_Timer > 0) {
            Target_pos = PlayerTarget.position;
            PlayerSpotted_Timer -= Time.deltaTime;
            CoolDown = 3f;
        } else if(CoolDown > 0f && IsInFov()) {
            PlayerSpotted_Timer = 6f;
            CoolDown -= Time.deltaTime;
        } else {
            //Change state
            CurrentAI_State = AI_States.Investigate;
        }
        //Move
        Move();
    }

    void Move() {
        //Set destination
        NavAgent.destination = Target_pos;
    }

    public bool IsInFov() {
        Vector3 dir = PlayerTarget.position - this.transform.position;
        float angle = Vector3.Angle(dir, this.transform.forward);
        //check if in fov
        if (angle <= Fov) {
            RaycastHit FovHit;
            if (Physics.Raycast(this.transform.position, dir.normalized, out FovHit)) {
                if (FovHit.collider.name == PlayerController.p.name) {
                    //set target
                    Target_pos = PlayerTarget.position;
                    return true; //return true if object is detected
                }
            }
        }
        return false; //True is not returned, then return false
    }

}