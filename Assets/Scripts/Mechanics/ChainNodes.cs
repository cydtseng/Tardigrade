using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChainNodes : MonoBehaviour
{
    public List<Transform> nodes;  // List of all nodes in the scene
    public Transform player;
    public float interactionRange = 2.0f;
    public float followSpeed = 5f;  
    public float nodeSpacing = 1.5f;
    public float pulseDuration = 0.3f;
    public float pulseScaleMultiplier = 1.3f;
    public Color chainColor = Color.green; 
    private List<Transform> chain; 

    void Start()
    {
        chain = new List<Transform>();
    }

    void Update()
    {
        CheckNodeInteraction();
        HandleNodeFollowing();
    }

    void CheckNodeInteraction()
    {
        foreach (Transform node in nodes)
        {
            if (!chain.Contains(node))
            {
                float distanceToNode = Vector3.Distance(player.position, node.position);
                
                if (distanceToNode < interactionRange)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        OnNodeReached(node);
                    }
                }
            }
        }
    }
    
    void OnNodeReached(Transform node)
    {
        if (!chain.Contains(node))
        {
            chain.Add(node);
            node.DOScale(node.localScale * pulseScaleMultiplier, pulseDuration).SetLoops(2, LoopType.Yoyo);
            node.GetComponent<Renderer>().material.DOColor(chainColor, 0.5f);
            node.DOShakePosition(0.5f, 0.5f, 10, 90, false, true);
            Debug.Log("Node added to the chain!");
            if (chain.Count == nodes.Count)
            {
                Debug.Log("All nodes chained!");
            }
        }
    }

    
    void HandleNodeFollowing()
    {
        for (int i = 0; i < chain.Count; i++)
        {
            Transform node = chain[i];

            // The first node follows the player, others follow the previous node at a set distance
            Transform target = (i == 0) ? player : chain[i - 1];

            // Ensure each node follows at a fixed distance
            float distance = Vector3.Distance(node.position, target.position);
            if (distance > nodeSpacing)
            {
                // Move the node smoothly towards the target position, keeping the fixed spacing
                Vector3 direction = (target.position - node.position).normalized;  // Get the direction to the target
                Vector3 newPosition = target.position - direction * nodeSpacing;    // Position to maintain fixed spacing

                // Smooth movement with Lerp
                node.position = Vector3.Lerp(node.position, newPosition, followSpeed * Time.deltaTime);

                // Calculate 2D rotation based on direction
                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    node.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}
