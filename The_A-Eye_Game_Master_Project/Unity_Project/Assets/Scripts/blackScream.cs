using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class blackScream : MonoBehaviour {

    AudioControl _ac;
    public GameObject black;

    string FileName1 = "TriggerEventLogX";
    string FileName2 = "TriggerEventLogY";

    private double[] _ealiestPupilSizes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public EyeTribeUnityScript _eyeData;

    private double InitialPupilSize;

    bool running = false;
    bool InitialPupilSizeCalculated = false;

    bool _calm = true;
    bool _scared = false;

    public bool BlackScreen = true;
    public bool WhiteScreen = false;
    public bool ColorSwitchScreen = false;

    private Image Screen;

    // Use this for initialization
    void Start () {

        InitialPupilSize = 0.0f;

        System.IO.FileStream x = System.IO.File.Create(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt");
        System.IO.FileStream y = System.IO.File.Create(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt");

        x.Close();
        y.Close();

        Cursor.visible = false;

        _ac = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioControl>();

        Screen = black.GetComponent<Image>();

        if (BlackScreen)
        {
            //According the tests cyan was the mmost stabile
            Screen.color = Color.cyan;
        }
        else if (WhiteScreen)
        {
            Screen.color = Color.white;
        }
        else if(ColorSwitchScreen)
        {
            StartCoroutine(SwitchColorsOnScreen());
        }
        else
        {
            throw new Exception("No game mode was chosen on the Blackscream Script");
        }

        //Start data gathering
        StartCoroutine(MeasureInitialPupilSize());

        StartCoroutine(ScareDection());
    }
	
	// Update is called once per frame
	void Update () {
        if (!running && !ColorSwitchScreen)
        {
            StartCoroutine(Scare());
        }
	}
    
    IEnumerator Scare()
    {
        running = true;
        yield return new WaitForSeconds(15);
        //_ac.BabyStart();
        //System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        //System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "45" + Environment.NewLine);
        //yield return new WaitForSeconds(UnityEngine.Random.Range(8, 12));
        //_ac.BabyStart();
        //System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        //System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "45" + Environment.NewLine);
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
        _ac.ScreamerStart();
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "45" + Environment.NewLine);
        yield return new WaitForSeconds(0.5f);
        _ac.ScreamerStop();
        //yield return new WaitForSeconds(UnityEngine.Random.Range(8, 12));
        //_ac.BabyStart();
        //System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        //System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "45" + Environment.NewLine);
        running = false;
    }

    IEnumerator MeasureInitialPupilSize()
    {
        yield return new WaitForSeconds(0.5f);
        int count = 0;

        while (Time.time < 14)
        {
            InitialPupilSize = InitialPupilSize + ((_eyeData.LeftEye.PupilSize + _eyeData.RightEye.PupilSize) / 2);
            count++;
            yield return null;
        }
        InitialPupilSize = InitialPupilSize / count;
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", InitialPupilSize + Environment.NewLine);
        InitialPupilSizeCalculated = true;
    }

    IEnumerator ScareDection()
    {
        int count1 = 0;
        double sum = 0.0f;
        double average = 0;
        while (InitialPupilSizeCalculated)
        {
            _ealiestPupilSizes[count1 % 10] = ((_eyeData.LeftEye.PupilSize + _eyeData.RightEye.PupilSize) / 2);
            count1++;
            for(int i = 0; i<_ealiestPupilSizes.Length; i++)
            {
                sum += _ealiestPupilSizes[i];
            }
            average = sum / _ealiestPupilSizes.Length;
            if (average > 3 + InitialPupilSize)
            {
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "5" + Environment.NewLine);
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator SwitchColorsOnScreen()
    {
        while (true)
        {
            if (Screen.color == Color.white)
            {
                Screen.color = Color.yellow;
            }
            else if (Screen.color == Color.yellow)
            {
                Screen.color = Color.red;
            }
            else if (Screen.color == Color.red)
            {
                Screen.color = Color.magenta;
            }
            else if (Screen.color == Color.magenta)
            {
                Screen.color = Color.green;
            }
            else if (Screen.color == Color.green)
            {
                Screen.color = Color.cyan;
            }
            else if (Screen.color == Color.cyan)
            {
                Screen.color = Color.blue;
            }
            else if (Screen.color == Color.blue)
            {
                Screen.color = Color.black;
            }
            else if (Screen.color == Color.black)
            {
                Screen.color = Color.white;
            }
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
            System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "20" + Environment.NewLine);
            yield return new WaitForSeconds(12f);
        }
    }
}
