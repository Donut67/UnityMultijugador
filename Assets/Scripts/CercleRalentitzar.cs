using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CercleRalentitzar : MonoBehaviour {
    public float temps = 0.5f;

    void Update(){
        temps -= Time.deltaTime;
        if(temps <= 0.0f) Destroy(this);
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().RalentitzarJugador(temps);
        }
    }

}
