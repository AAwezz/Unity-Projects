using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pauseScene : MonoBehaviour {

    public Text timer;
    public float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time > 10)
        {
            Application.LoadLevel("gameOverSceneAW");
        }
        timer.text = "Time: "+time;
	}
}
