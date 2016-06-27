using UnityEngine;
using System.Collections;

public class Pitcher : MonoBehaviour {
	Animator animator;
	public bool throw_start = false;
	public bool throwing = false;
	GameObject ball;
	public Transform parent;
	public Vector3 init_ball_transform;
	Vector3 dir;
	bool first = true;
	public bool is_swing = false;

	public int throw_type = 0; //0 : -> catcher, 1 : batting

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		ball = GameObject.Find ("ball");
		init_ball_transform = ball.transform.position;
		parent = ball.transform.parent;
		Debug.Log ("pither start");
	}

	void throwBall(){
		throw_start = false;
		animator.SetTrigger ("throwing");
		if(!is_swing)
			GameObject.Find("InGameUI").GetComponent<InGame>().createRunner ();
	}

	void ThrowBall(){
		if (throw_type == 0) {
			dir = GameObject.FindGameObjectWithTag ("Chand").transform.position - GameObject.FindGameObjectWithTag ("Phand").transform.position;
		} else if(throw_type == 1){
			dir = GameObject.FindGameObjectWithTag ("Bhand").transform.position - GameObject.FindGameObjectWithTag ("Phand").transform.position;
		}
		dir.Normalize ();
		throwing = true; 
		first = true;
		Rigidbody rigidbodys = ball.GetComponent<Rigidbody> ();
		rigidbodys.isKinematic = false;
		rigidbodys.useGravity = false;
		rigidbodys.AddTorque (0,0,-10);
		ball.transform.parent = null;
	}

	void StartBall(){
		Rigidbody rigidbodys = ball.GetComponent<Rigidbody> ();
		rigidbodys.isKinematic = true;
		rigidbodys.useGravity = false;
		ball.transform.position = init_ball_transform;
		ball.transform.SetParent (parent);
		throwing = false;

	}
		
	void transformBall(){
		ball.transform.Translate (dir * Time.deltaTime * 30, Space.World);
		if (throw_type == 0) {
			if ((ball.transform.position - GameObject.FindGameObjectWithTag ("Chand").transform.position).magnitude < 3.0f) {
				dir = GameObject.FindGameObjectWithTag ("Chand").transform.position - ball.transform.position;
				dir.Normalize ();
				if (first) {
					first = false;
					GameObject.Find ("C").GetComponent<Catcher> ().animationCenterCatch ();
					GameObject.Find ("batter").GetComponent<Batter> ().animationBatting ();
				}
			}

			if ((ball.transform.position - GameObject.FindGameObjectWithTag ("Chand").transform.position).magnitude < 0.5f) {
				Rigidbody rigidbodys = ball.GetComponent<Rigidbody> ();
				rigidbodys.isKinematic = true;
				rigidbodys.useGravity = false;
				throwing = false;
				Transform cparent = GameObject.FindGameObjectWithTag ("Chand").transform;
				ball.transform.position = cparent.position + new Vector3 (-0.6f, -0.01f, 0.13f);
				ball.transform.SetParent (cparent);
			} 
		} else if (throw_type == 1) {
			if ((ball.transform.position - GameObject.FindGameObjectWithTag ("Bhand").transform.position).magnitude < 18.0f) {
				dir = GameObject.FindGameObjectWithTag ("Bhand").transform.position - ball.transform.position;
				dir.Normalize ();
				if (first) {
					first = false;
					GameObject.Find ("batter").GetComponent<Batter> ().animationBatting ();
				}
			}

			if ((ball.transform.position - GameObject.FindGameObjectWithTag ("Bhand").transform.position).magnitude < 0.5f) {
				throwing = false;
				Rigidbody rigidbodys = ball.GetComponent<Rigidbody> ();
				rigidbodys.isKinematic = false;
				rigidbodys.useGravity = true;
				rigidbodys.velocity = Vector3.zero;
				rigidbodys.angularVelocity = Vector3.zero;
				rigidbodys.AddTorque (0,0,-10);
				Vector3 throwAngle = -transform.forward*300;
				throwAngle.y = 250;
				float throwPower = 0.08f;
				rigidbodys.AddForce (throwAngle * throwPower, ForceMode.Impulse);
			} 
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (throw_start)
			throwBall ();
		if (throwing)
			transformBall ();
	}
}
