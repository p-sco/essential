using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAbility_Tutorial : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("UI").transform.Find("AbilityUI").gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
