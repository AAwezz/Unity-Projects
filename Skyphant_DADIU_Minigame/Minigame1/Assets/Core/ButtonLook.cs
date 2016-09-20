using UnityEngine;
using System.Collections;

public class ButtonLook : MonoBehaviour
{

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ButtonPressed()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }

    public void ButtonNotPressed()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.7f);
    }
}
