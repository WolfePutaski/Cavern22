using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text ammoCounter;
    public TMP_Text healthCounter;

    private PlayerShoot playerShoot;
    private PlayerHealth playerHealth; 

    // Start is called before the first frame update
    void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerHealth = GetComponent<PlayerHealth>();
    }


    private void Update()
    {
        ammoCounter.text = "Ammo: " + playerShoot.ammoInMag;
        
        healthCounter.text = "Health:";

        for (int i = 0; i < playerHealth.healthCurrent; i++)
        {
            healthCounter.text += " o";
        }
    }

}
