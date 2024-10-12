using System.Collections;
using UnityEngine;

public class PlayBumpOnlyOnPlayerCollision : MusicManager
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Play();
        }
    }
}
