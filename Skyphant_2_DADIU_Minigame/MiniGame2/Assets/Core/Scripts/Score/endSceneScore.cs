using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class endSceneScore : MonoBehaviour {
    public Text highscore, showCurrentScore;

    // Use this for initialization
    void Start () {
        highscore.text = LanguageManager.Instance.Get("Phrases/HighScore") + PlayerPrefs.GetInt("Highscore" + Application.loadedLevelName);
        if (ScoreControl.CurrentScore > PlayerPrefs.GetInt("Highscore"+ Application.loadedLevelName))
        {
            PlayerPrefs.SetInt("Highscore" + Application.loadedLevelName, ScoreControl.CurrentScore);
        }
        showCurrentScore.text = LanguageManager.Instance.Get("Phrases/Score") + ScoreControl.CurrentScore;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
