using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMapText : MonoBehaviour
{
    // Start is called before the first frame update
    public string MapText;
    private ShowMapTextController controller;

    private void Start()
    {
        controller = GameObject.Find("MapTextDisplay").GetComponent<ShowMapTextController>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hiy");
        controller.DisplayText(MapText);
    }
}
