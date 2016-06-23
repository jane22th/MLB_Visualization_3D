using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public float distance = 5.0f;
	public float horizontalAngle = 0.0f;
	public float rotAngle = 180.0f; // 화면 가로폭만큼 커서를 이동시켰을 때 몇 도 회전하는가.
	public float verticalAngle = 10.0f;
	public Transform lookTarget;
	public Vector3 offset = Vector3.zero;

	/*test*/
	public bool auto = true;
	public Vector2 delta_position;
    Vector3 init_position;
    Quaternion init_rotation;
    GameObject sub_camera;

    public int c_num = 1;
	/*test*/

	InputProcessor inputManager;
	void Start()
	{
		//Time.timeScale = 0.1f;
		inputManager = FindObjectOfType<InputProcessor>();
        init_position = transform.position;
        init_rotation = transform.rotation;
        sub_camera = GameObject.Find("subCamera");
        sub_camera.SetActive(false);

	}

	// Update is called once per frame
	void LateUpdate () {
		// 드래그 입력으로 카메라 회전각을 갱신한다.

		/*test*/
        if(c_num == 1){
            lookTarget = GameObject.Find("Center Position").transform;
            offset = new Vector3(0, 20, 0);
            if (auto) {
                float anglePerPixel = rotAngle / (float)Screen.width;
                Vector2 delta = delta_position;
                horizontalAngle += delta.x * anglePerPixel;
                horizontalAngle = Mathf.Repeat(horizontalAngle, 360.0f);
                verticalAngle -= delta.y * anglePerPixel;
                verticalAngle = Mathf.Clamp(verticalAngle, -60.0f, 60.0f);
                sub_camera.SetActive(false);
            } else {
                transform.position = init_position;
                transform.rotation = init_rotation;
                sub_camera.SetActive(true);
            }
            /*test*/

            if (!auto && inputManager.MovedR()) {
                float anglePerPixel = rotAngle / (float)Screen.width;
                Vector2 delta = inputManager.GetDeltaPositionR();
                horizontalAngle += delta.x * anglePerPixel;
                horizontalAngle = Mathf.Repeat(horizontalAngle, 360.0f);
                verticalAngle -= delta.y * anglePerPixel;
                verticalAngle = Mathf.Clamp(verticalAngle, -60.0f, 60.0f);
            }

            // 카메라의 위치와 회전각을 갱신한다.
            if (auto && lookTarget != null) {
                Vector3 lookPosition = lookTarget.position + offset;
                // 주시 대상에서 상대 위치를 구한다.
                Vector3 relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -distance);

                // 주시 대상의 위치에 오프셋을 더한 위치로 이동시킨다.
                transform.position = lookPosition + relativePos;

                // 주시 대상을 주시하게 한다.
                transform.LookAt(lookPosition);

                // 장애물을 피한다.
                RaycastHit hitInfo;
                if (Physics.Linecast(lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Ground")))
                    transform.position = hitInfo.point;
            }
        }
        else{
            // 카메라의 위치와 회전각을 갱신한다.
            lookTarget = GameObject.Find("ball").transform;
            offset = new Vector3(0, 0, 0);
            verticalAngle = 40.0f;
            horizontalAngle = 0.0f;
            if (lookTarget != null) {
                Vector3 lookPosition = lookTarget.position + offset;
                // 주시 대상에서 상대 위치를 구한다.
                Vector3 relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -distance);

                // 주시 대상의 위치에 오프셋을 더한 위치로 이동시킨다.
                transform.position = lookPosition + relativePos;

                // 주시 대상을 주시하게 한다.
                transform.LookAt(lookPosition);

                // 장애물을 피한다.
                RaycastHit hitInfo;
                if (Physics.Linecast(lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Ground")))
                    transform.position = hitInfo.point;
            }
        }
	}
}
