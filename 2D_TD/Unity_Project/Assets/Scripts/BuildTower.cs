using UnityEngine;
using System.Collections;

public class BuildTower : MonoBehaviour {

    public static bool selected = false;

    void OnMouseDown()
    {
        selected = !selected;
    }
}
