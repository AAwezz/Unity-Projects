using UnityEngine;
using System.Collections;
using System;

public class EyeRayCaster : MonoBehaviour {

    //float timeLookedAt = 0f;
    private GameObject _ActiveObject;
    private GameObject _GazeIndicator;
    string FileName1 = "TriggerEventLogX";
    string FileName2 = "TriggerEventLogY";
    

    public bool StartTriggerZones;
    // Use this for initialization
    void Start () {
        _GazeIndicator = GameObject.FindGameObjectWithTag("gazeIndicator");
        System.IO.FileStream x = System.IO.File.Create(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt");
        System.IO.FileStream y = System.IO.File.Create(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt");
        x.Close();
        y.Close();
        StartTriggerZones = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!StartTriggerZones)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, _GazeIndicator.transform.position - Camera.main.transform.position, out hit, 100))
        {
            if (hit.transform.root.tag == "Enemy")
            {
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "3" + Environment.NewLine);
            }

            if(hit.transform.tag == "LookAtObject")
            {
                Debug.Log("rammer nu");
                if (hit.transform.gameObject.GetComponent<ChairMovement>())
                {
                    _ActiveObject = hit.transform.gameObject;
                    hit.transform.gameObject.GetComponent<ChairMovement>().LookedAt = true;
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "6" + Environment.NewLine);
                    return;
                }
                if (hit.transform.gameObject.GetComponent<PianoTrigger>())
                {
                    _ActiveObject = hit.transform.gameObject;
                    hit.transform.gameObject.GetComponent<PianoTrigger>().LookedAt = true;
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "9" + Environment.NewLine);
                    return;
                }
                if (hit.transform.gameObject.GetComponent<ToiletTrigger>())
                {
                    _ActiveObject = hit.transform.gameObject;
                    hit.transform.gameObject.GetComponent<ToiletTrigger>().LookedAt = true;
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "12" + Environment.NewLine);
                    return;
                }
                if (hit.transform.gameObject.GetComponent<MirrorTrigger>())
                {
                    _ActiveObject = hit.transform.gameObject;
                    hit.transform.gameObject.GetComponent<MirrorTrigger>().LookedAt = true;
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "15" + Environment.NewLine);
                    return;
                }
                if (hit.transform.gameObject.GetComponent<TVTrigger>())
                {
                    _ActiveObject = hit.transform.gameObject;
                    hit.transform.gameObject.GetComponent<TVTrigger>().LookedAt = true;
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                    System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "18" + Environment.NewLine);
                    return;
                }
            }
            if (_ActiveObject != null)
            {
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName1 + ".txt", Time.time.ToString("F2") + Environment.NewLine);
                System.IO.File.AppendAllText(Application.dataPath + "/Data/Triggers/" + FileName2 + ".txt", "0" + Environment.NewLine);
                if (_ActiveObject.GetComponent<ChairMovement>() != null)
                    _ActiveObject.GetComponent<ChairMovement>().LookedAt = false;
                if (_ActiveObject.GetComponent<PianoTrigger>() != null)
                    _ActiveObject.GetComponent<PianoTrigger>().LookedAt = false;
                if (_ActiveObject.GetComponent<MirrorTrigger>() != null)
                    _ActiveObject.GetComponent<MirrorTrigger>().LookedAt = false;
                if (_ActiveObject.GetComponent<ToiletTrigger>() != null)
                    _ActiveObject.GetComponent<ToiletTrigger>().LookedAt = false;
                if (_ActiveObject.GetComponent<TVTrigger>() != null)
                    _ActiveObject.GetComponent<TVTrigger>().LookedAt = false;
            }
            _ActiveObject = null;
        }
    }
}
