using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLanternItem : HandItem
{
    public Light lanternLight;
    //public boolean isEnabled;

    public override void UseItem(FirtsPersonController player)
    {
        if (lanternLight)
        {
            lanternLight.enabled = !lanternLight.enabled;
            player.PlayerAudioSource.PlayOneShot(player.LanternClick);
        }
    }
}
