using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUpgrades : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("UI").transform.Find("UpgradeUI").gameObject.SetActive(true);
            GameObject.Find("UI").transform.Find("StatsUI").SendMessage("SlideUI");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("UI").transform.Find("UpgradeUI").gameObject.SetActive(false);
            GameObject.Find("UI").transform.Find("StatsUI").SendMessage("SlideUI");
        }
    }
}
