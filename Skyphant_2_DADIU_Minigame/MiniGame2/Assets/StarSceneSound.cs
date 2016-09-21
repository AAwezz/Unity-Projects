using UnityEngine;
using System.Collections;

public class StarSceneSound : MonoBehaviour {

	private AudioManager audioMngr;
	// Use this for initialization
	void Start () {
		audioMngr = GameObject.FindObjectOfType<AudioManager> ();
		audioMngr.SayHelloPlay ();
	}
	

}
