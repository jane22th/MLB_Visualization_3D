using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {
	int end_base;
	int final_base;
	Vector3 go_dir;
	public int current_base;
	public bool running = false;
	public int speed = 100;
	GameObject t;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}

	public void runAnimation(){
		animator.SetBool ("isrun", true);
	}

	public void outAnimation(){
		animator.SetTrigger ("isout");
	}

	void EndOut(){
		Destroy (gameObject, 1.0f);
	}

	void EndGoal(){
		Destroy (gameObject, 1.5f);
	}

	public void setInfo(int e){
		final_base = e;
		if (final_base - current_base == 1)
			end_base = e;
		else {
			end_base = current_base + 1;
		}

		string str = end_base.ToString () + "BR";
		t = GameObject.Find (str);
		GetComponent<Fielder> ().target = t.transform;

		go_dir = t.transform.position - transform.position;
		go_dir.y = 0;
		go_dir.Normalize ();

		runAnimation ();
		running = true;
	}

	void run(){
		transform.Translate (go_dir * Time.deltaTime * speed, Space.World);
		if (end_base == 4) {
			if (Mathf.Abs ((t.transform.position - transform.position).magnitude) <= 5.0f) {
				animator.SetBool ("isgoal", true);
				animator.SetBool ("isrun", false);
			}
		}
		//Debug.Log (Mathf.Abs((InGame.base_position [end_base] - transform.position).magnitude));
		if (Mathf.Abs((t.transform.position - transform.position).magnitude) <= 0.6f) {
			running = false;
			current_base = end_base;
			if (current_base != final_base) {
				setInfo (final_base);
			}
			else {
				name = "batter" + end_base.ToString ();
				if (end_base != 4) {
					gameObject.GetComponent<Fielder> ().target = GameObject.Find ("ball").transform;
					Vector3 final_pos = t.transform.position;
					final_pos.y = 0;
					final_pos.y += transform.position.y;
					transform.position = final_pos;
					animator.SetBool ("isrun", false);
					animator.SetBool ("isgoal", false);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			run ();
		}
	}
}
