using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool RotateLeft, RotateRight = false; 
	public float MovementRange = 5f;
	public Animator RotateAnim;
	public Transform Middle, LeftPos, RightPos;
	public float MoveSpeed = 0.5f;
	public float StartSpeed;
	// Use this for initialization

	public float additionSpeed = 0.1f;
	public bool StartThemeMusic = false;
	private AudioManager audioMngr;

	void Start () {
		StartSpeed = MoveSpeed;
		audioMngr = GameObject.FindObjectOfType<AudioManager> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(!RotateLeft && !RotateRight)
		{
			//MoveFromTo(transform, Middle, MoveSpeed);
			RotateAnim.SetBool("TurnRight", false);
			RotateAnim.SetBool("TurnLeft", false);
		}
		else if(RotateLeft)
		{
			MoveSpeed += additionSpeed;
			MoveFromTo(transform, LeftPos, MoveSpeed);
			/*
			Vector3 newPos = Vector3.left * Time.deltaTime * MoveSpeed;
			transform.Translate(newPos);
			*/
			RotateAnim.SetBool("TurnRight", false);
			RotateAnim.SetBool("TurnLeft", true);
		}
		else if(RotateRight)
		{
			MoveSpeed += additionSpeed;
			MoveFromTo(transform, RightPos, MoveSpeed);
			//transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
			//Vector3.ClampMagnitude(newPos, MovementRange)
			/*
			Vector3 newPos = Vector3.right * Time.deltaTime * MoveSpeed;
			transform.Translate(newPos);
			*/
			RotateAnim.SetBool("TurnLeft", false);
			RotateAnim.SetBool("TurnRight", true);

		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		if(StartThemeMusic)
		{
			audioMngr.ThemeMusicPlay ();
			StartThemeMusic = false;
		}
	}

	void MoveFromTo(Transform from, Transform to, float speed)
	{
		transform.position = Vector3.Lerp(from.transform.position, to.transform.position, speed * Time.deltaTime);
		//transform.rotation = Quaternion.Lerp(from.transform.rotation, to.transform.rotation, speed * Time.deltaTime);
	}

}
