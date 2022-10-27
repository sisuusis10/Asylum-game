using UnityEngine;
using UnityEngine.AI;

public class DefaultAI : MonoBehaviour {

    //Variables
    public bool IsLocked;

    //Components
    private CapsuleCollider Scollider;
    private SubCollider ChildCollider;
    private AudioSource S_Source;

    //Movement
    public float MoveSpeed;
    public float MoveSpeed_Walk = 1f, MoveSpeed_Run = 2f;

    //Navigation
    private NavMeshAgent Nav;
    public Vector3 GoPos;

    //Step Sounds
    private RaycastHit GroundCheck;
    private AudioClip WalkSound;
    [SerializeField]
    private float PlayTimer;
    public float PlayTimer_Max = 0.5f;

    //States
    public enum Path_States { Searching, Random, PlayerFound, Hide };
    public Path_States Current_Path;

    private void Awake() {
        Nav = this.GetComponent<NavMeshAgent>();
        Scollider = this.GetComponent<CapsuleCollider>();
        ChildCollider = this.GetComponentInChildren<SubCollider>();
        S_Source = this.GetComponent<AudioSource>();
    }

    private void Start() {
    }

    // Update is called once per frame
    void Update() {
        StateMachine();
        if (!IsLocked) {
            StepSoundHandler();
        }

    }

    private void OnTriggerStay(Collider other) {
        if (!IsLocked && other.gameObject.tag == "Player" && !PlayerController.p.Asthma_Immunity) {

        }
    }



    private void StateMachine() {
        //Pick State
        StateConditions();

        //RunState
        switch (Current_Path) {
            case Path_States.Random:
                RandomArea();
                break;
            case Path_States.PlayerFound:
                PlayerFound();
                break;
            case Path_States.Hide:
                HideState();
                break;
        }
    }

    private void StateConditions() {

    }

    private void RandomArea() {
        if (this.transform.position.x == GoPos.x && this.transform.position.z == GoPos.z) {
            GoPos = LocalPaths.find.RandomPath();
        }
    }

    private void HideState() {
        IsLocked = true;
        Scollider.enabled = false;
    }

    private void PlayerFound() {
        GoPos = PlayerController.p.transform.position;
    }

    public void StepSoundHandler() {
        //Check Material
        if (Physics.Raycast(this.transform.position, -transform.up, out GroundCheck, 2f)) {
            if (GroundCheck.collider.gameObject.GetComponent<AudioMaterial>()) {

                //    print("Material:" + GroundCheck.collider.gameObject.GetComponent<AudioMaterial>().SoundType);

                this.WalkSound = SoundSource.s.Get_StepSound(GroundCheck.collider.gameObject.GetComponent<AudioMaterial>().SoundType);
            }
        }
        //Play sound
        if (PlayTimer < PlayTimer_Max) {
            PlayTimer += Time.deltaTime;
        } else {
            PlayTimer = 0f; //Reset Timer
            try {
                S_Source.clip = WalkSound;
                S_Source.pitch = 1f + Random.Range(-0.1f, 0.1f);
                S_Source.Play();
            } catch {
                print("No Audio Clip Found!");
            }
        }
    }

}
