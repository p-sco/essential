using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTeleporter : MonoBehaviour
{
    private float currentDegrees = 0f;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, currentDegrees);
        currentDegrees += 8;
    }
}
