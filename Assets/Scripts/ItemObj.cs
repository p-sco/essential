using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemObj : MonoBehaviour, Pickupable
{
    public Item item;
    public float amplitude = 0.1f;
    public float frequency = 1f;

    Vector2 posOffset = new Vector2();
    Vector2 tempPos = new Vector2();
    Vector2 endPoint = new Vector2();
    bool doneMove = false;

    void Start()
    {
        endPoint.x = transform.position.x + Random.Range(-1.0f, 1.0f);
        endPoint.y = transform.position.y + Random.Range(-1.0f, 1.0f);
        Debug.Log(endPoint);
        posOffset = endPoint;
    }

    void Update()
    {
        if (!doneMove)
        {
            tempPos = Vector2.MoveTowards(transform.position, endPoint, 2 * Time.deltaTime);
            if (transform.position.x >= endPoint.x && transform.position.y >= endPoint.y)
                doneMove = true;
        }
        else
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        }

        transform.position = tempPos;
    }

    public Item Pickup()
    {
        Destroy(gameObject, 0.1f);
        return item;
    }
    



}
