using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Collidable {

    public string[] sceneNames;
    protected override void OnCollide(Collider2D coll) {
        if (coll.name == "Player_0") {

            //save the game
            GameManager.instance.SaveState();
            //Teleport player to random selected scene, make sure it is part of project
            string randomSceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            UnityEngine.SceneManagement.SceneManager.LoadScene(randomSceneName);

        }
    }

}
