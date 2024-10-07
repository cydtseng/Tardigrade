using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectionWithTrack : MonoBehaviour
{
    public FMODUnity.EventReference music;

    // Parameters for main track
    public bool setParameters = false;
    public MusicMenuItem biome;
    public bool mute = false;

    void Start() {
        MusicManager mm = PersistentState.state.GetMusicManager();
        mm.Reload(music);
        if (setParameters)
            mm.SetParameters(biome, mute, play: true);
    }
}
