using UnityEngine;
using System.Collections;

public class MainCameraHandler : MonoBehaviour {
	public float distance = 5.0f;
	public float horizontalAngle = 0.0f;
	public float rotAngle = 180.0f; // 화면 가로폭만큼 커서를 이동시켰을 때 몇 도 회전하는가.
	public float verticalAngle = 10.0f;
	public Transform lookTarget;
	public Vector3 offset = Vector3.zero;

	Vector3 init_position;
	Quaternion init_rotation;
	public bool auto = true;
	public Vector2 delta_position;

	public static int cnum = 1;
	public static bool change_camera = false;
	InputProcessor input_processor;
	bool is_following = false;
	GameObject selected_object;

	// Use this for initialization
	void Start () {
		input_processor = FindObjectOfType<InputProcessor>();
		init_position = transform.position;
		init_rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		// 드래그 입력으로 카메라 회전각을 갱신한다.

		lookTarget = GameObject.Find("Center Position").transform;
		offset = new Vector3(0, 20, 0);
		if (auto) {
			float anglePerPixel = rotAngle / (float)Screen.width;
			Vector2 delta = delta_position;
			horizontalAngle += delta.x * anglePerPixel;
			horizontalAngle = Mathf.Repeat(horizontalAngle, 360.0f);
			verticalAngle -= delta.y * anglePerPixel;
			verticalAngle = Mathf.Clamp(verticalAngle, -60.0f, 60.0f);
		} else {
			lookTarget = null;

			if (change_camera) {
				change_camera = false;
				changeCamera ();
			}

			if (is_following) {
				transform.position = selected_object.transform.position + new Vector3(0, 2, 0);
				transform.rotation = selected_object.transform.rotation;
			}
		}
			

		//카메라의 위치와 회전각을 갱신한다.
		if (lookTarget != null) {
			Vector3 lookPosition = lookTarget.position + offset;
			// 주시 대상에서 상대 위치를 구한다.
			Vector3 relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -distance);

			// 주시 대상의 위치에 오프셋을 더한 위치로 이동시킨다.
			// 주시 대상을 주시하게 한다.

			if (CameraManager.focus_num == 4) {
				transform.position = lookPosition + relativePos;
				transform.LookAt (lookPosition);
			}

			// 장애물을 피한다.
			RaycastHit hitInfo;
			if (Physics.Linecast (lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer ("Ground"))) {
				if (CameraManager.focus_num == 4)
					transform.position = hitInfo.point;
			}
		}
	}

	public void changeCamera(){
		if (!CameraManager.cam_info [cnum].is_following) {
			is_following = false;
			Camera.main.transform.position = CameraManager.cam_info [cnum].pos;
			Camera.main.transform.rotation = CameraManager.cam_info [cnum].rot;
			Camera.main.GetComponent<Camera> ().fieldOfView = CameraManager.cam_info [cnum].fov;
		} else {
			is_following = true;
			selected_object = CameraManager.cam_info [cnum].look_target;
		}
	}
}
