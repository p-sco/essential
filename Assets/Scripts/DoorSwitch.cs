using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{

    Collider2D door;
    private SpriteRenderer sprite;
    public Sprite doorOpen;
    private Transform player;
    public bool isBossDoor = false;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = transform.Find("Door").GetComponent<SpriteRenderer>();
        door = GetComponent<Collider2D>();
        door.isTrigger = false;
        player = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > gameObject.transform.position.y + 0.5f)
        {
            sprite.sortingLayerName = "OverPlayer";
        }
        else
        {
            sprite.sortingLayerName = "UnderPlayer";
        }
    }

    public void BossDead()
    {
        door.isTrigger = true;
        if (isBossDoor == true)
        {
            sprite.sprite = doorOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isBossDoor == false)
        {
            door.isTrigger = true;
            sprite.sprite = doorOpen;
        }

    }
}
