using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public List<int> llista;
    public List<GameObject> llistaButton;
    public int seleccio;
    
    // Start is called before the first frame update
    void Start() {
        llista = new List<int>();
        for(int i = 0; i < gameObject.transform.childCount; i++) {
            llistaButton.Insert(i, gameObject.transform.GetChild(i).gameObject);
        }

        seleccio = -1;
    }

    // Update is called once per frame
    void Update() {
        for(int i = 0; i < llistaButton.Count; i++) {
            // llistaButton[i].GetComponent<SelectionBox>().selected;
        }
    }
}
