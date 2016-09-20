using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public Color endColor;
    public Color startColor;

    private bool _running = false;
    Color current;

	// Use this for initialization
	void Start () {
        current = GetComponent<Renderer>().material.color;
    }

    public void RunEnum()
    {
        if (_running == false) StartCoroutine(ChangeColor());
    }

    void OnMouseDown()
    {
        //GetComponent<Renderer>().material.color = Color.black;
    }

    IEnumerator ChangeColor()
    {
        _running = true;
        current = GetComponent<Renderer>().material.color;
        if( current == endColor)
        {
            GetComponent<Renderer>().material.color = startColor;
        } else
        {
            GetComponent<Renderer>().material.color = endColor;
        }
        Debug.Log("changed color");
        yield return new WaitForSeconds(1);
        _running = false;

        yield return null;
    }
}
