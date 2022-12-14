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
    private float PlayerSpotted_Timer = 0f, CoolDown = 0f, ReactionTime = 0f, AttackTimer = 0f, Step_timer = 0f, Step_timerMax;
    public int DealDamage = 4;
    private AudioSource _Audio;

    // Start is called before the first frame update
    void Start() {
        NavAgent = this.GetComponent<NavMeshAgent>();
        PlayerTarget = PlayerController.p.transform;
        _Audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if(IsActive) {
            stateMachine();
        }
    }

    void stateMachine() {
        if(AttackTimer > 0f) { AttackTimer -= Time.deltaTime; }
        switch (CurrentAI_State) {
            case AI_States.Patrol:
                AI_Patrol();
                return;
            case AI_States.Investigate:
                AI_Investigate();
                AudioLevel();
                return;
            case AI_States.Hunt:
                AI_Hunt();
                AudioLevel();
                return;
            case AI_States.Attack:
                AI_Attack();
                AudioLevel();
                return;
        }
    }

    void AI_Patrol() {
        if(IsInFov()) {
            float d = Vector3.Distance(this.transform.position, Target_pos) / 3f;
            ReactionTime += Time.deltaTime;
            print(d);
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
            if (d < 1f || NavAgent.velocity.magnitude == 0) {
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
        if (d < 1.4f) {
            SetAIstate(AI_States.Attack);
        }
        //Move
        Move();
    }

    public void AI_Attack() {
        if(AttackTimer <= 0f) {
            AttackTimer = 1.5f;
            PlayerController.p.Damage(DealDamage, this.transform.forward);
            GameManagerScript.game.SetScreenBlood(2f, 0.3f);
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
        //Play walking sound effect
        WalkSfx();
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
            SoundSource.s.SetLevels(a, true);
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

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            PlayerSpotted_Timer = 0.5f;
            SetAIstate(AI_States.Hunt);
        }
    }
    public void WalkSfx() {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(this.transform.position, Vector3.down, out hit);

        if (NavAgent.velocity != Vector3.zero) {
            Step_timer += Time.deltaTime;
            if (CurrentAI_State !=  AI_States.Hunt) {
                Step_timerMax = 0.8f;
            }
            else {
                Step_timerMax = 0.5f;
            }

            if (Step_timer >= Step_timerMax) {
                Step_timer = 0;
                if (hit.collider != null) {
                    SoundMaterial s = hit.collider.gameObject.GetComponent<SoundMaterial>();
                    if (s != null) {
                        _Audio.clip = SoundSource.s.Get_StepSound(s.Material);
                    }

                    _Audio.pitch = Random.Range(0.5f, 1f);

                    _Audio.Play();
                }
            }
        }
        else {
            Step_timer = 0f;
        }

    }
}
