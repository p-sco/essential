using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    public GameObject destroyEffect;
    public float speed;
    public float cooldown;
    public float damage;
    public Sprite sprite;
    public float knockback;
    public LayerMask whatIsSolid;
    private PlayerController player;
    private float currentCooldown;
    private float currentDamage;

    void Start()
    {
        player = GameObject.Find("/Player").GetComponent<PlayerController>();
        currentDamage = damage + player.abilityPower * 1.2f;
        currentCooldown = System.Math.Max(1, cooldown - player.abilityCooldown);
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 0.5f, whatIsSolid);
        if(hitInfo.collider != null){
            if(hitInfo.collider.gameObject.layer.Equals(9)){
                DestroyFireball();
            }
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.collider.tag);

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<enemy>().TakeDamage(currentDamage);
            Vector2 direction = other.GetComponent<Rigidbody2D>().transform.position - gameObject.transform.position;
            other.GetComponent<enemy>().Knockback(direction, knockback);
            DestroyFireball();
        }
    }

    void DestroyFireball(){
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    override
    public void cast(Transform playerTransform, Vector2 direction)
    {
        Transform tmp = GameObject.Find("UI").transform.Find("AbilityUI").transform.Find("AbilityPanel").transform.Find("CooldownText");
        PlayerController player = GameObject.Find("/Player").GetComponent<PlayerController>();

        Debug.Log("LastDirection: " + player.GetLastDirection());
        if (player.GetLastDirection() == 1)
        {
            Instantiate(this, playerTransform.position, playerTransform.rotation);
            tmp.SendMessage("SetCooldown", System.Math.Max(1, cooldown - player.abilityCooldown));
        }
        else if (player.GetLastDirection() == 2)
        {
            Instantiate(this, playerTransform.position, playerTransform.rotation * Quaternion.Euler(0f, 0f, -180f));
            tmp.SendMessage("SetCooldown", System.Math.Max(1, cooldown - player.abilityCooldown));
        }
        else if (player.GetLastDirection() == 3)
        {
            Instantiate(this, playerTransform.position, playerTransform.rotation * Quaternion.Euler(0f, 0f, -90f));
            tmp.SendMessage("SetCooldown", System.Math.Max(1, cooldown - player.abilityCooldown));
        }
        else if (player.GetLastDirection() == 4)
        {
            Instantiate(this, playerTransform.position, playerTransform.rotation * Quaternion.Euler(0f, 0f, -270f));
            tmp.SendMessage("SetCooldown", System.Math.Max(1, cooldown - player.abilityCooldown));
        }
        else
        {
            Instantiate(this, playerTransform.position, playerTransform.rotation);
            tmp.SendMessage("SetCooldown", System.Math.Max(1, cooldown - player.abilityCooldown));
        }
    }

    public override void endAbility()
    {
        
    }

    override
    public float getCooldown()
    {
        return System.Math.Max(1, cooldown - GameObject.Find("/Player").GetComponent<PlayerController>().abilityCooldown);
    }

    override
    public Sprite getSprite()
    {
        return sprite;
    }

    override
    public void updateCooldown(float cdr)
    {
        currentCooldown = System.Math.Max(1, cooldown - cdr);
    }
}
