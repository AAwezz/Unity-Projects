using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LevelManagement : MonoBehaviour {

    public GameObject MonsterDoorOpen, MonsterDoorClosed;

    public GameObject PressEToOpenDoor;
    public GameObject PressEToPickUpKey;

    public GameObject KeyToPickUp;
    public GameObject blackscreen;
    public Text objectiveText;

    public bool TriggerByEyesight = true;
    public bool TriggerByCollider = false;
    public bool TriggerBySeen = false;

    public int RequiredKeyEvents = 3;
    public int FiredEvents;
    public int FiredKeyEvents;

    private bool _key;
    private AudioControl _ac;
    private EnemyController _enemy;
    private LightController _LightControl;
    private EyeRayCaster _triggerZoneManagement;

    public static bool FirstTimeOpening = true,
                 SecoundTimeOpening = false;

    string FileName1 = "TriggerEventLogX";
    string FileName2 = "TriggerEventLogY";

    // Use this for initialization
    void Start() {
        blackscreen.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        objectiveText.text = "Find the main entrance";
        RenderSettings.ambientIntensity = 0.2f;
        _ac = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioControl>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        _LightControl = this.gameObject.GetComponent<LightController>();
        _triggerZoneManagement = this.gameObject.GetComponent<EyeRayCaster>();

        _key = false;
        FiredEvents = 0;
        FiredKeyEvents = 0;
    }

    // Update is called once per frame
    void Update() {

        if (Time.timeScale == 0)
        {
            objectiveText.text = ".................";
            blackscreen.SetActive(true);
            //if (Input.GetKeyDown("r"))
            //{
            //    SceneManager.LoadScene("Start Scene");
            //}
        }

        if (FiredKeyEvents >= RequiredKeyEvents && !KeyToPickUp.gameObject.activeSelf && !_key)
        {
            KeyToPickUp.SetActive(true);
        }

        //Handle Key pick up
        if (KeyToPickUp.gameObject.activeSelf && PressEToPickUpKey.activeSelf)
        {
            if (Input.GetKeyDown("e"))
            {
                //Open door
                _key = true;
                PressEToPickUpKey.SetActive(false);
                KeyToPickUp.SetActive(false);

                //Next time will be secound time
                SecoundTimeOpening = true;
                objectiveText.text = "Use the key to get out";
            }
        }
        //Handle opening main entrance
        if (PressEToOpenDoor.gameObject.activeSelf)
        {
            //First try
            if (Input.GetKeyDown("e") && FirstTimeOpening)
            {
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "60" + Environment.NewLine);

                // change objective
                objectiveText.text = "Find the hidden key";

                //Remove Text
                PressEToOpenDoor.SetActive(false);

                //We have now tried opening once
                FirstTimeOpening = false;

                //Mess with the lights
                StartCoroutine(_LightControl.FlashAllLightForSecounds(5.0f));

                //Enable TriggerZones
                _triggerZoneManagement.StartTriggerZones = true;

                //Open monster door
                MonsterDoorOpen.SetActive(true);
                _ac.StopStartMusic();
                _ac.StartScaryMusic();
                _ac.StartMetalDoor();
                _ac.PlayFlickering();
                MonsterDoorClosed.SetActive(false);

                RenderSettings.ambientIntensity = 0.01f;

                //Activate Monster
                _enemy.Hunting = true;



            }
            //secound try
            if (Input.GetKeyDown("e") && SecoundTimeOpening && _key && PressEToOpenDoor.gameObject.activeSelf)
            {
                //Open door
                Application.Quit();

                //UnityEditor.EditorApplication.isPlaying = false;
            }
        }


	}


}
