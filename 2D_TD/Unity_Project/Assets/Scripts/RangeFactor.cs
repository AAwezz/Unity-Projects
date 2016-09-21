using UnityEngine;
using System.Collections;

public class RangeFactor : MonoBehaviour {

    public GameObject thisTower;

	// Use this for initialization
	void changeRange() 
    {
        Shooting s = thisTower.GetComponent<Shooting>();
        transform.localScale = new Vector3(s.range * 1.81f, s.range * 1.81f, 0.1f);
	}
}
