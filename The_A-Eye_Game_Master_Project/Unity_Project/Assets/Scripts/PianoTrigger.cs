using UnityEngine;
using System.Collections;
using System;

public class PianoTrigger : MonoBehaviour {
    public bool LookedAt;
    public bool KeyTrigger;
    private float timeLookedAt;
    private float timeToTrigger;
    bool _move = false;

    GameObject lm;
    AudioControl _ac;
    LevelManagement lmScript;

    public GameObject lamp, head, model, blackscreen;

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
        timeToTrigger = 1f;
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
        void Update ()
    {
        if (lmScript.MonsterDoorClosed.activeSelf)
        {
            return;
        }
        if (lmScript.TriggerByEyesight)
        {
            TriggerPianoByEyesight();
        }
        if (lmScript.TriggerByCollider)
        {
            TriggerPianoByCollider();
        }
        if (lmScript.TriggerBySeen)
        {
            TriggerPianoBySeen();
        }
    }

    private void TriggerPianoByEyesight()
    {
        if (_move && !EventFired)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 3); //make light blink
            StartCoroutine(PianoSequence());
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
        //stop piano tune if not allready done

        timeLookedAt = 0;
    }
    private void TriggerPianoByCollider()
    {
        if (_cc.Collision && !EventFired)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 3); //make light blink
            StartCoroutine(PianoSequence());
            return;
        }
    }
    private void TriggerPianoBySeen()
    {
        if (_move && !EventFired && !_renderer.isVisible)
        {
            lm.GetComponent<LightController>().OneLampBlink(lamp, 3); //make light blink
            StartCoroutine(PianoSequence());
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
        //stop piano tune if not allready done
    }

    IEnumerator PianoSequence()
    {

        System.IO.File.AppendAllText(Application.dataPath +
            "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
        System.IO.File.AppendAllText(Application.dataPath +
            "/Data/Triggers/" + FileName2 + ".txt", "46" + Environment.NewLine);

        EventFired = true;
        _ac.StartScare(); //play scare sound
        blackscreen.SetActive(true);//turn off camera for a bit
        head.SetActive(true); //setactive true head
        model.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        blackscreen.SetActive(false);
        _ac.StartPiano();//start play a piano tune
        yield return new WaitForSeconds(1);
        head.SetActive(false); //setactive false head
        model.SetActive(false);

        lm.GetComponent<LevelManagement>().FiredEvents++;
        if (KeyTrigger)
        {
            lm.GetComponent<LevelManagement>().FiredKeyEvents++;
        }
    }
}
