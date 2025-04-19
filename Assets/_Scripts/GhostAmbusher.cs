using System.Collections.Generic;
using UnityEngine;

public class GhostAmbusher : GhostBehaviour
{
    [SerializeField] private Pacman pacman;

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
            Vector2 targetPosition = PredictPacmanPosition();
            Node startNode = node;

            Node bestMove = MinimaxDecision(startNode, targetPosition, 3, true, float.MinValue, float.MaxValue);
            if (bestMove != null)
            {
                Vector2 direction = (bestMove.transform.position - startNode.transform.position).normalized;
                this.ghost.movement.SetDirection(direction);
            }
        }
    }

    private Vector2 PredictPacmanPosition()
    {
        Vector2 pacmanPos = this.ghost.target.position;
        Vector2 direction = pacman.movement.direction;
        Vector2 predictedPos = pacmanPos;

        RaycastHit2D hit = Physics2D.Raycast(pacmanPos , direction, 4f, this.nodeLayer);

        if (hit.collider != null)
        {
            Node node = hit.collider.GetComponent<Node>();
            predictedPos = node.transform.position;
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                if (availableDirection != -direction)
                {
                    predictedPos = predictedPos + availableDirection;
                    break;
                }
            }
        } else
        {
            predictedPos = pacmanPos + 4f*direction;
        }

        return predictedPos;
    }

    private Node MinimaxDecision(Node node, Vector2 target, int depth, bool isMaximizing, float alpha, float beta)
    {
        if (depth == 0 || node == null)
        {
            return node;
        }

        Node bestMove = null;
        float bestScore = isMaximizing ? float.MinValue : float.MaxValue;

        foreach (Node nextNode in node.neighbors)
        {
            if (nextNode == null) continue;

            float score;
            if (depth == 1 || nextNode == null)
            {
                score = EvaluateNode(nextNode, target);
            }
            else
            {
                Node result = MinimaxDecision(nextNode, target, depth - 1, !isMaximizing, alpha, beta);
                score = EvaluateNode(result, target);
            }

            if (isMaximizing)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = nextNode;
                }
                alpha = Mathf.Max(alpha, score);
            }
            else
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = nextNode;
                }
                beta = Mathf.Min(beta, score);
            }

            if (beta <= alpha)
            {
                break; // Alpha-Beta pruning
            }
        }

        return bestMove;
    }

    private float EvaluateNode(Node node, Vector2 target)
    {
        return -Vector2.Distance(node.transform.position, target);
    }
}
