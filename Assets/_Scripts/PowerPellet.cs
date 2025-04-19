using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 10.0f;
    public override int getPoints()
    {
        return this.points*5;
    }

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
    }
}
