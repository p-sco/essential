using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDemon : enemy
{
    public float attackTime = 1f;
    public float timeToAttack = 1f;
    public float timeAfterAttack = 1f;
    public int attackRange = 2;
    public float attackForce = 2f;
    public string bossLevel = "Pyro";
    private Vector3 attackDirection;
    private float timeToNextAttack = 0f;

    new void Start()
    {
        base.Start();
    }
    new void Update()
    {
        base.Update();

        
        

        if(IsNearPlayer(attackRange) && !isAttacking)
        {
            Attack();
        }

    }

    override
    public void Attack()
    {
        if (Time.time >= timeToNextAttack)
        {
            isAttacking = true;
            attackDirection = playerRb.transform.position - gameObject.transform.position;
            StartCoroutine(StartAttack());

            timeToNextAttack = Time.time + 1f * attackTime;
        }
    }

    private IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(timeToAttack);
        //Lunge
        rb.AddForce(attackDirection * attackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(timeAfterAttack);
        isAttacking = false;

    }
    
    

}
