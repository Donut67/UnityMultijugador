using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour{
    [SerializeField] private List<Transform> playerList = new List<Transform>();

    // Start is called before the first frame update
    void Awake() {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Jugador");
        foreach(GameObject g in gos) {
            playerList.Add(g.transform);
        }
    }

    // Update is called once per frame
    void Update() {

        // Vector2 extrem1 = new Vector2(0.0f, 0.0f);
        // Vector2 extrem2 = new Vector2(0.0f, 0.0f);
        // Vector2 extrem3 = new Vector2(0.0f, 0.0f);
        // Vector2 extrem4 = new Vector2(0.0f, 0.0f);

        // foreach(Transform t in playerList){
        //     if(extrem1.x > t.position.x) extrem1 = t.position;
        //     if(extrem2.y < t.position.y) extrem2 = t.position;
        //     if(extrem3.x < t.position.x) extrem3 = t.position;
        //     if(extrem4.y > t.position.y) extrem4 = t.position;
        // }

        // Vector2 resultant = extrem1;
        // int count = 1;
        // if(extrem1 != extrem2) {
        //     resultant += extrem2;
        //     count ++;
        // }
        // if(extrem1 != extrem3 && extrem2 != extrem3) {
        //     resultant += extrem3;
        //     count ++;
        // }
        // if(extrem1 != extrem4 && extrem2 != extrem4 && extrem3 != extrem4) {
        //     resultant += extrem4;
        //     count++;
        // }
        // Debug.Log(count);

        Vector2 resultant = new Vector2(0.0f, 0.0f);
        foreach(Transform t in playerList) resultant += new Vector2(t.position.x, t.position.y);

        float maxDistance = 0.0f;

        for(int i = 0; i < playerList.Count - 1; i++){
            for(int j = i + 1; j < playerList.Count; j++){
                float dist = Vector2.Distance(playerList[i].position, playerList[j].position);
                if(dist > maxDistance) maxDistance = dist;
            }
        }

        // gameObject.transform.position = new Vector3(resultant.x / playerList.Count, resultant.y / playerList.Count, -maxDistance / 4);
        // GetComponent<Camera>().orthographicSize = maxDistance;
        gameObject.transform.position = Vector3.Lerp(
            gameObject.transform.position,
            new Vector3(resultant.x /playerList.Count, resultant.y /playerList.Count, -(maxDistance / 4 + 5)),
            Time.deltaTime * 3
        );
    }
}
