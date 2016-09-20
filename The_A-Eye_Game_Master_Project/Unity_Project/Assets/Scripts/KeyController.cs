using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour
{

    public GameObject PressEText;

    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
