using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSpells : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("UI").transform.Find("ChangeSpell").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("UI").transform.Find("ChangeSpell").gameObject.SetActive(false);
        }
    }
}
