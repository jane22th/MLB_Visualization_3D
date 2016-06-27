using UnityEngine;
using System.Collections;


public class InputProcessor : MonoBehaviour {

	Vector2 slideStartPosition;
	Vector2 prevPosition;
	Vector2 m_delta_R = Vector2.zero;
	Vector2 m_delta_L = Vector2.zero;
	Vector2 m_delta_M = Vector2.zero;
	Vector2 m_delta_P = Vector2.zero;
	bool m_moved_R = false;
	bool m_moved_L = false;
	bool m_moved_M = false;
	bool m_moved_P = false;
	int m_moved_cnt = 0;

    GameObject m_state;
    bool m_is_state_visible, m_is_moving;

	public int alt_cnt = 0;
	InGame ingame;

	// Use this for initialization
	void Start () {
        m_state = GameObject.Find("GameStateUI/State");

        m_is_state_visible = false;
        m_is_moving = false;

		ingame = GameObject.Find ("InGameUI").GetComponent<InGame>();
	}

    bool checkEndGame()
    {
        GameObject par = GameObject.Find("playerlist");
        foreach (Transform child in par.transform)
        {
            if (child.gameObject.GetComponent<Fielder>().is_animating)
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown (KeyCode.Tab) && CurrentState.current_date != null) {
			float dx = 10f;
			if (m_is_moving == false) {
				m_is_moving = true;
				if (m_is_state_visible) {
					StartCoroutine (moveGameState (dx, -665.0f));
				} else {
					StartCoroutine (moveGameState (dx, -360.0f));
				}
			}
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			if (CameraManager.m_is_custom) {
				CameraManager.m_fix_camera = true;
			} else {
                if (checkEndGame())
                {
                    GameObject.Find("PH").GetComponent<Fielder>().is_animating = true;
                    ingame.step = true;
                }
			}
		} else if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		} else if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)){
			if (!Camera.main.GetComponent<MainCameraHandler> ().auto) {
				if (CameraManager.m_is_custom)
					CameraManager.m_is_custom = false;
				else
					CameraManager.m_is_custom = true;
			}
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha1)){
			MainCameraHandler.cnum = 1;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha2)) {
			MainCameraHandler.cnum = 2;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha3)){
			MainCameraHandler.cnum = 3;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha4)) {
			MainCameraHandler.cnum = 4;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha5)){
			MainCameraHandler.cnum = 5;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha6)) {
			MainCameraHandler.cnum = 6;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha7)){
			MainCameraHandler.cnum = 7;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha8)) {
			MainCameraHandler.cnum = 8;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha9)){
			MainCameraHandler.cnum = 9;
			MainCameraHandler.change_camera = true;
		} else if (CameraManager.m_is_custom_enable && Input.GetKeyDown(KeyCode.Alpha0)) {
			MainCameraHandler.cnum = 0;
			MainCameraHandler.change_camera = true;
		} 


		setWASD ();

		if (!Camera.main.GetComponent<MainCameraHandler> ().auto) {
			if(!m_moved_L && !m_moved_R)
				setMButton ();
		}
    }

	void setWASD(){
		if (Input.GetKeyDown(KeyCode.W)) {
			m_moved_P = true;
			m_moved_cnt++;
			m_delta_P.y = 0.12f;
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			m_moved_P = true;
			m_moved_cnt++;
			m_delta_P.x = -0.12f;
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			m_moved_P = true;
			m_moved_cnt++;
			m_delta_P.y = -0.12f;
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			m_moved_P = true;
			m_moved_cnt++;
			m_delta_P.x = 0.12f;
		}

		if (Input.GetKeyUp(KeyCode.W)) {
			m_moved_cnt--;
			m_delta_P.y = 0;
		}
		if (Input.GetKeyUp(KeyCode.A)) {
			m_moved_cnt--;
			m_delta_P.x = 0;
		}
		if (Input.GetKeyUp(KeyCode.S)) {
			m_moved_cnt--;
			m_delta_P.y = 0;
		}
		if (Input.GetKeyUp(KeyCode.D)) {
			m_moved_cnt--;
			m_delta_P.x = 0;
		}

		if (m_moved_cnt == 0)
			m_moved_P = false;
	}

	void setMButton(){
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			m_delta_M.x = -1;
		} else if(Input.GetAxis ("Mouse ScrollWheel") < 0){
			m_delta_M.x = 1;
		}
	}

	void setRButton(){
		// 슬라이드 시작 지점.
		if (Input.GetButtonDown("RButton"))
			slideStartPosition = GetCursorPosition();

		// 화면 너비의 10% 이상 커서를 이동시키면 슬라이드 시작으로 판단한다.
		if (Input.GetButton("RButton")) {
			if (Vector2.Distance(slideStartPosition,GetCursorPosition()) >= (Screen.width * 0.01f))
				m_moved_R = true;
		}

		// 슬라이드가 끝났는가.
		if (!Input.GetButtonUp("RButton") && !Input.GetButton("RButton"))
			// 슬라이드는 끝났다.
			m_moved_R = false; 

		// 이동량을 구한다.
		if (m_moved_R)
			m_delta_R = GetCursorPosition() - prevPosition;
		else
			m_delta_R = Vector2.zero;

		// 커서 위치를 갱신한다.
		prevPosition = GetCursorPosition();
	}

	void setLButton(){
		// 슬라이드 시작 지점.
		if (Input.GetButtonDown("LButton"))
			slideStartPosition = GetCursorPosition();

		// 화면 너비의 10% 이상 커서를 이동시키면 슬라이드 시작으로 판단한다.
		if (Input.GetButton("LButton")) {
			if (Vector2.Distance(slideStartPosition,GetCursorPosition()) >= (Screen.width * 0.01f))
				m_moved_L = true;
		}

		// 슬라이드가 끝났는가.
		if (!Input.GetButtonUp("LButton") && !Input.GetButton("LButton"))
			// 슬라이드는 끝났다.
			m_moved_L = false; 

		// 이동량을 구한다.
		if (m_moved_L)
			m_delta_L = GetCursorPosition() - prevPosition;
		else
			m_delta_L = Vector2.zero;

		// 커서 위치를 갱신한다.
		prevPosition = GetCursorPosition();
	}
    
    IEnumerator moveGameState(float dx, float bound_x) {
        yield return new WaitForSeconds(0.01f);
        if(m_is_state_visible) {
            if(m_state.transform.localPosition.x > bound_x) {
                m_state.transform.localPosition = new Vector2(m_state.transform.localPosition.x - dx, 0);
                StartCoroutine(moveGameState(dx, bound_x));
            } else {
                m_state.transform.localPosition = new Vector2(bound_x, 0);
                m_is_moving = false;
                m_is_state_visible = false;
            }
        } else {
            if(m_state.transform.localPosition.x < bound_x) {
                m_state.transform.localPosition = new Vector2(m_state.transform.localPosition.x + dx, 0);
                StartCoroutine(moveGameState(dx, bound_x));
                yield return new WaitForSeconds(0.1f);
            } else {
                m_state.transform.localPosition = new Vector2(bound_x, 0);
                m_is_moving = false;
                m_is_state_visible = true;
            }
        }
    }

	// 슬라이드할 때 커서 이동량.
	public Vector2 GetDeltaPositionR()
	{
		return m_delta_R;
	}

	// 슬라이드할 때 커서 이동량.
	public Vector2 GetDeltaPositionL()
	{
		return m_delta_L;
	}

	// 슬라이드할 때 커서 이동량.
	public Vector2 GetDeltaPositionM()
	{
		return m_delta_M;
	}

	public Vector2 GetDeltaPositionP()
	{
		return m_delta_P;
	}

	public void SetDeltaPositionMZero()
	{
		m_delta_M = Vector2.zero;
	}

	// 슬라이드 중인가.
	public bool MovedR()
	{
		return m_moved_R;
	}

	// 슬라이드 중인가.
	public bool MovedL()
	{
		return m_moved_L;
	}

	// 슬라이드 중인가.
	public bool MovedM()
	{
		return m_moved_M;
	}

	// 슬라이드 중인가.
	public bool MovedP()
	{
		return m_moved_P;
	}

	public Vector2 GetCursorPosition()
	{
		return Input.mousePosition;
	}
}

