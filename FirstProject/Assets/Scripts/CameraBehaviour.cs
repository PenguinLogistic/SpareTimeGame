using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;
	
    // Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        //follows player with camera 
        transform.position = player.transform.position + offset;
    }
}
