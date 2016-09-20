using UnityEngine;
using System.Collections;

public class BlinkMatafakaBlink : MonoBehaviour {

    public GameObject bulb;
    public bool StartBlink;
    bool running;
    public float BlinkRangeMax;// timesOfBlinks;
    float timeBetweenBlinks;
    // Use this for initialization
    void Start () {
        //StartBlink = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (StartBlink && !running)
        {
            running = true;
            StartCoroutine(BlinkBitch());
        }
    }

    void turnOff()
    {
        bulb.SetActive(false);
    }

    void turnOn()
    {
        bulb.SetActive(true);
    }

    IEnumerator BlinkBitch()
    {
        //for(int i=0; i<timesOfBlinks; i++)
        //{
            timeBetweenBlinks = Random.Range(0, BlinkRangeMax);
            turnOff();
            yield return new WaitForSeconds(timeBetweenBlinks);
            timeBetweenBlinks = Random.Range(0, BlinkRangeMax);
            turnOn();
            yield return new WaitForSeconds(timeBetweenBlinks);
       // }
        running = false;
       // StartBlink = false;
    }
}
