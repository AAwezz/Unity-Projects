using UnityEngine;
using System.Collections;
using System;

public class LookAwayHandler : MonoBehaviour {

    private GameObject _GazeIndicator;
    private bool blinkTimer = false;
    private float blinkTimeMiliSecounds = 300f;

    private String FileName1 = "GameLookingDataX";
    private String FileName2 = "GameLookingDataY";

    // Use this for initialization
    void Start () {
        _GazeIndicator = GameObject.FindGameObjectWithTag("gazeIndicator");
        System.IO.FileStream x = System.IO.File.Create(Application.dataPath + "/Data/Looking/" + FileName1 + ".txt");
        System.IO.FileStream y = System.IO.File.Create(Application.dataPath + "/Data/Looking/" + FileName2 + ".txt");
        x.Close();
        y.Close();
    }
	
	// Update is called once per frame
	void Update () {
        //Did We blink?
        if (_GazeIndicator.transform.parent.GetComponent<EyeTribeUnityScript>().Blinking() && !blinkTimer)
        {
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Looking/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Looking/" + FileName2 + ".txt", "10" + Environment.NewLine);
            StartCoroutine(waitForNextBlink());
        }
        //Checks if we are looking at the screen
        else if (_GazeIndicator.transform.parent.GetComponent<EyeTribeUnityScript>().LookingAtScreen())
        {
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Looking/" + FileName1 + ".txt", Time.time.ToString("F2")+ Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Looking/" + FileName2 + ".txt", "0" + Environment.NewLine);
        } else
        {
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Looking/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Looking/" + FileName2 + ".txt", "5" + Environment.NewLine);
        }
    }

    IEnumerator waitForNextBlink()
    {
        blinkTimer = true;
        yield return new WaitForSeconds(blinkTimeMiliSecounds/1000);
        blinkTimer = false;
        yield return null;
    }
}
