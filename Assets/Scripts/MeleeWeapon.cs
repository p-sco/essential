using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private enemy enemy;
    public PlayerController player;
    public float knockback = 2;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.collider.tag);

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss") && player.isAttacking)
        {
            //Debug.Log("Hit");
            other.gameObject.GetComponent<enemy>().TakeDamage(player.attackDamage);
            Vector2 direction = other.GetComponent<Rigidbody2D>().transform.position - gameObject.transform.position;
            other.GetComponent<enemy>().Knockback(direction, knockback);
        }
    }
}
