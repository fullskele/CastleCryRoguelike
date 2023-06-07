using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNPC : Collidable
{
    public string message;
    private float cooldown = 4.0f;
    private float lastSpeak;

    protected override void Start() {
        base.Start();
        lastSpeak = -cooldown;
    }
    protected override void OnCollide(Collider2D coll) {
        if (Time.time - lastSpeak < cooldown)
            return;

        lastSpeak = Time.time;
        GameManager.instance.showText(message, 25, Color.white, transform.position + new Vector3(0, 0.8f, 0), Vector3.zero, cooldown);
        Debug.Log("Spoke");
        
    }
}
