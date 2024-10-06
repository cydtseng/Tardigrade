using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicParameters : MonoBehaviour
{
    public int biome = 0;
    public int mute = 0;

    void Start() {
        PersistentState.state.bgmSetParameters(biome, mute, start: true);
    }
}
