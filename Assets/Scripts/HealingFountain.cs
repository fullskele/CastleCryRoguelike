using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : SpecialTile
{
    // Start is called before the first frame update

    [SerializeField]
    private int healingAmount = 1;

    protected override void OnCollide(Collider2D coll) {
        if (coll.name != "Player_0") {
            return;
        }
        
        if (Time.time - lastActivate > abilityCooldown) {
            lastActivate = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }

    }
}
