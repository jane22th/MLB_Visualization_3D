using UnityEngine;
using System.Collections;

public class Fielder : MonoBehaviour {
	public Transform target;
	public string player_code;
	public bool is_animating = false;

	// 회전 속도.
	public float rotationSpeed = 360.0f;
	public float trackSpeed = 5.0f;

	Vector3 forceRotateDirection;

	// Use this for initialization
	void Start () {
		
	}

	void EndAnimation(){
		is_animating = false;
	}

	void StartAnimation(){
		is_animating = true;
	}

	void lookTarget(){
		forceRotateDirection = (target.position - transform.position);
		forceRotateDirection.y = 0;
		forceRotateDirection.Normalize();

		Quaternion characterTargetRotation = Quaternion.LookRotation(forceRotateDirection);
		transform.rotation = Quaternion.RotateTowards(transform.rotation,characterTargetRotation,rotationSpeed * Time.deltaTime);	
	}

	void trackTarget(){
		transform.Translate (Vector3.forward*Time.deltaTime*trackSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null)
			lookTarget ();

	}
}
