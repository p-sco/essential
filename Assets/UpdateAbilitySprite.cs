using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAbilitySprite : MonoBehaviour
{
    private GameObject player;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("/Player");
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerController>().selectedAbility.getSprite().Equals(image.sprite))
        {
            image.sprite = player.GetComponent<PlayerController>().selectedAbility.getSprite();
        }
    }
}
