using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : MonoBehaviour {
    public List<GameObject> equipSet = new List<GameObject>();

    // Use this for initialization
    void Start () {
        equipSet.Add(GameObject.Find("Sword"));
        equipSet.Add(GameObject.Find("Gun"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
