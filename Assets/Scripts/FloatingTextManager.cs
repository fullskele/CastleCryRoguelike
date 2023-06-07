using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour {

    public GameObject textContainer;
    public GameObject textPrefrab;

    private List<FloatingText> floatingTextArray = new List<FloatingText>();

    private void Update() {
        foreach(FloatingText txt in floatingTextArray) {
            txt.UpdateFloatingText();
        }
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        
        FloatingText floatingText = GetFloatingText();
        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize * 3;
        floatingText.txt.color = color;

        floatingText.obj.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }
    private FloatingText GetFloatingText() {

        FloatingText txt = floatingTextArray.Find(t => !t.active);

        if (txt == null) {
            txt = new FloatingText();
            txt.obj = Instantiate(textPrefrab);
            txt.obj.transform.SetParent(textContainer.transform);
            txt.txt = txt.obj.GetComponent<Text>();

            floatingTextArray.Add(txt);
        }

        return txt;
    }

}
