using UnityEngine;
using System.Collections;

public class CollisionController : MonoBehaviour {

    public bool Collision = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collision = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Collision = false;
        }
    }
}
