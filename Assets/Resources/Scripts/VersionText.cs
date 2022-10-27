using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour {

    //Variables
    public string Version = "Alpha";
    
    // Start is called before the first frame update
    void Start() {
        Version += " v"+Application.version;
        this.GetComponent<TextMeshProUGUI>().text = Version;
    }
}
