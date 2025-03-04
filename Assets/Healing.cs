using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : Ability
{

    public float cooldown;
    public Sprite sprite;
    public float healing;
    private Transform playerTrans;
    private float currentCooldown;
    private float currentHealing;

    private void Start()
    {
        playerTrans = GameObject.Find("/Player").transform;
        PlayerController player = GameObject.Find("/Player").GetComponent<PlayerController>();
        currentHealing = healing + player.abilityPower * 2f;
        currentCooldown = System.Math.Max(5, cooldown - player.abilityCooldown);
        Invoke("DestroyParticles", 3.0f);
    }

    void Update()
    {
        transform.position = playerTrans.position;
    }

    override
    public void cast(Transform playerTransform, Vector2 direction)
    {
        Instantiate(this, playerTransform.position, playerTransform.rotation);
        Transform tmp = GameObject.Find("/UI").transform.Find("AbilityUI").transform.Find("AbilityPanel").transform.Find("CooldownText");
        tmp.SendMessage("SetCooldown", System.Math.Max(5, cooldown - GameObject.Find("/Player").GetComponent<PlayerController>().abilityCooldown));
        currentHealing = GameObject.Find("/Player").GetComponent<PlayerController>().abilityPower * 2 + healing;
        GameObject.Find("/Player").SendMessage("GainHealth", currentHealing);
    }

    override
    public float getCooldown()
    {
        return System.Math.Max(5, cooldown - GameObject.Find("/Player").GetComponent<PlayerController>().abilityCooldown); 
    }

    override
    public Sprite getSprite()
    {
        return sprite;
    }

    override
    public void endAbility()
    { }

    void DestroyParticles()
    {
        Destroy(gameObject);
    }

    override
    public void updateCooldown(float cdr)
    {
        currentCooldown = System.Math.Max(5, cooldown - cdr);
    }
}
