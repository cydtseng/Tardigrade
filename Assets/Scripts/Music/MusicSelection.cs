using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelection : MonoBehaviour
{
    public MusicMenuItem biome;
    public bool mute = false;

    void Start() {
        PersistentState.state.GetMusicManager()
            .SetParameters(biome, mute, play: true);
    }
}
