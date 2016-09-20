using UnityEngine;
using System.Collections;
using System;

public class MirrorTrigger : MonoBehaviour {
    public bool LookedAt;
    public bool KeyTrigger;
    private float timeLookedAt;
    private float timeToTrigger;
    bool _move = false;

    GameObject lm;
    AudioControl _ac;
    LevelManagement lmScript;

    public GameObject lamp, head;
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
        timeToTrigger = 0.8f;
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
            TriggerMirrorByEyesight();
        }
        if (lmScript.TriggerByCollider)
        {
            TriggerMirrorByCollider();
        }
        if (lmScript.TriggerBySeen)
        {
            TriggerMirrorBySeen();
        }
    }

    private void TriggerMirrorByEyesight()
    {
        if (_move && !EventFired)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 3); //make light blink
            StartCoroutine(MirrorSequence());
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
    private void TriggerMirrorByCollider()
    {
        if (_cc.Collision && !EventFired)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 3); //make light blink
            StartCoroutine(MirrorSequence());
            return;
        }
    }
    private void TriggerMirrorBySeen()
    {
        if (_move && !EventFired && _renderer.isVisible)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 3); //make light blink
            StartCoroutine(MirrorSequence());
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

    IEnumerator MirrorSequence()
    {
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "52" + Environment.NewLine);
        EventFired = true;
        head.SetActive(true); // head appear at the towels
        _ac.PlayScaryVoice(); // play a scream
        yield return new WaitForSeconds(1.5f);
        head.SetActive(false); // head disappear

        lm.GetComponent<LevelManagement>().FiredEvents++;
        if (KeyTrigger)
        {
            lm.GetComponent<LevelManagement>().FiredKeyEvents++;
        }
    }
}
