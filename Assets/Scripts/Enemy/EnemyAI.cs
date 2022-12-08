using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    //Variables
    private NavMeshAgent NavAgent;
    //AI states
    public enum AI_States { Patrol, Investigate, Hunt };
    public AI_States CurrentAI_State = AI_States.Patrol;

    //
    private Vector3 Target_pos;
    private Transform PlayerTarget;
    // Start is called before the first frame update
    void Start() {
        NavAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        
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

    }
    void AI_Investigate() {

    }

    void AI_Hunt() {

    }

    void Mov() {
        //

    }

}
