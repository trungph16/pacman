using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehaviour : MonoBehaviour
{
    public Ghost ghost { get; private set; }

    public float duration;

    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

    public void Enable()
    {
        Enable(this.duration);
    }

    public virtual void Enable(float duration)
    {
        this.enabled = true;

        CancelInvoke(nameof(Disable));
        Invoke(nameof(Disable), duration);
    }
    public virtual void Disable()
    {
        this.enabled = false;

        CancelInvoke(nameof(Disable));
    }
}
