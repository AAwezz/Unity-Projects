using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreControl : MonoBehaviour
{


    public static int CurrentScore;

    public Text scoreField;

    private GameObject pointTextREF;

    // Use this for initialization
    void Start () {
        //PlayerPrefs.SetInt("Highscore" + Application.loadedLevelName, 0);
        pointTextREF = GameObject.FindGameObjectWithTag("PointText");
        CurrentScore = 0;
        scoreField.text = "" + CurrentScore;
	}
	
	// Update is called once per frame
	void Update () {


	
	}

    public void AddPoints(int points)
    {

        CurrentScore += points;

        pointTextREF.GetComponent<PointTextControl>().GotPoints(points);

        scoreField.text = "" + CurrentScore;  

    }

}
