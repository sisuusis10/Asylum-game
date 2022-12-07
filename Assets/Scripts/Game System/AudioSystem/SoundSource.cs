using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {

    public static SoundSource s;
    
    //StepSound database
    public enum S_Material { Wood, Acrylic, Grass, Stone, Metal, Carpet }
    public AudioClip[] StepAudioClips;

    //Ambience stuff
    public AudioSource Source_Ambience, Source_AmbienceHorror, Source_Horror;
    [SerializeField]
    private float Volume_Ambience, Volume_AmbienceHorror = 0f, Volume_Horror = 0f, Volume_Lerp = 0.1f;
    //Lock
    public bool IsPaused;

    
    private void Awake() {
        s = this;
        Volume_Ambience = Source_Ambience.volume;
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

    private void Update() {
        if(!IsPaused) {
            SetVolumes(Volume_Ambience, Volume_AmbienceHorror, Volume_Horror, true);
        }
        else {
            SetVolumes(0,0,0, false);
        }
    }
    public void SetLevels(float Ambience_Horror_Ratio, bool Horror) {
        Volume_Ambience = Ambience_Horror_Ratio;
        Volume_AmbienceHorror = 1 - Ambience_Horror_Ratio;

        Volume_Horror = (Horror) ? 0.5f * Volume_AmbienceHorror : 0f;
    }

    private void SetVolumes(float a, float a_h, float _Horror, bool _lerp) {
        if(!_lerp) {
            Source_Ambience.volume = a;
            Source_AmbienceHorror.volume = a_h;
            Source_Horror.volume = _Horror;
        } else {
            Source_Ambience.volume = Mathf.Lerp(Source_Ambience.volume, a, Volume_Lerp);
            Source_AmbienceHorror.volume = Mathf.Lerp(Source_AmbienceHorror.volume, a_h, Volume_Lerp);
            Source_Horror.volume = Mathf.Lerp(Source_Horror.volume, _Horror, Volume_Lerp);
        }
    }

}
