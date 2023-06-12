using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable {

    //Logic
    protected bool collected;

    protected override void OnCollide(Collider2D coll) {
        if (coll.name == "Player_0") {
            if (dungeonGen.isGeneratingLevel) {
                DeleteSelf();
            }
            OnCollect();
        }
    }

    protected virtual void OnCollect() {
        collected = true;
    }

    public void DeleteSelf() {
        Destroy(gameObject);
    }
}
