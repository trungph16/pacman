using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public LayerMask nodeLayer;
    public List<Vector2> availableDirections { get; private set; }
    public List<Node> neighbors { get; private set; }

    private void Awake()
    {
        this.neighbors = new List<Node>();
    }
    private void Start()
    {
        this.availableDirections = new List<Vector2>();
        

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);

    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        if (hit.collider == null)
        {
            this.availableDirections.Add(direction);
        }
    }

    public void FindNeighbors()
    {
        foreach (Vector2 direction in this.availableDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)this.transform.position + direction, direction, 100f, this.nodeLayer);
            if (hit.collider != null)
            {
                Node neighbor = hit.collider.GetComponent<Node>();
                if (neighbor != null)
                {
                    this.neighbors.Add(neighbor);
                }
            }
        }
    }
}





