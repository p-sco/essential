using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;



public abstract class enemy : MonoBehaviour
{
    [System.Serializable]
    public class Droppings
    {
        public Essence essence;
        public float chance;
    }
    public int MaxDrop;
    public int MinDrop;
    public float speed;
    public float health;
    public float recoveryTime;
    public float knockback;
    public float patrolInterval = 2f;
    public float viewDistance = 5f;
    public float viewAngle = 70f;
    public int keepDistance = 0;
    [SerializeField]
    public List<Droppings> droppings;
    protected bool isKnocked;
    protected Rigidbody2D rb;
    protected Rigidbody2D playerRb;
    protected Transform playerPos;
    protected PlayerController player;
    protected Vector2 direction;
    protected Animator anim;
    protected float knockedTime;
    protected SpriteRenderer renderers;
    protected float timeToNextPatrol = 0f;
    protected bool playerFound = false;
    protected int layerMask = 1 << 9;
    protected Vector3 lastSeenPos = Vector3.zero;
    protected Vector2 patrolPoint;
    protected bool isAttacking = false;
    protected CircleCollider2D colliders;
    protected AudioSource takeDamageAudio;


    // Start is called before the first frame update
    protected void Start()
    {

        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        renderers = gameObject.GetComponent<SpriteRenderer>();
        colliders = GetComponent<CircleCollider2D>();
        takeDamageAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isKnocked && Time.time >= knockedTime)
        {
            isKnocked = false;
            rb.velocity = Vector2.zero;
        }

        playerFound |= FindThePlayer();
        if (playerFound)
        {
            
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }
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

