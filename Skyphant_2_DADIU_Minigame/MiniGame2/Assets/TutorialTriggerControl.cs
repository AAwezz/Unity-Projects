using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialTriggerControl : MonoBehaviour
{


    private GameObject TUI;
    private Text TextBox;
    

    public string TutorialText;

	// Use this for initialization
	void Awake ()
	{

	    TUI = GameObject.FindGameObjectWithTag("TutorialUI");
	    TextBox = TUI.gameObject.GetComponentInChildren<Text>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {

            Time.timeScale = 0f;

            TextBox.text = GetComponent<TextMesh>().text;
            //make a delay
            

            TUI.SetActive(true);


        }        
    }

}
