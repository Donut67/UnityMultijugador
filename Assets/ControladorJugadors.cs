using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJugadors : MonoBehaviour {
    
    private ClientHandler ch = null;
    [SerializeField] private GameObject P1Prefab;
    [SerializeField] private GameObject P2Prefab;
    [SerializeField] private GameObject P3Prefab;
    [SerializeField] private GameObject P4Prefab;

    /*void Awake() {
        ch = GameObject.FindWithTag("Handler").GetComponent<ClientHandler>();

        int quants = 0, pos = 0;
        foreach(int i in ch.seleccions) if(i != -1) quants ++;

        foreach(int i in ch.seleccions){
            if(i != -1) {
                GameObject go;

                if(i == 0) go = Instantiate(P1Prefab, gameObject.transform.position, Quaternion.identity);
                else if(i == 1) {
                    if(quants > 2) go = Instantiate(P2Prefab, gameObject.transform.position, Quaternion.identity);
                    else go = Instantiate(P3Prefab, gameObject.transform.position, Quaternion.identity);
                }else if(i == 2) go = Instantiate(P2Prefab, gameObject.transform.position, Quaternion.identity);
                else go = Instantiate(P4Prefab, gameObject.transform.position, Quaternion.identity);
                    
                go.GetComponent<PlayerMovement>().player = i + 1;
                if(pos == 0) go.GetComponent<PlayerMovement>().SetHabilitat("Doble salt");
                if(pos == 1) go.GetComponent<PlayerMovement>().SetHabilitat("Dash");
                if(pos == 2) go.GetComponent<PlayerMovement>().SetHabilitat("Ralentitzar");
                if(pos == 3) go.GetComponent<PlayerMovement>().SetHabilitat("Potencia");
                if(ch.jugador == i) go.GetComponent<PlayerMovement>().ControlPlayer();
            }
            pos ++;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
