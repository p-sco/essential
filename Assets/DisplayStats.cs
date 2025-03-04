using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayStats : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI attackDMG;
    public TextMeshProUGUI attackSPEED;
    public TextMeshProUGUI healthHP;
    public TextMeshProUGUI movSPEED;
    public TextMeshProUGUI spellPOWER;
    public TextMeshProUGUI spellCD;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStats()
    {
        attackDMG.SetText(player.attackDamage.ToString());
        attackSPEED.SetText(player.attackSpeed.ToString());
        healthHP.SetText(player.maxHealth.ToString());
        movSPEED.SetText(player.speed.ToString());
        spellPOWER.SetText(player.abilityPower.ToString());
        spellCD.SetText(player.abilityCooldown.ToString());

    }
}
