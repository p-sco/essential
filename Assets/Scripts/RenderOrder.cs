using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RenderOrder : MonoBehaviour
{

    private TilemapRenderer tilemap;
    private Transform playerPosition;



    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<TilemapRenderer>();
        playerPosition = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            if(IsNear(child, playerPosition))
            {
                if(child.position.y < playerPosition.position.y)
                {
                    tilemap.sortingLayerName = "OverPlayer";
                } else
                {
                    tilemap.sortingLayerName = "UnderPlayer";
                }
            }
        }

    }

    private bool IsNear(Transform a, Transform b)
    {
        int MinDist = 0;
        int MaxDist = 3;
        return Vector3.Distance(a.position, b.position) >= MinDist && Vector3.Distance(a.position, b.position) <= MaxDist
            ? true
            : false;
    }
}
