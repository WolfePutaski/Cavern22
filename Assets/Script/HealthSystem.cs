using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float healthStart;
    public float healthCurrent;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        healthCurrent = healthStart;
    }
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Damage(float damage)
    {
        healthCurrent -= damage;
    }
}
