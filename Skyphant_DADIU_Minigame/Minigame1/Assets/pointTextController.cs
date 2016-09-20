using UnityEngine;
using System.Collections;

public class pointTextController : MonoBehaviour {

    GameObject elephant;

	// Use this for initialization
	void Start () {

        elephant = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Renderer>().enabled = GetComponent<Animation>().IsPlaying("pointFlyUp");
       

    }

    public void showPoints(int points)
    {

        GetComponent<TextMesh>().text = points.ToString();
        this.transform.position = elephant.transform.position;
        GetComponent<Animation>().Play();

    }
}
