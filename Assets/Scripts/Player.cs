using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Mover {
    private SpriteRenderer spriteRenderer;

    //logic
    private bool isAlive = true;

    //controls
    private Vector2 pointerInput, movementInput;
    private AgentMover agentMover;
    public Vector2 PointerInput => pointerInput;
    [SerializeField]
    private InputActionReference movement, attack, pointerPos;

    private WeaponParent weaponParent;


    protected override void Start () {
        base.Start();
        weaponParent = GetComponentInChildren<WeaponParent>();
        agentMover = GetComponent<AgentMover>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (!isAlive)
            return;

        weaponParent.pointerPos = pointerInput;
        pointerInput = GetPointerInput();
        movementInput = movement.action.ReadValue<Vector2>();
        agentMover.MovementInput = movementInput;
    }

    private void OnEnable() {
        attack.action.performed += PerformAttack;
    }

    private void OnDisable() {
        attack.action.performed -= PerformAttack;
    }

    private void PerformAttack(InputAction.CallbackContext obj) {
        weaponParent.Attack();
        
    }

    private Vector2 GetPointerInput () {
        Vector3 mousePos = pointerPos.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void SwapSprite(int skinID) {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }
    public void OnLevelUp() {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }
    public void SetLevel(int level) {
        for (int i = 0; i < level; i++) {
            OnLevelUp();
        }
    }
    protected override void ReceiveDamage(Damage dmg) {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }
    public void Heal(int healingAmount) {
        if (hitpoint == maxHitpoint)
            return;
        
        hitpoint += healingAmount;
        if (hitpoint > maxHitpoint) 
            hitpoint = maxHitpoint;
        GameManager.instance.showText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }
    public void Respawn() {
        Heal(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }

    protected override void Death() {
        isAlive = false;
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
    }
}
