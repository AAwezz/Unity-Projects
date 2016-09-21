using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

    public GUIText showLife;
    public static int amountOfLife = 10;

    void Update()
    {
        showLife.text = "" + amountOfLife;
    }
}
