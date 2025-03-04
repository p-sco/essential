using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanged : MonoBehaviour
{
    public void ChangeVol(float newValue)
    {
        AudioListener.volume = newValue;
    }
}
