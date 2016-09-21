using UnityEngine;
using System.Collections;

public class resetCreepsAlive : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        SpawnWave.creepsAlive = 0;
    }
}
