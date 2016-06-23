using UnityEngine;
using System.Collections;

public class Catcher : MonoBehaviour {
	Animator animator;
	GameObject ball;
	Transform cparent;
	public bool is_catch = true;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		ball = GameObject.Find ("ball");
	}

	public void animationCenterCatch(){
		if(is_catch)
			animator.SetTrigger ("catch");
	}

	void ChangeHand(){
		cparent = GameObject.FindGameObjectWithTag ("Chand2").transform;
		ball.transform.SetParent (cparent);
	}

	void ShootBall(){
		ball.transform.parent = null;
		ball.transform.SetParent (GameObject.Find("PH").GetComponent<Pitcher>().parent);
		ball.transform.position = GameObject.Find("PH").GetComponent<Pitcher>().init_ball_transform;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
