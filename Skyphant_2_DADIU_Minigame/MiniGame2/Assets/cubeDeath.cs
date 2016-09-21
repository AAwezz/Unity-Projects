using UnityEngine;
using System.Collections;

public class cubeDeath : MonoBehaviour {

    GameObject CameraHolder = null;
    public float cameraSpeedEnd = 5;
    Vector3 endPos;
    private bool smoothEnd = false;
    private bool lookUp = false;
    float t;
    Vector3 startPos;
    public float rotationAmount = 30.0f;

    void Start()
    {
        CameraHolder = GameObject.Find("CameraHolder");
        endPos = transform.GetChild(0).position;
    }

    void FixedUpdate()
    {
        if (smoothEnd)
        {
            //We move the camera slowly after we reach the goal
            t += Time.deltaTime / cameraSpeedEnd;
            CameraHolder.transform.position = Vector3.Lerp(startPos, endPos, t);
            if(Vector3.Distance(CameraHolder.transform.position, endPos) < 1)
            {
                smoothEnd = false;
                lookUp = true;
                t = 0;
                startPos = CameraHolder.transform.position;
            }
        }

        if (lookUp)
        {
            //We rotate the camera
            Vector3 rot = CameraHolder.transform.rotation.eulerAngles;
            rot.x = rot.x - rotationAmount * Time.deltaTime;
            CameraHolder.transform.eulerAngles = rot;

            //We move the camera up a bit
            Vector3 end = new Vector3(CameraHolder.transform.position.x, CameraHolder.transform.position.y + 5, CameraHolder.transform.position.z);
            t += Time.deltaTime / cameraSpeedEnd;
            CameraHolder.transform.position = Vector3.Lerp(startPos, end, t);

            if (CameraHolder.transform.eulerAngles.x == 270)
            {
               Application.LoadLevel("gameOverSceneAW");
            }


        }

    }



    void OnTriggerEnter(Collider col)
	{
        if (col.tag == "Player")
        {
            CameraHolder.transform.parent = null;
            CameraHolder.GetComponent<SmoothFollow>().enabled = false;
            startPos = CameraHolder.transform.position;
            t = 0;
            smoothEnd = true;
        }
	}

}
