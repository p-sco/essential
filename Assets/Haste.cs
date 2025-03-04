using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : Ability
{
    public float cooldown;
    public Sprite sprite;
    public float newAttackSpeed;
    private PlayerController player;
    private Transform playerTrans;
    private float duration;
    private float currentCooldown;
    private float currentDuration;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("/Player");
        playerTrans = playerObject.transform;
        player = playerObject.GetComponent<PlayerController>();
        player.baseAttackSpeed = player.attackSpeed;
        player.attackSpeed *= newAttackSpeed;
        currentDuration = duration + player.abilityPower;
        currentCooldown = System.Math.Max(5, cooldown - player.abilityCooldown);
        Invoke("endAbility", currentDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTrans.position;
    }

    override
    public void cast(Transform playerTransform, Vector2 direction)
    {
        Instantiate(this, playerTransform.position, playerTransform.rotation);
        playerTransform.Find("hand").transform.Find("weapon_anime_sword").GetComponent<SpriteRenderer>().color = Color.red;
        Transform tmp = GameObject.Find("/UI").transform.Find("AbilityUI").transform.Find("AbilityPanel").transform.Find("CooldownText");
        tmp.SendMessage("SetCooldown", System.Math.Max(5, cooldown - GameObject.Find("/Player").GetComponent<PlayerController>().abilityCooldown));
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
    {
        GameObject playerObject = GameObject.Find("/Player");
        playerObject.transform.Find("hand").transform.Find("weapon_anime_sword").GetComponent<SpriteRenderer>().color = Color.white;
        playerObject.GetComponent<PlayerController>().ResetAttackSpeed();
        Destroy(gameObject);
    }

    override
    public void updateCooldown(float cdr)
    {
        currentCooldown = System.Math.Max(5, cooldown - cdr);
    }
}
