using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

    public GameObject[] Hearts;

    public int lives = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void loseLife(int life)
    {
        if(life < Hearts.Length && life >= 0)
            Hearts[life].SetActive(false);
    }
}
