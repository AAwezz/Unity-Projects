using Assets.Core;
using UnityEngine;

public class GUIManager : MonoBehaviour
{

    public GameObject Credits;
    public GameObject CreditsText;
    public float CreditSpeed;
	public AudioManager AudioMngr;

    private bool _doingCredits;

    void Awake()
    {
		/*
        LanguageManager.Instance.LoadLanguage("English");
        Debug.Log(LanguageManager.Instance.Get("Phrases/Hello"));

        LanguageManager.Instance.LoadLanguage("Swedish");
        Debug.Log(LanguageManager.Instance.Get("Phrases/Hello"));
		*/
		if(!PlayerPrefs.HasKey("Language"))
		{
			PlayerPrefs.SetString("Language", "English");
			LanguageManager.Instance.LoadLanguage(PlayerPrefs.GetString ("Language"));
		}
		else
		{
			LanguageManager.Instance.LoadLanguage(PlayerPrefs.GetString ("Language"));
		}

		AudioMngr.WindPlay ();
    }

    void Update()
    {
        if (_doingCredits && CreditsText.transform.position.y < 1000)
        {
            CreditsText.transform.Translate(Vector3.up * Time.deltaTime * CreditSpeed);
        }
    }

	public void OnStartGameClick()
	{
		AudioMngr.ButtonClickPlay ();
		Debug.Log ("Start the gaddeim game");
		Application.LoadLevel (Constants.Scenes.MainLevelSceneName);
	}

    public void OnLanguageClick(string language)
    {
		AudioMngr.ButtonClickPlay ();
        LanguageManager.Instance.LoadLanguage(language);
		PlayerPrefs.SetString ("Language", language);
        LocalizedText[] texts = FindObjectsOfType<LocalizedText>();
        foreach (LocalizedText text in texts)
        {
            text.LocalizeText();
        }
    }

	public void OnCreditsClick()
	{
		AudioMngr.ButtonClickPlay ();
	    CreditsText.transform.position = new Vector3(CreditsText.transform.position.x, -1000);
        _doingCredits = true;
        Credits.SetActive(true);
    }

    public void CloseCredits()
    {
		AudioMngr.ButtonClickPlay ();
        _doingCredits = false;
        Credits.SetActive(false);
    }
}