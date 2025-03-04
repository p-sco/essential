using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class EssenceGameObject : MonoBehaviour
{
    public Essence essence;
    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(essence);
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        spriteRenderer.sprite = essence.sprite;
        gameObject.name = essence.displayName;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
