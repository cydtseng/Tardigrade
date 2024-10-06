using UnityEngine;

public class WithPersistentState : MonoBehaviour
{
    protected PersistentState state;
    
    void Awake() {
        state = FindObjectOfType<PersistentState>();
    }
}
