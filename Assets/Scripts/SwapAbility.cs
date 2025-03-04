using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAbility : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void SelectFireball()
    {
        player.selectedAbility = player.spells[0].GetComponent<Ability>();
    }

    public void SelectHealing()
    {
        player.selectedAbility = player.spells[1].GetComponent<Ability>();
    }

    public void SelectHaste()
    {
        player.selectedAbility = player.spells[2].GetComponent<Ability>();
    }

    public void SelectInvulnerability()
    {
        player.selectedAbility = player.spells[3].GetComponent<Ability>();
    }
}
