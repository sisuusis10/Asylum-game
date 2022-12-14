using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneScript : MonoBehaviour
{

    private float t = 0.5f;
    private bool once;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(t > 0) {
            t -= Time.deltaTime;
        } else if(Input.anyKeyDown && !once) {
            SceneManager.LoadSceneAsync("Demo Game");
            once = true;
        }   
    }
}