    public void TakeDamage(float damage)
    {
        health -= damage;

        StartCoroutine(Flash());
        takeDamageAudio.Play();
        if (health <= 0)
        {
            DropDroppings();

            if (gameObject.CompareTag("Boss"))
            {

                GameObject.Find("Grid").transform.Find("DoorBoss1").SendMessage("BossDead");

                string bossName = gameObject.GetComponent<BigDemon>().bossLevel;

                GameObject.Find("Player").SendMessage("BossKilled", bossName);
            }

            Destroy(gameObject);
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

    void MoveTowardsPoint(Vector2 location)
    {


        Vector2 pointToLocation = new Vector2(location.x - transform.position.x, location.y - transform.position.y);
        direction = pointToLocation.normalized;
        float radius = colliders.radius;

        Vector2 perRight = Vector2.Perpendicular(pointToLocation).normalized * radius;
        Vector2 perLeft = Vector2.Perpendicular(pointToLocation).normalized * -radius;

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + colliders.offset.y), pointToLocation);
        Debug.DrawRay(new Vector2(perRight.x + transform.position.x, perRight.y + transform.position.y + colliders.offset.y), pointToLocation, Color.yellow);
        Debug.DrawRay(new Vector2(perLeft.x + transform.position.x, perLeft.y + transform.position.y + colliders.offset.y), pointToLocation, Color.blue);

        RaycastHit2D rightBound = Physics2D.Raycast(new Vector2(perRight.x + transform.position.x, perRight.y + transform.position.y + colliders.offset.y), pointToLocation, Mathf.Infinity, layerMask);
        RaycastHit2D leftBound = Physics2D.Raycast(new Vector2(perLeft.x + transform.position.x, perLeft.y + transform.position.y + colliders.offset.y), pointToLocation, Mathf.Infinity, layerMask);


        if (rightBound.distance < pointToLocation.magnitude || leftBound.distance < pointToLocation.magnitude) //Adjust Direction
        {

            if (rightBound.distance > leftBound.distance)
            {
                float percent = (1 - (leftBound.distance / pointToLocation.magnitude));
                Vector2 target = pointToLocation + location + (Vector2.Perpendicular(pointToLocation) * percent);
                Vector2 moveTowardsPlayers = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (leftBound.distance < 0.8f)
                {
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, rb.position + Vector2.Perpendicular(pointToLocation), speed * Time.deltaTime);
                }
                else
                {
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, target * (IsNearPlayer(keepDistance) ? -1 : 1), speed * Time.deltaTime);
                }
                Flip();
                anim.SetFloat("speed", Mathf.Abs(moveTowardsPlayers.normalized.magnitude));
                rb.MovePosition(moveTowardsPlayers);
            }
            else
            {
                float percent = (1 - (rightBound.distance / pointToLocation.magnitude));

                Vector2 target = pointToLocation + location + (Vector2.Perpendicular(pointToLocation) * percent * -1);
                Vector2 moveTowardsPlayers;
                if (rightBound.distance < 0.8f)
                {
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, rb.position + Vector2.Perpendicular(pointToLocation) * -1, speed * Time.deltaTime) * (IsNearPlayer(keepDistance) ? -1 : 1);
                }
                else
                {
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, target * (IsNearPlayer(keepDistance) ? -1 : 1), speed * Time.deltaTime);
                }
                Flip();
                anim.SetFloat("speed", Mathf.Abs(moveTowardsPlayers.normalized.magnitude));
                rb.MovePosition(moveTowardsPlayers);
            }
        }
        else
        {
            Vector2 moveTowardsPlayers = Vector2.MoveTowards(rb.position, location * (IsNearPlayer(keepDistance) ? -1 : 1), speed * Time.deltaTime);
            Flip();
            anim.SetFloat("speed", Mathf.Abs(moveTowardsPlayers.normalized.magnitude));
            rb.MovePosition(moveTowardsPlayers);
        }
    }

    void Patrol()
    {
        if (Time.time >= timeToNextPatrol)
        {
            timeToNextPatrol = Time.time + patrolInterval;
            patrolPoint = new Vector2(rb.position.x + Random.Range(-2f, 2f), rb.position.y + Random.Range(-2f, 2f));
            Collider2D point = Physics2D.OverlapPoint(patrolPoint);
            if (point != null)
            {

                while (point.gameObject.layer != 10)
                {
                    patrolPoint = new Vector2(rb.position.x + Random.Range(-2f, 2f), rb.position.y + Random.Range(-2f, 2f));
                    point = Physics2D.OverlapPoint(patrolPoint);
                    if (point == null)
                    {
                        patrolPoint = rb.position;
                        break;
                    }
                }
            }
            else
            {
                patrolPoint = rb.position;
            }
        }

        MoveTowardsPoint(patrolPoint);
        if (rb.position == patrolPoint)
        {
            anim.SetFloat("speed", 0);
        }

    }

    bool FindThePlayer()

    {
        if (Vector3.Distance(transform.position, playerRb.transform.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerRb.transform.position - transform.position).normalized;
            float direct = 1f;
            if (direction.x < 0)
            {
                direct = -1f;
            }
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.right * direct, directionToPlayer);
            // if (angleBetweenGuardAndPlayer < viewAngle / 2)
            if (true)
            {
                if (!Physics.Linecast(transform.position, playerRb.transform.position))
                {

                    return true;
                }

            }
        }
        return false;
    }

    void MoveTowardsPlayer()
    {
        if (!Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + colliders.offset.y), playerRb.transform.position, layerMask))
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + colliders.offset.y), playerRb.transform.position, Color.green);
            lastSeenPos = playerRb.transform.position;
            if (!isKnocked && !isAttacking)
            {
                //rb.MovePosition(moveTowardsPlayers);
                MoveTowardsPoint(playerRb.position);
            }
        }
        else
        {
            Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + colliders.offset.y), playerRb.transform.position, Color.red);
            if (!isKnocked && !isAttacking)
            {
                //rb.MovePosition(moveTowardsPlayers);
                MoveTowardsPoint(lastSeenPos);
            }
            if (rb.transform.position == lastSeenPos)
            {
                playerFound = false;
            }
        }
    }

    protected bool IsNearPlayer(int dist)
    {
        int MinDist = 0;
        int MaxDist = dist;
        return Vector3.Distance(playerRb.transform.position, rb.transform.position) >= MinDist && Vector3.Distance(playerRb.transform.position, rb.transform.position) <= MaxDist
            ? true
            : false;
    }

    protected void DropDroppings()
    {
        int dropAmount = (int)Random.Range(MinDrop, MaxDrop);
        for(int i = 0; i < dropAmount; i++)
        {
            float dropChance = Random.Range(0f, 1f);
            float dropChanceCount = 0f;
            Debug.Log(dropChance);
            foreach (Droppings drop in droppings)
            {
                dropChanceCount += drop.chance;
                
                if(dropChance <= dropChanceCount)
                {
                    Vector2 dropPos = new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));

                    drop.essence.Drop(dropPos);
                    break;
                }
            }
        }
    }

    public abstract void Attack();
}
