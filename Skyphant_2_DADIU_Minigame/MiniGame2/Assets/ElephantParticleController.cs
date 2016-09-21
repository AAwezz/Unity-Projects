using UnityEngine;
using System.Collections;

public class ElephantParticleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "PickUp")
        {
            //GetComponentInChildren<ParticleSystem>().Play();

        }

     

    }
}
