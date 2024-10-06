using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public FMODUnity.EventReference music;
    private FMOD.Studio.EventInstance instance;

    void Awake() {
        instance = FMODUnity.RuntimeManager.CreateInstance(music);
    }

    void OnDestroy() {
        instance.release();
    }

    public void Play() {
        if (IsPlaying()) return;
        instance.start();
    }

    public void Stop() {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public bool IsPlaying() {
        return !IsStopped();
    }

    public bool IsStopped() {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        instance.getPlaybackState(out playbackState);
        return playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    public void SetParameters(MusicMenuItem biome, bool mute = false, bool play = false) {
        instance.setParameterByName("Mute", (mute) ? 1 : 0);
        instance.setParameterByName("Biome", (int)biome);
        if (play) Play();
    }
}
