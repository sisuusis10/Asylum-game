using UnityEngine;

public class ControlsHandler : MonoBehaviour {

    public static ControlsHandler get;

    public KeyCode Interact, Interact_2, Skip, SprintKey, SneakKey, FlashLightKey, InhalerKey, PauseKey;

    // Start is called before the first frame update
    void Start() {
        get = this;

        if(Interact == KeyCode.None) {
            Interact = KeyCode.E;
        }
        if (Interact_2 == KeyCode.None) {
            Interact_2 = KeyCode.Return;
        }
        if (Skip == KeyCode.None) {
            Skip = KeyCode.X;
        }
        if (SprintKey == KeyCode.None) {
            SprintKey = KeyCode.LeftShift;
        }
        if (SneakKey == KeyCode.None) {
            SneakKey = KeyCode.LeftControl;
        }
        if (InhalerKey == KeyCode.None) {
            InhalerKey = KeyCode.Q;
        }
        if (PauseKey == KeyCode.None) {
            PauseKey = KeyCode.Escape;
        }
        if(FlashLightKey == KeyCode.None) {
            FlashLightKey = KeyCode.F;
        }
    }
}
