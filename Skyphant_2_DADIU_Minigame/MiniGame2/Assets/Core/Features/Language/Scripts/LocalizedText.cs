using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string localizedID = string.Empty, localizedID2 = string.Empty, localizedID3 = string.Empty;
    private Text tutoText;

    void Awake()
    {
        GameObject TUI = GameObject.FindGameObjectWithTag("TutorialUI");
        if (TUI != null)
        {
            Text tutoText = TUI.gameObject.GetComponentInChildren<Text>();
        }
    }

    void Start()
    {
        LocalizeText();
    }

    public void LocalizeText()
    {
        Text text = GetComponent<Text>();
        TextMesh textMesh = GetComponent<TextMesh>();
        if (tutoText != null)
        {
            tutoText.text = LanguageManager.Instance.Get(localizedID3);
        }
        if (textMesh != null)
        {
            textMesh.text = LanguageManager.Instance.Get(localizedID2);
        }
        if (text != null)
        {
            text.text = LanguageManager.Instance.Get(localizedID);
        }
    }
}
