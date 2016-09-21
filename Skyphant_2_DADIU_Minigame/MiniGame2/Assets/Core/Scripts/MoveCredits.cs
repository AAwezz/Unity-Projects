using UnityEngine;
using System.Collections;

public class MoveCredits : MonoBehaviour {


    public float CreditsSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        this.transform.Translate(Vector3.up * CreditsSpeed * Time.deltaTime);

	}
}
