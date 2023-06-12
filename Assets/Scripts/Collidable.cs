using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour {

    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];
    public RoomFirstDungeonGen dungeonGen;

    private void Awake() {
        dungeonGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<RoomFirstDungeonGen>();
    }

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() {
        // Collisions

        //take box collider, search for things in collision with it, and add to hits array
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null)
                continue;


            OnCollide(hits[i]);

            //Array is not cleaned, so must clean ourselves
            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll) {
        //replace this when inherited
        Debug.Log("Collision not implemented for " + this.name);
    }
}
