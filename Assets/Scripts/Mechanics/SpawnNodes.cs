using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to the object with ChainNodes
public class SpawnNodes : MonoBehaviour
{
    public List<GameObject> templateNodes;  // nodes to duplicate from
    private float spawnMargin = 3f;
    public float firstSpawnDurationSeconds = 10f;
    public float spawnIntervalSeconds = 5f;
    private int triggerCountdown;

    // Camera parameters
    private float hh;
    private float hw;

    void Start()
    {
        triggerCountdown = (int)(50 * firstSpawnDurationSeconds);
        hh = Camera.main.orthographicSize;  // half-height
        hw = hh * Camera.main.aspect; // half-width
    }

    // After every 5s, spawn a new node outside of the camera viewport
    void FixedUpdate()
    {
        triggerCountdown--;
        if (triggerCountdown < 0) {
            // Trigger only once any directional key is pressed
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if ((x == 0) && (y == 0)) return;

            Debug.LogFormat("User pressed direction: x = {0}, y = {1}", x, y);
            triggerCountdown = (int)(50 * spawnIntervalSeconds);

            // Clone a new object
            GameObject templateObj = templateNodes[Random.Range(0, templateNodes.Count)];
            Vector3 position = Camera.main.transform.position;

            // Select spawn locations that player is headed towards
            float z = 20;  // put in front of space background
            if ((x != 0) && (y != 0)) {  // choose only one direction to spawn
                if (Random.Range(0, 2) == 0) {
                    x = 0;
                } else {
                    y = 0;
                }
            }
            if (x != 0) {  // spawn either on left or right of screen
                if (x > 0) {  // spawn on right side
                    position += new Vector3(hw + spawnMargin, Random.Range(-hh/2, hh/2), z);
                } else {
                    position += new Vector3(-hw - spawnMargin, Random.Range(-hh/2, hh/2), z);
                }
            } else {
                if (y > 0) {  // spawn above
                    position += new Vector3(Random.Range(-hw/2, hw/2), hh + spawnMargin, z);
                } else {
                    position += new Vector3(Random.Range(-hw/2, hw/2), -hh - spawnMargin, z);
                }
            }
            // Add random rotation
            float rotationAngle = Random.Range(-180, 180);

            // Spawn target object
            GameObject obj = Instantiate(templateObj, position, Quaternion.Euler(0, 0, rotationAngle));
            obj.SetActive(true);

            // Associate object with ChainNodes script
            GetComponent<ChainNodes>().nodes.Add(obj.transform);
        }
    }
}
