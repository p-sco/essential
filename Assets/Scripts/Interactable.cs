using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{


    public abstract void Drop(Transform t);

    public abstract void Pickup();
}
