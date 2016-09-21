using UnityEngine;
using System.Collections;

public class money : MonoBehaviour {

    public GUIText showGold;
    public static int amountOfGold = 100;

    void Update()
    {
        showGold.text = "" + amountOfGold;
    }
}
