using UnityEngine;
using System.Collections;

public class SpawnPointController : MonoBehaviour {

    public static float distance;

    public GameObject SpawnPoint;

    public static GameObject[] spawns;

	// Use this for initialization
	void Start () {

        //Creating two spawnpoint at either side of the main spawn depending on the distance value set by the VariableController
        Instantiate(SpawnPoint, new Vector3(transform.position.x + distance, transform.position.y, transform.position.z), Quaternion.identity);
        Instantiate(SpawnPoint, new Vector3(transform.position.x - distance, transform.position.y, transform.position.z), Quaternion.identity);
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }
	
	// Update is called once per frame
	void Update () {
	


	}
}
