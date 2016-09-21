using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public float strenghtOfAttraction = 2.5f;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.transform.tag == "PickUp")
		{
			StartCoroutine(LerpTowardsPlayer(col.transform));
		}
	}

	IEnumerator LerpTowardsPlayer(Transform coin)
	{
		//Transform parent = transform.parent;
		Vector3 offset = coin.position - transform.position;
		float magsqr = offset.sqrMagnitude;

		while(magsqr > 3f)
		{
			coin.position = Vector3.Lerp(coin.position, transform.position, strenghtOfAttraction * Time.deltaTime);
			offset = coin.position - transform.position;
			magsqr = offset.sqrMagnitude;
			yield return null;
		}

		coin.position = transform.position;
		//Debug.Log ("Collected");

	}
}
