using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGame : MonoBehaviour {
	public GameObject manager;
	public bool step = false;
	public int current_pitcher_id;
	public int current_batter_id;
	public int step_index = -1;
	public int step_atbat_index = 0;
	public bool is_top = true;
	public string current_half = "top";
	GameObject batter_maruta;
	public StepByPitch step_by_pitch;

	int current_inning = 2;

	int[] inning_away_score;
	int[] inning_home_score;

	int accumulated_home_score = 0, accumulated_away_score = 0;

	Sprite[] number_sprites;
	bool going = false;

	// Use this for initialization
	void Start () {
		CurrentState.runner = new GameObject[4];

		batter_maruta = GameObject.Find ("batter_maruta");
		batter_maruta.SetActive (false);

		manager = GameObject.Find("InGameManger");
		manager.SetActive(false);
		number_sprites = Resources.LoadAll<Sprite> ("numbers");

		inning_away_score = new int[11];
		inning_home_score = new int[11];

		step_by_pitch = GameObject.Find ("InGameUI").GetComponent<StepByPitch> ();
	}

	void pitcherNameSet(){
		if (current_pitcher_id != int.Parse(step_by_pitch.pitcher)) {
			if (is_top) {
				for (int i = 0; i < CurrentState.list_h.Length; i++) {
					if (int.Parse(step_by_pitch.pitcher) == (int)CurrentState.list_h [i] [3]) {
						current_pitcher_id = int.Parse (step_by_pitch.pitcher);
						GameObject.Find ("PH").GetComponentInChildren<TextMesh> ().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
						GameObject.Find ("PH").GetComponent<Fielder>().player_code = CurrentState.list_h [i] [3].ToString();
						GameObject.Find ("HomePH").GetComponent<Text>().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
						GameObject.Find ("Homepa").GetComponent<Text>().text = "PITCHING : " + CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
					}
				}
			} else {
				for (int i = 0; i < CurrentState.list_a.Length; i++) {
					if (int.Parse(step_by_pitch.pitcher) == (int)CurrentState.list_a [i] [3]) {
						current_pitcher_id = int.Parse(step_by_pitch.pitcher);
						GameObject.Find ("PH").GetComponentInChildren<TextMesh>().text = CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();	
						GameObject.Find ("PH").GetComponent<Fielder>().player_code = CurrentState.list_a [i] [3].ToString();
						GameObject.Find ("AwayPH").GetComponent<Text>().text = CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();
						GameObject.Find ("Awaypa").GetComponent<Text>().text = "PITCHING : " + CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();
					}
				}
			}
		}
	}

	void batterNameSet(){
		Debug.Log ("batter id : " + int.Parse(step_by_pitch.batter) + " vs " + current_batter_id);
		if (current_batter_id != int.Parse(step_by_pitch.batter)) {
			Debug.Log ("batter id : " + int.Parse(step_by_pitch.batter));
			if (!is_top) {
				for (int i = 0; i < CurrentState.list_h.Length; i++) {
					if (int.Parse(step_by_pitch.batter) == (int)CurrentState.list_h [i] [3]) {
						current_batter_id = int.Parse(step_by_pitch.batter);
						GameObject.Find ("batter").GetComponentInChildren<TextMesh> ().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
						GameObject.Find ("batter").GetComponent<Fielder>().player_code = CurrentState.list_h [i] [3].ToString();
						GameObject.Find ("Homepa").GetComponent<Text>().text = "AT BAT : " + CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
					}
				}
			} else {
				for (int i = 0; i < CurrentState.list_a.Length; i++) {
					if (int.Parse(step_by_pitch.batter) == (int)CurrentState.list_a [i] [3]) {
						current_batter_id = int.Parse(step_by_pitch.batter);
						GameObject.Find ("batter").GetComponentInChildren<TextMesh>().text = CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();	
						GameObject.Find ("batter").GetComponent<Fielder>().player_code = CurrentState.list_a [i] [3].ToString();
						GameObject.Find ("Awaypa").GetComponent<Text>().text = "AT BAT : " + CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();
					}
				}
			}
			standSet ();
		}
	}

	void halfSet(){
		if (!is_top && (step_by_pitch.inning+1) % 2 == 0) {
			is_top = true;
			GameObject.Find ("playerlist").GetComponent<LineUpEntry> ().setTopLineUp ();
			Debug.Log ("to top");
		} else if(is_top && (step_by_pitch.inning+1) % 2 != 0) {
			is_top = false;
			GameObject.Find ("playerlist").GetComponent<LineUpEntry> ().setBottomLineUp ();
			Debug.Log ("to bottom");
		}
	}

	void desSet(){
		GameObject.Find ("Description").GetComponent<Text> ().text = step_by_pitch.getDesFromPitch();
	
	}

	void standSet(){
		GameObject batter = GameObject.Find ("batter");
		if (step_by_pitch.getStandFromAtbat().Equals("R")) {
			batter.transform.position = new Vector3 (516.85f, -3.06f, -867.59f);	
			batter.transform.localScale = new Vector3(1, 1, 1);
		} else {
			batter.transform.position = new Vector3 (515.41f, -3.06f, -866.168f);	
			batter.transform.localScale = new Vector3(-1, 1, 1);
		}
	}
		
	public void createRunner(){
		GameObject par = GameObject.Find ("playerlist");

		string[][] lb = new string[step_by_pitch.list_base.Count][];

		for (int i = 0; i < step_by_pitch.list_base.Count; i++) {
			lb[i] = step_by_pitch.list_base [i].Split (new char[]{','});
			Debug.Log ("i : " + i + "base : " + lb [i] [1]);
		}
			
		CurrentState.runner [0] = GameObject.Find ("batter");
		if (GameObject.Find ("batter1"))
			CurrentState.runner [1] = GameObject.Find ("batter1");
		if (GameObject.Find ("batter2"))
			CurrentState.runner [2] = GameObject.Find ("batter2");
		if (GameObject.Find ("batter3"))
			CurrentState.runner [3] = GameObject.Find ("batter3");

		for (int i = 0; i < 4; i++) {
			if (CurrentState.runner [i]) {
				for (int j = 0; j < step_by_pitch.list_base.Count; j++) {
					if (CurrentState.runner [i].GetComponent<Fielder> ().player_code.Equals (lb [j] [0])) {
						if (lb [j] [1].Equals ("1B")) {
							if (CurrentState.runner [i].GetComponent<Runner> ().current_base != 1) {
								CurrentState.runner [i].GetComponent<Runner> ().setInfo (1);
							}
						} else if (lb [j] [1].Equals ("2B")) {
							if (CurrentState.runner [i].GetComponent<Runner> ().current_base != 2) {
								CurrentState.runner [i].GetComponent<Runner> ().setInfo (2);
							}
						} else if (lb [j] [1].Equals ("3B")) {
							if (CurrentState.runner [i].GetComponent<Runner> ().current_base != 3) {
								CurrentState.runner [i].GetComponent<Runner> ().setInfo (3);
							}
						}  else {
							//runner일 경우에만 
							if (i != 0 && lb [j] [2].Equals ("True"))
								CurrentState.runner [i].GetComponent<Runner> ().setInfo (4);
							else {
								Debug.Log ("out animation");
								CurrentState.runner [i].GetComponent<Runner> ().outAnimation ();
							}
						}
					} 
				}
			}
		}
	}


	void scoreBSOSet(){
		GameObject par = GameObject.Find ("scoreboard");
		int inning = step_by_pitch.inning/2;

		int home_score = step_by_pitch.home_score;
		int away_score = step_by_pitch.away_score;

		if (is_top) {
			if (current_inning == step_by_pitch.inning)
				inning_away_score [inning] = away_score - accumulated_away_score;
			else {
				accumulated_away_score += inning_away_score [inning - 1];
				inning_away_score [inning] = away_score - accumulated_away_score;
			}
		} else {
			if (current_inning == step_by_pitch.inning)
				inning_home_score [inning] = home_score - accumulated_home_score;
			else {
				accumulated_home_score += inning_home_score [inning - 1];
				inning_home_score [inning] = home_score - accumulated_home_score;
			}
		}

		current_inning = step_by_pitch.inning;

		foreach (Transform child in par.transform) {
			if (child.gameObject.name.Equals ("B")) {
				child.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.ball];
			} else if (child.gameObject.name.Equals ("S")) {
				child.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.strike];
			} else if (child.gameObject.name.Equals ("O")) {
				child.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.out_cnt];
			} else if (child.gameObject.name.Equals ("away")) {
				foreach (Transform childtochild in child.transform) {
					if (int.Parse (childtochild.gameObject.name) <= inning) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [inning_away_score [int.Parse (childtochild.gameObject.name)]];
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
					}

					if (int.Parse (childtochild.gameObject.name) == 11) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.away_score/10];
					}

					if (int.Parse (childtochild.gameObject.name) == 12) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.away_score%10];
					}
				}
			} else if (child.gameObject.name.Equals ("home")) {
				foreach (Transform childtochild in child.transform) {
					if (int.Parse (childtochild.gameObject.name) < inning || (int.Parse (childtochild.gameObject.name) == inning && !is_top)) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [inning_home_score [int.Parse (childtochild.gameObject.name)]];
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
					}

					if (int.Parse (childtochild.gameObject.name) == 11) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.home_score/10];
					}

					if (int.Parse (childtochild.gameObject.name) == 12) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().sprite = number_sprites [step_by_pitch.home_score%10];
					}
				}
			}
		}
	}

	void goStep(){
		step = false;

		if (GameObject.Find ("batter") == null) {
			GameObject batter = Instantiate (batter_maruta);
			batter.name = "batter";
			batter.GetComponent<Fielder> ().player_code = "0";
			batter.transform.SetParent (GameObject.Find("playerlist").transform);
			batter.SetActive (true);
			GameObject bat = GameObject.FindGameObjectWithTag ("bat2");
			bat.tag = "bat";
			GameObject batpos = GameObject.FindGameObjectWithTag ("Bhand2");
			batpos.tag = "Bhand";
		}

		desSet ();
		step_by_pitch.readDesFromPitch ();

		if(step_by_pitch.is_changed_inning)
			halfSet ();
		pitcherNameSet ();
		batterNameSet ();
		scoreBSOSet ();
		GameObject.Find ("PH").GetComponent<Pitcher> ().throw_start = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (step) {
			step = false;
			goStep ();
		}
	}
}
