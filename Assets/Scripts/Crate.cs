using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    // Start is called before the first frame update
    protected override void Death() {
        Destroy(gameObject);
    }
}
