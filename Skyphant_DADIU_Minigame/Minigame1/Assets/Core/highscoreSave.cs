using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class highscoreSave : MonoBehaviour {

    //public Text highscoreText;

    private string _currentLanguage;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //PlayerPrefs.SetInt("Best score", 0);
    }

	// Use this for initialization
	void Start () {
        this.GetComponent<Text>().text = (LanguageManager.Instance.Get("Phrases/HighScore") + PlayerPrefs.GetInt("Best score"));
	    _currentLanguage = PlayerPrefs.GetString("Language");
	}
	
	// Update is called once per frame
	void Update () {
	    if (PlayerPrefs.GetString("Language") != _currentLanguage)
	    {
	        this.GetComponent<Text>().text = (LanguageManager.Instance.Get("Phrases/HighScore") +
	                                          PlayerPrefs.GetInt("Best score"));
	        _currentLanguage = PlayerPrefs.GetString("Language");
	    }
	    //    if (ScoreSystem.points > PlayerPrefs.GetInt("Best score"))
        //    {
        //        this.GetComponent<Text>().text = ("Best score: " + PlayerPrefs.GetInt("Best score"));
        //    }
    }
}
