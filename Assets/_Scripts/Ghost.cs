using UnityEngine;

public class Ghost : MonoBehaviour 
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostFrightened frightened { get; private set; }

    public GhostBehaviour typeChase;

    public GhostBehaviour initialBehaviour;

    public Transform target;

    public int points = 200;


    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        frightened = GetComponent<GhostFrightened>();
    }

    public void ResetState()
    {
        this.movement.ResetState();
        this.gameObject.SetActive(true);

        this.scatter.Disable();
        this.typeChase.Disable();
        this.frightened.Disable();
        

        if (this.home != this.initialBehaviour)
        {
            this.home.Disable();
        }

        if(this.initialBehaviour != null)
        {
            this.initialBehaviour.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PacMan"))
        {
            if (this.frightened.enabled)
            {
                GameManager.Instance.GhostEaten(this);
            }
            else
            {
                GameManager.Instance.PacmanEaten();
            }
        }
    }
}
