using UnityEngine;
using System.Collections;

public class shitgame : MonoBehaviour {

    public GameObject s1, s2, s3,s4,s5;

	// Use this for initialization
	void Start () {
        StartCoroutine(lort());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator lort()
    {
        s1.SetActive(true);
        yield return new WaitForSeconds(1);
        s2.SetActive(true);

        yield return new WaitForSeconds(1);
        s1.SetActive(true);
        s2.SetActive(false);
        yield return new WaitForSeconds(1);
        s2.SetActive(true);

        yield return new WaitForSeconds(1);
        s1.SetActive(true);
        s2.SetActive(false);
        yield return new WaitForSeconds(1);
        s2.SetActive(true);

        yield return new WaitForSeconds(1);
        s1.SetActive(true);
        s2.SetActive(false);
        yield return new WaitForSeconds(1);
        s2.SetActive(true);

        //yield return new WaitForSeconds(5);
        //s3.SetActive(true);
        //yield return new WaitForSeconds(5);
        //s4.SetActive(true);
        //yield return new WaitForSeconds(5);
        //s5.SetActive(true);
    }
}
