using System;
using UnityEngine;

public class DirectPlayOneShot : MonoBehaviour
{
    public FMODUnity.EventReference music;
    private FMOD.Studio.EventInstance instance;
    public float startDelay = 0f;
    public float endDelay = 1000f;
    private float currTime = 0f;

    void Awake() {
        instance = FMODUnity.RuntimeManager.CreateInstance(music);
    }

    void Start() {
        instance.start();
    }

    void FixedUpdate() {
        currTime += Time.deltaTime;
        if (currTime > endDelay) {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    void OnDestroy() {
        instance.release();
    }

}
