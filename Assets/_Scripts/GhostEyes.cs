using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public Movement movement { get; private set; }

    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    public void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.movement = GetComponentInParent<Movement>();
    }
    private void Update()
    {
        if (this.movement == null)
        {
            return;
        }

        if(this.movement.direction == Vector2.up)
        {
            this.spriteRenderer.sprite = up;
        }
        else if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = down;
        }
        else if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = left;
        }
        else if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = right;
        }
    }
}
