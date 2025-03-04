using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKiller : MonoBehaviour
{
    public GameObject killer;
    public Transform spawner;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(killer, spawner.position, Quaternion.identity);
            GameObject.Find("Player").GetComponent<PlayerController>().health = 1;
            Destroy(gameObject);
        }
    }
}
