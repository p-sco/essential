using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public float recoveryTime;
    public float knockback;
    protected Rigidbody2D rb;
    protected Rigidbody2D playerRb;
    protected PlayerController player;
    protected Vector2 direction;
    protected Animator anim;
    public bool isKnocked;
    protected float knockedTime;
    protected SpriteRenderer renderers;
    protected AudioSource takeDamageAudio;


    void Start()
    {

    }

    void Update()
    {


    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //player losing health
            other.gameObject.SendMessage("TakeDamage", 1);
            //player knockback
            direction = other.GetComponent<Rigidbody2D>().transform.position - gameObject.transform.position;
            other.GetComponent<PlayerController>().Knockback(direction, knockback);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        takeDamageAudio.Play();

        StartCoroutine(Flash());
        if (health <= 0)
        {
            Destroy(this.gameObject);
            if (gameObject.CompareTag("Boss"))
            {
                GameObject.Find("Grid").transform.Find("DoorBoss1").SendMessage("BossDead");
            }
        }
    }

    public IEnumerator Flash()
    {
        Color32 whateverColor = new Color32(255, 182, 182, 255); //edit r,g,b and the alpha values to what you want
        for (var n = 0; n < 3; n++)
        {
            renderers.material.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            renderers.material.color = whateverColor;
            yield return new WaitForSeconds(0.1f);
        }
        renderers.material.color = Color.white;
    }

    public void Knockback(Vector2 direction, float power)
    {
        isKnocked = true;
        rb.AddForce(direction * power, ForceMode2D.Impulse);
        knockedTime = Time.time + 1f / recoveryTime;
    }

    protected void Flip()
    {
        if (direction.x <= -0.5)
            rb.transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x > 0.5)
            rb.transform.localScale = new Vector3(1, 1, 1);
    }
}
