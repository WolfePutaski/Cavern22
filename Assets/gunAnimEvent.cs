using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAnimEvent : MonoBehaviour
{
    public PlayerShoot playerGunFunction;

    void newMag()
    {
        playerGunFunction.SendMessage("ReloadMag", SendMessageOptions.RequireReceiver);
    }

    void checkReturnToAim()
    {
        playerGunFunction.SendMessage("ReturnToAim", SendMessageOptions.RequireReceiver);

    }

    void PlaySound(AudioClip audioClip)
    {
        GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
}
