using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTag : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject tag;

    public int id = -1;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate() {
        tag.gameObject.transform.position = player.position + new Vector3(0.0f, 2.5f, 0.0f);
    }

    public void SetPlayerId(int value) {
        id = value;
        tag.gameObject.GetComponent<TextMeshPro>().text = "P" + id;
    }
}
