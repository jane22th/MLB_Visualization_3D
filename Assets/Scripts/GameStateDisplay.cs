using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class GameStateDisplay : MonoBehaviour {
	public GameObject m_game_mode_canvas, m_game_select_canvas, m_game_load_canvas;
	public GameObject m_year_obj, m_month_obj, m_day_obj, m_search_obj, m_back_obj;
	public GameObject m_game_list_obj;
	public Dropdown m_year, m_month, m_day;
	public Button m_search, m_back_btn;

	const int START_YEAR = 1950; // year of the initial game
	int[] m_day_list_ = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }; // [0] is nothing. just for start index to 1 not 0.

	void Start() {
		m_game_mode_canvas = GameObject.Find("GameModeUI");
		m_game_load_canvas = GameObject.Find("GameLoadUI");
		m_game_select_canvas = GameObject.Find("GameSelectUI");

		m_year_obj = GameObject.Find("Year_Dropdown");
		m_month_obj = GameObject.Find("Month_Dropdown");
		m_day_obj = GameObject.Find("Day_Dropdown");
		m_search_obj = GameObject.Find("Search");
		m_back_obj = GameObject.Find("GameSelectUI/Back");

		m_game_list_obj = GameObject.Find("ButtonList");

		m_year = m_year_obj.GetComponent<Dropdown>();
		m_month = m_month_obj.GetComponent<Dropdown>();
		m_day = m_day_obj.GetComponent<Dropdown>();
		m_search = m_search_obj.GetComponent<Button>();
		m_back_btn = m_back_obj.GetComponent<Button>();

		// add listener for each object
		m_month.onValueChanged.AddListener(delegate {
			monthHandler(m_month);
		});
		m_search.onClick.AddListener(delegate {
			searchHandler(m_search);
		});
		m_back_btn.onClick.AddListener(delegate {
			backHandler();
		});

		m_year.options.Clear();
		m_month.options.Clear();
		m_day.options.Clear();

		// year option (START_YEAR(1950) ~ 2016)
		m_year.options.Add(new Dropdown.OptionData() { text = "select" }); // default value. It means that option is not selected
		for (int i = START_YEAR; i <= 2016; i++) {
			m_year.options.Add(new Dropdown.OptionData() { text = "" + i });
		}
		m_year.RefreshShownValue();

		// month option (1 ~ 12)
		m_month.options.Add(new Dropdown.OptionData() { text = "select" });
		for (int i = 1; i <= 12; i++) {
			m_month.options.Add(new Dropdown.OptionData() { text = "" + i });
		}
		m_month.RefreshShownValue();

		// day option
		m_day.options.Add(new Dropdown.OptionData() { text = "select" });
		m_day.RefreshShownValue();
	}

	void monthHandler(Dropdown obj) {
		try {
			if (m_month.value == 2 && isLeapYear(int.Parse(m_year.options[m_year.value].text)) == true) {
				m_day_list_[2] = 29;
			} else {
				m_day_list_[2] = 28;
			}
		} catch {
		}
		m_day.options.Clear();
		m_day.options.Add(new Dropdown.OptionData() { text = "select" });

		for (int i = 1; i <= m_day_list_[m_month.value]; i++) {
			m_day.options.Add(new Dropdown.OptionData() { text = "" + i });
		}

		m_day.value = 0;
		m_day.RefreshShownValue();
	}

	void searchHandler(Button obj) {
		/*m_year.value = 2012;
		m_month.value = 4;
		m_day.value = 2;*/
		if (m_year.value != 0 && m_month.value != 0 && m_day.value != 0) { // if the game day is correctly selected
			foreach (Transform c in m_game_list_obj.transform) { // clear game list
				GameObject.Destroy(c.gameObject);
			}
			string date = "" + (m_year.value + 1949) + "-" + m_month.value + "-" + m_day.value;
			CurrentState.current_date = date;
			string query = "SELECT home_fname, away_fname, game_id FROM game where date = {d '" + date + "'}";

			DBSubject db = new DBSubject();
			List<object>[] list = db.requestQuery(query);
			Debug.Log(list.Length);
			for(int i=0; i<list.Length; i++) {
				string home = list[i][0].ToString();
				string away = list[i][1].ToString();
				string game_id = list[i][2].ToString();

				GameObject btn_obj = new GameObject();
				btn_obj.name = "GameButton_" + (i+1);
				btn_obj.transform.SetParent(m_game_list_obj.transform);

				GameObject btn_text = new GameObject();
				btn_text.name = "text";
				btn_text.transform.SetParent(btn_obj.transform);

				Text txt = btn_text.AddComponent<Text>();
				txt.text = home.PadRight(30, ' ') + "vs" + away.PadLeft(30, ' ');
				txt.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
				txt.color = Color.black;
				txt.alignment = TextAnchor.MiddleCenter;
				RectTransform txt_r = txt.GetComponent<RectTransform>();
				txt_r.sizeDelta = new Vector2(500, 30);

				Image img = btn_obj.AddComponent<Image>();
				img.sprite = m_search_obj.GetComponent<Image>().sprite;
				img.type = Image.Type.Sliced;
				img.fillCenter = true;

				Button btn = btn_obj.AddComponent<Button>();
				btn.targetGraphic = img;

				RectTransform btn_r = btn_obj.GetComponent<RectTransform>();
				btn_r.position = new Vector3(0, 0, 0);
				btn_r.localPosition = new Vector3(321.5f, -30 - i*40, 0);
				btn_r.sizeDelta = new Vector2(500, 30);

				btn.onClick.AddListener(delegate {
					selectHandler(home + "," + away + "," + game_id);
				});
			}
		}
	}

	void selectHandler(string match) {
		string[] args = match.Split(','); // args[0] = home, args[1] = away, args[2] = game_id
		CurrentState.game_id = args[2];
		Camera.main.GetComponent<MainCameraHandler>().auto = false;
		Camera.main.transform.position = CameraManager.cam_info [1].pos;
		Camera.main.transform.rotation = CameraManager.cam_info [1].rot;
		Camera.main.GetComponent<Camera>().fieldOfView = CameraManager.cam_info [1].fov;

		m_game_mode_canvas.SetActive(false);
		m_game_load_canvas.SetActive(false);
		m_game_select_canvas.SetActive(false);
		GameObject.Find("InGameUI").GetComponent<InGame>().manager.SetActive (true);

		GameObject.Find("GameStateUI").GetComponent<TabUI>().setTeamInfo();
		GameObject.Find ("InGameUI").GetComponent<StepByPitch> ().getDBInfo ();
		
	}

	void backHandler() {
		m_game_load_canvas.SetActive(true);
		m_game_select_canvas.SetActive(false);
	}

	bool isLeapYear(int n) { // check that n is leap year
		if ((n % 4 == 0 && n % 100 != 0) || n % 400 == 0) {
			return true;
		} else {
			return false;
		}
	}
}

