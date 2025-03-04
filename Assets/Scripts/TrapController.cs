using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    Animator anim;
    public float relauchTime = 4f;
    public float delayTime = 2f;
    private float nextTimeToLaunch = 0f;
    private bool launched = false;
    private bool notHit = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTimeToLaunch)
        {
            anim.SetBool("Launch", false);
            launched = false;
            notHit = true;
        }


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (launched && notHit)
            {
                collision.gameObject.SendMessage("TakeDamage", 10.0);
                notHit = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Time.time >= nextTimeToLaunch)
            {
                nextTimeToLaunch = Time.time + 1f * relauchTime;
                StartCoroutine(launch());
            }
            
        }
    }


    IEnumerator launch()
    {
        yield return new WaitForSeconds(delayTime);
        anim.SetBool("Launch", true);
        launched = true;
    }
}
