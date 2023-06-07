using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGen), true)]
public class RandomDungeonGenEditor : Editor {

    AbstractDungeonGen generator;

    private void Awake() {
        generator = (AbstractDungeonGen)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon")) {
            generator.GenerateDungeon();
        }
    }
}
