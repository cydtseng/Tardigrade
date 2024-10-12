using System.Collections.Generic;
using UnityEngine;

public class OrganicTunnel : MonoBehaviour
{
    public GameObject tunnelSegmentPrefab; // Prefab for a single tunnel segment
    public int segmentCount = 20;          // Number of tunnel segments
    public float segmentWidth = 1.0f;      // Width of each segment
    public Transform player;               // Reference to the player's position
    public float bulgeRadius = 2f;         // How much the tunnel bulges around the player
    public float bulgeStrength = 0.5f;     // Strength of the bulge effect

    public float segmentHeight = 3f;
    public float segmentHeightVariability = 0.1f;  // how much the tunnel varies in height
    public float segmentHeightTimeConstant = 0.1f;   // exponential filter time constant
    public float segmentHeightOffset = 0f;      // offset relative to tardigrade


    private List<GameObject> tunnelSegments = new List<GameObject>();
    private List<Vector3> initialScales = new List<Vector3>(); // Store the initial scales of the segments

    void Start()
    {
        GenerateTunnel();
    }

    void Update()
    {
        UpdateTunnelBulge();
    }

    void GenerateTunnel()
    {

        // Calculate the left edge of the camera's view in world space
        Camera cam = Camera.main;
        float cameraLeftEdge = cam.transform.position.x - (cam.orthographicSize * cam.aspect);

        // Initial perturbation
        float y = 0;
        float alpha = segmentHeightTimeConstant;

        // Start generating the tunnel segments from the camera's left edge
        for (int i = 0; i < segmentCount; i++)
        {
            // Perturbation
            float dy = Random.Range(-segmentHeightVariability, segmentHeightVariability);

            // Perturb relative to previous y value using an exponential filter
            y = ((1 - alpha) * y) + alpha * dy;

            // Position the segments starting from the left edge of the camera
            GameObject segment = Instantiate(tunnelSegmentPrefab, new Vector3(cameraLeftEdge + i * 1f, y + segmentHeightOffset, 0), Quaternion.identity);

            // Set the initial scale of each segment
            segment.transform.localScale = new Vector3(segmentWidth, segmentHeight, 1f);

            // Store the initial scale in the list for later use
            initialScales.Add(segment.transform.localScale);

            tunnelSegments.Add(segment);
        }
    }

    void UpdateTunnelBulge()
    {
        float sineFrequency = 0.5f;  // Adjust for slower or faster pulsation
        float sineAmplitude = 0.05f; // Controls how much the tunnel pulsates outward

        for (int i = 0; i < tunnelSegments.Count; i++)
        {
            GameObject segment = tunnelSegments[i];
            Vector3 initialScale = initialScales[i];  // Retrieve the initial scale for this segment

            // Calculate the distance between the player and the current segment
            float distanceToPlayer = Vector2.Distance(segment.transform.position, player.position);

            // Calculate the bulging effect for this segment
            float bulgeAmount = Mathf.Lerp(bulgeStrength, 0, distanceToPlayer / bulgeRadius);

            // Add pulsating effect with sine wave for organic pulsation
            float pulseEffect = Mathf.Sin(Time.time * sineFrequency) * sineAmplitude;

            // Blend bulging with nearby segments for smooth transition
            float blendFactor = Mathf.Clamp01(1 - distanceToPlayer / bulgeRadius);

            // Combine proximity-based bulge with pulsation
            bulgeAmount += pulseEffect * blendFactor;

            // Apply scaling to simulate bulge, maintaining the initial scale
            segment.transform.localScale = Vector3.Lerp(segment.transform.localScale,
                new Vector3(initialScale.x + bulgeAmount, initialScale.y + bulgeAmount, initialScale.z), 0.5f);

            // Optionally, apply a smaller scale effect to neighboring segments for smoothness
            if (i > 0)  // Affect the previous segment
            {
                GameObject prevSegment = tunnelSegments[i - 1];
                Vector3 prevInitialScale = initialScales[i - 1];
                prevSegment.transform.localScale = Vector3.Lerp(prevSegment.transform.localScale,
                    new Vector3(prevInitialScale.x + bulgeAmount * 0.5f, prevInitialScale.y + bulgeAmount * 0.5f, prevInitialScale.z), 0.5f);
            }

            if (i < tunnelSegments.Count - 1)  // Affect the next segment
            {
                GameObject nextSegment = tunnelSegments[i + 1];
                Vector3 nextInitialScale = initialScales[i + 1];
                nextSegment.transform.localScale = Vector3.Lerp(nextSegment.transform.localScale,
                    new Vector3(nextInitialScale.x + bulgeAmount * 0.5f, nextInitialScale.y + bulgeAmount * 0.5f, nextInitialScale.z), 0.5f);
            }
        }
    }

}
