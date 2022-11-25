using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {

    public static SoundSource s;
    
    //StepSound database
    public enum S_Material { Wood, Acrylic, Grass, Stone, Metal, Carpet }
    public AudioClip[] StepAudioClips;

    //Lock
    public bool IsPaused;

    
    private void Awake() {
        s = this;
    }

    public AudioClip Get_StepSound(S_Material m) {
        switch(m) {
            case S_Material.Wood:
                return StepAudioClips[0];
            case S_Material.Acrylic:
                return StepAudioClips[1];
            case S_Material.Grass:
                return StepAudioClips[2];
            case S_Material.Stone:
                return StepAudioClips[3];
            case S_Material.Metal:
                return StepAudioClips[4];
            case S_Material.Carpet:
                return StepAudioClips[5];
        }
        //In case non found / avoid error set default
        return StepAudioClips[0];
    }

}
