using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreBoss : Enemy
{
    public float attackTime = 1f;
    public float timeToAttack = 1f;
    public float timeAfterAttack = 1f;
    public float patrolInterval = 2f;
    public int attackRange = 2;
    public float attackForce = 2f;
    public float viewDistance = 5f;
    public float viewAngle = 70f;
    private float timeToNextAttack = 0f;
    private float timeToNextPatrol = 0f;
    private bool isAttacking = false;
    private Vector3 attackDirection;
    private bool playerFound = false;
    private int layerMask = 1 << 9;
    private Vector3 lastSeenPos = Vector3.zero;
    private Vector2 patrolPoint;
    CircleCollider2D collider;


    void Start()
    {
        //layerMask = ~layerMask;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        renderers = gameObject.GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();

    }
    void Update()
    {
        playerFound |= FindThePlayer();

        if(playerFound) {


            if (!Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + collider.offset.y), playerRb.transform.position, layerMask)) {
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + collider.offset.y), playerRb.transform.position, Color.green);
                lastSeenPos = playerRb.transform.position;
                if (!isKnocked && !isAttacking)
                {
                    //rb.MovePosition(moveTowardsPlayers);
                    MoveTowardsPoint(playerRb.position);
                }
            } else {
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + collider.offset.y), playerRb.transform.position, Color.red);
                if (!isKnocked)
                {
                    //rb.MovePosition(moveTowardsPlayers);
                    MoveTowardsPoint(lastSeenPos);
                }
                if(rb.transform.position == lastSeenPos) {
                    playerFound = false;
                }
            }

        } else
        {
            Patrol();
        }

        
        if(isKnocked && Time.time >= knockedTime) {
            isKnocked = false;
            rb.velocity = Vector2.zero;
        }

        if(IsNear(attackRange) && !isAttacking)
        {
            if(Time.time >= timeToNextAttack)
            {
                isAttacking = true;
                attackDirection = playerRb.transform.position - gameObject.transform.position;
                StartCoroutine(Attack());
                
                timeToNextAttack = Time.time + 1f * attackTime;
            }
        }

        base.Flip();
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeToAttack);
        //Lunge
        rb.AddForce(attackDirection * attackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(timeAfterAttack);
        isAttacking = false;

    }
    
    private bool IsNear(int dist)
    {
        int MinDist = 0;
        int MaxDist = dist;
        return Vector3.Distance(playerRb.transform.position, rb.transform.position) >= MinDist && Vector3.Distance(playerRb.transform.position, rb.transform.position) <= MaxDist
            ? true
            : false;
    }

    bool FindThePlayer()

    {
        if (Vector3.Distance(transform.position, playerRb.transform.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerRb.transform.position - transform.position).normalized;
            float direct = 1f;
            if(direction.x < 0)
            {
                direct = -1f;
            }
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.right * direct, directionToPlayer);
           // if (angleBetweenGuardAndPlayer < viewAngle / 2)
            if(true)
            {
                if (!Physics.Linecast(transform.position, playerRb.transform.position))
                {

                    return true;
                }

            }
        }
        return false;
    }
   
    void MoveTowardsPoint(Vector2 location)
    {
        

        Vector2 pointToLocation = new Vector2(location.x - transform.position.x, location.y - transform.position.y);
        direction = pointToLocation.normalized;
        float radius = collider.radius;

        Vector2 perRight = Vector2.Perpendicular(pointToLocation).normalized * radius;
        Vector2 perLeft = Vector2.Perpendicular(pointToLocation).normalized * -radius;
        
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + collider.offset.y), pointToLocation);
        Debug.DrawRay( new Vector2(perRight.x + transform.position.x, perRight.y + transform.position.y + collider.offset.y), pointToLocation, Color.yellow);
        Debug.DrawRay(new Vector2(perLeft.x + transform.position.x, perLeft.y + transform.position.y + collider.offset.y), pointToLocation, Color.blue);

        RaycastHit2D rightBound = Physics2D.Raycast(new Vector2(perRight.x + transform.position.x, perRight.y + transform.position.y + collider.offset.y), pointToLocation, Mathf.Infinity, layerMask);
        RaycastHit2D leftBound = Physics2D.Raycast(new Vector2(perLeft.x + transform.position.x, perLeft.y + transform.position.y + collider.offset.y), pointToLocation, Mathf.Infinity, layerMask);

        
        if (rightBound.distance < pointToLocation.magnitude || leftBound.distance < pointToLocation.magnitude) //Adjust Direction
        {
           
            if(rightBound.distance > leftBound.distance)
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
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
                }
                base.Flip();
                anim.SetFloat("speed", Mathf.Abs(moveTowardsPlayers.normalized.magnitude));
                rb.MovePosition(moveTowardsPlayers);
            } else
            {
                float percent = (1 - (rightBound.distance / pointToLocation.magnitude));
                
                Vector2 target = pointToLocation + location + (Vector2.Perpendicular(pointToLocation) * percent * -1);
                Vector2 moveTowardsPlayers;
                if (rightBound.distance < 0.8f)
                {
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, rb.position + Vector2.Perpendicular(pointToLocation) * -1, speed * Time.deltaTime);
                }
                else
                {
                    moveTowardsPlayers = Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime);
                }
                base.Flip();
                anim.SetFloat("speed", Mathf.Abs(moveTowardsPlayers.normalized.magnitude));
                rb.MovePosition(moveTowardsPlayers);
            }
        } else
        {

            Vector2 moveTowardsPlayers = Vector2.MoveTowards(rb.position, location, speed * Time.deltaTime);
            base.Flip();
            anim.SetFloat("speed", Mathf.Abs(moveTowardsPlayers.normalized.magnitude));
            rb.MovePosition(moveTowardsPlayers);
        }





    }

    void Patrol()
    {
        if(Time.time >= timeToNextPatrol)
        { 
            timeToNextPatrol = Time.time + patrolInterval;
            patrolPoint = new Vector2(rb.position.x + Random.Range(-2f, 2f), rb.position.y + Random.Range(-2f, 2f));
            Collider2D point = Physics2D.OverlapPoint(patrolPoint);
            if(point != null)
            {

                while (point.gameObject.layer != 10 )
                {
                    patrolPoint = new Vector2(rb.position.x + Random.Range(-2f, 2f), rb.position.y + Random.Range(-2f, 2f));
                    point = Physics2D.OverlapPoint(patrolPoint);
                    if(point == null)
                    {
                        patrolPoint = rb.position;
                        break; 
                    }
                }
            } else
            {
                patrolPoint = rb.position;
            }
        }

        MoveTowardsPoint(patrolPoint);
        if(rb.position == patrolPoint)
        {
            anim.SetFloat("speed", 0);
        }
        
    }
}
