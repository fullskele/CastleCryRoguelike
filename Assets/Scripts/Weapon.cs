using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable {

    // Damage structure
    public int[] damagePoint = {1, 2, 3, 4, 5, 6, 7};
    public float[] pushForce = {2.0f, 2.2f, 2.5f, 3f, 3.2f, 3.6f, 4.0f};

    // Upgrades
    private Animator anim;
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start() {
        
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();
        //KeyCode.Space is space
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (Time.time - lastSwing > cooldown) {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll) {
        if (coll.name == "Player_0") {
            return;
        }
        if (coll.tag == "Fighter") {
            Debug.Log(coll.name);

            //Create new damage object and send to hit fighter
            Damage dmg = new Damage() {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
        
    }

    private void Swing() {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon() {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        //change stats
    }

    public void SetWeaponLevel(int level) {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

    }
}