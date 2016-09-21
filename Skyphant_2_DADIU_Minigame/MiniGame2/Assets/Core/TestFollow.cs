using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestFollow : MonoBehaviour {
	
	public Transform[] path;

	public SmoothQuaternion sr;
	float t = 0;
	public float speed = 7.5f;

	public Vector3[] GizmoPath;
	private Vector3 position;

	public GameObject[] FindPathByObjects;


	int CompareObNames( GameObject x, GameObject y )
	{
		return x.name.CompareTo( y.name );
	}

	void Start()
	{
		sr = transform.rotation;
		sr.Duration = 0.5f;
		FindPathByObjects = GameObject.FindGameObjectsWithTag("WayPoint");

		path = new Transform[FindPathByObjects.Length];
		
		Array.Sort(FindPathByObjects, CompareObNames);


		for(int i = 0; i < FindPathByObjects.Length; i++)
		{
			path[i] = FindPathByObjects[i].transform;
		}
		//Array.Sort(path);



		GizmoPath = new Vector3[path.Length];
		position = transform.position;


		for(int i = 0; i < path.Length; i++)
		{
			GizmoPath[i] = path[i].position;
		}

	}

	// Update is called once per frame
	void FixedUpdate () {
        if(speed == 0)
            return;

		Quaternion q = new Quaternion(); // = transform.localRotation;
		transform.position = Spline.MoveOnPath(path, transform.position, ref t, ref q, speed, 700, EasingType.Sine, true, true);
		sr.Value = q;
		transform.rotation = sr;

    }

	void OnDrawGizmos () {
		Spline.GizmoDraw(GizmoPath, 100.5f, EasingType.Sine, true, true);
	}

	void MoveFromTo(Transform from, Transform to, float speed)
	{
		
		transform.position = Vector3.Lerp(from.transform.position, to.transform.position, speed);
		transform.rotation = Quaternion.Lerp(from.transform.rotation, to.transform.rotation, speed);
	}
}