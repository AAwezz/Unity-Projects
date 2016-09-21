using UnityEngine;
using System.Collections;

public class ShowAssest : MonoBehaviour {

    bool showing = false;
    public GameObject range, sell, upgrade;

    void OnMouseDown()
    {
        showing = !showing;
        if (showing)
        {
            range.SetActive(true);
            sell.SetActive(true);
            upgrade.SetActive(true);
        }
        else
        {
            range.SetActive(false);
            sell.SetActive(false);
            upgrade.SetActive(false);
        }
    }
}
