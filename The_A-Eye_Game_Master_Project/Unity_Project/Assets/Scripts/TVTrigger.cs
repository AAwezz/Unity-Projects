using UnityEngine;
using System.Collections;
using System;

public class TVTrigger : MonoBehaviour {
    public bool LookedAt;
    public bool KeyTrigger;
    private float timeLookedAt;
    private float timeToTrigger;
    bool _move = false;

    GameObject lm;
    AudioControl _ac;
    LevelManagement lmScript;

    public GameObject lamp, lamp2, screen1, screen2;

    public GameObject TriggerCollider;
    private CollisionController _cc;
    private Renderer _renderer;

    private bool EventFired = false;
    private bool CollisionHappend = false;

    string FileName1 = "TriggerEventLogX";
    string FileName2 = "TriggerEventLogY";

    // Use this for initialization
    void Start()
    {
        timeToTrigger = 0.5f;
        lm = GameObject.FindGameObjectWithTag("LevelManager");
        _ac = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioControl>();
        lmScript = lm.GetComponent<LevelManagement>();
        //Needed for collider detection
        if (lmScript.TriggerByCollider)
        {
            _cc = TriggerCollider.GetComponent<CollisionController>();
        }
        //Needed for Has been seen detection
        _renderer = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lmScript.MonsterDoorClosed.activeSelf)
        {
            return;
        }
        if (lmScript.TriggerByEyesight)
        {
            TriggerTVByEyesight();
        }
        if (lmScript.TriggerByCollider)
        {
            TriggerTVByCollider();
        }
        if (lmScript.TriggerBySeen)
        {
            TriggerTVBySeen();
        }
    }

    private void TriggerTVByEyesight()
    {
        if (_move && !EventFired)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 6); //make light blink
            lm.GetComponent<LightController>().OneLampBlink(lamp2, 6); //make light blink
            StartCoroutine(TVSequence());
            return;
        }
        if (LookedAt)
        {
            timeLookedAt += Time.deltaTime;
            if (timeLookedAt >= timeToTrigger)
            {
                _move = true;
                Debug.Log("HAR KIGGET i OVER " + timeToTrigger + " SEKUNDER NU");
            }
            return;
        }
        timeLookedAt = 0;
    }
    private void TriggerTVByCollider()
    {
        if (_cc.Collision && !EventFired)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 6); //make light blink
            lm.GetComponent<LightController>().OneLampBlink(lamp2, 6); //make light blink
            StartCoroutine(TVSequence());
            return;
        }
    }
    private void TriggerTVBySeen()
    {
        if (_move && !EventFired && !_renderer.isVisible)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 6); //make light blink
            lm.GetComponent<LightController>().OneLampBlink(lamp2, 6); //make light blink
            StartCoroutine(TVSequence());
            return;
        }
        if (LookedAt)
        {
            timeLookedAt += Time.deltaTime;
            if (timeLookedAt >= timeToTrigger)
            {
                _move = true;
                Debug.Log("HAR KIGGET i OVER " + timeToTrigger + " SEKUNDER NU");
            }
            return;
        }
        timeLookedAt = 0;
    }

    IEnumerator TVSequence()
    {
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "55" + Environment.NewLine);

        EventFired = true;
        screen1.SetActive(true); // change texture to "stracht" picture
        _ac.StartTV();// start "flimmer" sound
        yield return new WaitForSeconds(4);
        screen1.SetActive(false);
        _ac.StopTV();
        _ac.ScreamerStart(); // play high sound

        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "55" + Environment.NewLine);

        screen2.SetActive(true); // change picture to "screamer" or something
        yield return new WaitForSeconds(1);
        screen2.SetActive(false);
        _ac.ScreamerStop();
        // change picture back to black

        lm.GetComponent<LevelManagement>().FiredEvents++;
        if (KeyTrigger)
        {
            lm.GetComponent<LevelManagement>().FiredKeyEvents++;
        }
    }
}
