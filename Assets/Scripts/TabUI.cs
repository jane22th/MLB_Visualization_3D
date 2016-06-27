using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TabUI : MonoBehaviour {
	GameObject m_speed_obj, m_interval_obj, m_manual_obj;
    Slider m_speed_bar, m_interval_bar;
	Text m_speed_txt, m_interval_txt, m_manual_txt;
	Button m_manual_btn;

	Image m_home_logo, m_away_logo;
	Sprite[] m_logo_img;

	string[] m_logo_team;

	const int MAJOR_TEAM_CNT = 30;
	bool m_is_manual;

	float m_sum_time;

	Text home_info_name, away_info_name, home_info_score, away_info_score;

	Coroutine auto_mode;

	GameObject par;

	// Use this for initialization
	void Start () {
        m_speed_obj = GameObject.Find("GameStateUI/State/Speed");
        m_interval_obj = GameObject.Find("GameStateUI/State/Interval");
		m_manual_obj = GameObject.Find("GameStateUI/State/manual");

        m_speed_bar = GameObject.Find("GameStateUI/State/Speed/Slider").GetComponent<Slider>();
        m_interval_bar = GameObject.Find("GameStateUI/State/Interval/Slider").GetComponent<Slider>();

        m_speed_txt = GameObject.Find("GameStateUI/State/Speed/text_2").GetComponent<Text>();
        m_interval_txt = GameObject.Find("GameStateUI/State/Interval/text_2").GetComponent<Text>();
		m_manual_txt = GameObject.Find("GameStateUI/State/manual/text").GetComponent<Text>();

		m_manual_btn = m_manual_obj.GetComponent<Button>();

        m_speed_bar.onValueChanged.AddListener(delegate { speedHandler(); });
        m_interval_bar.onValueChanged.AddListener(delegate { intervalHandler(); });
		m_manual_btn.onClick.AddListener(delegate { manualHandler(); });

		home_info_name = GameObject.Find("GameStateUI/State/HomeInfo/Name").GetComponent<Text>();
		away_info_name = GameObject.Find("GameStateUI/State/AwayInfo/Name").GetComponent<Text>();

		home_info_score = GameObject.Find("GameStateUI/State/HomeInfo/Score").GetComponent<Text>();
		away_info_score = GameObject.Find("GameStateUI/State/AwayInfo/Score").GetComponent<Text>();

		m_home_logo = GameObject.Find("GameStateUI/State/HomeInfo/Logo").GetComponent<Image>();
		m_away_logo = GameObject.Find("GameStateUI/State/AwayInfo/Logo").GetComponent<Image>();

		m_logo_team = new string[MAJOR_TEAM_CNT]{"Philadelphia Phillies", "Washington Nationals", "Atlanta Braves", "Florida Marlins", "NewYork Mets", "Cincinnati Reds",
								   "Pittsburgh Pirates", "St. Louis Cardinals", "Milwaukee Brewers", "Houston Astros", "Chicago Cubs", "Arizona Diamondbacks",
								   "San Diego Padres", "Los Angeles Dodgers", "Colorado Rockies", "SanFrancisco Giants", "Tampa Bay Rays", "Toronto Blue Jays",
								   "New York Yankees", "Boston Red Sox", "Baltimore Orioles", "Minnesota Twins", "Clevelland Indians", "Detroit Tigers",
								   "Kansas City Royals", "Chicago White Sox", "Texas Rangers", "Los Angeles Angels", "Seattle Mariners", "Oakland Athletics"};

		m_logo_img = new Sprite[MAJOR_TEAM_CNT];
		for(int i=0; i<MAJOR_TEAM_CNT; i++) {
			m_logo_img[i] = Resources.Load<Sprite>("Logo/"+(i+1));
		}

		setTeamInfo();

		m_is_manual = true;
		m_interval_obj.SetActive(false);

		par = GameObject.Find ("playerlist");
	}

	public void setTeamInfo() {
		if(CurrentState.current_date != null && CurrentState.game_id != null) {
			string query = "select home_fname, away_fname, home_wins, home_loss, away_wins, away_loss, home_team_code, away_team_code from game where game_id = '" + CurrentState.game_id + "'";
			DBSubject db = new DBSubject();
			List<object>[] list = db.requestQuery(query);
			string home = list[0][0].ToString(), away = list[0][1].ToString();

			home_info_name.text = home;
			home_info_score.text = "Win : " + list[0][2] + "   Lose : " + list[0][4];
			away_info_name.text = away;
			away_info_score.text = "Win : " + list[0][3] + "   Lose : " + list[0][5];

			CurrentState.home_team_code = list[0][6].ToString();
			CurrentState.away_team_code = list[0][7].ToString();

			for(int i=0; i<MAJOR_TEAM_CNT; i++) {
				Debug.Log(m_logo_team[i] +", " + home + ", " + away);
				if(m_logo_team[i].CompareTo(home) == 0) {
					m_home_logo.sprite = (Sprite)m_logo_img[i];
				} else if(m_logo_team[i].CompareTo(away) == 0) {
					m_away_logo.sprite = (Sprite)m_logo_img[i];
				}
			}

			GameObject.Find("playerlist").GetComponent<LineUpEntry>().setLineUp ();
		}
	}

	public void speedHandler() {
		m_speed_txt.text = m_speed_bar.value/10 +"x";
		CurrentState.prev_time = m_speed_bar.value / 10;
	}

	public void setPause(){
		CurrentState.ispause = true;
		GameObject.Find ("Pause").GetComponent<Text> ().enabled = true;
		Time.timeScale = 0.0f;
	}

	public void setStart(){
		CurrentState.ispause = false;
		Time.timeScale = CurrentState.prev_time;
		GameObject.Find ("Pause").GetComponent<Text> ().enabled = false;
	}

	public void intervalHandler() {
		m_interval_txt.text = m_interval_bar.value + "sec";
	}

	void manualHandler() {
		if(m_is_manual == true) {
			m_is_manual = false;
			m_manual_txt.text = "Auto";
			m_interval_obj.SetActive(true);
			m_sum_time = 0;
			auto_mode = StartCoroutine(autoMode());
		} else {
			StopCoroutine(auto_mode);
			m_is_manual = true;
			m_manual_txt.text = "Manual";
			m_interval_obj.SetActive(false);
		}
	}

	bool checkEndGame(){
		foreach (Transform child in par.transform) {
			if (child.gameObject.GetComponent<Fielder> ().is_animating) {
				return false;
			}
		}
		return true;
	}

	IEnumerator autoMode() {
		yield return new WaitForSeconds(0.1f);
		if (checkEndGame ()) {
			m_sum_time += 0.1f;
			if (m_sum_time >= m_interval_bar.value) {
				GameObject.Find ("InGameUI").GetComponent<InGame> ().step = true;
				m_sum_time = 0;
			}
		} else {
			m_sum_time = 0;
		}
        if(!m_is_manual)
		    auto_mode = StartCoroutine(autoMode());
	}

	void Update() {
		
	}
}
