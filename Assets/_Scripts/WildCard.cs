using System.Collections.Generic;
using UnityEngine;

public class WildCard : GhostBehaviour
{
    public Pacman pacman;

    public Ghost blinky;

    public LayerMask nodeLayer;
    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 targetPosition = CalculateInkyTarget();
            Node startNode = node;

            Node bestMove = BidirectionalSearch(startNode, targetPosition);
            if (bestMove != null)
            {
                Vector2 direction = (bestMove.transform.position - startNode.transform.position).normalized;
                this.ghost.movement.SetDirection(direction);
            } else
            {
                this.ghost.movement.SetDirection(startNode.availableDirections[0]);
            }
        }
    }
   
    private Vector2 CalculateInkyTarget()
    {
        Vector2 pacmanPos = this.ghost.target.position;
        Vector2 direction = pacman.movement.direction;
        Vector2 blinkyPos = blinky.transform.position;

        Vector2 inkyTarget = pacmanPos + (pacmanPos - blinkyPos);

        if (inkyTarget.x > 12.5f)
        {
            inkyTarget.x = 12.5f;
        }
        else if (inkyTarget.x < -12.5f)
        {
            inkyTarget.x = -12.5f;
        }

        if (inkyTarget.y > 12.5f)
        {
            inkyTarget.y = 12.5f;
        }
        else if (inkyTarget.y < -15.5f)
        {
            inkyTarget.y = -15.5f;
        }

        return inkyTarget;
    }

    private Node BidirectionalSearch(Node startNode, Vector2 targetPosition)
    {
        Node targetNode;

        Collider2D hit = Physics2D.OverlapCircle(targetPosition, 10f, this.nodeLayer);
        if (hit != null)
        {
            targetNode = hit.GetComponent<Node>();
        }
        else
        {
            return null;
        }

        if (startNode == targetNode) return null;

        Queue<Node> startQueue = new Queue<Node>();
        Queue<Node> targetQueue = new Queue<Node>();
        HashSet<Node> startVisited = new HashSet<Node>();
        HashSet<Node> targetVisited = new HashSet<Node>();
        Dictionary<Node, Node> startCameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, Node> targetCameFrom = new Dictionary<Node, Node>();

        startQueue.Enqueue(startNode);
        startVisited.Add(startNode);
        targetQueue.Enqueue(targetNode);
        targetVisited.Add(targetNode);

        while (startQueue.Count > 0 && targetQueue.Count > 0)
        {
            Node meetingPoint = ExpandQueue(startQueue, startVisited, startCameFrom, targetVisited);
            if (meetingPoint != null) return ReconstructPath(startCameFrom, targetCameFrom, meetingPoint);

            meetingPoint = ExpandQueue(targetQueue, targetVisited, targetCameFrom, startVisited);
            if (meetingPoint != null) return ReconstructPath(startCameFrom, targetCameFrom, meetingPoint);
        }

        return null;
    }

    private Node ExpandQueue(Queue<Node> queue, HashSet<Node> visited, Dictionary<Node, Node> cameFrom, HashSet<Node> oppositeVisited)
    {
        Node current = queue.Dequeue();

        foreach (Node neighbor in current.neighbors)
        {
            if (oppositeVisited.Contains(neighbor))
            {   
                cameFrom[neighbor] = current;
                return neighbor;
            }

            if (!visited.Contains(neighbor))
            {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
                cameFrom[neighbor] = current;
            }
        }

        return null;
    }

    private Node ReconstructPath(Dictionary<Node, Node> startCameFrom, Dictionary<Node, Node> targetCameFrom, Node meetingPoint)
    {
        List<Node> path = new List<Node> { meetingPoint };

        Node current = meetingPoint;
        while (startCameFrom.ContainsKey(current))
        {
            current = startCameFrom[current];
            path.Insert(0, current);
        }

        current = meetingPoint;
        while (targetCameFrom.ContainsKey(current))
        {
            current = targetCameFrom[current];
            path.Add(current);
        }

        return path.Count > 1 ? path[1] : null;
    }

    
}