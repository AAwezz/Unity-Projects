using UnityEngine;
using System.Collections;

public class ForegroundCylinderRotation : MonoBehaviour {

    public static float fgspeed;

    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * Time.deltaTime * fgspeed);
    }
}
