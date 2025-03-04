
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public float speed = 5.0f;
    public float attackSpeed = 1.0f;
    public float baseAttackSpeed = 1.0f;
    public float attackDamage = 5.0f;
    public float abilityCooldown = 0f;
    public float maxHealth = 100.0f;
    public float abilityPower = 5.0f;

    private int darkEssence = 0;
    private int lightEssence = 0;
    public float health;
    public bool isAttacking;
    public HealthBar healthBar;
    public AudioClip attack;
    public AudioClip hurt;
    public AudioClip cast;
    public AudioClip death;
    public List<GameObject> spells;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private Vector2 direction;
    private Vector2 attackDirection;
    private Animator anim;
    private bool facingRight;
    private float nextTimeToAttack = 0f;
    public bool isKnocked;
    protected float knockedTime;
    public float recoveryTime;
    protected SpriteRenderer renderers;
    private AudioSource source;
    private float timeBtwAbility = 0f;
    public Ability selectedAbility;
    private bool onCooldown;
    private bool abilityActivated;
    public TextMeshProUGUI darkEssenceText;
    public TextMeshProUGUI lightEssenceText;
    private bool isColliding = false;
    private bool isInvincible;

    //bosses
    private bool boss1Dead;
    private bool boss2Dead;
    private bool boss3Dead;
    private bool boss4Dead;

    //Last Direction
    // 1 == up
    // 2 == down
    // 3 == right
    // 4 == left
    private int lastDirection;

    void Awake()
    {
        selectedAbility = spells[0].GetComponent<Ability>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (selectedAbility == null)
        {
            selectedAbility = spells[0].GetComponent<Ability>();
        }

        selectedAbility.endAbility();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    { 
        isInvincible = false;
        health = maxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        healthBar.SetTotalHealth(health);
        renderers = gameObject.GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        onCooldown = false;
        abilityActivated = false;
        maxHealth = health;
        lastDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        isColliding = false;
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        attackDirection = new Vector2(Input.GetAxisRaw("AttackStickX"), Input.GetAxisRaw("AttackStickY"));
        anim.SetFloat("speed", Mathf.Abs(direction.magnitude));

        velocity = direction.normalized * speed;

        if (direction.normalized.y > 0)
            lastDirection = 1;
        else if (direction.normalized.y < 0)
            lastDirection = 2;
        else if (direction.normalized.x > 0)
            lastDirection = 3;
        else if (direction.normalized.x < 0)
            lastDirection = 4;

        if (isKnocked && Time.time >= knockedTime)
        {
            isKnocked = false;
            rb.velocity = Vector2.zero;
        }

        Flip();

        if(attackDirection.normalized.x >= 1.0f)
        {
            if(Time.time >= nextTimeToAttack)
            {
                anim.SetTrigger("Attack");
                rb.transform.localScale = new Vector3(1, 1, 1);
                facingRight = true;           
                nextTimeToAttack = Time.time + 1f / attackSpeed;
                source.clip = attack;
                source.Play();
                isAttacking = true;
            }
        } 
        else if(attackDirection.normalized.x <= -1.0f)
        {
            if(Time.time >= nextTimeToAttack)
            {
                anim.SetTrigger("Attack");
                rb.transform.localScale = new Vector3(-1, 1, 1);
                facingRight = true;
                nextTimeToAttack = Time.time + 1f / attackSpeed;
                source.clip = attack;
                source.Play();
                isAttacking = true;
            }
        } 
        else if (attackDirection.normalized.y >= 1.0f)
        {
            if (Time.time >= nextTimeToAttack)
            {
                anim.SetTrigger("Attack-Up");
                nextTimeToAttack = Time.time + 1f / attackSpeed;
                source.clip = attack;
                source.Play();
                isAttacking = true;
            }
        } 
        else if (attackDirection.normalized.y <= -1.0f)
        {
            if (Time.time >= nextTimeToAttack)
            {
                anim.SetTrigger("Attack-Down");
                nextTimeToAttack = Time.time + 1f / attackSpeed;
                source.clip = attack;
                source.Play();
                isAttacking = true;
            }
        }


        //=-=-=-=-=-=-=-=- ABILITY STUFF =-=-=-=-=-=-=-=-

        //FIREBALL
        if (timeBtwAbility <= 0)
        {
            //Change to work with controller input 
            if(Input.GetKey("space"))
            {
                selectedAbility.cast(transform, direction.normalized);
                timeBtwAbility = selectedAbility.getCooldown();
                source.clip = cast;
                source.Play();
            }
        }
        else
        {
            timeBtwAbility -= Time.deltaTime;
        }

        //=-=-=-=-=-=-=-=- ABILITY STUFF =-=-=-=-=-=-=-=-
    }

    private void FixedUpdate()
    {
        if (!isKnocked)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void Flip()
    {
        Debug.Log(isAttacking);
        if (direction.normalized.x <= -0.5)
        {
            
            if (Time.time >= nextTimeToAttack)
            {
                facingRight = false;
                rb.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (direction.normalized.x > 0.5)
        {
            if (Time.time >= nextTimeToAttack)
            {
                facingRight = true;
                rb.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            health -= damage;
            healthBar.UpdateHealth(health);
            StartCoroutine(Flash());
            if (health <= 0)
            {
                source.clip = death;
                source.Play();
                health = maxHealth;
                healthBar.UpdateHealth(health);
                darkEssence = 0;
                lightEssence = 0;
                darkEssenceText.text = darkEssence.ToString();
                lightEssenceText.text = lightEssence.ToString();
                GameObject.Find("StatsUI").SendMessage("UpdateStats");

                SceneManager.LoadScene(1);

            }
            else
            {
                source.clip = hurt;
                source.Play();
            }
            isInvincible = true;
            Invoke("SetInvincibleFalse", 0.5f);
        }
    }

    public void Upgrade(int up)
    {

        if(up == 1)
        {
            if(darkEssence >= 5)
            {
                attackDamage = attackDamage + 3.0f;
                darkEssence -= 5;
            }
        } else if(up == 2)
        {
            if (lightEssence >= 5)
            {
                attackSpeed = attackSpeed + 0.3f;
                lightEssence -= 5;
            }
        } else if(up == 3)
        {
            if (darkEssence >= 5)
            {
                speed = speed + 0.2f;
                darkEssence -= 5;
            }
        } else if(up == 4)
        {
            if (lightEssence >= 5)
            {
                maxHealth = maxHealth + 5f;
                lightEssence -= 5;
            }

        } else if(up == 5)
        {
            if (darkEssence >= 5)
            {
                abilityPower = abilityPower + 0.5f;
                darkEssence -= 5;
            }

        } else if(up == 6)
        {
            if (lightEssence >= 5)
            {
                abilityCooldown = abilityCooldown + 0.5f;
                lightEssence -= 5;
                selectedAbility.updateCooldown(abilityCooldown);
            }
            
        }
        darkEssenceText.text = darkEssence.ToString();
        lightEssenceText.text = lightEssence.ToString();
        GameObject.Find("StatsUI").SendMessage("UpdateStats");
    }

    public void GainHealth(int healing){
        health = System.Math.Min(maxHealth,health+healing);
        healthBar.UpdateHealth(health);
    }

    public void Knockback(Vector2 direction, float power)
    {
        isKnocked = true;
        rb.AddForce(direction * power, ForceMode2D.Impulse);
        knockedTime = Time.time + 1f / recoveryTime;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isColliding) return;
        isColliding = true;
        if (collision.gameObject.tag == "Essence")
            {
            Essence tmp = collision.gameObject.GetComponent<EssenceGameObject>().essence;
            Destroy(collision.gameObject);
            if(tmp.type == EssenceType.Dark)
            {
                darkEssence++;
                darkEssenceText.text = darkEssence.ToString();
            } else if(tmp.type == EssenceType.Light)
            {
                lightEssence++;
                lightEssenceText.text = lightEssence.ToString();
            }

        }
    }

    public void SetInvincible(bool isInvincible)
    {
        this.isInvincible = isInvincible;   
    }

    public void ResetAttackSpeed()
    {
        this.attackSpeed = baseAttackSpeed;
    }

    public void SetInvincibleFalse()
    {
        isInvincible = false;
    }

    public void BossKilled(string boss)
    {
        if (boss == "Nightmare")
        {
            boss1Dead = true;
            Debug.Log("Boss1Dead");
        }
            
        else if (boss == "Catacombs")
        {
            boss2Dead = true;
            Debug.Log("Boss2Dead");
        }
            
        else if (boss == "Pyre")
        {
            boss3Dead = true;
            Debug.Log("Boss3Dead");
        }
            
        else if (boss == "EssenceOfWater")
        {
            boss4Dead = true;
            Debug.Log("Boss4Dead");
        }
            

        if(boss1Dead && boss2Dead && boss3Dead && boss4Dead)
        {
            SceneManager.LoadScene(8);
            Debug.Log("AllBossesDead");
        }
    }

    public int GetLastDirection(){
        return lastDirection;
    }

}
