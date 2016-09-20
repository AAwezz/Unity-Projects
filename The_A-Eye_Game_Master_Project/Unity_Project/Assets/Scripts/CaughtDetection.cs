using UnityEngine;
using System.Collections;

public class CaughtDetection : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0.0f;
        }
    }
}
