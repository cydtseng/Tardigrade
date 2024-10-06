using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PersistentState : MonoBehaviour
{
    public static PersistentState state;
    private int scoreCounter = 0;

    // Track music
    public FMODUnity.EventReference bgmAudio;  // to reference BGM
    private FMOD.Studio.EventInstance bgmAudioEvent;

    void Awake()
    {
        if (state != null && state != this) {
            Destroy(this.gameObject);
            return;
        } else {
            state = this;
            DontDestroyOnLoad(this.gameObject);
            bgmAudioEvent = FMODUnity.RuntimeManager.CreateInstance(bgmAudio);
        }
    }
 
    void Start() {
        
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementScore() {
        scoreCounter += 1;
    }

    public int getScore() {
        return scoreCounter;
    }

    public void addToScore(int value) {
        scoreCounter += value;
    }

    // Manage main music track
    // We rely on the fact that this object doesn't get despawned during scene swapping
    public void bgmStart() {
        if (bgmIsPlaying()) return;
        bgmAudioEvent.start();
    }
    public bool bgmIsStopped() {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        bgmAudioEvent.getPlaybackState(out playbackState);
        return playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
    public bool bgmIsPlaying() {
        return !bgmIsStopped();
    }
    public void bgmStop() {
        bgmAudioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public FMOD.Studio.EventInstance getBgmAudioEvent() {
        return bgmAudioEvent;
    }
    public void bgmSetParameters(int biome, int mute = 0, bool start = false) {
        bgmAudioEvent.setParameterByName("Mute", mute);
        bgmAudioEvent.setParameterByName("Biome", biome);
        if (start) bgmStart();
    }
}
