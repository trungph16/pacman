using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehaviour
{
    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Node startNode = node;
            Vector2 targetPosition = this.ghost.target.position;

            List<Node> path = FindPath(startNode, targetPosition);
            
            if (path.Count > 1)
            {
                Vector2 direction = (path[1].transform.position - startNode.transform.position).normalized;
                this.ghost.movement.SetDirection(direction);
            } else
            {
                Vector2 direction = (targetPosition - (Vector2)startNode.transform.position).normalized;
                direction.x = Mathf.Round(direction.x);
                direction.y = Mathf.Round(direction.y);
                this.ghost.movement.SetDirection(direction);
            }
        }
    }

    private List<Node> FindPath(Node startNode, Vector2 target)
    {
        List<Node> closestNode = NodeManager.Instance.GetClosestNode(this.ghost.target.position);

        
        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, float> gScore = new Dictionary<Node, float> { [startNode] = 0 };
        Dictionary<Node, float> fScore = new Dictionary<Node, float> { [startNode] = Heuristic(startNode, target) };

        while (openSet.Count > 0)
        {
            Node current = GetNodeWithLowestFScore(openSet, fScore);

            foreach (Node node in closestNode)
            {
                if (Vector2.Distance(current.transform.position, node.transform.position) < 0.1f)
                {
                    return ReconstructPath(cameFrom, current);
                }
            }
            
            openSet.Remove(current);
            closedSet.Add(current);
            
            foreach (Node neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector2.Distance(current.transform.position, neighbor.transform.position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, Mathf.Infinity))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, target);
                
            }
        }

        return new List<Node>();
    }

    private Node GetNodeWithLowestFScore(List<Node> openSet, Dictionary<Node, float> fScore)
    {
        Node bestNode = openSet[0];
        float bestScore = fScore[bestNode];

        foreach (Node node in openSet)
        {
            if (fScore[node] < bestScore)
            {
                bestNode = node;
                bestScore = fScore[node];
            }
        }
        return bestNode;
    }

    private List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        List<Node> path = new List<Node> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    private float Heuristic(Node a, Vector2 b)
    {
        return Vector2.Distance(a.transform.position, b);
    }
}
