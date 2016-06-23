using UnityEngine;
using System.Collections;

public class Batter : MonoBehaviour {
	GameObject bat;
	public bool is_hit = false;
	public bool batting = false;

	// Use this for initialization
	void Start () {
		bat = GameObject.FindGameObjectWithTag("bat");
	}

	void EndHit(string state){
		GetComponent<Animator> ().SetBool ("ishit", false);
		GetComponent<Animator> ().SetBool ("batting", false);
		if(state.Equals("hit"))
			GetComponent<Runner> ().runAnimation ();
	}

	void ThrowBat(){
		bat.SetActive(false);
		GameObject.Find("InGameUI").GetComponent<InGame>().createRunner ();
	}


	public void setBatting(bool hit){
		is_hit = hit;
	}

	public void animationBatting(){
		if(is_hit)
			GetComponent<Animator> ().SetBool ("ishit", true);
		else
			GetComponent<Animator> ().SetBool ("ishit", false);
		if(batting)
			GetComponent<Animator> ().SetBool ("batting", true);
		else
			GetComponent<Animator> ().SetBool ("batting", false);
	}

	// Update is called once per frame
	void Update () {

	}
}