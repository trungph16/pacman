using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class GhossChaseAndScatter : GhostBehaviour
{
    public Transform pacman;
    public float panicDistance = 3f;
    public Vector2 cornerPosition;
    private int count;
    private List<Vector2> path;

    private void Start()
    {
        count = 5;
        path = new List<Vector2>();
    }
    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.ghost.frightened.enabled || !this.enabled)
        {
            count = 5;
            return;
        }

        Node node = other.GetComponent<Node>();
        float distanceToPacman = Vector2.Distance(node.transform.position, pacman.position);
        bool isTargetPacman = true;

        if (this.enabled && (Vector2)node.transform.position == cornerPosition)
        {
            count = 5;
            this.ghost.movement.SetDirection(node.availableDirections[0]);
            return;
        } 
        
        if (distanceToPacman < panicDistance)
        {
            isTargetPacman = false;
        }
        else
        {
            isTargetPacman = true;
        }

        if (this.enabled  && !isTargetPacman)
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
            count = 0;
            path = HillClimbing(node, isTargetPacman);
        }

        if (node != null && this.enabled  && (count == 5 || count == path.Count))
        {
            count = 0;
            path = HillClimbing(node, isTargetPacman);
        }
        
        if(path[count] == Vector2.zero)
        {
            this.ghost.movement.SetDirection(node.availableDirections[0]);
            count = 5;
            return;
        }

        this.ghost.movement.SetDirection(path[count].normalized);
        count++;
    }
    public List<Vector2> HillClimbing(Node beginNode, bool target)
    {
        Stack<Node> stack = new Stack<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        List<Vector2> path = new List<Vector2>();
        stack.Push(beginNode);
        Node currentNode;
        visited.Add(beginNode);
        Vector2 direction = new Vector2(1f, 1f);
        while (stack.Count > 0)
        {
            currentNode = stack.Pop();
            
            if(RaycastHitsPacman(currentNode) != Vector2.zero)
            {
                path.Add(RaycastHitsPacman(currentNode));
                return path;
            }

            if ((Vector2)currentNode.transform.position == cornerPosition)
            {
                path.Add(cornerPosition - (Vector2)currentNode.transform.position);
                return path;
            }

            List<Node> neighbors = new List<Node>();
            foreach (Node neighbor in currentNode.neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    neighbors.Add(neighbor);
                }
            }

            if (target)
            {
                neighbors.Sort((b, a) => ( 0.5 * DistanceToPacman(a)).CompareTo( 0.5 * DistanceToPacman(a)));
            }
            else
            {
                neighbors.Sort((b, a) => ( 0.5 * DistancetoCorner(a)).CompareTo( 0.5 * DistancetoCorner(b)));
            }

            

            if (neighbors.Count > 0)
            {
                direction = -(Vector2)currentNode.transform.position + (Vector2)neighbors[neighbors.Count - 1].transform.position;
                foreach (Node neighbor in neighbors)
                {
                    stack.Push(neighbor);
                }
                path.Add(direction);
            }    
        }
        
        return path;
    }

    public float DistanceNode(Node node1, Node node2)
    {
        return Vector2.Distance(node1.transform.position, node2.transform.position);
    }
    public Vector2 RaycastHitsPacman(Node node)
    {
        foreach (Vector2 direction in node.availableDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)node.transform.position, direction, 50f, LayerMask.GetMask("PacMan"));
            if (hit.collider != null)
            {
                return direction;
            }
        }

        return Vector2.zero; 
    }

    public float DistanceToPacman(Node node)
    {
        return Vector2.Distance(node.transform.position, pacman.position);
    }

    public float DistancetoCorner(Node node)
    {
        return Vector2.Distance(node.transform.position, cornerPosition);
    }
}


    