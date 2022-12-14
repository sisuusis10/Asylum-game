using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    //Variables
    public bool IsActive = true;
    private NavMeshAgent NavAgent;
    //AI states
    public enum AI_States { Patrol, Investigate, Hunt, Attack };
    public AI_States CurrentAI_State = AI_States.Patrol, PreviousAI_State;

    //Behaviour
    public float Fov = 30f;

    //Player stuff
    [SerializeField]
    private Vector3 Target_pos;
    [SerializeField]
    private Transform PlayerTarget;
    [SerializeField]
    private float PlayerSpotted_Timer = 0f, CoolDown = 0f, ReactionTime = 0f, AttackTimer = 0f;
    public int DealDamage = 4;

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
        AudioLevel();
        if(AttackTimer > 0f) { AttackTimer -= Time.deltaTime; }
        switch (CurrentAI_State) {
            case AI_States.Patrol:
                AI_Patrol();
                return;
            case AI_States.Investigate:
                AI_Investigate();
                return;
            case AI_States.Hunt:
                AI_Hunt();
                return;
            case AI_States.Attack:
                AI_Attack();
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
                SetAIstate(AI_States.Hunt);
                return;
            }
        } else {
            ReactionTime = 0f;
        }

        //Random path
        if (CoolDown <= 0f || Target_pos.x == Mathf.Infinity) {
            CoolDown = Random.Range(0f, 5f);
            Target_pos = RandomPath();
        } else {
            float d = Vector3.Distance(transform.position, new Vector3(Target_pos.x, transform.position.y, Target_pos.z));
            print(d);
            if (d < 1f) {
                CoolDown -= Time.deltaTime;
            }
        }
        Move();
    }
    void AI_Investigate() {
        SetAIstate(AI_States.Patrol);
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
            SetAIstate(AI_States.Investigate);
            return;
        }
        //Attack
        float d = Vector3.Distance(this.transform.position, Target_pos);
        if (d < 1f) {
            SetAIstate(AI_States.Attack);
        }
        //Move
        Move();
    }

    public void AI_Attack() {
        if(AttackTimer <= 0f) {
            AttackTimer = 1.5f;
            PlayerController.p.Damage(DealDamage, this.transform.forward);
        } else {
            SetAIstate(PreviousAI_State);
        }
    }

    public void SetAIstate(AI_States _state) {
        if(CurrentAI_State != _state) {
            PreviousAI_State = CurrentAI_State;
            CurrentAI_State = _state;
        }
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

    //Audio level Variables
    public float AudioLevel_MaxDistance = 30f;
    public void AudioLevel() {
        float d = Vector3.Distance(PlayerTarget.position, transform.position);
        if (PlayerSpotted_Timer > 0f && d < AudioLevel_MaxDistance) {
            float a = Mathf.Tan(Mathf.Abs(d / AudioLevel_MaxDistance));
            SoundSource.s.SetLevels(a, false);
        }
        else {
            SoundSource.s.SetLevels(1f, false);
        }
        if (d < 25) {
            float r = (d < 20 && PlayerRaycast.uh.IsInFov(this.transform)) ? 1f : 0f;
            PostprocessingEffects.effects.SetChromaticIntensity(r);
        }
    }

    public Vector3 RandomPath() {
        float rand_d = Random.Range(5f, 20f);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * rand_d;

        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, rand_d, 1);

        return navHit.position;
    }

}
