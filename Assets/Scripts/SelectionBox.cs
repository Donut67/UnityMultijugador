using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionBox : MonoBehaviour {

    public Image im;
    public Image fons;
    public bool selected;
    private CharacterSelection cs = null;
    private int position;
    private Color[] colors = {Color.cyan, Color.red, Color.yellow, Color.green};
    // Start is called before the first frame update
    void Start() {
        selected = false;
    }

    void Awake() {
        GameObject go = GameObject.FindWithTag("Selector");
        cs = go.GetComponent<CharacterSelection>();
        for(int i = 0; i < go.transform.childCount; i++) {
            if(GameObject.ReferenceEquals(go.transform.GetChild(i).gameObject, gameObject)) position = i;
        }
    }

    public void TrySelect() {
        cs.SetCurrentSelection(position);
    }

    public void Select(int j) {
        selected = true;
        im.enabled = false;
        fons.color = colors[j];
    }

    public void DisSelect() {
        selected = false;
        im.enabled = true;
    }
}
