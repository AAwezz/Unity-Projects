using UnityEngine;
using System.Collections;

public class resetScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.SetInt("Highscore", 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
