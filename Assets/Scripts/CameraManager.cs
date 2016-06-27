using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
	public static GameObject[] m_sub_camera;
	public static bool m_is_custom = false;
	public static bool m_fix_camera = false;
	public static bool m_is_custom_enable = true;
	public GameObject ball;
	public Vector3 offset;

	static bool[] m_sub_camera_toggle_value;

	GameObject m_camera_ui;
	public static GameObject m_camera_setting;

	public static int focus_num = 4;

	public class CamInfo : MonoBehaviour{
		public Vector3 pos;
		public float fov;
		public Quaternion rot;
		public bool is_following = false;
		public GameObject look_target;

	}

	public static CamInfo[] cam_info;
	// Use this for initialization
	void Start () {
		m_sub_camera = GameObject.FindGameObjectsWithTag ("SubCamera");
		m_camera_setting = GameObject.Find("SettingManager");
		m_camera_setting.SetActive (false);

		foreach (GameObject x in m_sub_camera) {
			x.SetActive (false);
		}

		m_camera_ui = GameObject.Find ("CameraUI");

		m_sub_camera_toggle_value = new bool[4];

		cam_info = new CamInfo[10];
		for (int i = 0; i < 10; i++) {
			cam_info [i] = new CamInfo ();
		}

		cam_info [1].pos = new Vector3(532.1243f, -2.067582f, -845.7255f);
		cam_info [1].rot = new Quaternion (0.01944782f, 0.9141079f, 0.04415768f, -0.4025899f);
		cam_info [1].fov = 60;

		cam_info [2].pos = new Vector3(515.567f, -2.253678f, -868.0277f);
		cam_info [2].rot = new Quaternion(-0.06007061f, 0.3794549f, 0.0246977f, 0.9229277f);
		cam_info [2].fov = 60;

        cam_info [3].is_following = true;
		//cam_info [3].look_target =;

		cam_info [4].pos = new Vector3 (529.0781f, 78.40499f, -854.8462f);
		cam_info [4].rot = new Quaternion(0.531617f, 0.3018428f, -0.2103319f, 0.762912f);
		cam_info [4].fov = 69;
	
		for(int i=0;i<4;i++){
            if (i == 2) continue;
			m_sub_camera [i].transform.position = cam_info [i+1].pos;
			m_sub_camera [i].transform.rotation = cam_info [i+1].rot;
			m_sub_camera [i].GetComponent<Camera>().fieldOfView = cam_info [i+1].fov;
		}
	}

	void checkCameraUI(){
		if (m_is_custom) {
			m_camera_ui.SetActive (true);
			Time.timeScale = 0;
		} else {
			Time.timeScale = CurrentState.prev_time;
			m_camera_ui.SetActive (false);
		}
	}

	void multiviewCamera(){
		bool[] toggle_value = getSubCameraToggle ();

		for (int i = 0; i < toggle_value.Length; i++) {
			if (toggle_value [i]) {
				m_sub_camera [i].SetActive (true);
			} else {
				m_sub_camera [i].SetActive (false);
			}
		}
	}

	void fixCamera(){
		m_fix_camera = false;
		m_is_custom_enable = false;
		m_camera_setting.SetActive (true);
	}

	public static bool[] getSubCameraToggle(){
		m_sub_camera_toggle_value [0] = GameObject.Find ("Left Top Camera Manager").GetComponent<Toggle>().isOn;
		m_sub_camera_toggle_value [1] = GameObject.Find ("Right Top Camera Manager").GetComponent<Toggle>().isOn;
		m_sub_camera_toggle_value [3] = GameObject.Find ("Left Bottom Camera Manager").GetComponent<Toggle>().isOn;
		m_sub_camera_toggle_value [2] = GameObject.Find ("Right Bottom Camera Manager").GetComponent<Toggle>().isOn;

		return m_sub_camera_toggle_value;
	}

	// Update is called once per frame
	void Update () {
		if (!CurrentState.ispause) {
			checkCameraUI ();
		}
		if (m_camera_ui.activeSelf) {
			multiviewCamera ();
			if (m_fix_camera) {
				fixCamera ();
			}
		}

		if (ball == null) {
			ball = GameObject.Find ("ball");
		}

        if (MainCameraHandler.cnum == 3 && !m_is_custom)
        {
            if (!GameObject.Find("PH").GetComponent<Pitcher>().throwing)
            {
                Camera.main.transform.position = ball.transform.position + offset;
                Camera.main.transform.LookAt(new Vector3(585.543f, -2.053032f, -794.3815f));
            }
            else
            {
                Camera.main.transform.position = ball.transform.position + offset;
                Camera.main.transform.LookAt(GameObject.Find("C").transform.position + 2 * offset);
            }
        }

		if (!GameObject.Find ("PH").GetComponent<Pitcher> ().throwing) {
			m_sub_camera [3].transform.position = ball.transform.position + offset;
			m_sub_camera [3].transform.LookAt (new Vector3 (585.543f, -2.053032f, -794.3815f));
		} else {
			m_sub_camera [3].transform.position = ball.transform.position + offset;
			m_sub_camera [3].transform.LookAt (GameObject.Find("C").transform.position + 2 * offset);
		}
	}
}
