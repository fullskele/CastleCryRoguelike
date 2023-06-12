using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : Collectable {

    protected float cooldown = 20f;

    protected override void OnCollect() {
        collected = true;
        //TODO: make collected become false after cooldown
    }
}
