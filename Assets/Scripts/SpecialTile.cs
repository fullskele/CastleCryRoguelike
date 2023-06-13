using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTile : Collidable {

    public float abilityCooldown = 10.0f;
    protected float lastActivate;

    protected override void Start() {
        base.Start();
        lastActivate = -abilityCooldown;
    }

    public void DeleteSelf() {
        Destroy(gameObject);
    }
}
