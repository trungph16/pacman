using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    public List<Node> nodes = new List<Node>();

    public LayerMask nodeLayer;
    public LayerMask obstacleLayer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        FindNodes();
    }

    private void Start()
    {
        Invoke("FindNeighbors", 2f);
        
    }
    private void FindNodes()
    {
        nodes.AddRange(Object.FindObjectsByType<Node>(FindObjectsSortMode.None));
    }

    private List<Vector2> CheckAvailableDirection(Vector2 position)
    {
        List<Vector2> availableDirections = new List<Vector2>();
        Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 direction in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direction, 1.5f, this.obstacleLayer);
            if (hit.collider == null)
            {
                availableDirections.Add(direction);
            }
        }
        return availableDirections;
    }

    public void FindNeighbors()
    {
        foreach (Node node in nodes)
        {
            node.FindNeighbors();
        }
    } 
    public List<Node> GetClosestNode(Vector2 position)
    {
        
        List<Vector2> availableDirections = CheckAvailableDirection(position);
        List<Node> closestNodes = new List<Node>();

        foreach (Vector2 direction in availableDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direction, 10f, this.nodeLayer);
            if (hit.collider != null)
            {
                closestNodes.Add(hit.collider.GetComponent<Node>());
            }
        }
    
        return closestNodes;
    }
}


