using UnityEngine;
using System.Collections;

public class NearMissControl : MonoBehaviour
{

    public float TextSpeed;

    private Transform elephantREF;
    // Use this for initialization
    void Start () {

        elephantREF = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update ()
	{
        transform.position += Vector3.back * Time.deltaTime * TextSpeed;
    }

    public void NearMiss()
    {
        transform.position = elephantREF.position;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }
}
