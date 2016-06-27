using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CameraSettingButton : MonoBehaviour {
	public static int m_cnum = 5;

	// Use this for initialization
	void Start () {

	}

	public void SetCamera(){
		GameObject input = GameObject.Find("inputtext");
		string str = input.GetComponent<Text>().text;
		m_cnum =  Convert.ToInt32(str);

		if (m_cnum >= 1 && m_cnum <= 4)
			return;

		if (!MoveCamera.is_object) {
			CameraManager.cam_info [m_cnum].pos = Camera.main.transform.position;
			CameraManager.cam_info [m_cnum].rot = Camera.main.transform.rotation;
			CameraManager.cam_info [m_cnum].fov = Camera.main.GetComponent<Camera> ().fieldOfView;
		} else {
			CameraManager.cam_info [m_cnum].is_following = true;
			CameraManager.cam_info [m_cnum].look_target = MoveCamera.selected_object;
		}

		CameraManager.m_camera_setting.SetActive (false);
		CameraManager.m_is_custom_enable = true;
        MainCameraHandler.cnum = m_cnum;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
