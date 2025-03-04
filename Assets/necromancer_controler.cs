using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class necromancer_controler : enemy
{
    public GameObject cast_obj;
    public Transform cast_spawn;
    public int attackRange = 5;
    private float timeToNextCast = 0f;
    private float castDelay = 0.6f;
    public float castSpeed = 2f;
    private float castLifetime = 0.8f;
    private GameObject instCast;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Debug.Log("Attack " + IsNearPlayer(attackRange));
        if(IsNearPlayer(attackRange))
        {
            Attack();
        }
    }

    private IEnumerator Cast()
    {
        yield return new WaitForSeconds(castDelay);
        Vector2 castDirection = rb.transform.position - playerRb.transform.position;
        isAttacking = false;
        
        instCast.GetComponent<Rigidbody2D>().AddForce(castDirection.normalized * -10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(castLifetime);
        Destroy(instCast);
    }
    override
    public void Attack()
    {
        if(CanAttack())
        {
            isAttacking = true;
            StartCoroutine(Cast());
            
            instCast = Instantiate(cast_obj, cast_spawn.position, cast_spawn.rotation);
            timeToNextCast = Time.time + 1f * castSpeed;
        }
    }

    private bool CanAttack()
    {
        return Time.time >= timeToNextCast;
    }
}
