using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : HealthSystem
{
    public GameObject wormToSpawn;
    public float chanceToSpawn;
    public override void SetCollideLayerDie()
    {
        base.SetCollideLayerDie();

        float spawnChanceCheck = Random.Range(0f, 1f);

        if (spawnChanceCheck > chanceToSpawn) { return; }

        GameObject worm = Instantiate(wormToSpawn, transform.position, transform.rotation);
        worm.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
    }
}
