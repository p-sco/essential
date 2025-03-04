using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMapTextController : MonoBehaviour
{
    public Text display;
    public Color newColor;
    public float fadeTime = 4f;
    public float displayTextTime = 0f;
    private float baseTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<Text>();
        display.enabled = false;
    }

    private void Update()
    {
        if (Time.time <= baseTime + fadeTime)
        {
            display.color = new Color(1f, 1f, 1f, ((baseTime + fadeTime) - Time.time) / fadeTime);
            if(Time.time >= baseTime + fadeTime)
            {
                display.enabled = false;
            }
        } 
    }

    public void DisplayText(string text)
    {
        display.text = text;
        display.color = Color.white;
        display.enabled = true;
        baseTime = Time.time;
    }

}
