using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LineUpEntry : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}

	void setScoreboard(){
		GameObject par = GameObject.Find ("scoreboard");
		foreach (Transform child in par.transform) {
			if (child.gameObject.name.Equals ("B")) {
				child.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			} else if (child.gameObject.name.Equals ("S")) {
				child.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			} else if (child.gameObject.name.Equals ("O")) {
				child.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			} else if (child.gameObject.name.Equals ("away")) {
				foreach (Transform childtochild in child.transform) {
					if (int.Parse (childtochild.gameObject.name) == 1) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;	
					}

					if (int.Parse (childtochild.gameObject.name) == 11) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;	
					}

					if (int.Parse (childtochild.gameObject.name) == 12) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;	
					}
				}
			} else if (child.gameObject.name.Equals ("home")) {
				foreach (Transform childtochild in child.transform) {
					if (int.Parse (childtochild.gameObject.name) == 11) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;	
					}

					if (int.Parse (childtochild.gameObject.name) == 12) {
						childtochild.gameObject.GetComponent<SpriteRenderer> ().enabled = true;	
					}
				}
			}
		}
	}

	void setTabLineUp(){
		
		for (int i = 0; i < CurrentState.list_h.Length; i++) {
			if (CurrentState.list_h [i] [2] != null) {
				string str = "Home" + CurrentState.list_h [i] [2];
				GameObject pname = GameObject.Find (str);
				if (pname) {
					pname.GetComponent<Text> ().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
				}
			}
		}

		for (int i = 0; i < CurrentState.list_a.Length; i++) {
			if (CurrentState.list_a [i] [2] != null) {
				string str = "Away" + CurrentState.list_a [i] [2];
				GameObject pname = GameObject.Find (str);
				if (pname) {
					pname.GetComponent<Text> ().text = CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();
				}
			}
		}

	}

	public void setLineUp(){
		string home_team_code = CurrentState.home_team_code;
		string away_team_code = CurrentState.away_team_code;

		string query = "SELECT first_name, last_name, current_position, id FROM player where team = '" + home_team_code + "'";
		string query2 = "SELECT first_name, last_name, current_position, id FROM player where team = '" + away_team_code + "'";

		DBSubject db = new DBSubject();
		CurrentState.list_h = db.requestQuery(query);
		CurrentState.list_a = db.requestQuery (query2);

		setScoreboard ();
		setTopLineUp ();

		string query3 = "select pitcher, batter, type, des, on_1b, on_2b, on_3b from pitch where game_id = '" + CurrentState.game_id + "'";
		string query4 = "select batter, half, inning, stand from atbat where game_id = '" + CurrentState.game_id + "'";

		CurrentState.list_pitch = db.requestQuery (query3);
		CurrentState.list_atbat = db.requestQuery (query4);

		for (int i = 0; i < CurrentState.list_h.Length; i++) {
			if ((int)CurrentState.list_pitch [0] [0] == (int)CurrentState.list_h [i] [3]) {
				GameObject.Find ("InGameUI").GetComponent<InGame> ().current_pitcher_id = (int)CurrentState.list_pitch [0] [0];
				GameObject.Find ("PH").GetComponentInChildren<TextMesh>().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
				GameObject.Find ("PH").GetComponent<Fielder> ().player_code = CurrentState.list_h [i] [3].ToString ();
				GameObject.Find ("HomePH").GetComponent<Text>().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
				GameObject.Find ("Homepa").GetComponent<Text>().text = "PITCHING : " + CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
			}
		}

		for (int i = 0; i < CurrentState.list_a.Length; i++) {
			if ((int)CurrentState.list_atbat [0] [0] == (int)CurrentState.list_a [i] [3]) {
				GameObject.Find ("InGameUI").GetComponent<InGame> ().current_batter_id = (int)CurrentState.list_atbat [0] [0];
				GameObject.Find ("batter").GetComponentInChildren<TextMesh>().text = CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();	
				GameObject.Find ("batter").GetComponent<Fielder> ().player_code = CurrentState.list_a [i] [3].ToString ();
				GameObject.Find ("Awaypa").GetComponent<Text>().text = "AT BAT : " + CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();
			}
		}

		setTabLineUp ();
	}

	public void setTopLineUp(){
		GameObject.Find ("InGameManger/TopBottom").GetComponent<Text> ().text = "Top";

		foreach (Transform child in transform) {
			for (int i = 0; i < CurrentState.list_h.Length; i++) {
				if (CurrentState.list_h [i] [2] != null) {
					if (!child.gameObject.name.Equals ("PH") && child.gameObject.name.Equals (CurrentState.list_h [i] [2])) {
						child.GetComponentInChildren<TextMesh> ().text = CurrentState.list_h [i] [0].ToString () + " " + CurrentState.list_h [i] [1].ToString ();
						child.gameObject.GetComponent<Fielder> ().player_code = CurrentState.list_h [i] [3].ToString();
					}
				}
			}
		}
	}

	public void setBottomLineUp(){
		GameObject.Find ("InGameManger/TopBottom").GetComponent<Text> ().text = "Bottom";

		foreach (Transform child in transform) {
			for (int i = 0; i < CurrentState.list_a.Length; i++) {
				if (CurrentState.list_a [i] [2] != null) {
					if (!child.gameObject.name.Equals ("PH") && child.gameObject.name.Equals (CurrentState.list_a [i] [2])) {
						child.GetComponentInChildren<TextMesh> ().text = CurrentState.list_a [i] [0].ToString () + " " + CurrentState.list_a [i] [1].ToString ();
						child.gameObject.GetComponent<Fielder> ().player_code = CurrentState.list_a [i] [3].ToString();
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
