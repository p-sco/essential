using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyExplosion", 1f);
    }

    void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
