using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {

    public static SoundSource s;
    public AudioSource SFX_source;
    public AudioSource SFX_source2;
    public AudioSource Ambience_source;
    public AudioSource Bass_source;

    //StepSound database
    public enum S_Material { Wood, Acrylic, Grass, Stone, Metal, Carpet }
    public AudioClip[] StepAudioClips;

    //Amient music
    public float Volume, Volume_Max;
    private bool AmbienceChanged;
    private AudioClip NewAmbience;
    //Lock
    public bool IsPaused;

    //Sound effect Priority
    private int Current_Priority;

    //Volumes
    public enum VolumeType { SoundEffects, Music }
    private float[] VolumeLevels;

    private void Awake() {
        s = this;
        Ambience_source = this.GetComponent<AudioSource>();
        Volume_Max = Ambience_source.volume;
    }

    public void PauseState(bool state) {
        if(state) {
            Bass_source.Pause();
            Ambience_source.Pause();
        } else {
            Bass_source.UnPause();
            Ambience_source.UnPause();
        }
    }

    private void FixedUpdate() {
        Ambience_source.volume = Volume;
        if(!IsPaused) {
            if (SceneHandler.scene.IsGettingReady || AmbienceChanged) {
                Volume = Mathf.MoveTowards(Volume, -0.1f, 0.02f);
            } else if (!SceneHandler.scene.IsGettingReady) {
                Volume = Mathf.MoveTowards(Volume, Volume_Max, 0.02f);
            }
        } else {
            Volume = 0f;
            Bass_source.volume = Volume;
        }

        //Change Ambient music
        if (AmbienceChanged && Volume <= 0f) {
            Ambience_source.clip = NewAmbience;
            Ambience_source.Play();
            AmbienceChanged = false;
        } else if (SceneHandler.scene.IsGettingReady) {
            AmbienceChanged = false;
        }
    }

    public void ChangeAmbience(AudioClip NewMusic) {
        NewAmbience = NewMusic;
        AmbienceChanged = true;
    }

    public void Play(AudioClip c, float v, int priority, bool random_p) {
        //determine if allowed to play audio
        bool CanClipPlay = false;
        if(SFX_source.isPlaying && priority >= Current_Priority || !SFX_source.isPlaying) {
            Current_Priority = priority;
            CanClipPlay = true;
        }

        //Play audio clip
        if(CanClipPlay) {
            try {
                SFX_source.volume = v;
                SFX_source.clip = c;
                if(random_p) {
                    SFX_source.pitch = 1f + Random.Range(-0.1f, 0.1f);
                } else {
                    SFX_source.pitch = 1f;
                }
                SFX_source.Play();
            }
            catch {
                print("Failed to play audio!");
            }
        }
    }

    //3D sound effects
    public void Play3D(Vector3 p) {
        this.transform.position = p;
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
