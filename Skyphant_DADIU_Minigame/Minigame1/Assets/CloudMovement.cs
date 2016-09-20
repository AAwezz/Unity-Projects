using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {

    public float speed = 1f;

    public float waitTime;

    float timeWaited;

    public float zOffset;

    public bool isInForeground;

    // Use this for initialization
    void Start () {

        GetComponentInParent<CloudController>().SpawnCloud(this.gameObject);
        timeWaited += 5;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        timeWaited += Time.deltaTime;

     

        if(waitTime < timeWaited)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }

        if(transform.position.y > 10 && GetComponent<Renderer>().isVisible == false)
        {
            GetComponentInParent<CloudController>().SpawnCloud(this.gameObject);
            timeWaited = 0;
           
        }

    }
}
