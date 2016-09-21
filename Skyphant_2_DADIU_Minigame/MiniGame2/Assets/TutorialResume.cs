using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialResume : MonoBehaviour
{

    public Text TextBox;

    private GameObject TUI;

    

	// Use this for initialization
	void Start ()
	{

	    TUI = GameObject.FindGameObjectWithTag("TutorialUI");

        TUI.SetActive(false);



	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Resume()
    {

        Time.timeScale = 1f;

        TUI.SetActive(false);

        
        
    }

}
