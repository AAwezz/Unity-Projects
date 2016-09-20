using UnityEngine;
using System.Collections;

public class BackgroundCylinderRotation : MonoBehaviour {

    public static float bgspeed;
    private bool isGameOver;

    // Use this for initialization
    void Start () {

     

}
	
	// Update is called once per frame
	void Update () {
        if(!isGameOver)
        transform.Rotate(Vector3.down * Time.deltaTime * bgspeed);
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
