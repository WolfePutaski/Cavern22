using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private Animator anim;
    private bool canTakeDamage;
    [SerializeField] private float damageDelay;
    private float damageDelayCount;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        canTakeDamage = damageDelayCount <= 0;
        if (damageDelayCount > 0) { damageDelayCount -= Time.deltaTime; }
    }    

    public override void Damage(float damage)
    {
        if (!canTakeDamage) { return; }
        base.Damage(damage);
        anim.Play("hurt");
        damageDelayCount = damageDelay;

        if (healthCurrent <= 0)
        {
            GameOver();
        }
    }


    protected virtual void GameOver()
    {
        Kill();
    }
}
