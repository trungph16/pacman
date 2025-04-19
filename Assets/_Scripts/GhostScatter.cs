using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnDisable()
    {
       this.ghost.typeChase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            int index = Random.Range(0, node.availableDirections.Count);

            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -this.ghost.movement.direction)
            {
                index = (index + 1) % node.availableDirections.Count;
            }
            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
