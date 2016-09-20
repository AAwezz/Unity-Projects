using UnityEngine;
using System.Collections;
using System;

public class GhoulSound : MonoBehaviour
{


    private AudioControl _ac;
    private GameObject _Player;

    public bool Growling = false;
    public bool breathing = false;

    void Start()
    {
        _ac = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioControl>();
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        if (Vector3.Distance(this.transform.position, _Player.transform.position) < 3)
        {
            if (!Growling)
            {
                StartCoroutine(Growl());
            }
            else
            {
                return;
            }
        }

        if (Vector3.Distance(this.transform.position, _Player.transform.position) < 7 && !breathing)
        {
            StartCoroutine(Breath());
        }
    }

    private IEnumerator Growl()
    {
        Growling = true;
        _ac.PlayGhoulGrowl();
        yield return new WaitForSeconds(8);
        Growling = false;
        yield return null;
    }

    private IEnumerator Breath()
    {
        breathing = true;
        _ac.PlayGhoulBreath();
        yield return new WaitForSeconds(8);
        breathing = false;
        yield return null;
    }
}
