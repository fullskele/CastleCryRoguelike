using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter {

    private Vector3 originalSize;


    protected  BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    public float ySpeed = 3.0f;
    public float xSpeed = 4.0f;

    // Start is called before the first frame update
    protected virtual void Start() {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
    }

    protected virtual void UpdateMotor(Vector3 input) {

        //Reset moveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Swap sprite direction, moving left or right
        if (moveDelta.x > 0) {
            transform.localScale = originalSize;
        } else if (moveDelta.x < 0) {
            transform.localScale = new Vector3(-originalSize.x, originalSize.y, originalSize.z);
        }

        //Add push vector (if any)
        moveDelta += pushDirection;

        //Reduce pushforce eveery frame based on recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);


        //blocking movement on Y axis
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null) {

            //make it move!
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //blocking movement on X axis
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null) {

            //make it move!
            transform.Translate( moveDelta.x * Time.deltaTime, 0, 0);
        }

    }
}
