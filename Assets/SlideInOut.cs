using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInOut : MonoBehaviour
{
    private bool opened = true;
    private RectTransform rtrans;
    private float baseTime = -10f;
    private bool makeSlide = false;
    public float slideTime = 1f;

    public float slideDistance = 100f;
    // Start is called before the first frame update
    void Start()
    {
        rtrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || makeSlide)
        {
            baseTime = Time.time;
            opened = !opened;
            makeSlide = false;
            Debug.Log(opened);
            
        }

        if(Time.time <= baseTime + slideTime)
        {
            rtrans.anchoredPosition = new Vector2((opened ? slideDistance : -slideDistance) + (((((baseTime + slideTime) - Time.time) / slideTime) * 45) - 45) * (opened ? -1 : 1), 0f);
        }
    }

    public void SlideUI(){
        makeSlide = true;
    }
}
