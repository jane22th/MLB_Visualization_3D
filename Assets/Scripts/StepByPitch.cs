using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StepByPitch : MonoBehaviour {
	List<object>[] list_pitch, list_atbat, list_runner;
	public List<string> list_base;
	int list_pitch_idx, list_atbat_idx, list_runner_idx;
	public int ball, strike, out_cnt;
	public int inning, home_score, away_score;
	public string pitcher, batter;
	public string first_base, second_base, third_base;

	// pitcher id, batter id, b, s, o, first second thrid base runner id, score per inning, current inning, current attacker
	// batter is left, right stand?, description

	// Use this for initialization
	void Start () {
		list_pitch_idx = list_atbat_idx = list_runner_idx = 0;

		ball = strike = out_cnt = 0;
		inning = 2; // inning / 2 = current inning. And if inning is even, then away is attacking. If inning is odd, then home is attacking.
		home_score = away_score = 0;

		list_base = new List<string>();
	}

	public void getDBInfo() {
		DBSubject db = new DBSubject();
		string query = "select num, des, on_1b, on_2b, on_3b, pitcher, batter, type from pitch where game_id = '" + CurrentState.game_id + "'";
		list_pitch = db.requestQuery(query);
		query = "select num, event, event2, event3, event4, home_team_runs, away_team_runs, stand from atbat where game_id = '" + CurrentState.game_id +"'";
		list_atbat = db.requestQuery(query);
		query = "select atbat, runner, score, start_base, end_base from runner where game_id = '" + CurrentState.game_id + "'";
		list_runner = db.requestQuery(query);
		db.closeAllConn();
		Debug.Log(CurrentState.game_id);
	}

	public string getNumFromPitch() {
		try {
			return list_pitch[list_pitch_idx+1][0].ToString();
		} catch {
			return null;
		}
	}

	public string getDesFromPitch() {
		try {
			return list_pitch[list_pitch_idx][1].ToString();
		} catch {
			return null;
		}
	}

	string getFirstBaseFromPitch() {
		try {
			return list_pitch[list_pitch_idx+1][2].ToString();
		} catch {
			return null;
		}
	}

	string getSecondBaseFromPitch() {
		try {
			return list_pitch[list_pitch_idx+1][3].ToString();
		} catch {
			return null;
		}
	}

	string getThirdBaseFromPitch() {
		try {
			return list_pitch[list_pitch_idx+1][4].ToString();
		} catch {
			return null;
		}
	}

	string getPitcher() {
		try {
			return list_pitch[list_pitch_idx][5].ToString();
		} catch {
			return null;
		}
	}

	string getBatter() {
		try {
			return list_pitch[list_pitch_idx][6].ToString();
		} catch {
			return null;
		}
	}

	public string getTypeFromPitch() {
		try {
			return list_pitch[list_pitch_idx+1][7].ToString();
		} catch {
			return null;
		}
	}

	string getEvent1FromAtbat(bool is_current) {
		int t = list_atbat_idx;
		if(is_current == false) t++;
		try {
			return list_atbat[t][1].ToString();
		} catch {
			return null;
		}
	}

	string getEvent2FromAtbat(bool is_current) {
		int t = list_atbat_idx;
		if(is_current == false) t++;
		try {
			return list_atbat[t][2].ToString();
		} catch {
			return null;
		}
	}

	string getEvent3FromAtbat(bool is_current) {
		int t = list_atbat_idx;
		if(is_current == false) t++;
		try {
			return list_atbat[t][3].ToString();
		} catch {
			return null;
		}
	}

	string getEvent4FromAtbat(bool is_current) {
		int t = list_atbat_idx;
		if(is_current == false) t++;
		try {
			return list_atbat[t][4].ToString();
		} catch {
			return null;
		}
	}

	string getHomeScoreFromAtbat(bool is_current) {
		int t = list_atbat_idx;
		if(is_current == false) t++;
		try {
			return list_atbat[t][5].ToString();
		} catch {
			return null;
		}
	}

	string getAwayScoreFromAtbat(bool is_current) {
		int t = list_atbat_idx;
		if(is_current == false) t++;
		try {
			return list_atbat[t][6].ToString();
		} catch {
			return null;
		}
	}

	public string getStandFromAtbat() {
		try {
			return list_atbat[list_atbat_idx][7].ToString();
		} catch {
			return null;
		}
	}

	string getNumFromRunner() {
		try {
			return list_runner[list_runner_idx][0].ToString();
		} catch {
			return null;
		}
	}

	string getRunnerFromRunner() {
		try {
			return list_runner[list_runner_idx][1].ToString();
		} catch {
			return null;
		}
	}

	string getScoreFromRunner() {
		try {
			return list_runner[list_runner_idx][2].ToString();
		} catch {
			return null;
		}
	}

	string getStartBaseFromRunner() {
		try {
			return list_runner[list_runner_idx][3].ToString();
		} catch {
			return null;
		}
	}

	string getEndBaseFromRunner() {
		try {
			return list_runner[list_runner_idx][4].ToString();
		} catch {
			return null;
		}
	}

	void checkStrike() {
		strike++;
		if(strike == 3) {
			checkOut();
			readDesFromAtbat();
		}
	}

	void checkOut() {
		out_cnt++;
		clearCount();
		if(out_cnt == 3) {
			out_cnt = 0;
			inning++;
			first_base = second_base = third_base = "";
		}
	}

	void checkBall() {
		ball++;
		if(ball == 4) {
			first_base = getFirstBaseFromPitch();
			second_base = getSecondBaseFromPitch();
			third_base = getThirdBaseFromPitch();
			clearCount();
			readDesFromAtbat();
		}
	}

	public void clearCount() {
		ball = strike = 0;
	}

	bool checkRunner() {
		string runner, start_base, end_base;
		bool flag = false;

		while(int.Parse(getNumFromPitch()) == int.Parse(getNumFromRunner()) + 1) {
			runner = getRunnerFromRunner();
			start_base = getStartBaseFromRunner();
			end_base = getEndBaseFromRunner();
			if(getScoreFromRunner().CompareTo("True") == 0) {
				if(inning%2 == 0) {
					away_score++;
				} else {
					home_score++;
				}
			}
			list_base.Add(runner + "," + end_base + "," + getScoreFromRunner());
			list_runner_idx++;
			flag = true;
		}

		return flag;
	}

	public void readDesFromPitch() {
		Debug.Log("pitch : " + getDesFromPitch());
		GameObject.Find ("PH").GetComponent<Pitcher>().throw_type = 0;
		GameObject.Find ("PH").GetComponent<Pitcher>().is_swing = false;

		list_base.Clear();
		if(getDesFromPitch().CompareTo("In play, out(s)") == 0) { // 현재 타자는 아웃, 그럼 주루도 아웃 되는 경우가 있나?
			checkOut();
			readDesFromAtbat();
		} else if(getDesFromPitch().CompareTo("Called Strike") == 0) { // 휘두르지 않고 스트라이크. 3s면 아웃
			checkStrike();
			GameObject.Find ("batter").GetComponent<Batter> ().batting = false;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = true;
		} else if(getDesFromPitch().CompareTo("Swinging Strike") == 0) { // 휘두르고 스트라이크. 3s면 아웃
			checkStrike();
			GameObject.Find ("batter").GetComponent<Batter> ().batting = true;
			GameObject.Find ("batter").GetComponent<Batter> ().is_hit = false;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = true;
		} else if(getDesFromPitch().CompareTo("Foul") == 0) { // 단순 foul. 아웃될 상황없음
			if(strike <= 1) {
				strike++;
			}
		} else if(getDesFromPitch().CompareTo("Ball") == 0) { // 4볼이면 진루
			checkBall();
			GameObject.Find ("batter").GetComponent<Batter> ().batting = false;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = true;
		} else if(getDesFromPitch().CompareTo("Swinging Strike (Blocked)") == 0) { // 쳤지만 빗맞고 공이 땅맞고 포수품으로 간 경우.
			checkStrike();
			GameObject.Find ("batter").GetComponent<Batter> ().batting = true;
			GameObject.Find ("batter").GetComponent<Batter> ().is_hit = false;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = true;
		} else if(getDesFromPitch().CompareTo("Foul Tip") == 0) { // 배트에 스쳤지만 그대로 포수에게 잡힘. 스트라이크로 인정.
			checkStrike();
			GameObject.Find ("batter").GetComponent<Batter> ().batting = false;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = true;
		} else if(getDesFromPitch().CompareTo("In play, run(s)") == 0) { // 쳤음. (안타 or 홈런)
			readDesFromAtbat();
			GameObject.Find ("PH").GetComponent<Pitcher>().throw_type = 1;
			GameObject.Find ("PH").GetComponent<Pitcher>().is_swing = true;
			GameObject.Find ("batter").GetComponent<Batter> ().batting = true;
			GameObject.Find ("batter").GetComponent<Batter> ().is_hit = true;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = false;
		} else if(getDesFromPitch().CompareTo("In play, no out") == 0) { // 안타인듯
			readDesFromAtbat();
			Debug.Log ("inplaynoout");
			GameObject.Find ("PH").GetComponent<Pitcher>().throw_type = 1;
			GameObject.Find ("PH").GetComponent<Pitcher>().is_swing = true;
			GameObject.Find ("batter").GetComponent<Batter> ().batting = true;
			GameObject.Find ("batter").GetComponent<Batter> ().is_hit = true;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = false;
		} else if(getDesFromPitch().CompareTo("Ball In Dirt") == 0) { // 4볼이면 진루
			checkBall();
			GameObject.Find ("batter").GetComponent<Batter> ().batting = false;
			GameObject.Find ("C").GetComponent<Catcher> ().is_catch = true;
		} else if(getDesFromPitch().CompareTo("Foul Bunt") == 0) { // strike와 같이 처리.
			checkStrike();
		} else if(getDesFromPitch().CompareTo("Hit By Pitch") == 0) { // 데드볼 진루
			readDesFromAtbat();
		} else if(getDesFromPitch().CompareTo("Intent Ball") == 0) { // 볼처리 (고의사구)
			checkBall();
		} else if(getDesFromPitch().CompareTo("Missed Bunt") == 0) { // 스트라이크 처리
			checkStrike();
		} else if(getDesFromPitch().CompareTo("Foul (Runner Going)") == 0) {
			if(strike <= 1) {
				strike++;
			}
		} else if(getDesFromPitch().CompareTo("Pitchout") == 0) {
			checkBall();
		}
		pitcher = getPitcher();
		batter = getBatter();

		string t_first_base, t_second_base, t_third_base;

		t_first_base = first_base;
		t_second_base = second_base;
		t_third_base = third_base;

		first_base = getFirstBaseFromPitch();
		second_base = getSecondBaseFromPitch();
		third_base = getThirdBaseFromPitch();

		if(!checkRunner()) {
			List<string> temp = new List<string>();
			if(!t_first_base.Equals(first_base)) {
				temp.Add(first_base);
			}
			if(!t_second_base.Equals(second_base)) {
				temp.Add(second_base);
			}
			if(!t_third_base.Equals(third_base)) {
				temp.Add(third_base);
			}
			for(int i=0; i<temp.Count; i++) {
				int n = int.Parse(getNumFromRunner());
				int idx = list_runner_idx;
				while(n == int.Parse(getNumFromRunner())) {
					string runner = getRunnerFromRunner();
					if(runner.Equals(temp[i])) {
						list_runner[list_runner_idx][1] = "a";
						list_base.Add(runner + "," + getEndBaseFromRunner() + "," + getScoreFromRunner());
					}					
					list_runner_idx++;
				}
				list_runner_idx = idx;
			}
		}

		list_pitch_idx++;

		if(list_pitch.Length == list_pitch_idx) {
			Debug.Log("game set");
			return ;
			//gameset
		}
	}

	public void readDesFromAtbat() {
		Debug.Log("atbat : " + getEvent1FromAtbat(true));
		// event1
		if(getEvent1FromAtbat(true).CompareTo("Groundout") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Strikeout") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Flyout") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Lineout") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Walk") == 0) {
			// to first base
		} else if(getEvent1FromAtbat(true).CompareTo("Sac Bunt") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Forceout") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Home Run") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Single") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Pop Out") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Bunt Pop Out") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Hit By Pitch") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Grounded Into DP") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Intent Walk") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Double Play") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Field Error") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Runner Out") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Double") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Bunt Lineout") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Sac Fly") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Fielder Choice Out") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Triple") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Catcher Interference") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Sac Fly DP") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Fielders Choice") == 0) {

		} else if(getEvent1FromAtbat(true).CompareTo("Bunt Groundout") == 0) {

		}

		// event2
		if(getEvent2FromAtbat(true).CompareTo("NULL") == 0) {
		} else if(getEvent2FromAtbat(true).CompareTo("Error") == 0) {
		} else if(getEvent2FromAtbat(true).CompareTo("Stolen Base 3B") == 0) {
		} else if(getEvent2FromAtbat(true).CompareTo("Stolen Base 2B") == 0) {
		} else if(getEvent2FromAtbat(true).CompareTo("Runner Out") == 0) {
		} else if(getEvent2FromAtbat(true).CompareTo("Wild Pitch") == 0) {
		}

		// event3
		if(getEvent3FromAtbat(true).CompareTo("NULL") == 0) {
		} else if(getEvent3FromAtbat(true).CompareTo("Error") == 0) {
		} else if(getEvent3FromAtbat(true).CompareTo("Stolen Base 3B") == 0) {
		} else if(getEvent3FromAtbat(true).CompareTo("Stolen Base 2B") == 0) {
		} else if(getEvent3FromAtbat(true).CompareTo("Runner Out") == 0) {
		} else if(getEvent3FromAtbat(true).CompareTo("Wild Pitch") == 0) {
		}

		// event 4
		if (getEvent4FromAtbat(true).CompareTo("NULL") == 0) {
		} else if (getEvent4FromAtbat(true).CompareTo("Error") == 0) {
		} else if (getEvent4FromAtbat(true).CompareTo("Stolen Base 3B") == 0) {
		} else if (getEvent4FromAtbat(true).CompareTo("Stolen Base 2B") == 0) {
		} else if (getEvent4FromAtbat(true).CompareTo("Runner Out") == 0) {
		} else if (getEvent4FromAtbat(true).CompareTo("Wild Pitch") == 0) {
		}

		list_atbat_idx++;
	}
	void GoNextStep() {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
