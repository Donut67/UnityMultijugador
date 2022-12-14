using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour {

    public Image im;
    public bool selected;
    // Start is called before the first frame update
    void Start() {
        selected = false;
    }

    public void Select() {
        selected = !selected;
        im.enabled = !selected;
    }

    public void DisSelect() {
        selected = false;
        im.enabled = !selected;
    }
}
