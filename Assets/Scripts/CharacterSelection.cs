using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public List<int> llista;
    public List<GameObject> llistaButton;
    public int seleccio;
    public int player;
    public bool ready;

    private ClientHandler ch = null;
    
    // Start is called before the first frame update
    void Start() {
        llista = new List<int>();
        for(int i = 0; i < gameObject.transform.childCount; i++) {
            llistaButton.Insert(i, gameObject.transform.GetChild(i).gameObject);
        }

        ready = false;
        seleccio = -1;
    }

    private void Awake()
    {
        ch = GameObject.FindWithTag("Handler").GetComponent<ClientHandler>();
    }

    public void SetCurrentSelection(int pos) {
        ch.SendToServer("SELECT|" + pos);
        GameObject.FindWithTag("Jugador").GetComponent<TextMeshProUGUI>().text = "SELECT|" + player + "|" + pos;
    }

    public void SelectBox(int i, int j) {
        if(i == player) seleccio = j;
        llistaButton[j].GetComponent<SelectionBox>().Select(i);
    }
    public void DisSelectBox(int i) {
        llistaButton[i].GetComponent<SelectionBox>().DisSelect();
    }

    public void Ready() {
        if(!ready) {
            GameObject.FindWithTag("Jugador").GetComponent<TextMeshProUGUI>().text = "LLEST";
            ch.SendToServer("LLEST|0");
        }
        ready = true;
    }

    void SetPlayer(int i) {
        player = i;
    }
}
