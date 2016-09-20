using UnityEngine;
using System.Collections;

public class LobbyDoorController : MonoBehaviour {

    public GameObject PressEText;

    // Use this for initialization
    void Start() {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && (LevelManagement.FirstTimeOpening || LevelManagement.SecoundTimeOpening))
        {
            
            PressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PressEText.SetActive(false);
        }
    }
}
