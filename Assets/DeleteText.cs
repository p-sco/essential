using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleteText : MonoBehaviour
{
    private int timer = 0;
    TextMeshPro tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        Invoke("DestroyText", 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer % 90 == 0)
            tmp.alpha = 0;
        else if (timer % 30 == 0)
            tmp.alpha = 255;

        timer++;
    }

    void DestroyText(){
        Destroy(gameObject);
    }
}
