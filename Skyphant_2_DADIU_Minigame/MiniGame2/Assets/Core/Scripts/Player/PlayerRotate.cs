using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRotate : MonoBehaviour , IPointerDownHandler, IPointerUpHandler {

	PlayerController PC; 

	// Use this for initialization
	void Start () {
		PC = GameObject.FindObjectOfType<PlayerController>();
	    Input.multiTouchEnabled = false;
	}
	
	
	public void OnPointerDown(PointerEventData eventData)
	{
        if(eventData.pointerCurrentRaycast.gameObject.name.Equals("Left"))
		{
			PC.RotateLeft = true;
		}
		else if(eventData.pointerCurrentRaycast.gameObject.name.Equals("Right"))
		{
			PC.RotateRight = true;
		}

	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		PC.RotateLeft = false;
		PC.RotateRight = false;
		PC.MoveSpeed = PC.StartSpeed;
	}
}
