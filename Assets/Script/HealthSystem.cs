using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float healthStart;
    private float _healthCurrent;
    public float healthCurrent { get { return _healthCurrent; } }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        _healthCurrent = healthStart;
    }
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Damage(float damage)
    {
        _healthCurrent -= damage;
    }

    public virtual void Push(Vector2 force)
    {
        if( TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(force,ForceMode2D.Impulse);
        }
        
    }

    public virtual void PlayAnimation(string animationState)
    {
        if (TryGetComponent(out Animator anim) && anim.enabled)
        {
            anim.Play(animationState);
        }
    }

    public virtual void SetCollideLayer(bool setActive)
    {
        int newLayer = LayerMask.NameToLayer(setActive ? "ActivePawn" : "DeadPawn");
        gameObject.layer = newLayer;

    }

    public virtual void SetCollideLayerDie()
    {
        int newLayer = LayerMask.NameToLayer("DeadPawn");
        gameObject.layer = newLayer;

    }
}
