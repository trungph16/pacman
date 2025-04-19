using UnityEngine;

public class Pellet : MonoBehaviour
{
    [SerializeField] protected int points = 10;

    protected virtual void Eat()
    {
        GameManager.Instance.PelletEaten(this);
    }

    public virtual int getPoints()
    {
        return points;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PacMan")) 
        {
            Eat();
        }
    }
}
