using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover {

    //Experience
    public int xpValue = 1;

    //Logic
    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    public RoomFirstDungeonGen dungeonGen;


    //Hitboxes
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start() {
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        //get first subchild, the 'hitbox'
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        base.Start();
    }

    private void Awake() {
        dungeonGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<RoomFirstDungeonGen>();
    }

    private void Update() {

        //Is player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength) {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength) {
                if(dungeonGen.isGeneratingLevel) {
                    DeleteSelf();
                }
                
                chasing = true;
            }

            if (chasing && !collidingWithPlayer) {
                //TODO: Fix knockback
                UpdateMotor((playerTransform.position - transform.position).normalized);
            } else if (!chasing) {
                //UpdateMotor(startingPosition - transform.position);
            }

        } else {
            //UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        //check for overlap  
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null)
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player_0") {
                collidingWithPlayer = true;
            }

            //Array is not cleaned, so must clean ourselves
            hits[i] = null;
        }
    }

    protected override void Death() {
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.showText("+" + xpValue + " XP!", 25, Color.magenta, transform.position, Vector3.up * 50, 1.3f);
        dungeonGen.UpdateEnemyCount(-1);
        Destroy(gameObject);
    }

    public void DeleteSelf() {
        dungeonGen.enemiesRemaining--;

        Destroy(gameObject);
    }
 
}
